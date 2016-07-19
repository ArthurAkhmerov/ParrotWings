using System;
using System.Collections.Generic;

namespace PW.Domain.Repositories
{
	public interface ITransferRepository: IEntityRepository<Transfer, Guid>
	{
		IReadOnlyCollection<Transfer> ListByUsername(string username);
		IReadOnlyCollection<Transfer> ListBy(DateTime from, DateTime to);
		IReadOnlyCollection<Transfer> ListBy(Guid userId, DateTime from, DateTime to);
		IReadOnlyCollection<Transfer> ListBy(Guid userId, string usernameTo, DateTime from, DateTime to);
		IReadOnlyCollection<Transfer> ListBy(Guid userId, DateTime from, DateTime to, int skip, int take);
		IReadOnlyCollection<Transfer> ListBy(Guid userId, string usernameTo, DateTime from, DateTime to, int skip, int take);
		int Count(Guid userId, DateTime @from, DateTime to);
		int Count(Guid userId, Guid userToId, DateTime @from, DateTime to);
		int Count(Guid userId, string usernameTo, DateTime @from, DateTime to);
		void MakeTransfers(Transfer[] transfers);
	}
}