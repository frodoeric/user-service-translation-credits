using UserService.Domain.ValueObjects;

namespace UserService.Infrastructure.Services
{
    public interface ICrmService
    {
        public Task RegisterUser(string name, string email);
        public Task UpdateUser(long userId, string name, string email);
    }
}