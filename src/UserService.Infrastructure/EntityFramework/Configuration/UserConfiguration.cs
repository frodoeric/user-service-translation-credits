using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace UserService.Infrastructure.EntityFramework;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
	public void Configure(EntityTypeBuilder<User> builder)
	{
		builder.Property(u => u.Email).HasColumnName("Email");
		builder.HasIndex(u => u.Email).IsUnique();

		builder.OwnsOne(u => u.Name).Property(e => e.Value).HasColumnName("Name");
	}
}
