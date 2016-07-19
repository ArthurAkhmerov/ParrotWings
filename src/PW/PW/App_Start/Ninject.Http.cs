using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.Dependencies;
using System.Web.Mvc;
using JetBrains.Annotations;
using Ninject;
using Ninject.Modules;
using PW.Infrastructure;
using PW.Persistence;

namespace PW
{
	/// <summary>
	/// Resolves Dependencies Using Ninject
	/// </summary>
	public class NinjectHttpResolver : System.Web.Http.Dependencies.IDependencyResolver, System.Web.Mvc.IDependencyResolver
	{
		public IKernel Kernel { get; }
		public NinjectHttpResolver(params INinjectModule[] modules)
		{
			Kernel = new StandardKernel(modules);
		}

		public NinjectHttpResolver(Assembly assembly)
		{
			Kernel = new StandardKernel();
			Kernel.Load(assembly);
		}

		public object GetService(Type serviceType)
		{
			return Kernel.TryGet(serviceType);
		}

		public IEnumerable<object> GetServices(Type serviceType)
		{
			return Kernel.GetAll(serviceType);
		}

		public void Dispose()
		{
			//Do Nothing
		}

		public IDependencyScope BeginScope()
		{
			return this;
		}
	}


	// List and Describe Necessary HttpModules
	// This class is optional if you already Have NinjectMvc
	[UsedImplicitly]
	public class NinjectHttpModules
	{
		//Return Lists of Modules in the Application
		public static NinjectModule[] Modules
			=> new NinjectModule[]
			{
				new InfrastructureModule(),
				new PersistenceModule(),
				new Module(),
			};

		//Main Module For Application
		private class Module : NinjectModule
		{
			public override void Load()
			{
			}
		}
	}


	/// <summary>
	/// Its job is to Register Ninject Modules and Resolve Dependencies
	/// </summary>
	public static class NinjectHttpContainer
	{
		private static NinjectHttpResolver _resolver;
		public static IKernel Kernel => _resolver.Kernel;
		//Register Ninject Modules
		public static void RegisterModules(INinjectModule[] modules)
		{
			_resolver = new NinjectHttpResolver(modules);
			GlobalConfiguration.Configuration.DependencyResolver = _resolver;
			DependencyResolver.SetResolver(_resolver);
		}

		public static void RegisterAssembly()
		{
			_resolver = new NinjectHttpResolver(Assembly.GetExecutingAssembly());
			//This is where the actual hookup to the Web API Pipeline is done.
			GlobalConfiguration.Configuration.DependencyResolver = _resolver;
			DependencyResolver.SetResolver(_resolver);
		}

		//Manually Resolve Dependencies
		public static T Resolve<T>()
		{
			return _resolver.Kernel.Get<T>();
		}
	}
}