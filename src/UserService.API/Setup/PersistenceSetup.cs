using UserService.Infrastructure.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace UserService.API.Setup;

public static class PersistenceSetup
{
	public static void AddPersistence(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddDbContext<ApplicationDbContext>(optionsBuilder =>
		{
			optionsBuilder.UseSqlServer(configuration.GetConnectionString("Database"), options =>
			{
				options.EnableRetryOnFailure();
			});
		});
	}

	public static void EnsurePersistenceIsReady(this WebApplication app)
	{
		using var scope = app.Services.CreateScope();
		var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
		dbContext.Database.EnsureCreated();
	}
}
