using CleanArchitecture.Application.Abstractions.Data;
using CleanArchitecture.Application.Abstractions.Messaging;
using CleanArchitecture.Domain.Abstractions;
using CleanArchitecture.Domain.Alquileres;
using Dapper;

namespace CleanArchitecture.Application.Vehiculos.SearchVehiculos;

internal sealed class SearchVehiculosQueryHandler
: IQueryHandler<SearchVehiculosQuery, IReadOnlyList<VehiculoResponse>>
{
    private static readonly int[] ActiveAlquilerStatus =
    {
        (int)AlquilerStatus.Reservado,
        (int)AlquilerStatus.Confirmado,
        (int)AlquilerStatus.Completado
    };

    private readonly ISqlConnectionFactory _sqlConnectionFactory;

    public SearchVehiculosQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
    }

    public async Task<Result<IReadOnlyList<VehiculoResponse>>> Handle(
        SearchVehiculosQuery request,
        CancellationToken cancellationToken)
    {
        //validar que las fechas en consulta sean coherentes
        if (request.fechaInicio >= request.fechaFin)
        {
            return new List<VehiculoResponse>();
        }

        var connection = _sqlConnectionFactory.CreateConnection();
        // Rellena el valor de n aquí
        var inClause = string.Join(",", ActiveAlquilerStatus);

        //seleccionar vehiculos que no esten en alquiler
        var querySql = $@"
            SELECT
                v.id AS Id,
                v.modelo AS Modelo,
                v.vin AS Vin,
                v.precio_monto AS Precio,
                v.precio_tipo_moneda AS TipoMoneda,
                d.direccion_pais AS Pais,
                d.direccion_departamento AS Departamento,
                d.direccion_provincia AS Provincia,
                d.direccion_ciudad AS Ciudad,
                d.direccion_calle AS Calle
            FROM vehiculos AS V
            LEFT JOIN alquileres AS A 
            ON v.id = a.vehiculo_id
            a.duracion_inicio >= @StarDate AND
            a.duracion_final <= @EndDate AND
            a.status IN ({inClause})
            WHERE a.id IS NULL";

        //QueryAsync<VehiculoResponse, DireccionResponse, VehiculoResponse> 
        //se utiliza para mapear los resultados de la consulta 
        //a las clases VehiculoResponse y DireccionResponse
        //y el tercero es el tipo de retorno de la consulta es decir VehiculoResponse
        //El resultado final es el ultimo parametro de la función y es el que se retorna
        //en este caso se retorna un vehiculo con su dirección
        //splitOn: "Pais" se utiliza para indicar que la columna "Pais" 
        //es la que se utiliza para dividir los resultados de la consulta
        //en las clases VehiculoResponse y DireccionResponse
        
        var vehiculos = await connection
        .QueryAsync<VehiculoResponse, DireccionResponse, VehiculoResponse>(
            querySql,
            (vehiculo, direccion) =>
            {
                vehiculo.Direccion = direccion;
                return vehiculo;
            },
            new
            {
                StarDate = request.fechaInicio,
                EndDate = request.fechaFin
            },
            splitOn: "Pais"
        );

        return vehiculos.ToList();

    }
}