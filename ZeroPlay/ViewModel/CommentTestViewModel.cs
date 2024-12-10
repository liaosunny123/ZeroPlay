// CommentTestViewModel.cs
using System.ComponentModel;
using System.Runtime.CompilerServices;

public class CommentTestViewModel : INotifyPropertyChanged
{
    private string _videoId = string.Empty;

    public event PropertyChangedEventHandler? PropertyChanged;

    public string VideoId
    {
        get => _videoId;
        set
        {
            if (_videoId != value)
            {
                _videoId = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(HasVideoId));
                OnPropertyChanged(nameof(CurrentVideoIdDisplay));
            }
        }
    }

    public bool HasVideoId => !string.IsNullOrWhiteSpace(VideoId);
    public string CurrentVideoIdDisplay => HasVideoId ? $"当前 Video ID: {VideoId}" : "请输入 Video ID";

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}