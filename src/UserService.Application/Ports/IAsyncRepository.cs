using System.Linq.Expressions;

namespace UserService.Application.Ports;

public interface IAsyncRepository
{
	Task<T?> Get<T>(object id) where T : class;

	Task<IEnumerable<T>> GetAll<T>() where T : class;

	Task<IEnumerable<T>> GetWhere<T>(Expression<Func<T, bool>> predicate) where T : class;

	Task<IEnumerable<T>> GetWhere<T, TProperty>(
		Expression<Func<T, bool>> predicate,
		Expression<Func<T, TProperty>> include) where T : class;

	void Add<T>(T entity) where T : class;

	void Remove<T>(T entity) where T : class;

	Task CommitChanges();
}