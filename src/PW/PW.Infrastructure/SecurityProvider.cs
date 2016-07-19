using System;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using PW.Domain.Infrastructure;

namespace PW.Infrastructure
{
	public class SecurityProvider : ISecurityProvider
	{
		private readonly JsonSerializerSettings _jsonSerializerSettings;

		public SecurityProvider()
		{
			_jsonSerializerSettings = new JsonSerializerSettings
			{
				DateFormatHandling = DateFormatHandling.IsoDateFormat
			};
		}

		public string CalculateMD5(params object[] data)
		{
			var dataAsString = JsonConvert.SerializeObject(data, _jsonSerializerSettings);
			var dataAsBytes = Encoding.UTF8.GetBytes(dataAsString);

			var md5Provider = new MD5CryptoServiceProvider();
			md5Provider.Initialize();

			var dataHash = md5Provider.ComputeHash(dataAsBytes, 0, dataAsBytes.Length);

			return ToHex(dataHash);
		}

		public string CalculateSHA256(params object[] data)
		{
			var dataAsString = JsonConvert.SerializeObject(data, _jsonSerializerSettings);
			var dataAsBytes = Encoding.UTF8.GetBytes(dataAsString);

			var sha256Provider = new SHA256CryptoServiceProvider();
			sha256Provider.Initialize();

			var dataHash = sha256Provider.ComputeHash(dataAsBytes, 0, dataAsBytes.Length);

			return ToHex(dataHash);
		}

		public string CreateSalt(int size)
		{
			var rng = new RNGCryptoServiceProvider();
			var buff = new byte[size];
			rng.GetBytes(buff);

			return Convert.ToBase64String(buff);
		}

		private string ToHex(byte[] bytes)
		{
			var result = new StringBuilder(bytes.Length * 2);

			foreach (byte x in bytes)
				result.Append(x.ToString("x2"));

			return result.ToString();
		}
	}
}