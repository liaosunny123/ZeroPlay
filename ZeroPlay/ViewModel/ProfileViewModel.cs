using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media.Imaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using ZeroPlay.Interface;
using ZeroPlay.Model;
using ZeroPlay.Service;
using ZeroPlay.ShareModel;
using static System.Net.WebRequestMethods;

namespace ZeroPlay.ViewModel
{
    public partial class ProfileViewModel : ObservableRecipient
    {
        private UserDataShareModel _userDataShareModel = App.GetRequiredService<UserDataShareModel>() ??
            throw new ApplicationException("Can not load user data resource.");
        private IZeroPlayService _clientService = App.GetRequiredService<IZeroPlayService>() ??
            throw new ApplicationException("Can not load user data resource.");

        public ObservableCollection<UserDataModel> FollowList { get; private set; }
            = new ObservableCollection<UserDataModel>();
        public ObservableCollection<UserProfileVideoModel> PostList { get; private set; }
            = new ObservableCollection<UserProfileVideoModel>();

        public ProfileViewModel()
        {
            UserData.PropertyChanged += (object? sender, PropertyChangedEventArgs e) =>
            {
                switch (e.PropertyName)
                {
                    case nameof(UserData.UserId):
                        OnPropertyChanged(nameof(UidStr)); break;
                    case nameof(UserData.Signature):
                        OnPropertyChanged(nameof(SignatureStr)); break;
                    case nameof(UserData.IsFollow):
                        OnPropertyChanged(nameof(FollowButtonContent)); break;
                    case nameof(UserData.FollowCount):
                        OnPropertyChanged(nameof(FollowTabViewItemHeader)); break;
                    case nameof(UserData.PostedCount):
                        OnPropertyChanged(nameof(PostTabViewItemHeader)); break;
                }
            };
        }

        [ObservableProperty]
        private UserDataModel userData = new UserDataModel();

        public string UidStr => $"uid: {UserData.UserId}";
        public string SignatureStr => $"个性签名: {UserData.Signature}";
        public string FollowButtonContent => UserData.IsFollow ? "取消关注" : "关注";
        public string PostTabViewItemHeader => $"投稿({UserData.PostedCount})";
        public string FollowTabViewItemHeader => $"关注({UserData.FollowCount})";

        public bool RequestUserData(int uid, out string message)
        {
            if (!_clientService.TryGetUserData(uid, _userDataShareModel.UserToken, out message)) return false;
            var userJson = JsonNode.Parse(message)!;

            if (!_clientService.TryGetFollowList(uid, _userDataShareModel.UserToken, out message)) return false;
            var followListJson = JsonNode.Parse(message)!;

            if (!_clientService.TryGetPostList(uid, _userDataShareModel.UserToken, out message)) return false;
            var postListJson = JsonNode.Parse(message)!;

            UserData.InitializeFromJson(userJson);
            FollowList.Clear();
            foreach (var followJson in followListJson.AsArray())
            {
                FollowList.Add(new UserDataModel(followJson!));
            }
            PostList.Clear();
            foreach (var post in postListJson.AsArray())
            {
                PostList.Add(new UserProfileVideoModel(post!));
            }

            return true;
        }

        public bool ToggleFollowUser(out string message)
        {
            if (!_clientService.TrySetFollow(
                UserData.UserId, _userDataShareModel.UserToken, !UserData.IsFollow, out message))
            {
                return false;
            }
            return true;
        }
    }
}
