using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media.Imaging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using ZeroPlay.Interface;
using ZeroPlay.Model;
using ZeroPlay.Service;
using ZeroPlay.ShareModel;

namespace ZeroPlay.ViewModel
{
    public partial class ProfileViewModel : ObservableRecipient, INotifyPropertyChanged
    {
		private UserDataModel _userDataModel = new UserDataModel();
		private UserDataShareModel _userDataShareModel = App.GetRequiredService<UserDataShareModel>() ??
			throw new ApplicationException("Can not load user data resource.");
		private IZeroPlayService _clientService = App.GetRequiredService<IZeroPlayService>() ?? throw new ApplicationException("Can not load user data resource.");

		public ProfileViewModel()
		{
			UserData.PropertyChanged += (object? sender, PropertyChangedEventArgs e) =>
			{
				if (e.PropertyName == nameof(UserData.UserId)) OnPropertyChanged(nameof(UidStr));
				else if(e.PropertyName == nameof(UserData.Signature)) OnPropertyChanged(nameof(SignatureStr));
			};
		}

		public UserDataModel UserData
		{
			get { return _userDataModel; }
			private set
			{
				UserData = value;
				OnPropertyChanged(nameof(UserData));
			}
		}

		public string UidStr => $"uid: {UserData.UserId}";
		public string SignatureStr => $"个性签名: {UserData.Signature}";

		public bool RequestUserData(int uid)
		{
			if (!_clientService.TryGetUserData(uid, _userDataShareModel.UserToken, out string message)) return false;
			var userJson = JsonNode.Parse(message)!;
			UserData.UserId = userJson["id"]!.GetValue<int>();
			UserData.UserName = userJson["name"]!.GetValue<string>();
			UserData.AvatarSrc = userJson["avatar"]!.GetValue<string>();
			UserData.BackgroundImageSrc = userJson["background_image"]!.GetValue<string>();
			UserData.FollowCount = userJson["follow_count"]!.GetValue<int>();
			UserData.FollowerCount = userJson["follower_count"]!.GetValue<int>();
			UserData.IsFollow = userJson["is_follow"]!.GetValue<bool>();
			UserData.Signature = userJson["signature"]!.GetValue<string>();
			UserData.TotalFavorated = userJson["total_favorited"]!.GetValue<int>();
			UserData.FavoriteCount = userJson["favorite_count"]!.GetValue<int>();
			UserData.PostedCount = userJson["work_count"]!.GetValue<int>();
			return true;
		}
    }
}
