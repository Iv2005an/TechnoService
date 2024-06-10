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

    public int StatusIndex => Convert.ToInt32(Status);
    public string StatusName =>
    new string[] {
            "В ожидании",
            "В процессе",
            "Выполнено",
            "Не выполнено" }[StatusIndex];

    public bool IsSuitable(string searchText) =>
        Id.ToString().Contains(searchText) ||
        StartDate.ToString().Contains(searchText) ||
        Client.FullName.Contains(searchText) ||
        Executor.FullName.Contains(searchText) ||
        Device.Contains(searchText) ||
        Type.Contains(searchText) ||
        Description.Contains(searchText) ||
        StatusName.Contains(searchText);
}
