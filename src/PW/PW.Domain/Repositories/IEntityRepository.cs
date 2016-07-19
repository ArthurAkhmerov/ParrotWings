using System;
using System.Collections.Generic;

namespace PW.Domain.Repositories
{
	public interface IEntityRepository<TEntity, TKey>
		where TEntity : class
		where TKey : IEquatable<TKey>
	{
		IReadOnlyCollection<TEntity> List();
		TEntity GetByKey(TKey id);
		void Delete(TEntity item);
		void DeleteByKey(TKey id);
		RepositoryResult<TEntity> SaveOrUpdate(TEntity item);
		bool Exists(TKey key);
		void Clear();
	}

	public class RepositoryResult<TEntity>
		where TEntity : class
	{
		public RepositoryActionType Type { get; set; }
		public Exception Exception { get; set; }
		public bool Success { get; set; }
		public TEntity Entity { get; set; }
	}

	public enum RepositoryActionType
	{
		EntityCreated, EntityUpdated, EntityDeleted
	}
}