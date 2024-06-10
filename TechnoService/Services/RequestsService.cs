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
            $"{request.Client.Id}," +
            $"{request.Executor.Id}," +
            $"'{request.Device}'," +
            $"'{request.Type}'," +
            $"'{request.Description}'," +
            $"{request.StatusIndex});", connection);
        await addRequestCommand.ExecuteNonQueryAsync();
    }
    public static async Task<List<RequestModel>> GetRequests()
    {
        Init();
        using SqlConnection connection = Database.Connection;
        using SqlCommand getRequestsCommand = new("SELECT * FROM requests", connection);
        using SqlDataReader reader = await getRequestsCommand.ExecuteReaderAsync();
        List<RequestModel> requests = [];
        while (await reader.ReadAsync())
            requests.Add(new(reader));
        return requests;
    }
    public static async Task UpdateRequest(RequestModel request)
    {
        Init();
        using SqlConnection connection = Database.Connection;
        using SqlCommand updateRequestCommand = new(
            "UPDATE requests SET " +
            $"start_date='{request.StartDate}'," +
            $"client_id={request.Client.Id}," +
            $"executor_id={request.Executor.Id}," +
            $"device='{request.Device}'," +
            $"type='{request.Type}'," +
            $"description='{request.Description}' " +
            $"WHERE id={request.Id}",
            connection);
        await updateRequestCommand.ExecuteNonQueryAsync();
    }
}
