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
		public IHttpActionResult SignIn([FromBody] AuthRequestVDTO dto)
		{
			try
			{
				if (dto == null) return BadRequest("body content cannot be null or empty");

				var usersByEmail = _userRepository.ListByEmail(dto.Email);
				if (!usersByEmail.Any())
				{
					return Ok(new AuthResultVDTO
					{
						Success = false,
						Message = "The email and password you entered don't match."
					});
				}

				var user = usersByEmail.First();
				if (_securityProvider.CalculateMD5(dto.Password, user.Salt) != user.HashedPassword)
				{
					return Ok(new AuthResultVDTO
					{
						Success = false,
						Message = "The email and password you entered don't match."
					});
				}


				//FormsAuthentication.SetAuthCookie(user.Email, false);
				var session = new Session(user.Id);
				_sessionRepository.SaveOrUpdate(session);

				var authResult = new AuthResultVDTO
				{
					Data = new AuthData
					{
						UserId = user.Id,
						SessionId = session.Id,
					},
					Success = true
				};

			//	var client = new OAuth2Client(new Uri("http://localhost:58255/connect/token"),
			//	"socialnetwork", "secret");

			//	var requestResponse = client.RequestAccessTokenUserName(dto.Email, dto.Password,
			//		"openid profile offline_access");

			//	var claims = new[]
			//	{
			//	new Claim("access_token", requestResponse.AccessToken),
			//	new Claim("refresh_token", requestResponse.RefreshToken)
			//};

			//	var claimsIdentity = new ClaimsIdentity(claims,
			//		DefaultAuthenticationTypes.ApplicationCookie);

			//	Request.GetOwinContext().Authentication.SignIn(claimsIdentity);

				return Ok(authResult);
			}
			catch (Exception ex)
			{
				return InternalServerError(ex);
			}
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