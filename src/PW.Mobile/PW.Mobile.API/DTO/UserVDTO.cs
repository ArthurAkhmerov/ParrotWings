using System;
using Newtonsoft.Json;

namespace PW.Mobile.API.DTO
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
	}
}