using System;
using MvvmCross.Core.ViewModels;
using PW.Mobile.API;
using PW.Mobile.Core.Services;

namespace PW.Mobile.Core.ViewModels
{
	public class LoadingViewModel: MvxViewModel
	{
		private readonly IAuthService _authService;
		private readonly IUserService _userService;
		private readonly ITransferService _transferService;

		public LoadingViewModel(
			IAuthService authService,
			IUserService userService,
			ITransferService transferService)
		{
			_authService = authService;
			_userService = userService;
			_transferService = transferService;
		}

		private string _messageInfo;
		public string MessageInfo
		{
			get { return _messageInfo; }
			set { SetProperty(ref _messageInfo, value); }
		}

		public async override void Start()
		{
			var auth = _authService.GetAuth();

			if (string.IsNullOrEmpty(auth?.AccessToken))
			{
				ShowViewModel<LoginViewModel>();
				Close(this);
				return;
			}

			MessageInfo = "Loading users...";
			await _userService.LoadUsersAsync();

			MessageInfo = "Loading transactions...";
			await _transferService.LoadTransfersAsync(DateTime.Now.AddMonths(-2), DateTime.Now.AddMonths(2));

			await _userService.JoinUserAsync(auth.UserId);

			ShowViewModel<MainViewModel>();
			Close(this);
		}
	}
}