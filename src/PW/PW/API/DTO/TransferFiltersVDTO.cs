using System;

namespace PW.API.DTO
{
	public class TransferFiltersVDTO
	{
		public Guid UserFromId { get; set; }
		public DateTime From { get; set; }
		public DateTime To { get; set; }
	}
}