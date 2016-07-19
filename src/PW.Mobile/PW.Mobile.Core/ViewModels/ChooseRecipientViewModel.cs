using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using MvvmCross.Core.ViewModels;
using MvvmCross.Plugins.Messenger;
using PW.Mobile.Core.Extensions;
using PW.Mobile.Core.Model;
using PW.Mobile.Core.Services;

namespace PW.Mobile.Core.ViewModels
{
	public class ChooseRecipientViewModel: MvxViewModel
	{
		private readonly IMvxMessenger _messenger;
		private readonly IUserService _userService;
		private string _searchText;
		private readonly Collection<User> _selectedUsers;
		public ObservableCollection<UserSelectable> Users { get; set; }

		
		public ChooseRecipientViewModel(IMvxMessenger messenger, IUserService userService)
		{
			_messenger = messenger;
			_userService = userService;
			Users = new ObservableCollection<UserSelectable>(_userService.GetUsers().Select(x => new UserSelectable(x)));
			_selectedUsers = new Collection<User>();
		}

		public void Init()
		{
		}

		public string SearchText
		{
			get { return _searchText; }
			set
			{
				SetProperty(ref _searchText, value);

				Users.Clear();
				Users.AddRange(_userService.GetUsers()
					.Where(x => x.Username.ToLower().Contains(SearchText.ToLower()) 
						|| x.Email.ToLower().Contains(SearchText.ToLower()))
					.OrderBy(x => x.Username)
					.Select(user => new UserSelectable(user, _selectedUsers.Contains(user))));
			}
		}

		private MvxCommand<UserSelectable> _changeUserStatusCommand;
		public IMvxCommand ChangeUserStatusCommand
		{
			get
			{
				return _changeUserStatusCommand = _changeUserStatusCommand ??
					new MvxCommand<UserSelectable>((user) =>
					{
						user.IsSelected = !user.IsSelected;

						if (user.IsSelected)
						{
							_selectedUsers.Add(user.Item);
						}
						else
						{
							_selectedUsers.Remove(user.Item);
						}
						//_messenger.Publish(new SelectedUsersMessage(this, "finished!"));
						//ShowViewModel<CreateTransferViewModel>();
					});
			}
		}

		private MvxCommand _finishCommand;
		public IMvxCommand FinishCommand
		{
			get
			{
				return _finishCommand = _finishCommand ??
					new MvxCommand(() =>
					{
						_messenger.Publish(new SelectedUsersMessage(this, _selectedUsers.ToArray()));
						ShowViewModel<CreateTransferViewModel>();
					});
			}
		}

		private MvxCommand _cancelCommand;
		public IMvxCommand CancelCommand
		{
			get
			{
				return _cancelCommand = _cancelCommand ??
					new MvxCommand(() =>
					{
						_messenger.Publish(new SelectedUsersMessage(this, null));
						ShowViewModel<CreateTransferViewModel>();
					});
			}
		}

		public void ChooseAllUsers()
		{
			_selectedUsers.Clear();

			foreach (var u in Users)
			{
				u.IsSelected = true;
				_selectedUsers.Add(u.Item);
			}
		}

		public void ChooseNone()
		{
			_selectedUsers.Clear();

			foreach (var u in Users)
			{
				u.IsSelected = false;
			}
		}
	}
}