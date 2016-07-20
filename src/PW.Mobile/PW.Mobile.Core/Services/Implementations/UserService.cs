using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MvvmCross.Plugins.Messenger;
using PW.Mobile.API;
using PW.Mobile.Core.Model;

namespace PW.Mobile.Core.Services.Implementations
{
	public class UserService : IUserService
	{
		private readonly IMvxMessenger _messenger;
		private readonly IPwApiClient _pwApiClient;
		private readonly IPwHub _pwHub;
		private readonly IAuthService _authService;
		private IList<User> _users;

		public UserService(IMvxMessenger messenger, IPwApiClient pwApiClient, IPwHub pwHub, IAuthService authService)
		{
			_messenger = messenger;
			_pwApiClient = pwApiClient;
			_pwHub = pwHub;
			_authService = authService;
			_users = new List<User>();
		}

		public IReadOnlyCollection<User> GetUsers()
		{
			return _users.ToArray();
		}

		public User GetCurrentUser()
		{
			var auth = _authService.GetAuth();
			if (auth == null) return null;

			return _users.FirstOrDefault(x => x.Id == auth.UserId);
		}

		public async Task LoadUsersAsync()
		{
			var dtos = await _pwApiClient.GetUsersAsync(0, 100);

			_users.Clear();

			foreach (var dto in dtos)
			{
				var user = new User(dto.Id, dto.Username, dto.Email, dto.Balance);
				_users.Add(user);
			}

			_users = _users.OrderBy(x => x.Username).ToList();
		}

		public async Task JoinUserAsync(Guid userId, Guid sessionId)
		{
			
			await _pwHub.JoinUserAsync(userId, sessionId);
		}

		public void ChangeBalance(int value)
		{
			GetCurrentUser().Balance += value;
			_messenger.Publish(new BalanceChangedMessage(this, GetCurrentUser().Balance));
		}
	}
}