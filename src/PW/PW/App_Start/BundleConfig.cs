using System.Web.Optimization;

namespace PW
{
	public class BundleConfig
	{
		// For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
		public static void RegisterBundles(BundleCollection bundles)
		{
			bundles.Add(new StyleBundle("~/Content/css").Include(
					  "~/Content/bootstrap.css",
					  "~/Content/site.css"));

			bundles.Add(new StyleBundle("~/pw-transfers/").Include(
					  "~/Content/toastr.css",
					  "~/Content/moment-datepicker/datepicker.css",
					  "~/Content/daterangepicker.css",
					  "~/Client/Transfers/makeTransferModal.css"
					  ));

			var pwScripts = new ScriptBundle("~/pw-scripts/");
			IncludeLibs(pwScripts);
			pwScripts
				// Shared
				.Include("~/Scripts/jquery.signalR-2.2.0.min.js")
				.Include("~/signalr/hubs")
				.Include("~/Client/Shared/pwModule.js")
				.Include("~/Client/Shared/pwConfig.js")
				.Include("~/Client/Shared/notificationService.js")
				.Include("~/Client/Shared/securityProvider.js")
				.Include("~/Client/Shared/pwApiClient.js")
				.Include("~/Client/Shared/pwHubClient.js")
				.Include("~/Client/Shared/authService.js")
				//
				.Include("~/Client/Transfers/userService.js")
				.Include("~/Client/Transfers/transferService.js")
				.Include("~/Client/Account/accountController.js")
				.Include("~/Client/Transfers/transfersController.js")
				.Include("~/Client/Transfers/makeTransferModalController.js");

			bundles.Add(pwScripts);
		}

		private static void IncludeLibs(ScriptBundle scripts)
		{
			scripts
				.Include("~/Scripts/jquery-1.10.2.js")
				.Include("~/Scripts/bootstrap.min.js")
				.Include("~/Scripts/moment.js")
				.Include("~/Scripts/modernizr-2.6.2.js")
				.Include("~/Scripts/lodash.js")
				.Include("~/Scripts/toastr.js")
				.Include("~/Scripts/md5.min.js")
				.Include("~/Scripts/angular.js")
				.Include("~/Scripts/angular-sanitize.min.js")
				.Include("~/Scripts/angular-ui-router.js")
				.Include("~/Scripts/angular-animate.min.js")
				.Include("~/Scripts/angular-cookies.js")
				.Include("~/Scripts/ui-bootstrap-tpls.js")
				.Include("~/Client/Shared/daterangepicker.js")
				.Include("~/Scripts/angular-daterangepicker.js");

		}
	}
}