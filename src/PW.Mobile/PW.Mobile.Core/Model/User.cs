using System;
using MvvmCross.Core.ViewModels;

namespace PW.Mobile.Core.Model
{
	public class User:MvxViewModel
	{
		public User(Guid id, string username, string email, int balance)
		{
			Id = id;
			Username = username;
			Email = email;
			Balance = balance;
		}

		public Guid Id { get; set; }
		public string Username { get; set; }
		public string Email { get; set; }
		public int Balance { get; set; }
	}
}