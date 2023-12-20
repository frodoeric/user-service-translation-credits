using System.Net.Http.Json;
using System.Text.RegularExpressions;
using UserService.Application.Models;
using UserService.Application.Ports;
using UserService.Domain.Core;
using UserService.Domain.ValueObjects;

namespace UserService.Application;

public class UserCreator
{
	private readonly IAsyncRepository repository;

	public UserCreator(IAsyncRepository repository)
	{
		this.repository = repository;
	}

	public async Task<Result<long, Error>> Create(UserData model)
	{
		var nameResult = Name.Create(model.Name);
		var emailResult = Email.Create(model.Email);

        if (nameResult.IsFailure || emailResult.IsFailure)
        {
            var errors = new List<string>();
            if (nameResult.IsFailure)
            {
                errors.Add(nameResult.Error.Message);
            }
            if (emailResult.IsFailure)
            {
                errors.Add(emailResult.Error.Message);
            }
            var combinedErrorMessage = string.Join(" ", errors);
            return Result.Failure<long, Error>(new Error(combinedErrorMessage));
        }

        User.Repository = new UserRepository(repository); // Get user repository ready in User

		var user = User.Create(nameResult.Value, emailResult.Value);

        if (user.IsFailure)
        {
            return Result.Failure<long, Error>(user.Error);
        }

        await RegisterInCrm(user.Value);

		return Result.Success<long, Error>(user.Value.Id);
	}

	private static Task RegisterInCrm(User user)
	{
		var httpClient = new HttpClient();
		httpClient.BaseAddress = new Uri("https://jsonplaceholder.typicode.com");
		var message = new HttpRequestMessage(HttpMethod.Post, "users");
		message.Content = JsonContent.Create(new { Name = user.Name, Email = user.Email });
		httpClient.Send(message);
		return Task.CompletedTask;
	}

	private class UserRepository : IUserRepository
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