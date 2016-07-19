using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MvvmCross.Plugins.Messenger;
using PW.Mobile.API;
using PW.Mobile.API.DTO;
using PW.Mobile.Core.Model;
using PW.Mobile.Core.ViewModels;

namespace PW.Mobile.Core.Services.Implementations
{
	public class TransferService : ITransferService
	{
		private readonly IMvxMessenger _messenger;
		private readonly IPwApiClient _pwApiClient;
		private readonly IPwHub _pwHub;
		private readonly IAuthService _authService;
		private readonly IUserService _userService;
		private readonly IList<Transfer> _transfers;

		private readonly MvxSubscriptionToken transferSentMessageToken;

		public Transfer CreateTransfer(TransferVDTO dto)
		{
			var isIncoming = IsIncoming(dto);
			var isOutcoming = IsOutcoming(dto);
			if (isIncoming)
			{
				var userFrom = _userService.GetUsers().FirstOrDefault(x => x.Id == dto.UserFromId);
				var userTo = _userService.GetUsers().FirstOrDefault(x => x.Id == dto.UserToId);

				DateTime convertedDate = DateTime.SpecifyKind(
					dto.CreatedAt,
					DateTimeKind.Utc);

				var transfer = new Transfer(dto.Id, userFrom, userTo, dto.Amount, convertedDate.ToLocalTime(), true);
				return transfer;
			}

			if (isOutcoming)
			{
				var userFrom = _userService.GetUsers().FirstOrDefault(x => x.Id == dto.UserFromId);
				var userTo = _userService.GetUsers().FirstOrDefault(x => x.Id == dto.UserToId);
				DateTime convertedDate = DateTime.SpecifyKind(
					dto.CreatedAt,
					DateTimeKind.Utc);

				var transfer = new Transfer(dto.Id, userFrom, userTo, dto.Amount, convertedDate.ToLocalTime(), false);
				return transfer;
			}
			return null;
		}

		public IReadOnlyCollection<TransferGroupViewModel> GetTransferGroups()
		{
			return _transfers.OrderByDescending(x => x.CreatedAt)
			.GroupBy(x => x.CreatedAt.Date)
			.OrderByDescending(x => x.Key)
			.Select(x => new TransferGroupViewModel(x.Key, x.ToList()))
			.ToList();
		}

		public IReadOnlyCollection<TransferGroupViewModel> GetTransferIncomingGroups()
		{
			return _transfers.Where(x => x.IsIncoming).OrderByDescending(x => x.CreatedAt)
			.GroupBy(x => x.CreatedAt.Date)
			.OrderByDescending(x => x.Key)
			.Select(x => new TransferGroupViewModel(x.Key, x.ToList()))
			.ToList();
		}

		public IReadOnlyCollection<TransferGroupViewModel> GetTransferOutcomingGroups()
		{
			return _transfers.Where(x => !x.IsIncoming).OrderByDescending(x => x.CreatedAt)
			.GroupBy(x => x.CreatedAt.Date)
			.OrderByDescending(x => x.Key)
			.Select(x => new TransferGroupViewModel(x.Key, x.ToList()))
			.ToList();
		}

		public TransferService(IMvxMessenger messenger, IPwApiClient pwApiClient, IPwHub pwHub, IAuthService authService, IUserService userService)
		{
			_messenger = messenger;
			_pwApiClient = pwApiClient;
			_pwHub = pwHub;
			_authService = authService;
			_userService = userService;
			_transfers = new List<Transfer>();

			transferSentMessageToken = messenger.Subscribe<TransferSentMessage>((result) =>
			{
				var transfer = CreateTransfer(result.Dto);
				if (transfer != null)
				{
					if (transfer.IsIncoming)
					{
						_transfers.Add(transfer);
						_userService.ChangeBalance(transfer.Amount);
					}

					if (!transfer.IsIncoming)
					{
						_transfers.Add(transfer);
						_userService.ChangeBalance(-transfer.Amount);
					}
				}
			});
		}

		private bool IsIncoming(TransferVDTO dto)
		{
			return dto.UserToId == _userService.GetCurrentUser().Id;
		}

		private bool IsOutcoming(TransferVDTO dto)
		{
			return dto.UserFromId == _userService.GetCurrentUser().Id;
		}

		public IReadOnlyCollection<Transfer> GetTransfers()
		{
			return _transfers.ToArray();
		}

		public IReadOnlyCollection<Transfer> GetIncomingTransfers()
		{
			return _transfers
				.Where(x => x.UserTo.Id == _authService.GetAuth().UserId)
				.ToArray();
		}

		public IReadOnlyCollection<Transfer> GetOutcomingTransfers()
		{
			return _transfers
				.Where(x => x.UserFrom.Id == _authService.GetAuth().UserId)
				.ToArray();
		}

		public async Task LoadTransfersAsync(DateTime from, DateTime to)
		{
			var currentUser = this._userService.GetCurrentUser();
			var dtos = await _pwApiClient.GetTransfersAsync(currentUser.Id, from, to);

			_transfers.Clear();

			foreach (var dto in dtos)
			{
				var transfer = CreateTransfer(dto);
				if (transfer != null) _transfers.Add(transfer);
			}
		}

		public async Task<SendTransferResultVDTO> SendTransfer(TransferRequestVDTO dto)
		{
			var auth = _authService.GetAuth();
			if (auth == null) return new SendTransferResultVDTO { Success = false };

			return await _pwHub.SendTransferAsync(dto, auth.SessionId);
		}
	}
}