using Microsoft.Data.SqlClient;

namespace TechnoService.Services;

public static class Database
{
    private const string _connectionString =
        "Server=localhost\\SQLEXPRESS;" +
        "Database=master;" +
        "Trusted_Connection=True;" +
        "TrustServerCertificate=True;";
    private const string _databaseName = "TechnoService";
    private const string _createDatabaseString =
        $"IF NOT EXISTS(" +
        $"SELECT name " +
        $"FROM sys.databases " +
        $"WHERE name = '{_databaseName}') " +
        $"BEGIN " +
        $"CREATE DATABASE TechnoService; " +
        $"END " +
        $"USE '{_databaseName}';";

    private static readonly SqlConnection _connection = new(_connectionString);

    private static void Init()
    {
        OpenConnection();
        if (_connection.Database != _databaseName)
        {
            SqlCommand createDatabaseCommand = new(_createDatabaseString, _connection);
            createDatabaseCommand.ExecuteNonQuery();
        }
    }
    public static void OpenConnection()
    {
        if (_connection.State == System.Data.ConnectionState.Closed) _connection.Open();
    }
    public static void CloseConnection()
    {
        if (_connection.State == System.Data.ConnectionState.Open) _connection.Close();
    }

    public static SqlConnection Connection
    {
        get
        {
            Init();
            return _connection;
        }
    }
}
