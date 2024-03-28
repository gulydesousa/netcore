
using CleanArchitecture.Domain.Shared;
using CleanArchitecture.Domain.Vehiculos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Infrastructure.Configurations;

internal sealed class VehiculoConfiguration : IEntityTypeConfiguration<Vehiculo>
{
    public void Configure(EntityTypeBuilder<Vehiculo> builder)
    {
        //Aqui definimos las configuraciones de la tabla Vehiculos
        //Se define el nombre de la tabla
        builder.ToTable("Vehiculos");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id);

        builder.Property(x => x.Modelo)
        .HasMaxLength(200)
        .HasConversion(x => x!.Value, value => new Modelo(value));

        builder.Property(x => x.Vin)
        .HasMaxLength(500)
        .HasConversion(x => x!.Value, value => new Vin(value));

        builder.OwnsOne(x => x.Precio, priceBuilder =>
        {
            priceBuilder.Property(x => x.TipoMoneda)
            .HasConversion(x => x.Codigo, codigo => TipoMoneda.FromCodigo(codigo!));
        });

        builder.OwnsOne(x => x.Mantenimiento, priceBuilder =>
        {
            priceBuilder.Property(x => x.TipoMoneda)
            .HasConversion(x => x.Codigo, codigo => TipoMoneda.FromCodigo(codigo!));
        });

        builder.OwnsOne(x => x.Direccion); 
    }
}
