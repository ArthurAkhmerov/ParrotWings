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
	public class InitController : ApiController
	{
		private readonly IUserRepository _userRepository;
		private readonly ITransferRepository _transferRepository;
		private readonly ISessionRepository _sessionRepository;
		private readonly ISecurityProvider _securityProvider;

		public InitController(IUserRepository userRepository, ITransferRepository transferRepository, ISessionRepository sessionRepository, ISecurityProvider securityProvider)
		{
			_userRepository = userRepository;
			_transferRepository = transferRepository;
			_sessionRepository = sessionRepository;
			_securityProvider = securityProvider;
		}

		public IHttpActionResult Get()
		{
			try
			{
				var existingUsers = _userRepository.List();
				if (!existingUsers.Any())
				{
					for (int i = 0; i < 20; i++)
					{
						var salt = _securityProvider.CreateSalt();
						var hashedPassword = _securityProvider.CalculateMD5($"username{i}", salt);
						var newUser = new User($"username{i}", $"username{i}@gmail.com", salt, hashedPassword, Role.User);
						_userRepository.SaveOrUpdate(newUser);
					}
				}

				return Ok();
			}
			catch (Exception ex)
			{
				return InternalServerError(ex);
			}
		}
	}
}