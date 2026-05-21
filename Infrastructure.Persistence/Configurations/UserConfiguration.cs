using Core.Domain.Entities;
using Core.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.FirstName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.LastName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.Email)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(x => x.PasswordHash)
            .IsRequired();

        builder.Property(x => x.Role)
            .IsRequired();

        builder.HasIndex(x => x.Email)
            .IsUnique();

        builder.HasData(new User
        {
            Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
            FirstName = "System",
            LastName = "Admin",
            Email = "admin@system.com",
            PasswordHash = "$2a$11$ol8zga7e7xihud6pJ0zLtOwq72anlCXLtxdE.0MoH3/cDJtQwJmVu",
            Role = Role.Admin,
            CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
        });
    }
}
