using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Security;
using Microsoft.Owin.Security.OAuth;
using PW.Domain.Infrastructure;
using PW.Domain.Repositories;

namespace PW.Providers
{
	public class SimpleAuthorizationServerProvider : OAuthAuthorizationServerProvider
	{
		private readonly IUserRepository _userRepository;
		private readonly ISecurityProvider _securityProvider;

		public SimpleAuthorizationServerProvider(IUserRepository userRepository, ISecurityProvider securityProvider)
		{
			_userRepository = userRepository;
			_securityProvider = securityProvider;
		}

		public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
		{
			context.Validated();
		}

		public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
		{

			context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });

			var user = _userRepository.ListByEmail(context.UserName).FirstOrDefault();
			if (user == null)
			{
				context.SetError("invalid_grant", "The user name or password is incorrect.");
				return;
			}

			
			if (_securityProvider.CalculateMD5(context.Password, user.Salt) != user.HashedPassword)
			{
				context.SetError("invalid_grant", "The user name or password is incorrect.");
				return;
			}


			var identity = new ClaimsIdentity(context.Options.AuthenticationType);
			
			identity.AddClaim(new Claim("username", context.UserName));
			identity.AddClaim(new Claim("role", "user"));

			context.Validated(identity);

			FormsAuthentication.SetAuthCookie(user.Email, false);

		}
	}
}