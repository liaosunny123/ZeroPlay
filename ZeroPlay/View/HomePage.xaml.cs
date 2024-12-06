using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using ZeroPlay.ViewModel;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ZeroPlay.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HomePage : Page
    {

        private HomeViewModel _viewModel;

        public HomePage()
        {
            this.InitializeComponent();

            // 创建视图模型实例
            _viewModel = new HomeViewModel();

            // 设置页面的数据上下文为视图模型，方便进行数据绑定
            this.DataContext = _viewModel;

            // 使用BindingOperations.SetBinding来设置FlipView的ItemsSource绑定
            BindingOperations.SetBinding(flipViewVertical, FlipView.ItemsSourceProperty, new Binding
            {
                Path = new PropertyPath("MediaItems"),
                Source = _viewModel
            });
        }
    }

    // 定义一个简单的数据项类，用于存储每个FlipView展示项对应的图像和名称信息

}
