using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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

            // 使用自己的 Title Bar
            ExtendsContentIntoTitleBar = true;
            SetTitleBar(AppTitleBar);
        }

        private void NavigateController_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
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
                        NavigationContentFrame.Content = App.GetRequiredService<ProfilePage>();
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
