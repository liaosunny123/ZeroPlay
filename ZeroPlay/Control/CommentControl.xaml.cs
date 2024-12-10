using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using ZeroPlay.Interface;
using ZeroPlay.Model;
using ZeroPlay.ShareModel;

namespace ZeroPlay.Control
{
    public sealed partial class CommentControl : UserControl, INotifyPropertyChanged
    {
        private readonly ICommentService _commentService;
        private readonly UserDataShareModel _userDataShareModel;  // 添加 UserDataShareModel
        private bool _isLoading;

        public event PropertyChangedEventHandler? PropertyChanged;

        public CommentControl()
        {
            this.InitializeComponent();
            _commentService = App.GetRequiredService<ICommentService>();
            _userDataShareModel = App.GetRequiredService<UserDataShareModel>();  
            Comments = new ObservableCollection<Comment>();
            PostCommentCommand = new AsyncRelayCommand(PostCommentAsync, CanPostComment);
        }

        public ObservableCollection<Comment> Comments { get; }

        public bool HasNoComments => Comments.Count == 0 && !IsLoading;

        public bool IsLoading
        {
            get => _isLoading;
            private set
            {
                if (_isLoading != value)
                {
                    _isLoading = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(HasNoComments));
                }
            }
        }

        public string VideoId
        {
            get => (string)GetValue(VideoIdProperty);
            set
            {
                SetValue(VideoIdProperty, value);
                _ = LoadCommentsAsync();
            }
        }

        public static readonly DependencyProperty VideoIdProperty =
            DependencyProperty.Register(
                nameof(VideoId),
                typeof(string),
                typeof(CommentControl),
                new PropertyMetadata(string.Empty));

        public string NewCommentText
        {
            get => (string)GetValue(NewCommentTextProperty);
            set
            {
                SetValue(NewCommentTextProperty, value);
                (PostCommentCommand as AsyncRelayCommand)?.NotifyCanExecuteChanged();
            }
        }

        public static readonly DependencyProperty NewCommentTextProperty =
            DependencyProperty.Register(
                nameof(NewCommentText),
                typeof(string),
                typeof(CommentControl),
                new PropertyMetadata(string.Empty));

        public ICommand PostCommentCommand { get; }

        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private bool CanPostComment()
        {
            return !string.IsNullOrWhiteSpace(NewCommentText) && !IsLoading;
        }

        private async Task LoadCommentsAsync()
        {
            if (string.IsNullOrEmpty(VideoId)) return;

            IsLoading = true;

            try
            {
                // 使用 UserDataShareModel 中的 Token
                if (_commentService.TryGetComments(VideoId, _userDataShareModel.UserToken, out var comments))
                {
                    Comments.Clear();
                    foreach (var comment in comments)
                    {
                        Comments.Add(comment);
                    }
                    OnPropertyChanged(nameof(HasNoComments));
                }
                else
                {
                    ShowMessage("Failed to load comments", InfoBarSeverity.Error);
                }
            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message, InfoBarSeverity.Error);
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task PostCommentAsync()
        {
            if (string.IsNullOrWhiteSpace(NewCommentText)) return;

            IsLoading = true;

            try
            {
                // 使用 UserDataShareModel 中的 Token
                if (_commentService.TryPostComment(VideoId, _userDataShareModel.UserToken, NewCommentText, out var errorMessage))
                {
                    NewCommentText = string.Empty;
                    await LoadCommentsAsync();
                    ShowMessage("Comment posted successfully", InfoBarSeverity.Success);
                }
                else
                {
                    ShowMessage(errorMessage, InfoBarSeverity.Error);
                }
            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message, InfoBarSeverity.Error);
            }
            finally
            {
                IsLoading = false;
            }
        }

        private void ShowMessage(string message, InfoBarSeverity severity)
        {
            MessageInfoBar.Message = message;
            MessageInfoBar.Severity = severity;
            MessageInfoBar.IsOpen = true;
        }
    }

    public class AsyncRelayCommand : ICommand
    {
        private readonly Func<Task> _execute;
        private readonly Func<bool>? _canExecute;
        private bool _isExecuting;

        public AsyncRelayCommand(Func<Task> execute, Func<bool>? canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter)
        {
            return !_isExecuting && (_canExecute?.Invoke() ?? true);
        }

        public async void Execute(object? parameter)
        {
            if (_isExecuting) return;

            try
            {
                _isExecuting = true;
                NotifyCanExecuteChanged();
                await _execute();
            }
            finally
            {
                _isExecuting = false;
                NotifyCanExecuteChanged();
            }
        }

        public void NotifyCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}