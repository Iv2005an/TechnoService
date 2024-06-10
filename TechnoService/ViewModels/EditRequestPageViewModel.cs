using CommunityToolkit.Mvvm.ComponentModel;
using TechnoService.Models;

namespace TechnoService.ViewModels;

public partial class EditRequestPageViewModel : ObservableObject
{
    [ObservableProperty]
    private RequestModel _editRequest;
}
