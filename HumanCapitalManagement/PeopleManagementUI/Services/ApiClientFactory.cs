using System.Net.Http.Headers;

namespace PeopleManagementUI.Services
{
    public class ApiClientFactory : IApiClientFactory
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;

        public ApiClientFactory(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
        }

        public HttpClient CreateAuthenticationClient()
        {
            var client = _httpClientFactory.CreateClient("AuthenticationClient");
            client.BaseAddress = new Uri(_configuration["Endpoints:AuthenticationBase"]);
            return client;
        }

        public HttpClient CreateEmployeeClient()
        {
            var client = _httpClientFactory.CreateClient("EmployeeClient");
            client.BaseAddress = new Uri(_configuration["Endpoints:EmployeeBase"]);
            var token = _httpContextAccessor.HttpContext?.Session.GetString("token");
            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            return client;
        }
    }
}

