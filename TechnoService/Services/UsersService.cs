using Microsoft.Data.SqlClient;

namespace TechnoService.Services;

public static class UsersService
{
    private static bool _isInit = false;
    private static void Init()
    {
        if (!_isInit)
        {
            using SqlCommand createUsersTableCommand = new(
                "CREATE TABLE IF NOT EXIST users(" +
                "id int IDENTITY(1, 1) NOT NULL PRIMARY KEY, " +
                "login nvarchar(50) NOT NULL, " +
                "password nvarchar(128) NOT NULL, " +
                "type tinyint NOT NULL DEFAULT 0)", Database.Connection);
            createUsersTableCommand.ExecuteNonQuery();
            _isInit = true;
        }
    }
}
