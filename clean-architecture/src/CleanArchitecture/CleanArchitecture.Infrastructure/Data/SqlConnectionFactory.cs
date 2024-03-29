using System.Data;
using CleanArchitecture.Application.Abstractions.Data;
using Microsoft.Data.SqlClient;

namespace CleanArchitecture.Infrastructure.Data;

internal sealed class SqlConnectionFactory : ISqlConnectionFactory
{
    private readonly string _connectionString;

    public SqlConnectionFactory(string connectionString)
    {
        _connectionString = connectionString;
    }

    //Este método se encarga de crear una conexión a la base de datos
    public IDbConnection CreateConnection()
    {
        var connection = new SqlConnection(_connectionString);
        connection.Open();
        return connection;
    }
}