using System;

namespace PW.Persistence.DTO
{
	public class SessionPDTO
	{
		public Guid Id { get; set; }
		public Guid UserId { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime LastUsage { get; set; }
	}
}