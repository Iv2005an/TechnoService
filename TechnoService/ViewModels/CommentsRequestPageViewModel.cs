using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using TechnoService.Models;
using TechnoService.Services;

namespace TechnoService.ViewModels;

public partial class CommentsRequestPageViewModel : ObservableObject
{
    [ObservableProperty]
    private ObservableCollection<CommentModel> _comments;
    [ObservableProperty]
    private CommentModel _newComment = new();
    [RelayCommand]
    private async Task SendComment()
    {
        await CommentsService.SendComment(NewComment);
        NewComment.Text = null;
        Comments = new(CommentsService.GetComments(NewComment.Request.Id));
    }
}
