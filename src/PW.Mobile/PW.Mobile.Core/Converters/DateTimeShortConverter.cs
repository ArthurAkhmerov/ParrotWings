using System;
using System.Globalization;
using MvvmCross.Platform.Converters;

namespace PW.Mobile.Core.Converters
{
	public class DateTimeShortConverter : MvxValueConverter<DateTime, string>
	{
		protected override string Convert(DateTime value, Type targetType, object parameter, CultureInfo culture)
		{
			return value.ToString("HH:mm");
		}
	}
}