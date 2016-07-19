using System.Web.Mvc;
using PW.Domain.Repositories;

namespace PW.Controllers
{
	[Authorize]
	public class TransferController: Controller
	{
		public ActionResult Index()
		{
			return View();
		}
	}
}