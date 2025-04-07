using System.Data;
using Microsoft.Data.SqlClient;

public class DatabaseContext
{
    private readonly string _connectionString;
    public DatabaseContext(string connectionString)
    {
        _connectionString = connectionString;
    }

    public IDbConnection CreateConnection()
        => new SqlConnection(_connectionString);
}
