using System;
using System.Linq;
using System.Threading.Tasks;
using MvvmCross.Platform;
using PW.Mobile.API;
using PW.Mobile.API.DTO;
using PW.Mobile.Core.Model;

namespace PW.Mobile.Core.Services.Implementations
{
	public class AuthService : IAuthService
	{
		private readonly IPwApiClient _pwApiClient;
		private readonly ISettingsProvider _settingsProvider;

		public AuthService(IPwApiClient pwApiClient, ISettingsProvider settingsProvider)
		{
			_pwApiClient = pwApiClient;
			_settingsProvider = settingsProvider;
		}

		public async Task<Auth> SignIn(string email, string password)
		{
			var authRequestDto = new AuthRequestVDTO
			{
				Email = email,
				Password = password
			};

			var token = await _pwApiClient.GetTokenAsync(authRequestDto.Email, authRequestDto.Password); ;

			if (token != null && !string.IsNullOrEmpty(token.AccessToken))
			{
				var users = await _pwApiClient.GetUsersAsync(authRequestDto.Email);
				var user = users.FirstOrDefault();
				if (user != null)
				{
					var auth = new Auth(user.Id, token.AccessToken);
					_settingsProvider.SaveAuth(auth);
					return auth;
				}
				else
				{
					return null;
				}
				
			}
			else
			{
				return null;
			}
		}

		public async Task<AuthResultVDTO> SignUp(string name, string email, string password)
		{
			var dto = new SignupRequestVDTO
			{
				Name = name,
				Email = email,
				Password = password
			};

			return await _pwApiClient.SignUp(dto);
		}

		public void SignOut()
		{
			var auth = _settingsProvider.GetAuth();
			auth.AccessToken = null;

			_settingsProvider.SaveAuth(auth);
		}

		public bool IsSignedIn()
		{
			var auth = _settingsProvider.GetAuth();
			return !string.IsNullOrEmpty(auth?.AccessToken);
		}

		public Auth GetAuth()
		{
			return _settingsProvider.GetAuth();
		}
	}
}