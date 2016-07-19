using MvvmCross.Platform;
using MvvmCross.Platform.IoC;
using PW.Mobile.Core.Services;
using PW.Mobile.Core.Services.Implementations;
using PW.Mobile.Core.ViewModels;

namespace PW.Mobile.Core
{
    public class App : MvvmCross.Core.ViewModels.MvxApplication
    {
        public override void Initialize()
        {
			base.Initialize();

			var authService = Mvx.Resolve<IAuthService>();
			if (authService.IsSignedIn())
	        {
		        RegisterAppStart<LoadingViewModel>();
	        }
	        else
	        {
		        RegisterAppStart<LoginViewModel>();
			}
        }
	}
}
