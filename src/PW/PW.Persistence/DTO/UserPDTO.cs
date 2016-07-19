using System;
using System.Collections;
using System.Collections.Generic;

namespace PW.Persistence.DTO
{
	public class UserPDTO
	{
		public Guid Id { get; set; }
		public string Username { get; set; }
		public string Email { get; set; }
		public string Salt { get; set; }
		public string HashedPassword { get; set; }
		public DateTime CreatedAt { get; set; }
		public int Role { get; set; }
		public int State { get; set; }
		public int Balance { get; set; }

		public virtual ICollection<TransferPDTO> TransfersFrom { get; set; }
		public virtual ICollection<TransferPDTO> TransfersTo { get; set; }
	}
}