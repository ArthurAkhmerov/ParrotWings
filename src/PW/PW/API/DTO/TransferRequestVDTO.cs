using System;
using Newtonsoft.Json;

namespace PW.API.DTO
{
	public class TransferRequestVDTO
	{
		[JsonProperty("userFromId")]
		public Guid UserFromId { get; set; }
		[JsonProperty("recipientsIds")]
		public Guid[] RecipientsIds { get; set; }
		[JsonProperty("amount")]
		public int Amount { get; set; }
	}
}