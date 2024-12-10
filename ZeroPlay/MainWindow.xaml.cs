using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.ApplicationSettings;
using ZeroPlay.Control;
using ZeroPlay.Interface;
using ZeroPlay.ShareModel;
using ZeroPlay.View;
using System.Text.Json.Nodes;
using Windows.Gaming.Input;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ZeroPlay
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();

            // 使用自己的 Title Bar
            ExtendsContentIntoTitleBar = true;
            SetTitleBar(AppTitleBar);
			App.GetRequiredService<HomePage>()!.InitDelegate(ChangeToOtherProfile);
		}

        private UserDataShareModel UserData => App.GetRequiredService<UserDataShareModel>() ?? 
            throw new ApplicationException("Can not load user data resource.");

        private async void NavigateController_SelectionChanged(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            // 判断预先设置的页面
            if (NavigateController.SelectedItem == NavigateController.SettingsItem)
            {
                NavigationContentFrame.Content = App.GetRequiredService<SettingsPage>();
            }

            // 接下来判断其他的自定义页面
            if (NavigateController.SelectedItem is NavigationViewItem selectedItem)
            {
                string pageTag = selectedItem.Tag.ToString()!;
                switch (pageTag)
                {
                    case "HomePage":
                        NavigationContentFrame.Content = App.GetRequiredService<HomePage>();
                        break;
                    case "ProfilePage":
                        // 守卫路由
                        if (!UserData.IsLogin)
                        {
                            var result = await AskUserToLoginIn();

                            if (!result)
                            {
                                // 如果失败了，那么就回到首页
                                NavigationContentFrame.Content = App.GetRequiredService<HomePage>();
                                NavigateController.SelectedItem = HomePageItem;
                                return;
                            }
                        }

						var profilePage = App.GetRequiredService<ProfilePage>()!;
						profilePage.ViewModel.RequestUserData(UserData.UserId, out _);
                        NavigationContentFrame.Content = App.GetRequiredService<ProfilePage>();
                        break;
                    case "CommentTestPage":
                        NavigationContentFrame.Content = App.GetRequiredService<CommentTestPage>();
                        break;
                    case "ChatPage":
                        NavigationContentFrame.Content = App.GetRequiredService<ChatPage>();
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// 要求用户登录
        /// </summary>
        private async Task<bool> AskUserToLoginIn()
        {
            var loginDialogContent = new LoginDialogContent();

            var loginDialog = new ContentDialog
            {
                Title = "登录/注册",
                Content = loginDialogContent,
                IsPrimaryButtonEnabled = false,
                DefaultButton = ContentDialogButton.Close,
                XamlRoot = Content.XamlRoot
            };

            await loginDialog.ShowAsync();

            var client = App.GetRequiredService<IZeroPlayService>()!;

            if (loginDialogContent.IsCancelled)
            {
                return false;
            }

            if (loginDialogContent.IsLogin)
            {
                // 判断登入操作
                if (!client.TryLogin(loginDialogContent.LoginUsername, loginDialogContent.LoginPassword, out var errorOrToken))
                {
                    await new ContentDialog
                    {
                        Title = "登录失败!",
                        Content = errorOrToken,
                        CloseButtonText = "确定",
                        XamlRoot = Content.XamlRoot,
                    }.ShowAsync();
                    return false;
                }

				var loginTokenAndId = JsonNode.Parse(errorOrToken)!;
				UserData.UserToken = loginTokenAndId["token"]!.ToString();
				UserData.UserId = loginTokenAndId["user_id"]!.GetValue<int>();
                UserData.IsLogin = true;
                return true;
            }

            if (!client.TryRegister(loginDialogContent.RegisterUsername, loginDialogContent.RegisterPassword, out var error))
            {
                await new ContentDialog
                {
                    Title = "注册失败!",
                    Content = error,
                    CloseButtonText = "确定",
                    XamlRoot = Content.XamlRoot,
                }.ShowAsync();
                return false;
            }

			var tokenAndId = JsonNode.Parse(error)!;
			UserData.UserToken = tokenAndId["token"]!.ToString();
			UserData.UserId = tokenAndId["user_id"]!.GetValue<int>();
            UserData.IsLogin = true;
            return true;
        }

		public void ChangeToOtherProfile()
		{
			NavigationContentFrame.Content = App.GetRequiredService<ProfilePage>();
		}
	}
}
