using System;
using System.Collections.Generic;

namespace PW.Domain.Repositories
{
	public interface ISessionRepository: IEntityRepository<Session, Guid>
	{
		IReadOnlyCollection<Session> ListByUser(Guid userId);
	}
}