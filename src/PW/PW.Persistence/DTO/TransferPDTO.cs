using System;

namespace PW.Persistence.DTO
{
	public class TransferPDTO
	{
		public Guid Id { get; set; }
		public int Amount { get; set; }
		public DateTime CreatedAt { get; set; }

		public Guid UserFromId { get; set; }
		public virtual UserPDTO UserFrom { get; set; }

		public Guid UserToId { get; set; }
		public virtual UserPDTO UserTo { get; set; }
	}
}