using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PW.Mobile.Core.Extensions
{
	public static class DateTimeExtensions
	{
		private static readonly CultureInfo CurrentCulture = new CultureInfo("en-US");

		public static string ToFormattedDate(this DateTime dateTime)
		{
			if (dateTime.Date == DateTime.Now.Date)
			{
				return "Today";
			}

			if (dateTime.Date == DateTime.Now.Date.AddDays(-1))
			{
				return "Yesterday";
			}

			return dateTime.ToString("M", CurrentCulture);
		}
	}
}
