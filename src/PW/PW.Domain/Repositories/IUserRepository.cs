using System;
using System.Collections.Generic;

namespace PW.Domain.Repositories
{
	public interface IUserRepository: IEntityRepository<User, Guid>
	{
		IReadOnlyCollection<User> ListByIds(Guid[] ids);
		IReadOnlyCollection<User> ListByEmail(string email);
		IReadOnlyCollection<User> ListBySearchText(string searchText);
	}
}