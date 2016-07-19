

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
	public class TransfersListViewModel: MvxViewModel
	{
		private readonly IMvxMessenger _messenger;
		private readonly ITransferService _transferService;
		private readonly MvxSubscriptionToken transferSentMessageToken;
		//public ObservableCollection<Transfer> Transfers { get; set; }

		public TransfersListViewModel(IMvxMessenger messenger, ITransferService transferService)
		{
			_messenger = messenger;
			_transferService = transferService;
			Transfers = new ObservableCollection<TransferGroupViewModel>(transferService.GetTransferGroups());
			//Transfers = new ObservableCollection<Transfer>(transferService.GetTransfers());

			transferSentMessageToken = _messenger.Subscribe<TransferSentMessage>((result) =>
			{
				var newTransfer = _transferService.CreateTransfer(result.Dto);
				if (newTransfer != null)
				{
					//Transfers.Add(newTransfer);
					InvokeOnMainThread(() =>
					{
						if (Transfers.Count == 0 || Transfers.Last().Date.Date != DateTime.Now.Date)
						{
							Transfers.Add(new TransferGroupViewModel(DateTime.Now, new Collection<Transfer>()));
						}

						var lastTransferGroup = Transfers.Last().ToList();
						lastTransferGroup.Add(newTransfer);
						Transfers[Transfers.Count - 1] = new TransferGroupViewModel(DateTime.Now, lastTransferGroup);

						//NewMessage?.Invoke(newMessage);
					});
				}
			});
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