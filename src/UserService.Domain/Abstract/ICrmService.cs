namespace UserService.Infrastructure.Services
{
    public interface ICrmService
    {
        public Task RegisterUser(string name, string email);
    }
}