using System;
using Newtonsoft.Json;
using Plugin.Settings;
using PW.Mobile.Core.Model;

namespace PW.Mobile.Core
{
	public interface ISettingsProvider
	{
		Auth GetAuth();
		void SaveAuth(Auth auth);
	}

	public class SettingsProvider : ISettingsProvider
	{
		private const string AUTH = nameof(AUTH);

		public string GetBaseAddress()
		{
			return AppConstants.SERVER_ADDRESS;
		}

		public Auth GetAuth()
		{
			var authAsJson = CrossSettings.Current.GetValueOrDefault<string>(AUTH);

			return authAsJson != null
				? JsonConvert.DeserializeObject<Auth>(authAsJson)
				: null;
		}

		public void SaveAuth(Auth auth)
		{
			var authAsJson = JsonConvert.SerializeObject(auth);
			CrossSettings.Current.AddOrUpdateValue(AUTH, authAsJson);
		}
	}
}