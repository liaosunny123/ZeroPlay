using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.Linq;
using ZeroPlay.Interface;
using ZeroPlay.Model;
using ZeroPlay.Service;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Controls;
using ZeroPlay.ShareModel;
using Microsoft.UI.Xaml;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using System;
using System.ComponentModel;

namespace ZeroPlay.ViewModel
{
    public partial class ChatViewModel : ObservableRecipient
    {
        private DispatcherTimer _timer;
        private readonly IChatService _chatService = App.GetRequiredService<IChatService>() ?? 
            throw new ApplicationException("Can not load chat service.");
        private readonly UserDataShareModel _userDataShareModel = App.GetRequiredService<UserDataShareModel>() ??
            throw new ApplicationException("Can not load user data resource.");

        public ObservableCollection<Friend> FriendList { get; set; } = new();
        public ObservableCollection<Message> MessageList { get; set; } = new();

        [ObservableProperty]
        private Friend selectedFriend;

        [ObservableProperty]
        private Visibility isChatVisible = Visibility.Collapsed;

        [ObservableProperty]
        private string newMessageContent = string.Empty;

        private bool _isAutoScrollEnabled = true;

        public ChatViewModel()
        {
            // 监听用户数据变更
            _userDataShareModel.PropertyChanged += UserDataShareModel_PropertyChanged;

            // 如果已经登录，则加载好友列表
            if (_userDataShareModel.IsLogin)
            {
                LoadFriend();
            }

            InitializeMessageTimer();
        }

        private void UserDataShareModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(UserDataShareModel.IsLogin) && _userDataShareModel.IsLogin)
            {
                LoadFriend();
            }
        }

        private void InitializeMessageTimer()
        {
            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(5)
            };
            _timer.Tick += async (s, e) => await CheckNewMessages();
            _timer.Start();
        }

        private async Task CheckNewMessages()
        {
            if (SelectedFriend == null) return;

            long lastMessageTime = MessageList.Any()
                ? long.Parse(MessageList.Last().CreateTime)
                : 0;

            await LoadMessages(lastMessageTime);
        }

        private void LoadFriend()
        {
            string errorMsg;
            var friends = _chatService.GetFriendList(
                _userDataShareModel.UserId,
                _userDataShareModel.UserToken,
                out errorMsg
            );

            if (friends != null)
            {
                FriendList.Clear();
                foreach (var friend in friends)
                {
                    FriendList.Add(friend);
                }
            }
        }

        public async Task LoadMessages(long fromMessageId = 0)
        {
            if (SelectedFriend == null) return;

            string errorMsg;
            var messages = _chatService.GetMessageList(
                SelectedFriend.Id,
                _userDataShareModel.UserToken,
                fromMessageId,
                out errorMsg
            );

            if (messages?.Any() != true) return;

            var wasAtBottom = IsScrollAtBottom();
            var newMessages = messages.Where(newMsg =>
                !MessageList.Any(existingMsg => existingMsg.CreateTime == newMsg.CreateTime));

            foreach (var message in newMessages)
            {
                message.IsSentByMe = message.FromUserId == _userDataShareModel.UserId;
                MessageList.Add(message);
            }

            if (wasAtBottom || _isAutoScrollEnabled)
            {
                await Task.Delay(100);
                ScrollToBottom();
            }
        }

        [RelayCommand]
        private async Task SendMessageAsync()
        {
            if (SelectedFriend == null || string.IsNullOrWhiteSpace(NewMessageContent)) return;

            string errorMsg;
            bool success = _chatService.SendMessage(
                SelectedFriend.Id,
                _userDataShareModel.UserToken,
                NewMessageContent,
                out errorMsg);

            if (success)
            {
                _isAutoScrollEnabled = true;
                MessageList.Add(new Message
                {
                    Content = NewMessageContent,
                    FromUserId = _userDataShareModel.UserId,
                    ToUserId = SelectedFriend.Id,
                    CreateTime = DateTimeOffset.Now.ToUnixTimeMilliseconds().ToString(),
                    IsSentByMe = true
                });

                NewMessageContent = string.Empty;
                await Task.Delay(100);
                ScrollToBottom();
            }
        }

        private bool IsScrollAtBottom()
        {
            var scrollViewer = GetScrollViewer();
            if (scrollViewer == null) return false;

            var verticalOffset = scrollViewer.VerticalOffset;
            var scrollableHeight = scrollViewer.ScrollableHeight;

            return Math.Abs(verticalOffset - scrollableHeight) < 1.0;
        }

        private ScrollViewer _scrollViewer;

        public void SetScrollViewer(ScrollViewer scrollViewer)
        {
            _scrollViewer = scrollViewer;
        }

        private ScrollViewer GetScrollViewer()
        {
            return _scrollViewer;
        }

        public void ScrollToBottom()
        {
            var scrollViewer = GetScrollViewer();
            scrollViewer?.ChangeView(null, scrollViewer.ScrollableHeight, null);
        }
    }
}