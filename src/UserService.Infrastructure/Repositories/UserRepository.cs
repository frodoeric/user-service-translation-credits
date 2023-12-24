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

        public User? Get(long id) => repository.Get<User>(id).GetAwaiter().GetResult();

        public void Add(User user) => repository.Add(user);

        public IEnumerable<User> GetAll() => repository.GetAll<User>().GetAwaiter().GetResult();

        public void Update(User user) => repository.Update(user);

        public void Save() => repository.CommitChanges().GetAwaiter().GetResult();

        public void Remove(User user) => repository.Remove(user);

    }
}
