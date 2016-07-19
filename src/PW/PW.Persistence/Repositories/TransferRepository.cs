using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using PW.Domain;
using PW.Domain.Exceptions;
using PW.Domain.Repositories;
using PW.Persistence.DTO;
using PW.Persistence.Factories;

namespace PW.Persistence.Repositories
{
	public class TransferRepository: RepositoryBase<Transfer, Guid, TransferPDTO, TransferFactory>, ITransferRepository
	{
		public TransferRepository(TransferFactory factory, string connectionString) : base(nameof(Transfer.Id), factory, connectionString)
		{
		}

		public IReadOnlyCollection<Transfer> ListByUsername(string username)
		{
			return Query(x => x.UserFrom.Username.Equals(username, StringComparison.CurrentCultureIgnoreCase)
			                  || x.UserTo.Username.Equals(username, StringComparison.CurrentCultureIgnoreCase));
		}

		public IReadOnlyCollection<Transfer> ListBy(DateTime @from, DateTime to)
		{
			return Query(x => x.CreatedAt >= from && x.CreatedAt <= to);
		}

		public IReadOnlyCollection<Transfer> ListBy(Guid userId, DateTime @from, DateTime to)
		{
			return Query(x => (x.UserFromId == userId || x.UserToId == userId) && x.CreatedAt >= from && x.CreatedAt <= to);
		}

		public IReadOnlyCollection<Transfer> ListBy(Guid userId, string usernameTo, DateTime @from, DateTime to)
		{
			return Query(x => (x.UserFromId == userId || x.UserToId == userId) && x.UserTo.Username.Contains(usernameTo) && x.CreatedAt >= from && x.CreatedAt <= to);
		}

		public IReadOnlyCollection<Transfer> ListBy(Guid userId, DateTime @from, DateTime to, int skip, int take)
		{
			return Query(x => (x.UserFromId == userId || x.UserToId == userId) && x.CreatedAt >= from && x.CreatedAt <= to,
				x => x.OrderByDescending(y => y.CreatedAt).Skip(skip).Take(take));
		}

		public IReadOnlyCollection<Transfer> ListBy(Guid userId, string usernameTo, DateTime @from, DateTime to, int skip, int take)
		{
			return Query(x => (x.UserFromId == userId || x.UserToId == userId) && x.UserTo.Username.Contains(usernameTo) && x.CreatedAt >= from && x.CreatedAt <= to,
				x => x.OrderByDescending(y => y.CreatedAt).Skip(skip).Take(take));
		}

		public int Count(Guid userId, DateTime @from, DateTime to)
		{
			return Count(x => (x.UserFromId == userId || x.UserToId == userId) && x.CreatedAt >= from && x.CreatedAt <= to);
		}

		public int Count(Guid userId, Guid userToId, DateTime @from, DateTime to)
		{
			return Count(x => (x.UserFromId == userId || x.UserToId == userId) && x.UserToId == userToId && x.CreatedAt >= from && x.CreatedAt <= to);
		}

		public int Count(Guid userId, string usernameTo, DateTime @from, DateTime to)
		{
			return Count(x => (x.UserFromId == userId || x.UserToId == userId) && x.UserTo.Username.Contains(usernameTo) && x.CreatedAt >= from && x.CreatedAt <= to);
		}

		public void MakeTransfers(Transfer[] transfers)
		{
			using (var ctx = CreateContext())
			{
				var transaction = ctx.Database.BeginTransaction();
				try
				{
					foreach (var transfer in transfers)
					{
						var userFrom = ctx.Set<UserPDTO>().FirstOrDefault(x => x.Id == transfer.UserFromId);
						var userTo = ctx.Set<UserPDTO>().FirstOrDefault(x => x.Id == transfer.UserToId);

						userFrom.Balance -= transfer.Amount;
						if (userFrom.Balance < 0) throw new InvalidBalanceException();
						userTo.Balance += transfer.Amount;

						ctx.Entry(userFrom).Property(x => x.Balance).IsModified = true;
						ctx.Entry(userTo).Property(x => x.Balance).IsModified = true;

						var dto = _factory.CreateDTO(transfer);
						ctx.Set<TransferPDTO>().Add(dto);
					}

					ctx.SaveChanges();
					transaction.Commit();
				}
				catch (Exception ex)
				{
					transaction.Rollback();
					throw;
				}
			}
		}


		protected override IQueryable<TransferPDTO> Include(IQueryable<TransferPDTO> src)
		{
			return src.Include(x => x.UserFrom).Include(x => x.UserTo);
		}

	}
}