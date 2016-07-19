using System.Collections.Generic;
using System.Reflection;
using Android.Content;
using MvvmCross.Droid.Platform;
using MvvmCross.Core.ViewModels;
using MvvmCross.Droid.Support.V7.Fragging.Presenter;
using MvvmCross.Droid.Views;
using MvvmCross.Platform;
using MvvmCross.Platform.Platform;
using MvvmCross.Plugins.Messenger;
using PW.Mobile.API;
using PW.Mobile.Core;
using PW.Mobile.Core.Services;
using PW.Mobile.Core.Services.Implementations;
using PW.Mobile.Infrastructure.Services;
using PW.Mobile.Infrastructure.Services.Implementations;

namespace PW.Mobile.Android
{
    public class Setup : MvxAndroidSetup
    {
        public Setup(Context applicationContext) : base(applicationContext)
        {
        }

        protected override IMvxApplication CreateApp()
        {
			SetupIoC();
            return new Core.App();
        }

        protected override IMvxTrace CreateDebugTrace()
        {
            return new DebugTrace();
        }

		protected override IEnumerable<Assembly> AndroidViewAssemblies => new List<Assembly>(base.AndroidViewAssemblies)
		{
			typeof(global::Android.Support.V7.Widget.Toolbar).Assembly,
		};

		protected override IMvxAndroidViewPresenter CreateViewPresenter()
		{
			var mvxFragmentsPresenter = new MvxFragmentsPresenter(AndroidViewAssemblies);
			Mvx.RegisterSingleton<IMvxAndroidViewPresenter>(mvxFragmentsPresenter);
			return mvxFragmentsPresenter;
		}

		private void SetupIoC()
		{
			var securityProvider = new SecurityProvider();
			Mvx.RegisterSingleton<ISecurityProvider>(securityProvider);
			Mvx.RegisterSingleton<ISettingsProvider>(new SettingsProvider());
			Mvx.RegisterType<IPwApiClient>(() => new PwApiClient(AppConstants.SERVER_ADDRESS));
			var pwHub = new PwHub(Mvx.Resolve<IMvxMessenger>(), securityProvider, AppConstants.SERVER_ADDRESS);
			Mvx.RegisterSingleton<IPwHub>(pwHub);
			Mvx.RegisterType<IAuthService>(() => Mvx.IocConstruct<AuthService>());
			Mvx.RegisterSingleton<IUserService>(Mvx.IocConstruct<UserService>());
			Mvx.RegisterSingleton<ITransferService>(Mvx.IocConstruct<TransferService>());

			//var userService = new UserService();
			//var fileService = new FileService();
			//Mvx.RegisterSingleton<IFileService>(fileService);

			



			//var chatApiClient = new ChatApiClient(fileService, securityProvider, AppConstants.SERVER_ADDRESS);
			//Mvx.RegisterSingleton<IChatApiClient>(chatApiClient);

		

			//var messageService = Mvx.IocConstruct<MessageService>();
			//Mvx.RegisterSingleton<IMessageService>(messageService);

			//var channelService = Mvx.IocConstruct<ChannelService>();
			//Mvx.RegisterSingleton<IChannelService>(channelService);

			//var userService = Mvx.IocConstruct<UserService>();
			//Mvx.RegisterSingleton<IUserService>(userService);
		}
	}
}
