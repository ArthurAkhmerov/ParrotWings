using Newtonsoft.Json;

namespace PW.API.DTO
{
	public class SendTransferResultVDTO
	{
		[JsonProperty("success")]
		public bool Success { get; set; }
		[JsonProperty("message")]
		public string Message { get; set; }
	}
}