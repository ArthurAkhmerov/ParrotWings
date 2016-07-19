using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;
using PW.API.DTO;
using PW.Domain;
using PW.Domain.Exceptions;
using PW.Domain.Infrastructure;
using PW.Domain.Repositories;
using PW.Persistence.Exceptions;

namespace PW.Hubs
{
	public class PwClient
	{
		[JsonProperty("clientId")]
		public string ClientId { get; set; }

		[JsonProperty("userId")]
		public Guid UserId { get; set; }
	}

	public class PwHub: Hub
	{
		private readonly IUserRepository _userRepository;
		private readonly ITransferRepository _transferRepository;
		private readonly ISessionRepository _sessionRepository;
		private readonly ISecurityProvider _securityProvider;
		private static readonly IList<PwClient> ConnectedClientsList = new List<PwClient>();

		public PwHub(IUserRepository userRepository, ITransferRepository transferRepository, ISessionRepository sessionRepository,
			ISecurityProvider securityProvider)
		{
			_userRepository = userRepository;
			_transferRepository = transferRepository;
			_sessionRepository = sessionRepository;
			_securityProvider = securityProvider;
		}

		public SendTransferResultVDTO SendTransfer(TransferRequestVDTO dto, string hash)
		{
			try
			{
				var sessions = _sessionRepository.ListByUser(dto.UserFromId);

				if (sessions.All(x => _securityProvider.CalculateMD5(dto.UserFromId, dto.Amount, dto.RecipientsIds, x.Id) != hash))
				{
					return new SendTransferResultVDTO {Success = false};
				}

				var transfers = dto.RecipientsIds.Select(userToId => new Transfer(dto.UserFromId, userToId, dto.Amount)).ToArray();
				_transferRepository.MakeTransfers(transfers);

				foreach (var transfer in transfers)
				{
					var clients = ConnectedClientsList.Where(x => x.UserId == transfer.UserFromId || x.UserId == transfer.UserToId).ToList();

					var userFrom = this._userRepository.GetByKey(transfer.UserFromId);
					var userTo = this._userRepository.GetByKey(transfer.UserToId);
					var transferVdto = TransferVDTO.Create(transfer, userFrom, userTo);

					clients.ForEach(x => Clients.Client(x.ClientId).transferSent(transferVdto));
				}
			
				return new SendTransferResultVDTO {Success = true};
			}
			catch (InvalidBalanceException ex)
			{
				return new SendTransferResultVDTO {Success = false, Message = ex.Message};
			}
			catch (Exception ex)
			{
				return new SendTransferResultVDTO {Success = false, Message = ex.Message};
			}
		}

		public void JoinUser(PwClient dto, string hash)
		{
			try
			{
				var joinedUser = _userRepository.GetByKey(dto.UserId);
				var sessions = _sessionRepository.ListByUser(joinedUser.Id);

				if (sessions.All(x => _securityProvider.CalculateMD5(new { ClientId = dto.ClientId, UserId = dto.UserId }, x.Id) != hash
						&& _securityProvider.CalculateMD5(dto, x.Id) != hash))
					return;
				

				joinedUser.MarkLoggedIn();
				_userRepository.SaveOrUpdate(joinedUser);

				var clientId = Context.ConnectionId;
				var existingChatClient = ConnectedClientsList.FirstOrDefault(x => x.ClientId == clientId);
				if (existingChatClient != null)
					existingChatClient.UserId = joinedUser.Id;
				else
					ConnectedClientsList.Add(new PwClient() { UserId = joinedUser.Id, ClientId = clientId});

				var connectionIds = ConnectedClientsList
					.Select(x => x.ClientId).ToList();

				Clients.Clients(connectionIds)
					.userJoined(UserVDTO.Create(joinedUser));
			}
			catch (EntityNotFoundException)
			{
			}
			catch (Exception)
			{
			}
		}

		public void LeaveUser(UserVDTO dto, string hash)
		{
			var sessions = _sessionRepository.ListByUser(dto.Id);

			if (sessions.All(x => _securityProvider.CalculateMD5(dto, x.Id) != hash))
				return;

			LeaveUser(new PwClient() { UserId = dto.Id, ClientId = Context.ConnectionId});
		}

		public override Task OnDisconnected(bool stopCalled)
		{
			var disconnectedClient = ConnectedClientsList.FirstOrDefault(x => x.ClientId == Context.ConnectionId);
			if (disconnectedClient != null)
				LeaveUser(disconnectedClient);

			return base.OnDisconnected(stopCalled);
		}

		private void LeaveUser(PwClient client)
		{
			try
			{
				var userLeft = _userRepository.GetByKey(client.UserId);
				var clientsToRemove = ConnectedClientsList.Where(x => x.ClientId == client.ClientId).ToList();
				if (clientsToRemove.Any())
				{
					clientsToRemove.ForEach(x => ConnectedClientsList.Remove(x));
				}
				var clientsByUser = ConnectedClientsList.Where(x => x.UserId == userLeft.Id).ToList();
				if (!clientsByUser.Any())
				{
					userLeft.MarkLoggedIn(false);
					_userRepository.SaveOrUpdate(userLeft);
					Clients.All.userLeft(new UserVDTO { Id = client.UserId});
				}
			}
			catch (EntityNotFoundException)
			{
			}
			catch (Exception)
			{
				
			}
		}
	}
}