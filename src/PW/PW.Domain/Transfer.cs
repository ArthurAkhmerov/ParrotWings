using System;

namespace PW.Domain
{
	public class Transfer
	{
		public Transfer(User userFrom, User userTo, int amount)
		{
			Id = Guid.NewGuid();
			UserFrom = userFrom;
			UserTo = userTo;
			Amount = amount;
			CreatedAt = DateTime.UtcNow;
		}

		public Guid Id { get; }
		public User UserFrom { get; }
		public User UserTo { get; }
		public int Amount { get; }
		public DateTime CreatedAt { get; }

		internal Transfer(Guid id, User userFrom, User userTo, int amount, DateTime createdAt)
		{
			Id = id;
			UserFrom = userFrom;
			UserTo= userTo;
			Amount = amount;
			CreatedAt = createdAt;
		}

		protected bool Equals(Transfer other)
		{
			return Id.Equals(other.Id) && UserFrom.Equals(other.UserFrom) 
					&& UserTo.Equals(other.UserTo)
			       && Amount == other.Amount && CreatedAt.Equals(other.CreatedAt);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				var hashCode = Id.GetHashCode();
				hashCode = (hashCode * 397) ^ UserFrom.GetHashCode();
				hashCode = (hashCode * 397) ^ UserTo.GetHashCode();
				hashCode = (hashCode * 397) ^ Amount;
				hashCode = (hashCode * 397) ^ CreatedAt.GetHashCode();
				return hashCode;
			}
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != GetType()) return false;
			return Equals((Transfer) obj);
		}
	}
}