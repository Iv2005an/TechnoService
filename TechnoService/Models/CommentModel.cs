using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Data;
using TechnoService.Services;

namespace TechnoService.Models;

public partial class CommentModel : ObservableObject
{
    public CommentModel() { }
    public CommentModel(IDataRecord record)
    {
        Id = record.GetInt32(0);
        Request = RequestsService.GetRequest(record.GetInt32(2));
        Sender = UsersService.GetUser(record.GetInt32(2));
        SendDate = record.GetDateTime(3);
        Text = record.GetString(4);
    }
    [ObservableProperty]
    private int _id;
    [ObservableProperty]
    private RequestModel _request;
    [ObservableProperty]
    private UserModel _sender;
    [ObservableProperty]
    private DateTime _sendDate;
    [ObservableProperty]
    private string _text;
}
