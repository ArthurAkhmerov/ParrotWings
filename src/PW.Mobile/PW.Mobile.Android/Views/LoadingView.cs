using Android.App;
using Android.Content.PM;
using PW.Mobile.Core.ViewModels;

namespace PW.Mobile.Android.Views
{
	[Activity(Label = "Loading", LaunchMode = LaunchMode.SingleInstance, ScreenOrientation = ScreenOrientation.Portrait)]
	public class LoadingView: BaseActivity<LoadingViewModel>
	{
		protected override int LayoutResource => Resource.Layout.Page_Loading;
	}
}