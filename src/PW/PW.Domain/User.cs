using System;
using PW.Domain.Exceptions;

namespace PW.Domain
{
	public enum Role
	{
		Admin,
		User,
	}

	public enum State
	{
		Offline,
		Online
	}

	public class User
	{


		public User(string username, string email, string salt, string hashedPassword, Role role)
		{
			Id = Guid.NewGuid();
			Username = username;
			Email = email;
			Salt = salt;
			HashedPassword = hashedPassword;
			CreatedAt = DateTime.UtcNow;
			Role = role;
			State = State.Offline;
			Balance = 500;
		}

		public Guid Id { get; }
		public string Username { get; }
		public string Email { get; }
		public string Salt { get; }
		public string HashedPassword { get; }
		public DateTime CreatedAt { get; }
		public Role Role { get; set; }
		public State State { get; private set; }
		public int Balance { get; private set; }

		public void Credit(int value)
		{
			if (value < 0) throw new ArgumentException(nameof(value));
			Balance += value;
		}

		public void Debit(int value)
		{
			if (value < 0) throw new ArgumentException(nameof(value));
			if (Balance - value < 0) throw new InvalidBalanceException();

			Balance -= value;
		}

		public void MarkLoggedIn(bool isLoggedIn = true)
		{
			State = isLoggedIn ? State.Online : State.Offline;
		}

		internal User(Guid id, string username, string email, string salt, string hashedPassword, DateTime createdAt, Role role, State state, int balance)
		{
			Id = id;
			Username = username;
			Email = email;
			Salt = salt;
			HashedPassword = hashedPassword;
			CreatedAt = createdAt;
			Role = role;
			State = state;
			Balance = balance;
		}

		protected bool Equals(User other)
		{
			return Id.Equals(other.Id) && string.Equals(Username, other.Username)
			       && string.Equals(Email, other.Email) && string.Equals(Salt, other.Salt) &&
			       string.Equals(HashedPassword, other.HashedPassword) && CreatedAt.Equals(other.CreatedAt) && Role == other.Role &&
			       State == other.State && Balance == other.Balance;
		}

		public override int GetHashCode()
		{
			unchecked
			{
				var hashCode = Id.GetHashCode();
				hashCode = (hashCode * 397) ^ (Username?.GetHashCode() ?? 0);
				hashCode = (hashCode * 397) ^ (Email?.GetHashCode() ?? 0);
				hashCode = (hashCode * 397) ^ (Salt?.GetHashCode() ?? 0);
				hashCode = (hashCode * 397) ^ (HashedPassword?.GetHashCode() ?? 0);
				hashCode = (hashCode * 397) ^ CreatedAt.GetHashCode();
				return hashCode;
			}
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != GetType()) return false;
			return Equals((User)obj);
		}
	}
}