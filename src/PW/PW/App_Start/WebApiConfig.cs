using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Web.Http;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace PW
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

			config.Routes.MapHttpRoute(
			  name: "TransferCount",
			  routeTemplate: "api/transfer/count",
			  defaults: new { controller = "transfer", action = "count" }
			  );

			config.Routes.MapHttpRoute(
			  name: "Transfers",
			  routeTemplate: "api/transfer",
			  defaults: new { controller = "transfer", action = "get" }
			  );

			config.Routes.MapHttpRoute(
			  name: "SignUp",
			  routeTemplate: "api/auth/signup",
			  defaults: new { controller = "auth", action = "signup" }
			  );

			config.Routes.MapHttpRoute(
			  name: "Auth",
			  routeTemplate: "api/auth",
			  defaults: new { controller = "auth", action = "signin" }
			  );

			

			config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

			config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));
			config.Formatters.JsonFormatter.SerializerSettings.Converters.Add(new StringEnumConverter());
			var jsonFormatter = config.Formatters.OfType<JsonMediaTypeFormatter>().First();
			jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
		}
    }
}
