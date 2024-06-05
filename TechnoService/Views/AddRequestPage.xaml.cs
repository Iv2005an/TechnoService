using Microsoft.UI.Xaml.Controls;
using System.Threading.Tasks;
using TechnoService.Models;
using TechnoService.ViewModels;

namespace TechnoService.Views;

public sealed partial class AddRequestPage : Page
{
    public AddRequestPage(UserModel client)
    {
        InitializeComponent();
        _viewModel.Request.Client = client;
    }

    public readonly AddRequestPageViewModel _viewModel = new();

    public async Task AddRequest()
    {
        await _viewModel.AddRequestCommand.ExecuteAsync(null);
    }
}
