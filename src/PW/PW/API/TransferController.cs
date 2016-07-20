using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using PW.API.DTO;
using PW.Domain;
using PW.Domain.Infrastructure;
using PW.Domain.Repositories;

namespace PW.API
{
	public class TransferController: ApiController
	{
		private readonly ITransferRepository _transferRepository;
		private readonly IUserRepository _userRepository;
		private readonly ISessionRepository _sessionRepository;
		private readonly ISecurityProvider _securityProvider;

		public TransferController(ITransferRepository transferRepository, IUserRepository userRepository, ISessionRepository sessionRepository, ISecurityProvider securityProvider)
		{
			_transferRepository = transferRepository;
			_userRepository = userRepository;
			_sessionRepository = sessionRepository;
			_securityProvider = securityProvider;
		}

		public IHttpActionResult Get(Guid userId, DateTime from, DateTime to, string hash, int skip = 0, int take = 100)
		{
			try
			{
				var sessions = _sessionRepository.ListByUser(userId);
				if (sessions.All(x => _securityProvider.CalculateMD5(userId, x.Id) != hash))
				{
					return Unauthorized();
				}

				var transfers = _transferRepository.ListBy(userId, from, to, skip, take);
				var transfersVdtos = transfers.Select(TransferVDTO.Create).ToArray();

				return Ok(transfersVdtos);
			}
			catch (Exception ex)
			{
				return InternalServerError(ex);
			}
		}

		public IHttpActionResult Get(Guid userId, string usernameTo, DateTime from, DateTime to, string hash, int skip = 0, int take = 100)
		{
			try
			{
				var sessions = _sessionRepository.ListByUser(userId);
				if (sessions.All(x => _securityProvider.CalculateMD5(userId, x.Id) != hash))
				{
					return Unauthorized();
				}

				var transfers = _transferRepository.ListBy(userId, usernameTo, from, to, skip, take);
				var transfersVdtos = transfers.Select(TransferVDTO.Create).ToArray();

				return Ok(transfersVdtos);
			}
			catch (Exception ex)
			{
				return InternalServerError(ex);
			}
		}

		[HttpGet]
		public IHttpActionResult Count(Guid userId, DateTime from, DateTime to, string hash)
		{
			try
			{
				var sessions = _sessionRepository.ListByUser(userId);
				if (sessions.All(x => _securityProvider.CalculateMD5(userId, x.Id) != hash))
				{
					return Unauthorized();
				}

				var count = _transferRepository.Count(userId, from, to);
				return Ok(count);
			}
			catch (Exception ex)
			{
				return InternalServerError(ex);
			}
		}

		[HttpGet]
		public IHttpActionResult Count(Guid userId, string usernameTo, DateTime from, DateTime to, string hash)
		{
			try
			{
				var sessions = _sessionRepository.ListByUser(userId);
				if (sessions.All(x => _securityProvider.CalculateMD5(userId, x.Id) != hash))
				{
					return Unauthorized();
				}

				var count = _transferRepository.Count(userId, usernameTo, from, to);
				return Ok(count);
			}
			catch (Exception ex)
			{
				return InternalServerError(ex);
			}
		}
		
	}
}