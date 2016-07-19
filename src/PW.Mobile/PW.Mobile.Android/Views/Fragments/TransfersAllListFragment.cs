using Android.OS;
using Android.Runtime;
using Android.Views;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Droid.Support.V7.Fragging.Attributes;
using MvvmCross.Droid.Support.V7.Fragging.Fragments;
using PW.Mobile.Core.ViewModels;

namespace PW.Mobile.Android.Views.Fragments
{
	[MvxFragment(typeof(MainViewModel), Resource.Id.frameLayout)]
	[Register("pw.mobile.android.views.fragments.TransfersAllListFragment")]
	public class TransfersAllListFragment : BaseFragment<TransfersAllListViewModel>
	{
		protected override int FragmentId => Resource.Layout.Fragment_TransfersAllList;
	}
}