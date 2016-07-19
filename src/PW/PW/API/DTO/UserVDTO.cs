using System;
using Newtonsoft.Json;
using PW.Domain;

namespace PW.API.DTO
{
	public class UserVDTO
	{
		[JsonProperty("id")]
		public Guid Id { get; set; }
		[JsonProperty("username")]
		public string Username { get; set; }
		[JsonProperty("email")]
		public string Email { get; set; }
		[JsonProperty("createdAt")]
		public DateTime CreatedAt { get; set; }
		[JsonProperty("state")]
		public string State { get; set; }
		[JsonProperty("role")]
		public string Role { get; set; }
		[JsonProperty("balance")]
		public int Balance { get; set; }

		public static UserVDTO Create(User user)
		{
			if(user == null) throw new ArgumentNullException(nameof(user));

			return new UserVDTO
			{
				Id = user.Id,
				Username = user.Username,
				Email = user.Email,
				CreatedAt = user.CreatedAt,
				State = user.State.ToString("G"),
				Role = user.Role.ToString("G"),
				Balance = user.Balance
			};
		}
	}
}