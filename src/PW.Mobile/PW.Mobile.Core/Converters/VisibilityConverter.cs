using System.Globalization;
using MvvmCross.Platform.UI;
using MvvmCross.Plugins.Visibility;

namespace PW.Mobile.Core.Converters
{
	public class IsVisibleConverter : MvxBaseVisibilityValueConverter<bool>
	{
		protected override MvxVisibility Convert(bool value, object parameter, CultureInfo culture)
		{
			return value
				? MvxVisibility.Visible
				: MvxVisibility.Collapsed;
		}
	}

	public class IsNotVisibleConverter : MvxBaseVisibilityValueConverter<bool>
	{
		protected override MvxVisibility Convert(bool value, object parameter, CultureInfo culture)
		{
			return value
				? MvxVisibility.Collapsed
				: MvxVisibility.Visible;
		}
	}
}