using System;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using Android.Support.V4.Widget;
using Android.Views;
using PW.Mobile.Core;
using PW.Mobile.Core.ViewModels;

namespace PW.Mobile.Android.Views
{
	[Activity(LaunchMode = LaunchMode.SingleInstance, Icon = "@drawable/icon", ScreenOrientation = ScreenOrientation.Portrait)]
	public class MainView : BaseActivity<MainViewModel>
	{
		protected override int LayoutResource => Resource.Layout.Page_Main;

		private IMenu menu;

		DrawerLayout _drawerLayout;
		NavigationView _navigationView;

		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);

			_drawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
			_navigationView = FindViewById<NavigationView>(Resource.Id.nav_view);

			_navigationView.NavigationItemSelected += (sender, e) =>
			{
				e.MenuItem.SetChecked(true);
				switch (e.MenuItem.ItemId)
				{
					case Resource.Id.nav_users:
						Toolbar.Title = Resources.GetString(Resource.String.users);
						ShowFragmentAt(0);
						break;
					case Resource.Id.nav_transfers:
						Toolbar.Title = Resources.GetString(Resource.String.transfers);
						ShowFragmentAt(1);
						break;
					case Resource.Id.nav_create_transfer:
						Toolbar.Title = Resources.GetString(Resource.String.create_transfer);
						ShowFragmentAt(2);
						break;

				}
				_drawerLayout.CloseDrawers();
			};

			ShowFragmentAt(1);
		}

		void ShowFragmentAt(int position)
		{
			ViewModel.NavigateTo(position);
			Title = ViewModel.MenuItems[position].DisplayName;

			_drawerLayout.CloseDrawers();

			if (menu != null)
			{
				if (position == 1)
				{
					menu.FindItem(Resource.Id.transfers_date_this_week).SetVisible(true);
					menu.FindItem(Resource.Id.transfers_this_month).SetVisible(true);
					menu.FindItem(Resource.Id.transfers_two_month_ago).SetVisible(true);
					menu.FindItem(Resource.Id.transfers_three_months_ago).SetVisible(true);
				
				}
				else
				{
					menu.FindItem(Resource.Id.transfers_date_this_week).SetVisible(false);
					menu.FindItem(Resource.Id.transfers_this_month).SetVisible(false);
					menu.FindItem(Resource.Id.transfers_two_month_ago).SetVisible(false);
					menu.FindItem(Resource.Id.transfers_three_months_ago).SetVisible(false);
				}
			}
		}

		public override bool OnCreateOptionsMenu(IMenu menu)
		{
			this.menu = menu;

			MenuInflater.Inflate(Resource.Menu.transfers_date_menu, menu);

			return base.OnCreateOptionsMenu(menu);
		}

		public override bool OnOptionsItemSelected(IMenuItem item)
		{
			DateTime from;
			DateTime to;
			var today = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0).AddDays(1);

			switch (item.ItemId)
			{
				case AppConstants.ANDROID_HOME_BUTTON_ID:
					_drawerLayout.OpenDrawer(GravityCompat.Start);
					return true;
				case Resource.Id.transfers_date_this_week:
					from = today.AddDays(-7);
					to = today;
					ViewModel.ChangeDate(from, to);
					return true;
				case Resource.Id.transfers_this_month:
					from = today.AddMonths(-1);
					to = today;
					ViewModel.ChangeDate(from, to);
					return true;
				case Resource.Id.transfers_two_month_ago:
					from = today.AddMonths(-2);
					to = today;
					ViewModel.ChangeDate(from, to);
					return true;
				case Resource.Id.transfers_three_months_ago:
					from = today.AddMonths(-3);
					to = today;
					ViewModel.ChangeDate(from, to);
					return true;

			}

			return base.OnOptionsItemSelected(item);
		}
	}
}