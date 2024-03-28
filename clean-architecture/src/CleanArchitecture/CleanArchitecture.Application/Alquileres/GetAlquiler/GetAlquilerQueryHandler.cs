using CleanArchitecture.Application.Abstractions.Data;
using CleanArchitecture.Application.Abstractions.Messaging;
using CleanArchitecture.Domain.Abstractions;
using Dapper;

namespace CleanArchitecture.Application.Alquileres.GetAlquiler;

//Es internal porque solo se usa en este proyecto no necesitará exponerse a otros componentes externos
//Es sealed porque no se necesita que otras clases hereden de ella
//No necesitará heredar de ninguna clase porque implementa la interfaz IQuery 
//Implementa IQueryHandler con GetAlquilerQuery como el primer tipo genérico
//y AlquilerResponse como el segundo tipo genérico
//Recibe un mensaje de tipo GetAlquilerQuery y devuelve un mensaje de tipo AlquilerResponse
internal sealed class GetAlquilerQueryHandler : IQueryHandler<GetAlquilerQuery, AlquilerResponse>
{

    private readonly ISqlConnectionFactory _sqlConnectionFactory;

    public GetAlquilerQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
    }

//Este metodo es async porque se conecta a la base de datos
//Recibe un mensaje de tipo GetAlquilerQuery y devuelve un mensaje de tipo AlquilerResponse
//Recibe un mensaje de tipo GetAlquilerQuery y un CancellationToken
//Devuelve un Task que contiene un Result de tipo AlquilerResponse
//El método es async porque se conecta a la base de datos
//El método es publico porque se necesita acceder a él desde otras clases
    public async Task<Result<AlquilerResponse>> Handle(
        GetAlquilerQuery request
      , CancellationToken cancellationToken)
    {
        using var connection = _sqlConnectionFactory.CreateConnection();

        var sql = """
            SELECT id AS Id,
            user_id AS UserId,
            vehiculo_id AS VehiculoId,
            status AS Status,
            precio_por_periodo AS PrecioAlquiler,
            precio_por_periodo_tipo_moneda AS TipoMonedaAlquiler,
            precio_mantenimiento AS PrecioMantenimiento,
            precio_mantenimiento_tipo_moneda AS TipoMonedaMantenimiento,
            precio_accesorios AS PrecioAccesorios,
            precio_accesorios_tipo_moneda AS TipoMonedaAccesorios,
            precio_total AS PrecioTotal,
            precio_total_tipo_moneda AS TipoMonedaTotal,
            duracion_inicio AS DuracionInicio,
            duracion_final AS DuracionFinal,
            fecha_creacion AS FechaCreacion 
            FROM Alquileres 
            WHERE id = @Id
        """;

        var alquiler = await connection.QueryFirstOrDefaultAsync<AlquilerResponse>(
            sql,
            new { request.AlquilerId }
        );
        
        return alquiler!;
    }
}
