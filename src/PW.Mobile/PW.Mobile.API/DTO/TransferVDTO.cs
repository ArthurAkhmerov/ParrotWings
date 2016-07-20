using System;

namespace PW.Mobile.API.DTO
{
	public class TransferVDTO
	{
		public Guid Id { get; set; }
		public Guid UserFromId { get; set; }
		public string UserFromName { get; set; }
		public Guid UserToId { get; set; }
		public string UserToName { get; set; }
		public int Amount { get; set; }
		public DateTime CreatedAt { get; set; }

		
		
	}
}