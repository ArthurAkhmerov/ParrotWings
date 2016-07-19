using Android.App;
using Android.Content.PM;
using Android.Views;
using PW.Mobile.Core.ViewModels;

namespace PW.Mobile.Android.Views
{
	[Activity(Label = "", LaunchMode = LaunchMode.SingleInstance, ScreenOrientation = ScreenOrientation.Portrait)]
	public class ChooseRecipientView: BaseActivity<ChooseRecipientViewModel>
	{
		protected override int LayoutResource => Resource.Layout.Page_ChooseRecipient;

		protected override int ActionBarIcon => Resource.Drawable.ic_plus;

		public override bool OnCreateOptionsMenu(IMenu menu)
		{
			MenuInflater.Inflate(Resource.Menu.chooserecipient_menu, menu);

			return base.OnCreateOptionsMenu(menu);
		}

		public override bool OnOptionsItemSelected(IMenuItem item)
		{
			switch (item.ItemId)
			{
				case Resource.Id.сhoose_recipient_all:
					ViewModel.ChooseAllUsers();
					break;
				case Resource.Id.сhoose_recipient_none:
					ViewModel.ChooseNone();
					break;
				case Resource.Id.сhoose_recipient_ok:
					ViewModel.FinishCommand.Execute(null);
					break;
				case Resource.Id.сhoose_recipient_cancel:
					ViewModel.CancelCommand.Execute(null);
					break;
			}

			return base.OnOptionsItemSelected(item);
		}
	}
}