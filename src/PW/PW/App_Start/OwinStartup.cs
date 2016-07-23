//using Microsoft.Owin;
//using Microsoft.Owin.Cors;
//using Ninject;
//using Ninject.Http;
//using Owin;

using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Tokens;
using System.Threading.Tasks;
using System.Web.Http;
using IdentityServer3.AccessTokenValidation;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.AspNet.SignalR.Infrastructure;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using Owin;
using PW.Domain.Infrastructure;
using PW.Domain.Repositories;
using PW.Persistence.Repositories;
using PW.Providers;

namespace PW
{

	
	public class OwinStartup
	{
		private NinjectSignalRDependencyResolver _resolver;

		public void Configuration(IAppBuilder app)
		{
			var kernel = NinjectHttpContainer.Kernel;
			_resolver = new NinjectSignalRDependencyResolver(kernel);
			ConfigureOAuth(app);
			ConfigureSignalR(app);
			
			NinjectHttpContainer.RegisterModules(NinjectHttpModules.Modules);
		}

		public void ConfigureSignalR(IAppBuilder app)
		{
			app.Map("/signalr", map =>
			{
				map.UseCors(CorsOptions.AllowAll);

				map.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions()
				{
					Provider = new QueryStringOAuthBearerProvider()
				});

				var hubConfiguration = new HubConfiguration
				{
					Resolver = _resolver,
				};
				map.RunSignalR(hubConfiguration);
			});
		}

		public void ConfigureOAuth(IAppBuilder app)
		{
			OAuthAuthorizationServerOptions OAuthServerOptions = new OAuthAuthorizationServerOptions()
			{
				AllowInsecureHttp = true,
				TokenEndpointPath = new PathString("/token"),
				AccessTokenExpireTimeSpan = TimeSpan.FromDays(1),
				Provider = new SimpleAuthorizationServerProvider(_resolver.Resolve<IUserRepository>(), _resolver.Resolve<ISecurityProvider>()),
			};

			// Token Generation
			app.UseOAuthAuthorizationServer(OAuthServerOptions);
			app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());

		}
	}
}