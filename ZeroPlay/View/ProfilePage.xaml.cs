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
using Microsoft.UI.Xaml.Media.Animation;


// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ZeroPlay.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ProfilePage : Page
    {
		private Storyboard? _currentStoryboard = null;

        public ProfileViewModel ViewModel => App.GetRequiredService<ProfileViewModel>() ??
            throw new ApplicationException("Can not load Profile ViewModel.");

        public ProfilePage()
        {
            this.InitializeComponent();
			this.DataContext = ViewModel;
		}

		private void OnSubscribeButtonClick(object sender, RoutedEventArgs e)
		{
			if (!ViewModel.ToggleFollowUser(out string message))
			{
				ShowMessage(message);
			}
		}

		private void ShowMessage(string message)
		{
			if(_currentStoryboard != null)
			{
				_currentStoryboard.Stop();
				_currentStoryboard = null;
			}
			popupText.Text = message;
			var fadeOutAnimation = new DoubleAnimation
			{
				From = 1,
				To = 0,  
				Duration = new Duration(TimeSpan.FromSeconds(3))
			};

			var moveUpAnimation = new DoubleAnimation
			{
				From = 0, // 从当前位置开始
				To = -50, // 向上移动 50 像素
				Duration = new Duration(TimeSpan.FromSeconds(3)) // 设置持续时间
			};

			var translateTransform = new TranslateTransform();
			infoPopup.RenderTransform = translateTransform;
			Storyboard.SetTarget(moveUpAnimation, translateTransform);
			Storyboard.SetTargetProperty(moveUpAnimation, "Y");

			var storyboard = new Storyboard();
			_currentStoryboard = storyboard;
			Storyboard.SetTarget(fadeOutAnimation, infoPopup);
			Storyboard.SetTargetProperty(fadeOutAnimation, "Opacity");

			storyboard.Children.Add(moveUpAnimation);
			storyboard.Children.Add(fadeOutAnimation);
			infoPopup.IsOpen = true;
			storyboard.Begin();
			storyboard.Completed += (s, args) =>
			{
				infoPopup.IsOpen = false;  // 动画完成后关闭 Popup
			};

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

