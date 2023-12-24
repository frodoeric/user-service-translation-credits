using System.Net.Http;
using System.Net.Http.Json;
using UserService.Domain.ValueObjects;

namespace UserService.Infrastructure.Services
{
    public class CrmService : ICrmService
    {
        private readonly HttpClient httpClient;

        public CrmService()
        {
            httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("https://jsonplaceholder.typicode.com");
        }

        public Task RegisterUser(string name, string email)
        {
            var message = new HttpRequestMessage(HttpMethod.Post, "users");
            message.Content = JsonContent.Create(new { Name = name, Email = email });
            return httpClient.SendAsync(message);
        }

        public Task UpdateUser(long userId, string name, string email)
        {
            var message = new HttpRequestMessage(HttpMethod.Put, $"users/{userId}");
            message.Content = JsonContent.Create(new { Name = name, Email = email });
            return httpClient.SendAsync(message);
        }
    }
}
