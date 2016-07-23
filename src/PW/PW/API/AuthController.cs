using System;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Net.Http;
using System.Web.Http;
using System.Web.Security;
using Microsoft.AspNet.Identity;
using PW.API.DTO;
using PW.Domain;
using PW.Domain.Infrastructure;
using PW.Domain.Repositories;
using Thinktecture.IdentityModel.Clients;
using Microsoft.Owin.Security;
using System.Web.Http.Owin;

namespace PW.API
{
	public class AuthController : ApiController
	{
		private readonly IUserRepository _userRepository;
		private readonly ISessionRepository _sessionRepository;
		private readonly ISecurityProvider _securityProvider;

		public AuthController(IUserRepository userRepository, ISessionRepository sessionRepository, ISecurityProvider securityProvider)
		{
			_userRepository = userRepository;
			_sessionRepository = sessionRepository;
			_securityProvider = securityProvider;
		}

		[System.Web.Mvc.HttpPost]
		public IHttpActionResult SignUp([FromBody] SignupRequestVDTO dto)
		{
			try
			{
				if (dto == null) return BadRequest("body content cannot be null or empty");
				if (string.IsNullOrWhiteSpace(dto.Email) || !Validator.EmailIsValid(dto.Email)) return BadRequest("email address is not valid");
				if (string.IsNullOrWhiteSpace(dto.Name)) return BadRequest("name cannot be empty");
				if (string.IsNullOrWhiteSpace(dto.Password)) return BadRequest("password cannot be empty");

				var usersByEmail = _userRepository.ListByEmail(dto.Email);
				if (usersByEmail.Any())
				{
					return Ok(new AuthResultVDTO
					{
						Success = false,
						Message = "The email address is already taken. Please choose another one.."
					});
				}

				var salt = _securityProvider.CreateSalt();
				var hashedPassword = _securityProvider.CalculateMD5(dto.Password, salt);
				var newUser = new User(dto.Name, dto.Email, salt, hashedPassword, Role.User);
				_userRepository.SaveOrUpdate(newUser);
				
				var authResult = new AuthResultVDTO
				{
					Data = null,
					Success = true
				};

				return Ok(authResult);
			}
			catch (Exception ex)
			{
				return InternalServerError(ex);
			}
		}
	}
}