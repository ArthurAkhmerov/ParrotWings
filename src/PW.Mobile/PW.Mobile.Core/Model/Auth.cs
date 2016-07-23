using System;
using MvvmCross.Platform;
using PW.Mobile.API;
using PW.Mobile.API.DTO;

namespace PW.Mobile.Core.Model
{
	public class Auth
	{
		public Auth(Guid userId, string accessToken,
			bool isLoggedIn = true)
		{
			UserId = userId;
			AccessToken = accessToken;
			IsLoggedIn = isLoggedIn;
		}

		public bool IsLoggedIn { get; set; }
		public Guid UserId { get; }
		public string AccessToken { get; set; }
	}
}