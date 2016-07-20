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
		Task<UserVDTO[]> GetUsersAsync(int skip, int take);
		Task<UserVDTO[]> GetUsersAsync(string searchText);
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
			var json = JsonConvert.SerializeObject(dto);
			return await PostAsync<AuthResultVDTO>("/api/auth", new StringContent(json, Encoding.UTF8, "application/json"));
			
		}
		

		public async Task<AuthResultVDTO> SignUp(SignupRequestVDTO dto)
		{
			var json = JsonConvert.SerializeObject(dto);
			return await PostAsync<AuthResultVDTO>("/api/auth/signup", new StringContent(json, Encoding.UTF8, "application/json"));
		}

		public async Task<UserVDTO[]> GetUsersAsync(int skip, int take)
		{
			return await GetAsync<UserVDTO[]>($"/api/user?skip={skip}&take={take}");

		}

		public async Task<UserVDTO[]> GetUsersAsync(string searchText)
		{
			return await GetAsync<UserVDTO[]>($"/api/user?searchText={searchText}");
		}

		public async Task<TransferVDTO[]> GetTransfersAsync(Guid userId, DateTime from, DateTime to, Guid sessionId)
		{
			var hash = _securityProvider.CalculateMD5(userId, sessionId);
			return await GetAsync<TransferVDTO[]>($"/api/transfer?userId={userId}" +
												   $"&from={from.ToString("yyyy-MM-dd HH:mm")}" +
												   $"&to={to.ToString("yyyy-MM-dd HH:mm")}" +
												   $"&hash={hash}");
		}

		private async Task<T> PostAsync<T>(string requestUri, HttpContent content)
		{
			using (var client = new HttpClient())
			{
				client.BaseAddress = new Uri(_baseAddress);
				client.DefaultRequestHeaders.Accept.Clear();
				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

				var result = await client.PostAsync(requestUri, content);

				var rawResponse = result.Content.ReadAsByteArrayAsync().Result;
				var jsonResult = Encoding.UTF8.GetString(rawResponse, 0, rawResponse.Length);
				var authResult = JsonConvert.DeserializeObject<T>(jsonResult);

				return authResult;
			}
		}

		private async Task<T> GetAsync<T>(string requestUri)
		{
			using (var client = new HttpClient())
			{
				client.BaseAddress = new Uri(_baseAddress);
				client.DefaultRequestHeaders.Accept.Clear();
				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

				var result = await client.GetAsync(requestUri);

				var rawResponse = result.Content.ReadAsByteArrayAsync().Result;
				var jsonResult = Encoding.UTF8.GetString(rawResponse, 0, rawResponse.Length);
				var authResult = JsonConvert.DeserializeObject<T>(jsonResult);

				return authResult;
			}
		}
	}
}