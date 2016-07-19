using System;

namespace PW.Mobile.API.DTO
{
	public class AuthResultVDTO
	{
		public bool Success { get; set; }
		public AuthData Data { get; set; }
		public string Message { get; set; }
	}

	public class AuthData
	{
		public Guid SessionId { get; set; }
		public Guid UserId { get; set; }
	}
}