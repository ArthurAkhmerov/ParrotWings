using System;
using MvvmCross.Platform;
using PW.Mobile.API;
using PW.Mobile.API.DTO;

namespace PW.Mobile.Core.Model
{
	public class Auth
	{
		public Auth(Guid userId, Guid sessionId,
			bool isLoggedIn = true)
		{
			UserId = userId;
			SessionId = sessionId;
			IsLoggedIn = isLoggedIn;
		}

		public bool IsLoggedIn { get; set; }
		public Guid UserId { get; }
		public Guid SessionId { get; }
	}
}