using UserService.Application.Models;
using UserService.Domain.ValueObjects;

namespace UserService.Infrastructure.Services
{
    public interface ICrmService
    {
        public Task RegisterUser(User user);
        public Task UpdateUser(User user);
    }
}