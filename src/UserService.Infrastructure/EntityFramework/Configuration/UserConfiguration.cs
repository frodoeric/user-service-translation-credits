using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace UserService.Infrastructure.EntityFramework;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
	public void Configure(EntityTypeBuilder<User> builder)
	{
		builder.OwnsOne(u => u.Email).Property(u => u.Value).HasColumnName("Email");
        builder.OwnsOne(u => u.Email).HasIndex(u => u.Value).IsUnique();

		builder.OwnsOne(u => u.Name).Property(e => e.Value).HasColumnName("Name");
	}
}
