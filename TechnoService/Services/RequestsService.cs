using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Threading.Tasks;
using TechnoService.Models;

namespace TechnoService.Services;

public class RequestsService
{
    private static async Task Init()
    {
        using SqlConnection connection = Database.Connection;
        using SqlCommand createUsersTableCommand = new(
            "IF OBJECT_ID('requests', 'U') IS NULL " +
            "CREATE TABLE requests(" +
            "id int IDENTITY(1, 1) NOT NULL PRIMARY KEY," +
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
        await createUsersTableCommand.ExecuteNonQueryAsync();
    }
    public static async Task<List<RequestModel>> GetRequests()
    {
        await Init();
        using SqlConnection connection = Database.Connection;
        using SqlCommand getRequestsCommand = new("SELECT * FROM requests", connection);
        using SqlDataReader reader = await getRequestsCommand.ExecuteReaderAsync();
        List<RequestModel> requests = [];
        while (await reader.ReadAsync())
            requests.Add(new(reader));
        return requests;
    }
}
