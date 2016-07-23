using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using PW.API.DTO;
using PW.Domain;
using PW.Domain.Infrastructure;
using PW.Domain.Repositories;

namespace PW.Controllers
{
	[AllowAnonymous]
	public class AccountController : Controller
	{
		private readonly IUserRepository _userRepository;
		private readonly ISessionRepository _sessionRepository;
		private readonly ISecurityProvider _securityProvider;

		public AccountController(IUserRepository userRepository,ISessionRepository sessionRepository, ISecurityProvider securityProvider)
		{
			_userRepository = userRepository;
			_sessionRepository = sessionRepository;
			_securityProvider = securityProvider;
		}

		public ActionResult SignIn()
		{
			return View();
		}

		public ActionResult SignUp()
		{
			return View();
		}

		public ActionResult Signout()
		{
			FormsAuthentication.SignOut();
			return RedirectToAction("Signin", "Account");
		}
	}
}