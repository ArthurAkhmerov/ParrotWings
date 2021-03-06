﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Client;
using MvvmCross.Plugins.Messenger;
using Newtonsoft.Json;
using PW.Mobile.API.DTO;
using PW.Mobile.Infrastructure.Services;

namespace PW.Mobile.API
{
	public class TransferSentMessage : MvxMessage
	{
		public TransferSentMessage(object sender, TransferVDTO dto) : base(sender)
		{
			Dto = dto;
		}

		public TransferVDTO Dto { get; private set; }
	}

	public class BalanceChangedMessage : MvxMessage
	{
		public BalanceChangedMessage(object sender, int balance) : base(sender)
		{
			Balance = balance;
		}

		public int Balance { get; private set; }
	}

	public class PwClient
	{
		[JsonProperty("clientId")]
		public string ClientId { get; set; }

		[JsonProperty("userId")]
		public Guid UserId { get; set; }
	}

	public delegate void TransferSentEventHandler(TransferVDTO dto);

	public delegate void UserJoinedEventHandler(UserVDTO user);
	public delegate void UserLeavedEventHandler(UserVDTO user);

	public interface IPwHub
	{
		event TransferSentEventHandler TransferSent;
		event UserJoinedEventHandler UserJoined;
		event UserLeavedEventHandler UserLeft;

		Task<SendTransferResultVDTO> SendTransferAsync(TransferRequestVDTO dto);
		Task JoinUserAsync(Guid userId, string token);
		Task LeaveUserAsync(Guid userId);
	}

	public class PwHub : IPwHub
	{
		private readonly HubConnection _hubConnection;
		private readonly IHubProxy _hubProxy;
		private readonly IMvxMessenger _messenger;
		private readonly ISecurityProvider _securityProvider;

		public PwHub(IMvxMessenger messenger, ISecurityProvider securityProvider, string baseAddress)
		{

			_messenger = messenger;
			_securityProvider = securityProvider;

			_hubConnection = new HubConnection(baseAddress);
			_hubProxy = _hubConnection.CreateHubProxy("PwHub");

			_hubProxy.On<TransferVDTO>("transferSent", dto =>
			{
				_messenger.Publish(new TransferSentMessage(this, dto));
			});

			_hubProxy.On<UserVDTO>("userJoined", dto =>
					UserJoined?.Invoke(dto));

			_hubProxy.On<UserVDTO>("userLeft", dto =>
				UserLeft?.Invoke(dto));
		}

		public event TransferSentEventHandler TransferSent;
		public event UserJoinedEventHandler UserJoined;
		public event UserLeavedEventHandler UserLeft;

		public async Task<SendTransferResultVDTO> SendTransferAsync(TransferRequestVDTO dto)
		{
			try
			{
				return await _hubProxy.Invoke<SendTransferResultVDTO>("SendTransfer", dto);
			}
			catch (Exception ex)
			{
				throw;
			}

		}

		public async Task JoinUserAsync(Guid userId, string token)
		{
			_hubConnection.Headers.Add("access_token", token);
			await _hubConnection.Start();

			var dto = new PwClient
			{
				ClientId = "",
				UserId = userId,
			};

			await _hubProxy.Invoke("JoinUser", dto);
		}

		

		public async Task LeaveUserAsync(Guid userId)
		{
			var dto = new UserVDTO { Id = userId };

			await _hubProxy.Invoke("LeaveUser", dto);
		}

	}
}