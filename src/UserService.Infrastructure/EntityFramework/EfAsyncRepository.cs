using UserService.Application.Ports;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace UserService.Infrastructure.EntityFramework;

public class EfAsyncRepository : IAsyncRepository
{
	protected ApplicationDbContext Context;

	public EfAsyncRepository(ApplicationDbContext context)
	{
		Context = context;
	}

	public async Task<T?> Get<T>(object id) where T : class
	{
		return await Context.Set<T>().FindAsync(id);
	}

	public async Task<IEnumerable<T>> GetAll<T>() where T : class
	{
		return await Context.Set<T>().AsQueryable().ToListAsync();
	}

	public async Task<IEnumerable<T>> GetWhere<T>(Expression<Func<T, bool>> predicate) where T : class
	{
		return await Context.Set<T>().Where(predicate).ToListAsync();
	}

	public async Task<IEnumerable<T>> GetWhere<T, TProperty>(
		Expression<Func<T, bool>> predicate,
		Expression<Func<T, TProperty>> include) where T : class
	{
		return await Context.Set<T>().Where(predicate).Include(include).ToListAsync();
	}

	public void Add<T>(T entity) where T : class
	{
		Context.Set<T>().Add(entity);
	}

    public void Update<T>(T entity) where T : class
    {
        var entry = Context.Entry(entity);
        if (entry.State == EntityState.Detached)
        {
            Context.Set<T>().Attach(entity);
        }
        entry.State = EntityState.Modified;
    }

    public void Remove<T>(T entity) where T : class
	{
		Context.Set<T>().Remove(entity);
	}

	public async Task CommitChanges()
	{
		await Context.SaveChangesAsync();
	}
}