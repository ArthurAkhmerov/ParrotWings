using System;
using MvvmCross.Platform;
using PW.Mobile.API;
using PW.Mobile.API.DTO;

namespace PW.Mobile.Core.Model
{
	public class Auth
	{
		public Auth(Guid userId, string accessToken)
		{
			UserId = userId;
			AccessToken = accessToken;
		}

		public Guid UserId { get; }
		public string AccessToken { get; set; }
	}
}