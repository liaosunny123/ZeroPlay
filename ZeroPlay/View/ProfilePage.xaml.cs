using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using ZeroPlay.Control;
using ZeroPlay.ViewModel;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ZeroPlay.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ProfilePage : Page
    {

        public ProfileViewModel ViewModel => App.GetRequiredService<ProfileViewModel>() ??
            throw new ApplicationException("Can not load Profile ViewModel.");

        public ProfilePage()
        {
            this.InitializeComponent();
			this.DataContext = ViewModel;
			DetailedInfoTabView.SizeChanged += (s, e) => { UpdateTabWidths(); };
		}

		private void UpdateTabWidths()
		{
			if (DetailedInfoTabView.TabItems.Count == 0) return;

			// 每个选项卡的宽度 = TabView 的实际宽度 / 选项卡数量
			double tabWidth = DetailedInfoTabView.ActualWidth / DetailedInfoTabView.TabItems.Count;

			foreach (var tab in DetailedInfoTabView.TabItems)
			{
				if (tab is TabViewItem tabViewItem)
				{
					tabViewItem.Width = tabWidth;
				}
			}
		}

	}

	public class StringToImageSourceConverter : IValueConverter
	{
		public object? Convert(object value, Type targetType, object parameter, string language)
		{
			if (value is string uri && !string.IsNullOrEmpty(uri))
			{
				try
				{
					return new BitmapImage(new Uri(uri));
				}
				catch
				{
					return null;
				}
			}
			return null;
		}
		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotImplementedException();
		}
	}
}

