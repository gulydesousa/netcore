using CleanArchitecture.Domain.Alquileres;
using CleanArchitecture.Domain.Reviews;
using CleanArchitecture.Domain.Users;
using CleanArchitecture.Domain.Vehiculos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Infrastructure.Configurations;

internal sealed class ReviewConfiguration : IEntityTypeConfiguration<Review>
{
    public void Configure(EntityTypeBuilder<Review> builder)
    {
        builder.ToTable("Reviews");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Rating)
        .HasConversion(
            x => x.Value,
            x => Rating.Create(x).Value
        );

        builder.Property(x => x.Comentario)
        .HasMaxLength(200)
        .HasConversion(
            x => x.value,
            x => new Comentario(x));

        builder.HasOne<Vehiculo>()
        .WithMany()
        .HasForeignKey(x => x.VehiculoId);

        builder.HasOne<Alquiler>()
        .WithMany()
        .HasForeignKey(x => x.AlquilerId);

        builder.HasOne<User>()
        .WithMany()
        .HasForeignKey(x => x.UserId);
    }
}