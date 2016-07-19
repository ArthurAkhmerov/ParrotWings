using Android.App;
using Android.Content.PM;
using PW.Mobile.Core.ViewModels;

namespace PW.Mobile.Android.Views
{
	[Activity(Label = "Sign in", LaunchMode = LaunchMode.SingleInstance, ScreenOrientation = ScreenOrientation.Portrait)]
	public class LoginView : BaseActivity<LoginViewModel>
	{
		protected override int LayoutResource => Resource.Layout.Page_Login;
	}
}