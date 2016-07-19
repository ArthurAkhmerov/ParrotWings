using System;
using System.Collections.ObjectModel;
using System.Linq;
using MvvmCross.Core.ViewModels;
using MvvmCross.Plugins.Messenger;
using PW.Mobile.API;
using PW.Mobile.Core.Model;
using PW.Mobile.Core.Services;

namespace PW.Mobile.Core.ViewModels
{
	public class TransfersAllListViewModel : MvxViewModel
	{
		private readonly IMvxMessenger _messenger;
		private readonly ITransferService _transferService;
		private readonly MvxSubscriptionToken transferSentMessageToken;
		private readonly MvxSubscriptionToken dateChangedMessageToken;

		public TransfersAllListViewModel(IMvxMessenger messenger, ITransferService transferService)
		{
			_messenger = messenger;
			_transferService = transferService;
			Transfers = new ObservableCollection<TransferGroupViewModel>(transferService.GetTransferGroups());
			IsEmptyList = !Transfers.Any();

			transferSentMessageToken = _messenger.Subscribe<TransferSentMessage>((result) =>
			{
				var newTransfer = _transferService.CreateTransfer(result.Dto);
				if (newTransfer != null)
				{
					InvokeOnMainThread(() =>
					{
						if (Transfers.Count == 0 || Transfers.Last().Date.Date != DateTime.Now.Date)
						{
							Transfers.Add(new TransferGroupViewModel(DateTime.Now, new Collection<Transfer>()));
						}

						var lastTransferGroup = Transfers.Last().ToList();
						lastTransferGroup.Add(newTransfer);
						Transfers[Transfers.Count - 1] = new TransferGroupViewModel(DateTime.Now, lastTransferGroup);

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