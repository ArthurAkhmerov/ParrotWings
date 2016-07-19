using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using PW.API.DTO;
using PW.Domain;
using PW.Domain.Repositories;

namespace PW.API
{
	public class UserController : ApiController
	{
		private readonly IUserRepository _userRepository;

		public UserController(IUserRepository userRepository)
		{
			_userRepository = userRepository;
		}

		public IHttpActionResult Get()
		{
			try
			{
				var users = _userRepository.List();
				var usersVdtos = users.Select(UserVDTO.Create).ToArray();

				return Ok(usersVdtos);
			}
			catch (Exception ex)
			{
				return InternalServerError(ex);
			}
		}

		public IHttpActionResult Get(Guid id)
		{
			try
			{
				var user = _userRepository.GetByKey(id);
				var usersVdto = UserVDTO.Create(user);

				return Ok(usersVdto);
			}
			catch (Exception ex)
			{
				return InternalServerError(ex);
			}
		}

		public IHttpActionResult Get(string searchText)
		{
			try
			{
				if(searchText == null) return BadRequest();

				var users = _userRepository.ListBySearchText(searchText);
				var usersVdtos = users.Select(UserVDTO.Create).ToArray();

				return Ok(usersVdtos);
			}
			catch (Exception ex)
			{
				return InternalServerError(ex);
			}
		}
	}
}