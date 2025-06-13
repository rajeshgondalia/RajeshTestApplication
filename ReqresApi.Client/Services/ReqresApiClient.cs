using ReqresApi.Client.Interfaces;
using ReqresApi.Client.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;

namespace ReqresApi.Client.Services
{
    public class ReqresApiClient : IReqresApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;
        private readonly ILogger<ReqresApiClient> _logger;

        public ReqresApiClient(HttpClient httpClient, IOptions<ReqresApiClientOptions> options, ILogger<ReqresApiClient> logger)
        {
            _httpClient = httpClient;
            _baseUrl = options.Value.BaseUrl;
            _logger = logger;
        }

        // Creates a new HttpRequestMessage with the specified method and URL, adding the required API key header.
        private HttpRequestMessage CreateRequest(HttpMethod method, string url)
        {
            var request = new HttpRequestMessage(method, url);
            request.Headers.Add("x-api-key", "reqres-free-v1");
            return request;
        }

        public async Task<UserDto?> GetUserByIdAsync(int userId)
        {
            var request = CreateRequest(HttpMethod.Get, $"{_baseUrl}users/{userId}");
            var response = await _httpClient.SendAsync(request);

            if (response.StatusCode == HttpStatusCode.NotFound)
                return null;

            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var parsed = JsonSerializer.Deserialize<JsonElement>(json);

            if (!parsed.TryGetProperty("data", out var data))
            {
                _logger.LogError($"Failed to fetch user {userId}: {response.StatusCode}");
                throw new HttpRequestException($"User not found. Status: {response.StatusCode}");
            }

            return JsonSerializer.Deserialize<UserDto>(data.GetRawText());
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            var users = new List<UserDto>();
            int page = 1;
            int totalPages;

            do
            {
                var request = CreateRequest(HttpMethod.Get, $"{_baseUrl}users?page={page}");
                var response = await _httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();

                var result = JsonSerializer.Deserialize<UserListDto>(await response.Content.ReadAsStringAsync());
                if (result == null)
                {
                    _logger.LogError($"Failed to fetch users from page {page}");
                    break;
                }

                users.AddRange(result.Data);
                totalPages = result.TotalPages;
                page++;
            }
            while (page <= totalPages);

            return users;
        }
    }
}
