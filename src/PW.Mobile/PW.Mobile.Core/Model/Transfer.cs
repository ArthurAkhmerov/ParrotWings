using System;

namespace PW.Mobile.Core.Model
{
	public class Transfer
	{
		public Transfer(Guid id, User userFrom, User userTo, int amount, DateTime createdAt, bool isInconming)
		{
			Id = id;
			UserFrom = userFrom;
			UserTo = userTo;
			Amount = amount;
			CreatedAt = createdAt;
			IsIncoming = isInconming;
		}

		public Guid Id { get; set; }
		public User UserFrom { get; set; }
		public User UserTo { get; set; }
		public int Amount { get; set; }
		public DateTime CreatedAt { get; set; }
		public bool IsIncoming { get; set; }
	}
}