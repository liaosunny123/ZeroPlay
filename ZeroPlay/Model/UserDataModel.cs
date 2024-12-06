using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;

namespace ZeroPlay.Model
{
	public class UserDataModel : INotifyPropertyChanged
	{
		private int _userId;
		private string _userName = string.Empty;
		private string _avatarSrc = string.Empty;
		private string _backgroundImageSrc = string.Empty;
		private string _signature = string.Empty;
		private int _followCount;
		private int _followerCount;
		private bool _isFollow;
		private int _totalFavorated;
		private int _favoriteCount;
		private int _postedCount;

		public int UserId {
			get => _userId;
			set
			{
				_userId = value;
				OnPropertyChanged(nameof(UserId));
			}
		}
		public string UserName 
		{
			get => _userName;
			set
			{
				_userName = value;
				OnPropertyChanged(nameof(UserName));
			}
		}
		public string AvatarSrc {
			get => _avatarSrc;
			set
			{
				_avatarSrc = value;
				OnPropertyChanged(nameof(AvatarSrc));
			}
		}
		public string BackgroundImageSrc {
			get => _backgroundImageSrc;
			set
			{
				_backgroundImageSrc = value;
				OnPropertyChanged(nameof(BackgroundImageSrc));
			}
		}
		public int FollowCount 
		{
			get => _followCount;
			set
			{
				_followCount = value;
				OnPropertyChanged(nameof(FollowCount));
			}
		}
		public int FollowerCount 
		{
			get => _followerCount;
			set
			{
				_followerCount = value;
				OnPropertyChanged(nameof(FollowerCount));
			}
		}
		public bool IsFollow 
		{
			get => _isFollow;
			set
			{
				_isFollow = value;
				OnPropertyChanged(nameof(IsFollow));
			}
		}
		public string Signature 
		{
			get => _signature;
			set
			{
				_signature = value;
				OnPropertyChanged(nameof(Signature));
			}
		}
		public int TotalFavorated 
		{
			get => _totalFavorated;
			set
			{
				_totalFavorated = value;
				OnPropertyChanged(nameof(TotalFavorated));
			}
		}
		public int FavoriteCount 
		{
			get => _favoriteCount;
			set
			{
				_favoriteCount = value;
				OnPropertyChanged(nameof(FavoriteCount));
			}
		}
		public int PostedCount 
		{
			get => _postedCount;
			set
			{
				_postedCount = value;
				OnPropertyChanged(nameof(PostedCount));
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
