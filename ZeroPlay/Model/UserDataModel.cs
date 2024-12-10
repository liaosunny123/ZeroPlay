using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using System.Text.Json.Nodes;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ZeroPlay.Model
{
	public partial class UserDataModel : ObservableRecipient
	{
		[ObservableProperty]
		private int userId;

		[ObservableProperty]
		private string userName = string.Empty;

		[ObservableProperty]
		private string avatarSrc = string.Empty;

		[ObservableProperty]
		private string backgroundImageSrc = string.Empty;

		[ObservableProperty]
		private string signature = string.Empty;

		[ObservableProperty]
		private int followCount;

		[ObservableProperty]
		private int followerCount;

		[ObservableProperty]
		private bool isFollow;

		[ObservableProperty]
		private int totalFavorated;

		[ObservableProperty]
		private int favoriteCount;

		[ObservableProperty]
		private int postedCount;

		public void InitializeFromJson(JsonNode userJson)
		{
			UserId = userJson["id"]!.GetValue<int>();
			UserName = userJson["name"]!.GetValue<string>();
			AvatarSrc = userJson["avatar"]!.GetValue<string>();
			BackgroundImageSrc = userJson["background_image"]!.GetValue<string>();
			FollowCount = userJson["follow_count"]!.GetValue<int>();
			FollowerCount = userJson["follower_count"]!.GetValue<int>();
			IsFollow = userJson["is_follow"]!.GetValue<bool>();
			Signature = userJson["signature"]!.GetValue<string>();
			TotalFavorated = userJson["total_favorited"]!.GetValue<int>();
			FavoriteCount = userJson["favorite_count"]!.GetValue<int>();
			PostedCount = userJson["work_count"]!.GetValue<int>();
		}

		public UserDataModel(JsonNode userJson)
		{
			InitializeFromJson(userJson);
		}

		public UserDataModel() { }
	}
}
