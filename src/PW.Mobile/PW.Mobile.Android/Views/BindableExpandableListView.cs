﻿using System;
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

namespace PW.Mobile.Android.Views
{
	public class BindableGroupListView : MvxListView
	{
		public BindableGroupListView(Context context, IAttributeSet attrs)
			: this(context, attrs, new BindableGroupListAdapter(context))
		{
		}

		public BindableGroupListView(Context context, IAttributeSet attrs, BindableGroupListAdapter adapter)
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
			var flatItem = (BindableGroupListAdapter.FlatItem)item;

			if (flatItem.IsGroup)
				command = GroupClick;

			if (command == null)
				return;

			if (!command.CanExecute(flatItem.Item))
				return;

			command.Execute(flatItem.Item);
		}
	}

	public class BindableExpandableListView : ExpandableListView
	{
		public BindableExpandableListView(Context context, IAttributeSet attrs)
			: this(context, attrs, new BindableExpandableListAdapter(context))
		{
		}

		public BindableExpandableListView(Context context, IAttributeSet attrs, BindableExpandableListAdapter adapter)
			: base(context, attrs)
		{
			var groupTemplateId = MvxAttributeHelpers.ReadAttributeValue(context, attrs,
																		 MvxAndroidBindingResource.Instance
																								  .ListViewStylableGroupId,
																		 AndroidBindingResource.Instance
																							   .BindableListGroupItemTemplateId);

			var itemTemplateId = MvxAttributeHelpers.ReadListItemTemplateId(context, attrs);
			SetAdapter(adapter);
			adapter.GroupTemplateId = groupTemplateId;
			adapter.ItemTemplateId = itemTemplateId;
		}

		// An expandableListView has ExpandableListAdapter as propertyname, but Adapter still exists but is always null.
		protected BindableExpandableListAdapter ThisAdapter
		{
			get { return ExpandableListAdapter as BindableExpandableListAdapter; }
		}



		private IEnumerable _itemsSource;

		[MvxSetToNullAfterBinding]
		public virtual IEnumerable ItemsSource
		{
			get { return ThisAdapter.ItemsSource; }
			set { ThisAdapter.ItemsSource = value; }
		}

		public int ItemTemplateId
		{
			get { return ThisAdapter.ItemTemplateId; }
			set { ThisAdapter.ItemTemplateId = value; }
		}

		private ICommand _itemClick;

		public new ICommand ItemClick
		{
			get { return _itemClick; }
			set
			{
				_itemClick = value;
				if (_itemClick != null) EnsureItemClickOverloaded();
			}
		}

		public ICommand GroupClick { get; set; }

		private bool _itemClickOverloaded = false;

		private void EnsureItemClickOverloaded()
		{
			if (_itemClickOverloaded)
				return;

			_itemClickOverloaded = true;
			base.ChildClick +=
				(sender, args) => ExecuteCommandOnItem(this.ItemClick, args.GroupPosition, args.ChildPosition);
		}


		protected virtual void ExecuteCommandOnItem(ICommand command, int groupPosition, int position)
		{
			if (command == null)
				return;

			var item = ThisAdapter.GetRawItem(groupPosition, position);
			if (item == null)
				return;

			if (!command.CanExecute(item))
				return;

			command.Execute(item);
		}
	}

	public class AndroidBindingResource
	{
		public static readonly AndroidBindingResource Instance = new AndroidBindingResource();

		private AndroidBindingResource()
		{
			var setup = Mvx.Resolve<IMvxAndroidGlobals>();
			var resourceTypeName = setup.ExecutableNamespace + ".Resource";
			Type resourceType = setup.ExecutableAssembly.GetType(resourceTypeName);
			if (resourceType == null)
				throw new Exception("Unable to find resource type - " + resourceTypeName);
			try
			{
				BindableListGroupItemTemplateId =
					(int)
						resourceType.GetNestedType("Styleable")
						.GetField("MvxListView_GroupItemTemplate")
						.GetValue(null);
			}
			catch (Exception)
			{
				throw new Exception(
					"Error finding resource ids for MvvmCross.DeapBinding - please make sure ResourcesToCopy are linked into the executable");
			}
		}

		public int BindableListGroupItemTemplateId { get; private set; }
	}
}