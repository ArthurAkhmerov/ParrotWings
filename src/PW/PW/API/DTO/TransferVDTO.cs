using System;
using Newtonsoft.Json;
using PW.Domain;

namespace PW.API.DTO
{
	public class TransferVDTO
	{
		[JsonProperty("id")]
		public Guid Id { get; set; }
		[JsonProperty("userFromId")]
		public Guid UserFromId { get; set; }
		[JsonProperty("userFromName")]
		public string UserFromName { get; set; }
		[JsonProperty("userToId")]
		public Guid UserToId { get; set; }
		[JsonProperty("userToName")]
		public string UserToName { get; set; }
		[JsonProperty("amount")]
		public int Amount { get; set; }
		[JsonProperty("createdAt")]
		public DateTime CreatedAt { get; set; }

		public static TransferVDTO Create(Transfer transfer, User userFrom, User userTo)
		{
			return new TransferVDTO
			{
				Id = transfer.Id,
				UserFromId = transfer.UserFromId,
				UserFromName = userFrom.Username,
				UserToId = transfer.UserToId,
				UserToName = userTo.Username,
				Amount = transfer.Amount,
				CreatedAt = transfer.CreatedAt
			};
		}
	}
}