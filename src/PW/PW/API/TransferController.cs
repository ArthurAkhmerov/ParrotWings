using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using PW.API.DTO;
using PW.Domain;
using PW.Domain.Repositories;

namespace PW.API
{
	public class TransferController: ApiController
	{
		private readonly ITransferRepository _transferRepository;
		private readonly IUserRepository _userRepository;


		public TransferController(ITransferRepository transferRepository, IUserRepository userRepository)
		{
			_transferRepository = transferRepository;
			_userRepository = userRepository;
		}

		public IHttpActionResult Get(Guid userId, DateTime from, DateTime to, int skip = 0, int take = 100)
		{
			try
			{
				var transfers = _transferRepository.ListBy(userId, from, to, skip, take);
				var usersIds = transfers
					.Select(x => x.UserFromId)
					.Concat(transfers.Select(x => x.UserToId))
					.Distinct();

				var users = _userRepository.ListByIds(usersIds.ToArray());

				var transfersVdtos = transfers.Select(x =>
					TransferVDTO.Create(x,
					users.First(y => y.Id == x.UserFromId),
					users.First(y => y.Id == x.UserToId)))
				.ToArray();

				return Ok(transfersVdtos);
			}
			catch (Exception ex)
			{
				return InternalServerError(ex);
			}
		}

		public IHttpActionResult Get(Guid userId, string usernameTo, DateTime from, DateTime to, int skip = 0, int take = 100)
		{
			try
			{
				var transfers = _transferRepository.ListBy(userId, usernameTo, from, to, skip, take);
				var usersIds = transfers
					.Select(x => x.UserFromId)
					.Concat(transfers.Select(x => x.UserToId))
					.Distinct();

				var users = _userRepository.ListByIds(usersIds.ToArray());

				var transfersVdtos = transfers.Select(x =>
					TransferVDTO.Create(x,
					users.First(y => y.Id == x.UserFromId),
					users.First(y => y.Id == x.UserToId)))
				.ToArray();

				return Ok(transfersVdtos);
			}
			catch (Exception ex)
			{
				return InternalServerError(ex);
			}
		}

		[HttpGet]
		public IHttpActionResult Count(Guid userId, DateTime from, DateTime to)
		{
			try
			{
				var count = _transferRepository.Count(userId, from, to);
				return Ok(count);
			}
			catch (Exception ex)
			{
				return InternalServerError(ex);
			}
		}

		[HttpGet]
		public IHttpActionResult Count(Guid userId, string usernameTo, DateTime from, DateTime to)
		{
			try
			{
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