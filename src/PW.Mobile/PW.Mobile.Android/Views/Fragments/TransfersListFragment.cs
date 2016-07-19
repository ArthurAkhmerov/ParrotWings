using System.Collections.Generic;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using Android.Views;
using MvvmCross.Droid.Support.V4;
using MvvmCross.Droid.Support.V7.Fragging.Attributes;
using PW.Mobile.Core.ViewModels;

namespace PW.Mobile.Android.Views.Fragments
{
	[MvxFragment(typeof(MainViewModel), Resource.Id.frameLayout)]
	[Register("pw.mobile.android.views.fragments.TransfersListFragment")]
	public class TransfersListFragment :BaseFragment<TransfersListViewModel>
	{
		protected override int FragmentId => Resource.Layout.Fragment_TransfersList;

		public TransfersListFragment()
		{
			this.RetainInstance = true;
		}

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			var view = base.OnCreateView(inflater, container, savedInstanceState);

			var viewPager = view.FindViewById<ViewPager>(Resource.Id.viewpager);
			if (viewPager != null)
			{
				var fragments = new List<MvxFragmentPagerAdapter.FragmentInfo>
				{
					new MvxFragmentPagerAdapter.FragmentInfo("Incoming", typeof (TransfersIncomingListFragment),
						typeof (TransfersIncomingListViewModel)),
					new MvxFragmentPagerAdapter.FragmentInfo("Outcoming", typeof (TransfersOutcomingListFragment),
						typeof (TransfersOutcomingListViewModel)),
					new MvxFragmentPagerAdapter.FragmentInfo("All", typeof (TransfersAllListFragment),
						typeof (TransfersAllListViewModel)),
				};
				viewPager.Adapter = new MvxFragmentPagerAdapter(Activity, ChildFragmentManager, fragments);
			}

			var tabLayout = view.FindViewById<TabLayout>(Resource.Id.tabs);
			tabLayout.SetupWithViewPager(viewPager);

			return view;
		}
	}
}