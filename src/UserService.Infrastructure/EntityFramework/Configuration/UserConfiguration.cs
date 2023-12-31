﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserService.Domain.ValueObjects;

namespace UserService.Infrastructure.EntityFramework;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
	public void Configure(EntityTypeBuilder<User> builder)
	{

        builder.ToTable("Users");

        builder.HasKey(u => u.Id);

        builder.Property(u => u.Name)
               .HasConversion(
                   name => name.Value,
                   name => Name.Set(name).Value);

        builder.Property(u => u.Email)
               .HasConversion(
                   email => email.Value,
                   email => Email.Create(email).Value);

        builder.Property(u => u.TranslationCredits)
               .HasConversion(
                   credits => credits.Value,
                   credits => new TranslationCredits(credits));

        builder.Property(u => u.TotalCreditsSpent);
    }
}
