using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PW.Mobile.API.DTO;
using PW.Mobile.Core.Model;
using PW.Mobile.Core.ViewModels;

namespace PW.Mobile.Core.Services
{
	public interface ITransferService
	{
		IReadOnlyCollection<Transfer> GetTransfers();
		Task LoadTransfersAsync(DateTime from, DateTime to);
		Task<SendTransferResultVDTO> SendTransfer(TransferRequestVDTO dto);
		IReadOnlyCollection<Transfer> GetIncomingTransfers();
		IReadOnlyCollection<Transfer> GetOutcomingTransfers();
		Transfer CreateTransfer(TransferVDTO dto);
		IReadOnlyCollection<TransferGroupViewModel> GetTransferGroups();
		IReadOnlyCollection<TransferGroupViewModel> GetTransferIncomingGroups();
		IReadOnlyCollection<TransferGroupViewModel> GetTransferOutcomingGroups();
	}
}