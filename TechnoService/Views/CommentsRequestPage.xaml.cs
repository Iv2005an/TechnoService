using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using TechnoService.Helpers;
using TechnoService.Models;
using TechnoService.Services;
using TechnoService.Styles;
using TechnoService.ViewModels;

namespace TechnoService.Views;
public sealed partial class CommentsRequestPage : Page
{
    public CommentsRequestPage()
    {
        InitializeComponent();
    }
    private readonly CommentsRequestPageViewModel _viewModel = new();
    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        _viewModel.NewComment = (CommentModel)e.Parameter;
        _viewModel.Comments = new(CommentsService.GetComments(_viewModel.NewComment.Request.Id));
        base.OnNavigatedTo(e);
    }
    private void OnTextChanging(TextBox sender, TextBoxTextChangingEventArgs args)
    {
        TextHelper.TextCharsChecker(sender);
        if (sender.Text.Length > 0)
            sender.BorderBrush = BorderBrushes.TextBoxDefaultBorderBrush;
        else
            sender.BorderBrush = BorderBrushes.TextBoxUncorrectBorderBrush;
    }
    private async void SendButtonClick(object sender, RoutedEventArgs e)
    {
        _viewModel.NewComment.Text = _viewModel.NewComment.Text?.Trim();
        if (!string.IsNullOrEmpty(_viewModel.NewComment.Text))
        {
            await _viewModel.SendCommentCommand.ExecuteAsync(null);
            CommentsListView.ScrollIntoView(CommentsListView.Items[^1]);
        }
    }
}
