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

		//[HttpPost]
		//public ActionResult Signin(AuthRequestVDTO dto)
		//{
		//	var usersByEmail = _userRepository.ListByEmail(dto.Email);

		//	if (!usersByEmail.Any())
		//	{
		//		ViewBag.ErrorMessage = "The email and password you entered don't match.";
		//		return View();
		//	}
		//	var user = usersByEmail.First();

		//	if (_securityProvider.CalculateMD5(dto.Password, user.Salt) == user.HashedPassword)
		//	{
		//		var session = new Session(user.Id);
		//		_sessionRepository.SaveOrUpdate(session);
		//		FormsAuthentication.SetAuthCookie(user.Email, false);
		//		return RedirectToAction("Index", "Home");
		//	}

		//	ViewBag.ErrorMessage = "The email and password you entered don't match.";

		//	return View();
		//}


	}
}