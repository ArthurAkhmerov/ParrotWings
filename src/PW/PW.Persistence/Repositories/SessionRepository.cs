using System;
using System.Collections.Generic;
using PW.Domain;
using PW.Domain.Repositories;
using PW.Persistence.DTO;
using PW.Persistence.Factories;

namespace PW.Persistence.Repositories
{
	public class SessionRepository: RepositoryBase<Session, Guid, SessionPDTO, SessionFactory>, ISessionRepository
	{
		public SessionRepository(SessionFactory factory, string connectionString) : base(nameof(Session.Id), factory, connectionString)
		{
		}

		public IReadOnlyCollection<Session> ListByUser(Guid userId)
		{
			return Query(x => x.UserId == userId);
		}
	}
}