using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PW.Mobile.API.DTO;
using PW.Mobile.Infrastructure.Services;

namespace PW.Mobile.API
{
	public interface IPwApiClient
	{
		Task<AuthResultVDTO> AuthorizeAsync(AuthRequestVDTO dto);
		Task<AuthResultVDTO> SignUp(SignupRequestVDTO dto);
		Task<UserVDTO[]> GetUsersAsync();
		Task<TransferVDTO[]> GetTransfersAsync(Guid userId, DateTime from, DateTime to, Guid sessionId);
	}

	public class PwApiClient : IPwApiClient
	{
		private readonly string _baseAddress;
		private readonly ISecurityProvider _securityProvider;

		public PwApiClient(string baseAddress, ISecurityProvider securityProvider)
		{
			_baseAddress = baseAddress;
			_securityProvider = securityProvider;
		}

		public async Task<AuthResultVDTO> AuthorizeAsync(AuthRequestVDTO dto)
		{
			using (var client = new HttpClient())
			{
				client.BaseAddress = new Uri(_baseAddress);
				client.DefaultRequestHeaders.Accept.Clear();
				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

				var json = JsonConvert.SerializeObject(dto);
				var result =
					await client.PostAsync("/api/auth", new StringContent(json, Encoding.UTF8, "application/json"));

				var rawResponse = result.Content.ReadAsByteArrayAsync().Result;
				var jsonResult = Encoding.UTF8.GetString(rawResponse, 0, rawResponse.Length);
				var authResult = JsonConvert.DeserializeObject<AuthResultVDTO>(jsonResult);

				return authResult;
			}
		}

		public async Task<AuthResultVDTO> SignUp(SignupRequestVDTO dto)
		{
			using (var client = new HttpClient())
			{
				client.BaseAddress = new Uri(_baseAddress);
				client.DefaultRequestHeaders.Accept.Clear();
				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

				var json = JsonConvert.SerializeObject(dto);
				var result =
					await client.PostAsync("/api/auth/signup", new StringContent(json, Encoding.UTF8, "application/json"));

				var rawResponse = result.Content.ReadAsByteArrayAsync().Result;
				var jsonResult = Encoding.UTF8.GetString(rawResponse, 0, rawResponse.Length);
				var authResult = JsonConvert.DeserializeObject<AuthResultVDTO>(jsonResult);

				return authResult;
			}
		}

		public async Task<UserVDTO[]> GetUsersAsync()
		{
			using (var client = new HttpClient())
			{
				client.BaseAddress = new Uri(_baseAddress);
				client.DefaultRequestHeaders.Accept.Clear();
				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

				var result = await client.GetAsync($"/api/user");

				var rawResponse = result.Content.ReadAsByteArrayAsync().Result;
				var json = Encoding.UTF8.GetString(rawResponse, 0, rawResponse.Length);
				var usersVdtos = JsonConvert.DeserializeObject<UserVDTO[]>(json);

				return usersVdtos;
			}
		}

		public async Task<TransferVDTO[]> GetTransfersAsync(Guid userId, DateTime from, DateTime to, Guid sessionId)
		{
			using (var client = new HttpClient())
			{
				client.BaseAddress = new Uri(_baseAddress);
				client.DefaultRequestHeaders.Accept.Clear();
				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

				var hash = _securityProvider.CalculateMD5(userId, sessionId);

				var result = await client.GetAsync($"/api/transfer?userId={userId}" +
				                                   $"&from={from.ToString("yyyy-MM-dd HH:mm")}" +
				                                   $"&to={to.ToString("yyyy-MM-dd HH:mm")}" +
				                                   $"&hash={hash}");

				var rawResponse = result.Content.ReadAsByteArrayAsync().Result;
				var json = Encoding.UTF8.GetString(rawResponse, 0, rawResponse.Length);
				var transfersVdtos = JsonConvert.DeserializeObject<TransferVDTO[]>(json);

				return transfersVdtos;
			}
		}

	}
}