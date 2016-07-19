using System;
using System.Collections.Generic;
using System.Linq;
using PW.Domain;
using PW.Domain.Repositories;
using PW.Persistence.DTO;
using PW.Persistence.Factories;

namespace PW.Persistence.Repositories
{
	public class UserRepository: RepositoryBase<User, Guid, UserPDTO, UserFactory>, IUserRepository
	{
		public UserRepository(UserFactory factory, string connectionString) : base(nameof(User.Id), factory, connectionString)
		{
		}

		public IReadOnlyCollection<User> ListByIds(Guid[] ids)
		{
			return Query(x => ids.Contains(x.Id));
		}

		public IReadOnlyCollection<User> ListByEmail(string email)
		{
			return Query(x => x.Email.Equals(email, StringComparison.InvariantCultureIgnoreCase));
		}

		public IReadOnlyCollection<User> ListBySearchText(string searchText)
		{
			return Query(x => x.Username.Contains(searchText) || x.Email.Contains(searchText));
		}
	}
}