using CommunityToolkit.WinUI.UI.Controls;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using TechnoService.Models;
using TechnoService.ViewModels;

namespace TechnoService.Views;

public sealed partial class UsersPage : Page
{
    public UsersPage() => InitializeComponent();
    private readonly UsersPageViewModel _viewModel = new();
    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        Frame.BackStack.Clear();
        base.OnNavigatedTo(e);
    }
    private void EditUserClick(object sender, RoutedEventArgs e) =>
        Frame.Navigate(typeof(EditUserPage), UsersDataGrid.SelectedItem);
    private void UsersDataGridSorting(object sender, DataGridColumnEventArgs e)
    {
        Func<UserModel, object> sorter = null;
        switch (e.Column.Tag)
        {
            case "Id":
                sorter = (user) => user.Id;
                break;
            case "Login":
                sorter = (user) => user.Login;
                break;
            case "TypeName":
                sorter = (user) => user.TypeName;
                break;
            case "FullName":
                sorter = (user) => user.FullName;
                break;
            default:
                break;
        }
        if (e.Column.SortDirection == null || e.Column.SortDirection == DataGridSortDirection.Descending)
        {
            _viewModel.Users = new(_viewModel.Users.OrderBy(sorter));
            e.Column.SortDirection = DataGridSortDirection.Ascending;
        }
        else
        {
            _viewModel.Users = new(_viewModel.Users.OrderByDescending(sorter));
            e.Column.SortDirection = DataGridSortDirection.Descending;
        }
        foreach (var dgColumn in UsersDataGrid.Columns)
        {
            if (dgColumn.DisplayIndex != e.Column.DisplayIndex)
            {
                dgColumn.SortDirection = null;
            }
        }
    }
    private void UsersDataGridSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (UsersDataGrid.SelectedItem != null) EditUserButton.IsEnabled = true;
        else EditUserButton.IsEnabled = false;
    }
    private void UsersSearchTextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
    {
        if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            if (!string.IsNullOrEmpty(sender.Text))
                UsersDataGrid.ItemsSource = new ObservableCollection<UserModel>(
                    _viewModel.Users.Where((user) => user.IsSuitable(sender.Text)));
            else UsersDataGrid.ItemsSource = _viewModel.Users;
    }
}
