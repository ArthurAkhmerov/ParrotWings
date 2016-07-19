using System;
using System.Linq;
using MvvmCross.Core.ViewModels;
using MvvmCross.Plugins.Messenger;
using PW.Mobile.API;
using PW.Mobile.API.DTO;
using PW.Mobile.Core.Model;
using PW.Mobile.Core.Services;

namespace PW.Mobile.Core.ViewModels
{
	public class SelectedUsersMessage : MvxMessage
	{
		public SelectedUsersMessage(object sender, User[] recipients) : base(sender)
		{
			Recipients = recipients;
		}

		public User[] Recipients { get; private set; }
	}

	public class CreateTransferViewModel: MvxViewModel
	{
		private readonly IAuthService _authService;
		private readonly ITransferService _transferService;
		
		private int _amount;
		private User[] _recipients;

		private readonly MvxSubscriptionToken selectedUsersMessageToken;
		private readonly MvxSubscriptionToken transferSentMessageToken;


		public CreateTransferViewModel(IMvxMessenger messenger, IAuthService _authService, ITransferService transferService)
		{
			IsLoading = false;
			_recipients = new User[0];
			this._authService = _authService;
			_transferService = transferService;

			selectedUsersMessageToken = messenger.Subscribe<SelectedUsersMessage>((result) =>
			{
				if (result == null) return;

				RecipientInput = string.Join("; ", result.Recipients.Select(x => x.Username));
				_recipients = result.Recipients;
				SendCommand.RaiseCanExecuteChanged();
			});

			transferSentMessageToken = messenger.Subscribe<TransferSentMessage>((result) =>
			{
				
			});
		}

		private bool _isLoading;
		public bool IsLoading
		{
			get { return _isLoading; }
			set
			{
				SetProperty(ref _isLoading, value);
				SendCommand.RaiseCanExecuteChanged();
			}
		}

		private string _messageInfo;
		public string MessageInfo
		{
			get { return _messageInfo; }
			set { SetProperty(ref _messageInfo, value); }
		}

		private string _recipientInput;
		public string RecipientInput
		{
			get { return _recipientInput; }
			set
			{
				SetProperty(ref _recipientInput, value);
				SendCommand.RaiseCanExecuteChanged();
			}
		}

		private string _amountInput;
		public string AmountInput
		{
			get { return _amountInput; }
			set
			{
				SetProperty(ref _amountInput, value);
				SendCommand.RaiseCanExecuteChanged();
			}
		}

		private MvxCommand _chooseRecipientCommand;
		public IMvxCommand ChooseRecipientCommand
		{
			get
			{
				return _chooseRecipientCommand = _chooseRecipientCommand ??
					new MvxCommand(() =>
					{
						ShowViewModel<ChooseRecipientViewModel>();
					});
			}
		}

		private MvxCommand _sendCommand;
		public IMvxCommand SendCommand
		{
			get
			{
				return _sendCommand = _sendCommand ??
					new MvxCommand(async () =>
					{
						MessageInfo = "Sending...";
						IsLoading = true;
						try
						{
							var result = await _transferService.SendTransfer(new TransferRequestVDTO
							{
								Amount = _amount,
								UserFromId = _authService.GetAuth().UserId,
								RecipientsIds = _recipients.Select(x => x.Id).ToArray()
							});
							
							if (result != null)
							{
								if (result.Success)
								{
									MessageInfo = "The operation has  been completed";
								}
								else
								{
									MessageInfo = "The operation has not been completed. " + result.Message;
								}
							}
							else
							{
								MessageInfo = "An error occurred during processing. Please try again.";
							}
						}
						catch (Exception)
						{
							MessageInfo = "An error occurred during processing. Please try again.";
						}
						IsLoading = false;


					}, CanSend);
			}
		}

		public bool CanSend()
		{
			if (IsLoading) return false;
			if (string.IsNullOrWhiteSpace(AmountInput)) return false;

			var isNumber = int.TryParse(AmountInput, out _amount);
			return isNumber && _amount > 0 && _recipients != null && _recipients.Any();
		}
	}
}