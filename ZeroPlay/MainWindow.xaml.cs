using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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

            // ʹ���Լ��� Title Bar
            ExtendsContentIntoTitleBar = true;
            SetTitleBar(AppTitleBar);
        }

        private UserDataShareModel UserData => App.GetRequiredService<UserDataShareModel>() ?? 
            throw new ApplicationException("Can not load user data resource.");

        private async void NavigateController_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            // �ж�Ԥ�����õ�ҳ��
            if (NavigateController.SelectedItem == NavigateController.SettingsItem)
            {
                NavigationContentFrame.Content = App.GetRequiredService<SettingsPage>();
            }

            // �������ж��������Զ���ҳ��
            if (NavigateController.SelectedItem is NavigationViewItem selectedItem)
            {
                string pageTag = selectedItem.Tag.ToString()!;
                switch (pageTag)
                {
                    case "HomePage":
                        NavigationContentFrame.Content = App.GetRequiredService<HomePage>();
                        break;
                    case "ProfilePage":
                        // ����·��
                        if (!UserData.IsLogin)
                        {
                            var result = await AskUserToLoginIn();

                            if (!result)
                            {
                                // ���ʧ���ˣ���ô�ͻص���ҳ
                                NavigationContentFrame.Content = App.GetRequiredService<HomePage>();
                                NavigateController.SelectedItem = HomePageItem;
                                return;
                            }
                        }

                        NavigationContentFrame.Content = App.GetRequiredService<ProfilePage>();
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// Ҫ���û���¼
        /// </summary>
        private async Task<bool> AskUserToLoginIn()
        {
            var loginDialogContent = new LoginDialogContent();

            var loginDialog = new ContentDialog
            {
                Title = "��¼/ע��",
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
                // �жϵ������
                if (!client.TryLogin(loginDialogContent.LoginUsername, loginDialogContent.LoginPassword, out var errorOrToken))
                {
                    await new ContentDialog
                    {
                        Title = "��¼ʧ��!",
                        Content = errorOrToken,
                        CloseButtonText = "ȷ��",
                        XamlRoot = Content.XamlRoot,
                    }.ShowAsync();
                    return false;
                }

                UserData.UserToken = errorOrToken;
                UserData.IsLogin = true;
                return true;
            }

            if (!client.TryRegister(loginDialogContent.RegisterUsername, loginDialogContent.RegisterPassword, out var error))
            {
                await new ContentDialog
                {
                    Title = "ע��ʧ��!",
                    Content = error,
                    CloseButtonText = "ȷ��",
                    XamlRoot = Content.XamlRoot,
                }.ShowAsync();
                return false;
            }

            UserData.UserToken = error;
            UserData.IsLogin = true;
            return true;
        }
    }
}