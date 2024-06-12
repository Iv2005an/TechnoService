using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Threading.Tasks;
using TechnoService.Models;

namespace TechnoService.Services;

public static class CommentsService
{
    private static void Init()
    {
        using SqlConnection connection = Database.Connection;
        using SqlCommand createCommentsTableCommand = new(
            "IF OBJECT_ID('comments', 'U') IS NULL " +
            "CREATE TABLE comments(" +
            "id int IDENTITY(0, 1) NOT NULL PRIMARY KEY," +
            "request_id int NOT NULL," +
            "sender_id int NOT NULL," +
            "send_date datetime NOT NULL DEFAULT GETDATE()," +
            "text nvarchar(max) NOT NULL," +
            "FOREIGN KEY (request_id) REFERENCES requests (Id)," +
            "FOREIGN KEY (sender_id) REFERENCES users (Id));",
            connection);
        createCommentsTableCommand.ExecuteNonQuery();
    }
    public static List<CommentModel> GetComments(int requestId)
    {
        Init();
        using SqlConnection connection = Database.Connection;
        using SqlCommand getCommentsCommand = new(
            $"SELECT * FROM comments WHERE request_id={requestId};", connection);
        using SqlDataReader reader = getCommentsCommand.ExecuteReader();
        List<CommentModel> comments = [];
        while (reader.Read())
            comments.Add(new(reader));
        return comments;
    }
    public static async Task SendComment(CommentModel newComment)
    {
        Init();
        using SqlConnection connection = Database.Connection;
        using SqlCommand insertCommentCommand = new(
            "INSERT INTO comments VALUES(" +
            $"{newComment.Request.Id}," +
            $"{newComment.Sender.Id}," +
            $"GETDATE()," +
            $"N'{newComment.Text}');",
            connection);
        await insertCommentCommand.ExecuteNonQueryAsync();
    }
}
