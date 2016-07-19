using System.Web.Mvc;

namespace PW.Controllers
{
	[Authorize]
	public class HomeController: Controller
	{
		public ActionResult Index()
		{
			return View();
		}
	}
}