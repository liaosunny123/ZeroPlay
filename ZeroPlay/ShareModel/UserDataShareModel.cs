using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZeroPlay.ShareModel
{
    public partial class UserDataShareModel : ObservableRecipient
    {

        [ObservableProperty]
        private bool isLogin;

        [ObservableProperty]
        private string userToken;

        [ObservableProperty]
        private int userId; // 添加 UserId
        public UserDataShareModel() { }
    }
}
