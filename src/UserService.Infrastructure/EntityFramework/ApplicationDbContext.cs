using Microsoft.EntityFrameworkCore;

namespace UserService.Infrastructure.EntityFramework;

public class ApplicationDbContext : DbContext
{
	public DbSet<User> Users { get; set; } = null!;

	public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
		: base(options) { }

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		optionsBuilder.UseLazyLoadingProxies();
	}

	protected override void OnModelCreating(ModelBuilder builder)
	{
		builder.ApplyConfiguration(new UserConfiguration());

		base.OnModelCreating(builder);
	}
}