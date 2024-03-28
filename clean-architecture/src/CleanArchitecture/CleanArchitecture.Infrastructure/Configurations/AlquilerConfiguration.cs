using CleanArchitecture.Domain.Alquileres;
using CleanArchitecture.Domain.Shared;
using CleanArchitecture.Domain.Users;
using CleanArchitecture.Domain.Vehiculos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Infrastructure.Configurations;

internal sealed class AlquilerConfiguration : IEntityTypeConfiguration<Alquiler>
{
    public void Configure(EntityTypeBuilder<Alquiler> builder)
    {
        builder.ToTable("Alquileres");
        builder.HasKey(x => x.Id);

        builder.OwnsOne(x => x.PrecioPorPeriodo, precioBuilder =>
        {
            precioBuilder.Property(x => x.TipoMoneda)
            .HasConversion(tm => tm.Codigo, codigo => TipoMoneda.FromCodigo(codigo!));
        });

        builder.OwnsOne(x => x.PrecioMantenimiento, precioBuilder =>
        {
            precioBuilder.Property(x => x.TipoMoneda)
            .HasConversion(tm => tm.Codigo, codigo => TipoMoneda.FromCodigo(codigo!));
        });

        builder.OwnsOne(x => x.PrecioAccesorios, precioBuilder =>
        {
            precioBuilder.Property(x => x.TipoMoneda)
            .HasConversion(tm => tm.Codigo, codigo => TipoMoneda.FromCodigo(codigo!));
        });

        builder.OwnsOne(x => x.PrecioTotal, precioBuilder =>
        {
            precioBuilder.Property(x => x.TipoMoneda)
            .HasConversion(tm => tm.Codigo, codigo => TipoMoneda.FromCodigo(codigo!));
        });

        builder.OwnsOne(x => x.Duracion);

        builder.HasOne<Vehiculo>()
            .WithMany()
            .HasForeignKey(x => x.VehiculoId);

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(x => x.UserId);
    }
}
