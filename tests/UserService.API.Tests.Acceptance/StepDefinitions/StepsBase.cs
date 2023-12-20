using UserService.Application.Ports;
using Microsoft.Extensions.DependencyInjection;
using System.Linq.Expressions;
using System.Text.Json;

namespace UserService.API.Test.Acceptance.StepDefinitions;

public class StepsBase
{
	protected readonly ScenarioContext _scenarioContext;
	protected readonly TestFixture _fixture;

	protected readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
	{
		PropertyNameCaseInsensitive = true
	};

	public StepsBase(ScenarioContext scenarioContext, TestFixture fixture)
	{
		_scenarioContext = scenarioContext;
		_fixture = fixture;
	}

	protected async Task SaveEntity<T>(T entity) where T : class
	{
		using var scope = _fixture.ServiceProvider.CreateScope();
		var repository = scope.ServiceProvider.GetRequiredService<IAsyncRepository>();

		repository.Add(entity);
		await repository.CommitChanges();
	}

	protected async Task<T?> GetEntity<T>(object id) where T : class
	{
		using var scope = _fixture.ServiceProvider.CreateScope();
		var repository = scope.ServiceProvider.GetRequiredService<IAsyncRepository>();

		return await repository.Get<T>(id);
	}

	protected async Task<IEnumerable<T>> GetEntitiesWhere<T, TId>(Expression<Func<T, bool>> predicate) where T : Entity<TId>
	{
		using var scope = _fixture.ServiceProvider.CreateScope();
		var repository = scope.ServiceProvider.GetRequiredService<IAsyncRepository>();

		return await repository.GetWhere(predicate);
	}

	protected async Task<IEnumerable<T>> GetEntitiesWhere<T, TId, TProperty>(
		Expression<Func<T, bool>> predicate,
		Expression<Func<T, TProperty>> include) where T : Entity<TId>
	{
		using var scope = _fixture.ServiceProvider.CreateScope();
		var repository = scope.ServiceProvider.GetRequiredService<IAsyncRepository>();

		return await repository.GetWhere(predicate, include);
	}
}
