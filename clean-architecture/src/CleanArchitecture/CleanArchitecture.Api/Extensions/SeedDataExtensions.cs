using Bogus;
using CleanArchitecture.Application.Abstractions.Data;
using CleanArchitecture.Domain.Vehiculos;
using Dapper;

namespace CleanArchitecture.Api.Extensions;
public static class SeedDataExtensions
{
    public static void SeedData(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var sqlConnectionFactory = scope.ServiceProvider
                                    .GetRequiredService<ISqlConnectionFactory>();
        using var connection = sqlConnectionFactory.CreateConnection();

        var faker = new Faker();
        List<object> vehiculos = new();

        for (var i = 0; i < 100; i++)
        {
            vehiculos.Add(new
            {
                Id = Guid.NewGuid(),
                Vin = faker.Vehicle.Vin(),
                Modelo = faker.Vehicle.Model(),
                Pais = faker.Address.Country(),
                Departamento = faker.Address.State(),
                Provincia = faker.Address.County(),
                Ciudad = faker.Address.City(),
                Calle = faker.Address.StreetAddress(),
                PrecioMonto = faker.Random.Decimal(100, 1000),
                PrecioTipoMoneda = "USD",
                MantenimientoMonto = faker.Random.Decimal(100, 200),
                MantenimientoTipoMoneda = "USD",
                FechaUltimoAlquiler = faker.Date.Past(),
                Accesorios = string.Join(",", new List<string> {
                    ((int)Accesorio.Wifii).ToString(),
                    ((int)Accesorio.AndroidCar).ToString()
                }),
                Version = 1
            });
        }

        const string sql = @"INSERT INTO Vehiculos(
        [id], [vin], [modelo], 
        [direccion_pais], [direccion_departamento], [direccion_provincia], 
        [direccion_ciudad], [direccion_calle],
        [precio_monto], [precio_tipo_moneda],
        [mantenimiento_monto], [mantenimiento_tipo_moneda],
        [fecha_ultimo_alquiler], [accesorios], [version])
        VALUES(@Id, @Vin, @Modelo
        , @Pais, @Departamento, @Provincia
        , @Ciudad, @Calle
        , @PrecioMonto, @PrecioTipoMoneda
        , @MantenimientoMonto, @MantenimientoTipoMoneda
        , @FechaUltimoAlquiler, @Accesorios, @Version)";

        connection.Execute(sql, vehiculos);
    }

}

