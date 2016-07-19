using Ninject.Modules;
using PW.Domain.Infrastructure;

namespace PW.Infrastructure
{
	public class InfrastructureModule : NinjectModule
	{
		public override void Load()
		{
			Bind<ISecurityProvider>().To<SecurityProvider>().InSingletonScope();
		}
	}
}