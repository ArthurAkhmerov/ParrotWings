using System;

namespace PW.Domain
{
	public class Session
	{
		public Session(Guid userId)
		{
			Id = Guid.NewGuid();
			UserId = userId;
			CreatedAt = DateTime.UtcNow;
			LastUsage = DateTime.UtcNow;
		}

		public Guid Id { get; }
		public Guid UserId { get; }
		public DateTime CreatedAt { get; }
		public DateTime LastUsage { get; }

		internal Session(Guid id, Guid userId, DateTime createdAt, DateTime lastUsage)
		{
			Id = id;
			UserId = userId;
			CreatedAt = createdAt;
			LastUsage = lastUsage;
		}
	}
}