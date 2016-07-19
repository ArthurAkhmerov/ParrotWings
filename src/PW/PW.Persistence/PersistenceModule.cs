using System.Configuration;
using Ninject.Modules;
using PW.Domain.Repositories;
using PW.Persistence.Factories;
using PW.Persistence.Repositories;

namespace PW.Persistence
{
	public class PersistenceModule: NinjectModule
	{
		public override void Load()
		{
			BindRepositoryAndFactory<IUserRepository, UserRepository, UserFactory>();
			BindRepositoryAndFactory<ITransferRepository, TransferRepository, TransferFactory>();
			BindRepositoryAndFactory<ISessionRepository, SessionRepository, SessionFactory>();
		}

		private void BindRepositoryAndFactory<TRepositorInterface, TRepository, TFactory>() where TRepository : TRepositorInterface
		{
			Bind<TFactory>().ToSelf().InSingletonScope();
			Bind<TRepositorInterface>().To<TRepository>().InSingletonScope()
				.WithConstructorArgument("connectionString", ConfigurationManager.ConnectionStrings["pw"].ConnectionString);
		}
	}
}