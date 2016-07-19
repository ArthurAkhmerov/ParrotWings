using System;

namespace PW.Domain
{
	public class Transfer
	{
		public Transfer(Guid userFromId, Guid userToId, int amount)
		{
			Id = Guid.NewGuid();
			UserFromId = userFromId;
			UserToId = userToId;
			Amount = amount;
			CreatedAt = DateTime.UtcNow;
		}

		public Guid Id { get; }
		public Guid UserFromId { get; }
		public Guid UserToId { get; }
		public int Amount { get; }
		public DateTime CreatedAt { get; }

		internal Transfer(Guid id, Guid userFromId, Guid userToId, int amount, DateTime createdAt)
		{
			Id = id;
			UserFromId = userFromId;
			UserToId = userToId;
			Amount = amount;
			CreatedAt = createdAt;
		}

		protected bool Equals(Transfer other)
		{
			return Id.Equals(other.Id) && UserFromId.Equals(other.UserFromId) 
					&& UserToId.Equals(other.UserToId)
			       && Amount == other.Amount && CreatedAt.Equals(other.CreatedAt);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				var hashCode = Id.GetHashCode();
				hashCode = (hashCode * 397) ^ UserFromId.GetHashCode();
				hashCode = (hashCode * 397) ^ UserToId.GetHashCode();
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