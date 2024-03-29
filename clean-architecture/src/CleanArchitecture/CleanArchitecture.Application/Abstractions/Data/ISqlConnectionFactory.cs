using System.Data;

namespace CleanArchitecture.Application.Abstractions.Data;

public interface ISqlConnectionFactory
{
  //Esta interfaz se encarga de crear una conexión a la base de datos
  //Esta interfaz se utiliza para poder hacer la conexión a la base de datos
  //y poder hacer las consultas a la base de datos
  IDbConnection CreateConnection();
}