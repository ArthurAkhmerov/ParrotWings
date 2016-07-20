

using System;
using System.Collections.ObjectModel;
using System.Linq;
using MvvmCross.Binding.Droid;
using MvvmCross.Core.ViewModels;
using MvvmCross.Plugins.Messenger;
using PW.Mobile.API;
using PW.Mobile.Core.Model;
using PW.Mobile.Core.Services;

namespace PW.Mobile.Core.ViewModels
{
	public class TransfersIncomingListViewModel : MvxViewModel
	{
		private readonly IMvxMessenger _messenger;
		private readonly ITransferService _transferService;
		private readonly IUserService _userService;

		private readonly MvxSubscriptionToken transferSentMessageToken;
		private readonly MvxSubscriptionToken dateChangedMessageToken;

		public TransfersIncomingListViewModel(IMvxMessenger messenger, ITransferService transferService, IUserService userService)
		{
			_messenger = messenger;
			_transferService = transferService;
			_userService = userService;
			IsChoosingByDate = true;
			Transfers = new ObservableCollection<TransferGroupViewModel>(transferService.GetTransferIncomingGroups());
			IsEmptyList = !Transfers.Any();

			transferSentMessageToken = _messenger.Subscribe<TransferSentMessage>((result) =>
			{

				var newTransfer = _transferService.CreateTransfer(result.Dto);
				if (newTransfer != null && newTransfer.IsIncoming)
				{
					InvokeOnMainThread(() =>
					{
						if (Transfers.Count == 0 || Transfers.First().Date.Date != DateTime.Now.Date)
						{
							Transfers.Insert(0, new TransferGroupViewModel(DateTime.Now, new Collection<Transfer>()));
						}

						var firstTransferGroup = Transfers.First().ToList();
						firstTransferGroup.Insert(0, newTransfer);
						Transfers[0] = new TransferGroupViewModel(DateTime.Now, firstTransferGroup);

						IsEmptyList = !Transfers.Any();
					});
				}
			});

			dateChangedMessageToken = _messenger.Subscribe<DateChangedMessage>(date =>
			{
				InvokeOnMainThread(() =>
				{
					Transfers.Clear();
					foreach (var g in transferService.GetTransferIncomingGroups())
						Transfers.Add(g);
					
					IsEmptyList = !Transfers.Any();
				});
			});
		}

		private bool _isEmptyList;
		public bool IsEmptyList
		{
			get { return _isEmptyList; }
			set
			{
				SetProperty(ref _isEmptyList, value);
			}
		}

		private bool _isChoosingByDate;
		public bool IsChoosingByDate
		{
			get { return _isChoosingByDate; }
			set
			{
				SetProperty(ref _isChoosingByDate, value);
			}
		}


		private ObservableCollection<TransferGroupViewModel> _transfers;
		public ObservableCollection<TransferGroupViewModel> Transfers
		{
			get { return _transfers; }
			set
			{
				_transfers = value;
				RaisePropertyChanged(() => Transfers);
			}
		}
	}
}