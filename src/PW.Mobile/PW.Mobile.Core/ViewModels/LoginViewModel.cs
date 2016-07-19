using System.Threading.Tasks;
using System.Windows.Input;
using MvvmCross.Core.ViewModels;
using PW.Mobile.API;
using PW.Mobile.API.DTO;
using PW.Mobile.Core.Model;
using PW.Mobile.Core.Services;

namespace PW.Mobile.Core.ViewModels
{
	public class LoginViewModel : MvxViewModel
	{
		private readonly IPwApiClient _pwApiClient;
		private readonly IAuthService _authService;


		public LoginViewModel(IPwApiClient pwApiClient, IAuthService authService)
		{
			_pwApiClient = pwApiClient;
			_authService = authService;
		}

		public void Init()
		{
			Email = string.Empty;
			Password = string.Empty;
			MessageInfo = string.Empty;
#if DEBUG
			Email = "username1@gmail.com";
			Password = "username1";
#endif
		}

		public string Email { get; set; }

		public string Password { get; set; }

		private string _messageInfo;
		public string MessageInfo
		{
			get { return _messageInfo; }
			set { SetProperty(ref _messageInfo, value); }
		}

		private MvxCommand _tryLoginCommand;
		public IMvxCommand TryLoginCommand
		{
			get
			{
				return _tryLoginCommand = _tryLoginCommand ?? new MvxCommand(async () =>
				{
					var auth = await _authService.SignIn(Email, Password);

					if (auth != null)
					{
						ShowViewModel<LoadingViewModel>();
						Close(this);
					}
					else
					{
						MessageInfo = "The email and password you entered don't match.";
					}
				});
			}
		}

		private MvxCommand _signUpCommand;
		public IMvxCommand SignUpCommand
		{
			get
			{
				return _signUpCommand = _signUpCommand ?? new MvxCommand(() =>
				{
					ShowViewModel<SignupViewModel>();
					Close(this);
				});
			}
		}
	}
}