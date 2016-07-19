//using Microsoft.Owin;
//using Microsoft.Owin.Cors;
//using Ninject;
//using Ninject.Http;
//using Owin;

using System.Web.Http;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.AspNet.SignalR.Infrastructure;
using Owin;

namespace PW
{
	public class OwinStartup
	{
		

		public void Configuration(IAppBuilder app)
		{

			var kernel = NinjectHttpContainer.Kernel;
			var resolver = new NinjectSignalRDependencyResolver(kernel);

			//kernel.Bind(typeof(IHubConnectionContext<dynamic>)).ToMethod(context =>
			//		resolver.Resolve<IConnectionManager>().GetHubContext<ChatHub>().Clients
			//		 ).WhenInjectedInto<ITr>();

			var config = new HubConfiguration();
			config.Resolver = resolver;
			//app.UseCors(CorsOptions.AllowAll);
			app.MapSignalR(config);

			//GlobalConfiguration.Configuration
			//	.UseSqlServerStorage(ConfigurationManager.ConnectionStrings["chat"].ConnectionString);

			NinjectHttpContainer.RegisterModules(NinjectHttpModules.Modules);
			//GlobalConfiguration.Configuration.UseNinjectActivator(NinjectHttpContainer.Kernel);

			//NinjectHttpContainer.Kernel.Get<IUpdateUsersJob>().Start();

			//app.UseHangfireDashboard("/hangfire", new DashboardOptions
			//{
			//	AuthorizationFilters = new[] { new OwinAuthorizationFilter() }
			//});
			//app.UseHangfireServer();


		}
	}
}