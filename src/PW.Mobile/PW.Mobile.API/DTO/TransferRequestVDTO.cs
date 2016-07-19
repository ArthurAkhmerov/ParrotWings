using System;

namespace PW.Mobile.API.DTO
{
	public class TransferRequestVDTO
	{
		public Guid UserFromId { get; set; }
		public Guid[] RecipientsIds { get; set; }
		public int Amount { get; set; }
	}
}