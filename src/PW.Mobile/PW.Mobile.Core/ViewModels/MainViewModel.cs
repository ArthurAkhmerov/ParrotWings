using System;
using System.Linq;
using System.Threading.Tasks;
using MvvmCross.Core.ViewModels;
using MvvmCross.Plugins.Messenger;
using PW.Mobile.API;
using PW.Mobile.Core.Services;

namespace PW.Mobile.Core.ViewModels
{
	public class DateChangedMessage : MvxMessage
	{
		public DateChangedMessage(object sender, DateTime from, DateTime to) : base(sender)
		{
			From = from;
			To = to;
		}

		public DateTime From { get; private set; }
		public DateTime To { get; private set; }
	}

	public class MainViewModel: MvxViewModel
	{
		private readonly IMvxMessenger _messenger;
		private readonly IAuthService _authService;
		private readonly IUserService _userService;
		private readonly ITransferService _transferService;

		private MvxSubscriptionToken balanceChangedToken;

		public MainViewModel(IMvxMessenger messenger, IAuthService authService, IUserService userService,ITransferService transferService)
		{
			_messenger = messenger;
			_authService = authService;
			_userService = userService;
			_transferService = transferService;

			Username = _userService.GetCurrentUser().Username;
			Balance = _userService.GetCurrentUser().Balance;

			balanceChangedToken = _messenger.Subscribe<BalanceChangedMessage>(result =>
			{
				Balance = result.Balance;
			});
		}

		private string _username;
		public string Username
		{
			get { return _username; }
			set
			{
				SetProperty(ref _username, value);
			}
		}

		private int _balance;
		public int Balance
		{
			get { return _balance; }
			set
			{
				SetProperty(ref _balance, value);
			}
		}

		private bool _isLoading;
		public bool IsLoading
		{
			get { return _isLoading; }
			set
			{
				SetProperty(ref _isLoading, value);
			}
		}

		public readonly MenuItem[] MenuItems =
		{
			new MenuItem("Users", typeof(UsersListViewModel)), 
			new MenuItem("Transfers", typeof(TransfersListViewModel)), 
			new MenuItem("Create Transfer", typeof(CreateTransferViewModel)), 
		};
		

		public void ShowDefaultMenuItem()
		{
			NavigateTo(1);
		}

		public void NavigateTo(int position)
		{
			ShowViewModel(MenuItems[position].ViewModelType);
		}

		public void Init()
		{
			var users = _userService.GetUsers();
			if (!users.Any())
			{
				IsLoading = true;
				_userService.LoadUsersAsync();
			}
			else
			{
				IsLoading = false;
			}
		}

		private MvxCommand _singoutCommand;

		public IMvxCommand SignoutCommand
		{
			get
			{
				return _singoutCommand = _singoutCommand ?? new MvxCommand(() =>
				{
					_authService.SignOut();
					Close(this);
				});
			}
		}

		public async void ChangeDate(DateTime from, DateTime to)
		{
			await _transferService.LoadTransfersAsync(from, to);
			_messenger.Publish(new DateChangedMessage(this, from, to));
		}
	}

	public class MenuItem : Tuple<string, Type>
	{
		public MenuItem(string displayName, Type viewModelType)
			: base(displayName, viewModelType)
		{ }

		public string DisplayName
		{
			get { return Item1; }
		}

		public Type ViewModelType
		{
			get { return Item2; }
		}
	}
}