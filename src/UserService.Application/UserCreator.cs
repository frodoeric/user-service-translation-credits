using System.Net.Http.Json;
using System.Text.RegularExpressions;
using UserService.Application.Models;
using UserService.Application.Ports;
using UserService.Domain.Core;

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

		if (nameResult.IsFailure)
			return Result.Failure<long, Error>(new Error(nameResult.Error));

		if (!Regex.IsMatch(model.Email, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$"))
			throw new Exception("Email is invalid");

		User.Repository = new UserRepository(repository); // Get user repository ready in User

		User user;
		try
		{
			user = User.Create(nameResult.Value, model.Email);
		}
		catch (Exception ex)
		{
			return Result.Failure<long, Error>(
			new UniqueConstraintViolationError(
				"User with given Email already exists.", nameof(User), nameof(User.Email)));
		}

		RegisterInCrm(user);

		return Result.Success<long, Error>(user.Id);
	}

	private void RegisterInCrm(User user)
	{
		var httpClient = new HttpClient();
		httpClient.BaseAddress = new Uri("https://jsonplaceholder.typicode.com");
		var message = new HttpRequestMessage(HttpMethod.Post, "users");
		message.Content = JsonContent.Create(new { Name = user.Name, Email = user.Email });
		httpClient.Send(message);
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