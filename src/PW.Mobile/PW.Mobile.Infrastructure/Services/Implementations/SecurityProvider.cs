using System.Text;
using Newtonsoft.Json;
using PCLCrypto;

namespace PW.Mobile.Infrastructure.Services.Implementations
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

			var md5Provider = WinRTCrypto.HashAlgorithmProvider.OpenAlgorithm(HashAlgorithm.Md5);
			var dataHash = md5Provider.HashData(dataAsBytes);

			return ToHex(dataHash);
		}

		private string ToHex(byte[] bytes)
		{
			var result = new StringBuilder(bytes.Length * 2);

			foreach (var x in bytes)
				result.Append(x.ToString("x2"));

			return result.ToString();
		}
	}
}