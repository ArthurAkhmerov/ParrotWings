using Android.Runtime;
using MvvmCross.Droid.Support.V7.Fragging.Attributes;
using PW.Mobile.Core.ViewModels;

namespace PW.Mobile.Android.Views.Fragments
{
	[MvxFragment(typeof(MainViewModel), Resource.Id.frameLayout)]
	[Register("pw.mobile.android.views.fragments.CreateTransferFragment")]
	public class CreateTransferFragment: BaseFragment<CreateTransferViewModel>
	{
		protected override int FragmentId => Resource.Layout.Fragment_CreateTransfer;


	}
}