using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Data;
using TechnoService.Services;

namespace TechnoService.Models;

public partial class RequestModel : ObservableObject
{
    public RequestModel() { }
    public RequestModel(IDataRecord record)
    {
        Id = record.GetInt32(0);
        StartDate = record.GetDateTime(1);
        Client = UsersService.GetUser(record.GetInt32(2));
        Executor = UsersService.GetUser(record.GetInt32(3));
        Device = record.GetString(4);
        Type = record.GetString(5);
        Description = record.GetString(6);
        Status = (StatusTypes)Enum.GetValues(typeof(StatusTypes)).GetValue(record.GetByte(7));
    }

    [ObservableProperty]
    public int _id;
    [ObservableProperty]
    public DateTime _startDate;
    [ObservableProperty]
    public UserModel _client;
    [ObservableProperty]
    public UserModel _executor;
    [ObservableProperty]
    public string _device;
    [ObservableProperty]
    public string _type;
    [ObservableProperty]
    public string _description;
    [ObservableProperty]
    public StatusTypes _status;
}
