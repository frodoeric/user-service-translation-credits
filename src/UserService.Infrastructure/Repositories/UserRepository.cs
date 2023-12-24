using UserService.Application.Ports;

namespace UserService.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        public UserRepository(IAsyncRepository repository)
        {
            this.repository = repository;
        }

        private IAsyncRepository repository { get; init; }

        public void Add(User user) => repository.Add(user);

        public IEnumerable<User> GetAll() => repository.GetAll<User>().GetAwaiter().GetResult();

        public void Save() => repository.CommitChanges().GetAwaiter().GetResult();
    }
}
