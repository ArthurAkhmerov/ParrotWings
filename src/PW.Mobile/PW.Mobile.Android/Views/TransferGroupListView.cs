using System;
using System.Collections;
using System.Windows.Input;
using Android.Content;
using Android.Util;
using Android.Widget;
using MvvmCross.Binding.Attributes;
using MvvmCross.Binding.Droid.ResourceHelpers;
using MvvmCross.Binding.Droid.Views;
using MvvmCross.Platform;
using MvvmCross.Platform.Droid;
using PW.Mobile.Android.Views.Adapters;

namespace PW.Mobile.Android.Views
{
	public class TransferGroupListView : MvxListView
	{
		public TransferGroupListView(Context context, IAttributeSet attrs)
			: this(context, attrs, new TransferGroupListAdapter(context))
		{
		}

		public TransferGroupListView(Context context, IAttributeSet attrs, TransferGroupListAdapter adapter)
			: base(context, attrs, adapter)
		{
			adapter.GroupTemplateId = Resource.Layout.ListItem_TransferGroup;
		}

		public ICommand GroupClick { get; set; }

		protected override void ExecuteCommandOnItem(ICommand command, int position)
		{
			var item = Adapter.GetRawItem(position);
			if (item == null)
				return;
			var flatItem = (TransferGroupListAdapter.FlatItem)item;

			if (flatItem.IsGroup)
				command = GroupClick;

			if (command == null)
				return;

			if (!command.CanExecute(flatItem.Item))
				return;

			command.Execute(flatItem.Item);
		}
	}

}