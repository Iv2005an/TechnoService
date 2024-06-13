using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Threading.Tasks;
using TechnoService.Models;

namespace TechnoService.Services;

public class RequestsService
{
    private static void Init()
    {
        using SqlConnection connection = Database.Connection;
        using SqlCommand createUsersTableCommand = new(
            "IF OBJECT_ID('requests', 'U') IS NULL " +
            "CREATE TABLE requests(" +
            "id int IDENTITY(0, 1) NOT NULL PRIMARY KEY," +
            "start_date datetime NOT NULL DEFAULT GETDATE()," +
            "end_date datetime," +
            "client_id int NOT NULL," +
            "executor_id int NOT NULL," +
            "device nvarchar(max) NOT NULL," +
            "type nvarchar(max) NOT NULL," +
            "description nvarchar(max) NOT NULL," +
            "status tinyint NOT NULL," +
            "FOREIGN KEY (client_id) REFERENCES users (Id)," +
            "FOREIGN KEY (executor_id) REFERENCES users (Id));",
            connection);
        createUsersTableCommand.ExecuteNonQuery();
    }
    public static async Task AddRequest(RequestModel request)
    {
        Init();
        using SqlConnection connection = Database.Connection;
        using SqlCommand addRequestCommand = new(
            "INSERT INTO requests VALUES(" +
            $"GETDATE()," +
            "NULL," +
            $"{request.Client.Id}," +
            $"{request.Executor.Id}," +
            $"N'{request.Device}'," +
            $"N'{request.Type}'," +
            $"N'{request.Description}'," +
            $"{request.StatusIndex});", connection);
        await addRequestCommand.ExecuteNonQueryAsync();
    }
    public static async Task<List<RequestModel>> GetRequests(string condition = null)
    {
        Init();
        string commandString = "SELECT * FROM requests";
        if (!string.IsNullOrEmpty(condition))
        {
            commandString += " WHERE ";
            commandString += condition;
        }
        using SqlConnection connection = Database.Connection;
        using SqlCommand getRequestsCommand = new(commandString, connection);
        using SqlDataReader reader = await getRequestsCommand.ExecuteReaderAsync();
        List<RequestModel> requests = [];
        while (await reader.ReadAsync())
            requests.Add(new(reader));
        return requests;
    }
    public static RequestModel GetRequest(int id)
    {
        Init();
        using SqlConnection connection = Database.Connection;
        using SqlCommand getRequestCommand = new(
            "SELECT * " +
            "FROM requests " +
            $"WHERE id={id};",
            connection);
        using SqlDataReader reader = getRequestCommand.ExecuteReader();
        return reader.Read() ? new RequestModel(reader) : null;
    }
    public static async Task UpdateRequest(RequestModel request)
    {
        Init();
        using SqlConnection connection = Database.Connection;
        using SqlCommand updateRequestCommand = new(
            "UPDATE requests SET " +
            $"start_date='{request.StartDate}'," +
            $"end_date={(request.EndDate is null ? "NULL" : $"'{request.EndDate}'")}," +
            $"client_id={request.Client.Id}," +
            $"executor_id={request.Executor.Id}," +
            $"device=N'{request.Device}'," +
            $"type=N'{request.Type}'," +
            $"description=N'{request.Description}'," +
            $"status={request.StatusIndex} " +
            $"WHERE id={request.Id};",
            connection);
        await updateRequestCommand.ExecuteNonQueryAsync();
    }
    public static List<string> GetTypes()
    {
        Init();
        using SqlConnection connection = Database.Connection;
        using SqlCommand getTypesCommand = new("SELECT DISTINCT type FROM requests;", connection);
        using SqlDataReader reader = getTypesCommand.ExecuteReader();
        List<string> types = ["Все"];
        while (reader.Read()) types.Add(reader.GetString(0));
        return types;
    }
}
