using Android.App;
using Android.Content.PM;
using PW.Mobile.Core.ViewModels;

namespace PW.Mobile.Android.Views
{
	[Activity(Label = "Sign up", LaunchMode = LaunchMode.SingleInstance, ScreenOrientation = ScreenOrientation.Portrait)]
	public class SignUpView : BaseActivity<SignupViewModel>
	{
		protected override int LayoutResource => Resource.Layout.Page_SignUp;
	}
}