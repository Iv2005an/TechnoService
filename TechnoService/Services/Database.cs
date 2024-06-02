using Microsoft.Data.SqlClient;

namespace TechnoService.Services;

public static class Database
{
    private const string _databaseName = "TechnoService";
    private const string _createDatabaseString =
        $"IF NOT EXISTS(" +
        $"SELECT name " +
        $"FROM sys.databases " +
        $"WHERE name = '{_databaseName}') " +
        $"BEGIN " +
        $"CREATE DATABASE {_databaseName}; " +
        $"END ";
    private static string GetConnectionString(string databaseName = "master") =>
        $"Server=localhost\\SQLEXPRESS;" +
        $"Database={databaseName};" +
        $"Trusted_Connection=True;" +
        $"TrustServerCertificate=True;";
    private static void OpenConnection(SqlConnection connection)
    {
        if (connection.State == System.Data.ConnectionState.Closed) connection.Open();
    }

    public static SqlConnection Connection
    {
        get
        {
            SqlConnection connection = new(GetConnectionString());
            OpenConnection(connection);
            if (connection.Database != _databaseName)
            {
                using SqlCommand createDatabaseCommand = new(_createDatabaseString, connection);
                createDatabaseCommand.ExecuteNonQuery();
                if (connection.State == System.Data.ConnectionState.Open) connection.Close();
                connection.ConnectionString = GetConnectionString(_databaseName);
                OpenConnection(connection);
            }
            return connection;
        }
    }
}
