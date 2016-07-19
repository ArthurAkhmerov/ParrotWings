using System.Windows.Input;
using MvvmCross.Core.ViewModels;

namespace PW.Mobile.Core.Model
{
	public class UserSelectable : MvxViewModel
	{
		public UserSelectable(User item, bool isSelected = false)
		{
			Item = item;
			IsSelected = isSelected;
		}



		public User Item { get; set; }

		private bool _isSelected;
		public bool IsSelected
		{
			get { return _isSelected; }
			set { SetProperty(ref _isSelected, value); }
		}

		private MvxCommand<bool> _selectCommand;
		public ICommand SelectCommand
		{
			get
			{
				return _selectCommand = _selectCommand ??
					new MvxCommand<bool>((isSelected) =>
					{
						IsSelected = isSelected;
					});
			}
		}
	}
}