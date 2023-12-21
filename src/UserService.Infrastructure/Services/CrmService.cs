using System.Net.Http.Json;

namespace UserService.Infrastructure.Services
{
    public class CrmService : ICrmService
    {
        public Task RegisterUser(string name, string email)
        {
            var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("https://jsonplaceholder.typicode.com");
            var message = new HttpRequestMessage(HttpMethod.Post, "users");
            message.Content = JsonContent.Create(new { Name = name, Email = email });
            httpClient.Send(message);
            return Task.CompletedTask;
        }
    }
}
