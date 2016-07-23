using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using PW.Mobile.API;
using PW.Mobile.API.DTO;
using PW.Mobile.Core.Model;

namespace PW.Mobile.Core.Services
{
	public interface IUserService
	{
		IReadOnlyCollection<User> GetUsers();
		User GetCurrentUser();
		Task LoadUsersAsync();
		Task JoinUserAsync(Guid userId);
		void ChangeBalance(int value);
	}

	
}