using CleanArchitecture.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Infrastructure.Configurations;

internal sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id);

        builder.Property(x => x.Name)
        .HasMaxLength(200)
        .HasConversion(x => x!.Value, value => new Nombre(value));

        builder.Property(x => x.Apellido)
        .HasMaxLength(200)
        .HasConversion(x => x!.Value, value => new Apellido(value));

        builder.Property(x => x.Email)
        .HasMaxLength(400)
        .HasConversion(x => x!.Value, value => new Domain.Users.Email(value));

        //El valor del email no debe repetirse en la base de datos
        builder.HasIndex(x => x.Email).IsUnique();
    }
}