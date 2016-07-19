using System;
using System.Globalization;
using MvvmCross.Platform.Converters;
using PW.Mobile.Core.Extensions;

namespace PW.Mobile.Core.Converters
{
	public class DateFormattedConverter : MvxValueConverter<DateTime, string>
	{
		protected override string Convert(DateTime value, Type targetType, object parameter, CultureInfo culture)
		{
			return value.ToFormattedDate();
		}
	}
}