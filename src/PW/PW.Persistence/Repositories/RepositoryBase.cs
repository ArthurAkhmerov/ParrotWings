using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;
using Functional.Maybe;
using PW.Domain.Repositories;
using PW.Persistence.Exceptions;
using PW.Persistence.Factories;

namespace PW.Persistence.Repositories
{
	public abstract class RepositoryBase<TEntity, TKey, TDTO, TFactory>
		where TEntity : class
		where TDTO : class
		where TFactory : class, IFactory<TEntity, TDTO>
		where TKey : IEquatable<TKey>
	{
		protected readonly TFactory _factory;
		private readonly string _keyPropertyName;
		protected readonly Func<TEntity, TKey> _keyGetter;
		protected readonly string _connectionString;

		protected RepositoryBase(string keyPropertyName, TFactory factory, string connectionString)
		{
			Contract.Requires(!string.IsNullOrWhiteSpace(keyPropertyName));
			Contract.Requires(factory != null);
			Contract.Requires(!string.IsNullOrWhiteSpace(connectionString));

			_keyPropertyName = keyPropertyName;
			_keyGetter = CreateKeyGetter().Compile();
			_factory = factory;
			_connectionString = connectionString;
		}

		public virtual IReadOnlyCollection<TEntity> List()
		{
			return Query(_ => true);
		}

		public int Count(Expression<Func<TDTO, bool>> filter)
		{
			using (var ctx = CreateContext())
			{
				return ctx.Set<TDTO>().Count(filter);
			}
		}

		protected virtual IReadOnlyCollection<TEntity> Query(Expression<Func<TDTO, bool>> filter, Func<IQueryable<TDTO>, IQueryable<TDTO>> queryModifier = null)
		{
			if (queryModifier == null)
				queryModifier = src => src;

			using (var ctx = CreateContext())
				return queryModifier(
						Include(ctx.Set<TDTO>())
						.Where(filter)
					)
					.ToArray()
					.Select(_factory.CreateEntity)
					.ToArray();
		}

		protected virtual int Count(Expression<Func<TDTO, bool>> filter, Func<IQueryable<TDTO>, IQueryable<TDTO>> queryModifier = null)
		{
			if (queryModifier == null)
				queryModifier = src => src;

			using (var ctx = CreateContext())
				return queryModifier(
					Include(ctx.Set<TDTO>())
						.Where(filter)
					).Count();
		}

		public TEntity GetByKey(TKey key)
		{
			return FindByKey(key)
					.OrElse(() => new EntityNotFoundException(key.ToString(), typeof(TEntity).Name));
		}
		public Maybe<TEntity> FindByKey(TKey key)
		{
			using (var ctx = CreateContext())
				return FindDTOByKey(ctx, key)
						.Select(_factory.CreateEntity);
		}
		protected Maybe<TDTO> FindDTOByKey(DbContext ctx, TKey key) =>
			Include(ctx.Set<TDTO>())
				.FirstOrDefault(CreateKeyComparison(key))
				.ToMaybe();

		protected virtual DbContext CreateContext() => new DataContext(_connectionString);
		protected virtual IQueryable<TDTO> Include(IQueryable<TDTO> src) => src;
		private Expression<Func<TEntity, TKey>> CreateKeyGetter()
		{
			var p0 = Expression.Parameter(typeof(TEntity), "dto");
			return Expression.Lambda<Func<TEntity, TKey>>(Expression.Property(p0, _keyPropertyName), p0);
		}

		private Expression<Func<TDTO, bool>> CreateKeyComparison(TKey key)
		{
			var p0 = Expression.Parameter(typeof(TDTO), "dto");
			return Expression.Lambda<Func<TDTO, bool>>(
				Expression.Equal(
					Expression.Property(p0, _keyPropertyName),
					Expression.Constant(key)),
				p0);
		}

		public virtual RepositoryResult<TEntity> SaveOrUpdate(TEntity item)
		{
			using (var ctx = CreateContext())
			{
				var newVersionDTO = _factory.CreateDTO(item);

				var key = _keyGetter(item);
				var existingEntity = FindDTOByKey(ctx, key);
				if (existingEntity.HasValue)
				{
					ctx.Entry(existingEntity.Value).CurrentValues.SetValues(newVersionDTO);
					ctx.SaveChanges();
					return new RepositoryResult<TEntity> { Success = true, Type = RepositoryActionType.EntityUpdated, Entity = item };
				}
				ctx.Set<TDTO>().Add(newVersionDTO);
				ctx.SaveChanges();
				return new RepositoryResult<TEntity> { Success = true, Type = RepositoryActionType.EntityCreated, Entity = item };
			}
		}

		public void Delete(TEntity item)
		{
			DeleteByKey(_keyGetter(item));
		}

		public bool Exists(TKey key)
		{
			using (var ctx = CreateContext())
			{
				var existingEntity = FindDTOByKey(ctx, key);
				return existingEntity.HasValue;
			}
		}

		public void DeleteByKey(TKey key)
		{
			using (var ctx = CreateContext())
			{
				var existingEntity = FindDTOByKey(ctx, key);
				if (existingEntity.HasValue)
				{
					OnDeleting(ctx, existingEntity.Value);
					ctx.Entry(existingEntity.Value).State = EntityState.Deleted;
					ctx.SaveChanges();
				}
			}
		}

		public void Clear()
		{
			using (var ctx = CreateContext())
			{
				ctx.Set<TDTO>().RemoveRange(ctx.Set<TDTO>());
				ctx.SaveChanges();
			}
		}

		protected virtual void OnDeleting(DbContext ctx, TDTO value)
		{
		}
	}
}