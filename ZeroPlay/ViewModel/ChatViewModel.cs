using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.Linq;
using ZeroPlay.Interface;
using ZeroPlay.Model;
using ZeroPlay.Service;
using ZeroPlay.ShareModel;
using Microsoft.UI.Xaml;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using System;

namespace ZeroPlay.ViewModel
{
    public partial class ChatViewModel : ObservableRecipient
    {
        private IChatService _chatService = App.GetRequiredService<IChatService>() ?? throw new ApplicationException("Can not load chat service.");
        private UserDataShareModel _userDataShareModel = App.GetRequiredService<UserDataShareModel>() ??
            throw new ApplicationException("Can not load user data resource.");

        public ObservableCollection<Friend> FriendList { get; set; } = new ();
        public ObservableCollection<Message> MessageList { get; set; } = new ();

        [ObservableProperty]
        private Friend selectedFriend;

        [ObservableProperty]
        private Visibility isChatVisible = Visibility.Collapsed;

        [ObservableProperty]
        private string newMessageContent = string.Empty;

        public ChatViewModel()
        {
            LoadFriend();

        }

        // 加载好友列表
        private async void LoadFriend()
        {
            string errorMsg;
            var friends = _chatService.GetFriendList(
                _userDataShareModel.UserId,  // 使用已登录用户的 ID
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
                Console.WriteLine($"Successfully loaded {friends.Count} friends");
            }
            else
            {
                Console.WriteLine($"Failed to load friends: {errorMsg}");
            }
        }

        // 加载聊天记录
        public async Task LoadMessages()
        {
            if (SelectedFriend == null) return;

            string errorMsg;
            var messages = _chatService.GetMessageList(
                SelectedFriend.Id,  // 使用选中好友的ID
                _userDataShareModel.UserToken,
                0, // 从最开始加载消息
                out errorMsg
            );

            if (messages != null)
            {
                MessageList.Clear();
                foreach (var message in messages)
                {
                    message.IsSentByMe = message.FromUserId == _userDataShareModel.UserId;
                    MessageList.Add(message);
                }
                ScrollToBottom();
            }
            else
            {
                Console.WriteLine($"Failed to load messages: {errorMsg}");
            }
        }

        public void ScrollToBottom()
        {
            if (MessageList.Count > 0)
            {
                // 触发集合变更通知，促使 ListView 滚动到底部
                var lastMessage = MessageList[MessageList.Count - 1];
                MessageList.Remove(lastMessage);
                MessageList.Add(lastMessage);
            }
        }
        // 修改 SendMessageAsync 方法
        [RelayCommand]
        private async Task SendMessageAsync()
        {
            if (SelectedFriend == null || string.IsNullOrWhiteSpace(NewMessageContent)) return;

            string errorMsg;
            bool success = _chatService.SendMessage(SelectedFriend.Id, _userDataShareModel.UserToken, NewMessageContent, out errorMsg);
            if (success)
            {
                // Add the new message to the list
                MessageList.Add(new Message
                {
                    Content = NewMessageContent,
                    FromUserId = _userDataShareModel.UserId,
                    ToUserId = SelectedFriend.Id,
                    CreateTime = DateTimeOffset.Now.ToUnixTimeMilliseconds().ToString(),
                    IsSentByMe = true
                });

                NewMessageContent = string.Empty; // Clear input box
                ScrollToBottom(); // 滚动到底部
            }
            else
            {
                // Handle error
                Console.WriteLine($"Error sending message: {errorMsg}");
            }
        }




        // 模拟加载好友列表
        public void LoadFriendListFake()
        {
            // 使用静态数据代替真实 API 请求
            FriendList.Clear();
            FriendList.Add(new Friend { Id = 1, Name = "张三", Avatar = "avatar1.png" });
            FriendList.Add(new Friend { Id = 2, Name = "李四", Avatar = "avatar2.png" });
            FriendList.Add(new Friend { Id = 3, Name = "王五", Avatar = "avatar3.png" });
        }

        // 模拟加载聊天记录
        public void LoadMessagesFake()
        {
            if (SelectedFriend == null) return;

            // 使用静态数据代替真实 API 请求
            MessageList.Clear();
            MessageList.Add(new Message { Content = "你好！", FromUserId = 1, ToUserId = 2, CreateTime = DateTimeOffset.Now.ToUnixTimeMilliseconds().ToString(), IsSentByMe = false });
            MessageList.Add(new Message { Content = "你好！最近怎么样？", FromUserId = 2, ToUserId = 1, CreateTime = DateTimeOffset.Now.ToUnixTimeMilliseconds().ToString(), IsSentByMe = true });
            MessageList.Add(new Message { Content = "还不错，工作忙。", FromUserId = 1, ToUserId = 2, CreateTime = DateTimeOffset.Now.ToUnixTimeMilliseconds().ToString(), IsSentByMe = false });
        }

    }
}
