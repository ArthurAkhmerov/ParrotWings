using System;

namespace PW.Domain.Exceptions
{
	public class InvalidBalanceException : ApplicationException
	{
		public InvalidBalanceException() : base("invalid balance") {}
		public InvalidBalanceException(string message) : base(message) { }
	}
}