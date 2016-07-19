using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using MvvmCross.Core.ViewModels;
using PW.Mobile.Core.Model;
using PW.Mobile.Core.Services;

namespace PW.Mobile.Core.ViewModels
{
	public class  UsersListViewModel: MvxViewModel
	{
		private readonly IUserService _userService;
		public ObservableCollection<User> Users { get; set; }

		public UsersListViewModel(IUserService userService)
		{
			_userService = userService;
			Users = new ObservableCollection<User>(_userService.GetUsers());
		}

		public void Init()
		{
		}
	}
}