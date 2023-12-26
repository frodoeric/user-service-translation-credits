using System.Net.Http.Json;

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

        public Task RegisterUser(User user)
        {
            var message = new HttpRequestMessage(HttpMethod.Post, "users");
            message.Content = JsonContent.Create(new { user.Name, user.Email, user.TranslationCredits });
            return httpClient.SendAsync(message);
        }

        public Task UpdateUser(User user)
        {
            var message = new HttpRequestMessage(HttpMethod.Put, $"users/{user.Id}");
            message.Content = JsonContent.Create(new { user.Name, user.Email, user.TranslationCredits });
            return httpClient.SendAsync(message);
        }
    }
}
