using Android.OS;
using Android.Support.V7.Widget;
using Android.Views;
using PW.Mobile.Core;
using MvvmCross.Core.ViewModels;
using MvvmCross.Droid.Support.V7.AppCompat;


namespace PW.Mobile.Android.Views
{
	public abstract class BaseActivity<TViewModel> : MvxCachingFragmentCompatActivity<TViewModel> where TViewModel : class, IMvxViewModel
	{
		public new TViewModel ViewModel
		{
			get { return (TViewModel)base.ViewModel; }
			set { base.ViewModel = value; }
		}

		public Toolbar Toolbar
		{
			get;
			set;
		}
		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);
			SetContentView(LayoutResource);
			Toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
			if (Toolbar != null)
			{
				SetSupportActionBar(Toolbar);
				SupportActionBar.SetHomeAsUpIndicator(ActionBarIcon);
				SupportActionBar.SetDisplayHomeAsUpEnabled(true);
				SupportActionBar.SetHomeButtonEnabled(true);

			}
		}

		protected abstract int LayoutResource
		{
			get;
		}

		protected virtual int ActionBarIcon => Resource.Drawable.ic_menu_black_24dp;

		//protected virtual int ActionBarIcon
		//{
		//	set { Toolbar.SetNavigationIcon(value); }
		//}

		public override bool OnOptionsItemSelected(IMenuItem item)
		{
			var selectedItem = item.ItemId;

			if (selectedItem == AppConstants.ANDROID_HOME_BUTTON_ID)
			{
				Finish();
				return true;
			}

			return base.OnOptionsItemSelected(item);
		}
	}
}