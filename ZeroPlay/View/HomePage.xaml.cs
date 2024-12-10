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
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.UI.Popups;
using ZeroPlay.Interface;
using ZeroPlay.Model;
using ZeroPlay.Service;
using ZeroPlay.ViewModel;
using static ZeroPlay.ViewModel.HomeViewModel;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ZeroPlay.View
{

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HomePage : Page
    {
        public HomeViewModel ViewModel { get; }

        private MediaPlayer _currentMediaPlayer;

        public HomePage()
        {
            this.InitializeComponent();
            ViewModel = new HomeViewModel();

            FetchVideo();
        }

        private void FetchVideo()
        {
            var client = App.GetRequiredService<IZeroPlayService>();

            if (client!.TryFetchVideo(out List<VideoResp> list))
            {
                foreach (var video in list)
                {
                    DispatcherQueue.TryEnqueue(() =>
                    {
                        ViewModel.Videos.Add(new VideoItem
                        {
                            LikeNumStr = $"{video.FavoriteCount}点赞",
                            CommentNumStr = $"{video.CommentCount}评论",
                            Title = video.Title,
                            Description = video.Author.Name,
                            PlayUrl = video.PlayUrl,
                            AuthorAvatar = new BitmapImage(new Uri(video.Author.Avatar)),
                            AuthorName = "@" + video.Author.Name
                        });
                    });
                }
            }
        }


        private async static void FullScreenAndPlayVideoInWebView2(WebView2 webView2)
        {
            await webView2.EnsureCoreWebView2Async(null);
            // 执行JavaScript代码来触发视频自动播放，不同视频网站或视频嵌入方式可能代码略有不同，以下是通用示例
            await webView2.CoreWebView2.ExecuteScriptAsync("var videos = document.getElementsByTagName('video');if (videos[0])videos[0].requestFullscreen()&&videos[0].play();");
        }

        private void VideoFlipView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var container = 
                VideoFlipView.ContainerFromIndex(VideoFlipView.SelectedIndex) as FlipViewItem;

            if (container != null)
            {
               
                var view2 = FindWebView2(container);
                view2.Source = new Uri(ViewModel.Videos[VideoFlipView.SelectedIndex].PlayUrl);


                FullScreenAndPlayVideoInWebView2(view2);
            }

            var curIdx = VideoFlipView.SelectedIndex;

            // 来个双检锁
            if (curIdx > 0 && curIdx + 1 >= ViewModel.GetSize())
            {
                lock (this)
                {

                    if (curIdx > 0 && curIdx + 1 >= ViewModel.GetSize())
                    {
                        FetchVideo();
                        return;

                    };
                }
            }



            //   Debug.WriteLine(
            //sender.GetType()

            //);

            //var c = VideoFlipView.SelectedItem.GetType();




            //var b = a.FindName("MediaPlayer").GetType();

            //Debug.WriteLine(b);

            // 获取当前选中项的MediaPlayerElement
            /*            var container = VideoFlipView.ContainerFromIndex(VideoFlipView.SelectedIndex) as FlipViewItem;
                        if (container != null)
                        {
                            _currentMediaPlayer = FindMediaPlayerElement(container)?.MediaPlayer;
                            if (_currentMediaPlayer != null)
                            {

                                //_currentMediaPlayer.MediaPlayer?.Pause();
                                _currentMediaPlayer?.Play();
                            }
                        }*/
        }

        private async void WebView2_Loaded(object sender, RoutedEventArgs e)
        {

            var webView2 = sender as WebView2;

            if (webView2 != null && VideoFlipView.SelectedItem == webView2.DataContext)
            {
                // 将首个视频全屏
                if (VideoFlipView.SelectedIndex == 0)
                {
                    await webView2.EnsureCoreWebView2Async(null);

                    await webView2.CoreWebView2.ExecuteScriptAsync("var videos = document.getElementsByTagName('video');if (videos[0])videos[0].requestFullscreen();");

                }

            }
        }

        private void LikeButton_Click(object sender, RoutedEventArgs e)
        {

        }


        private void SubmitCommentButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MediaPlayer_Loaded(object sender, RoutedEventArgs e)
        {

            var mediaPlayer = sender as MediaPlayerElement;
            if (mediaPlayer != null && VideoFlipView.SelectedItem == mediaPlayer.DataContext)
            {
                _currentMediaPlayer = mediaPlayer.MediaPlayer;

                if (VideoFlipView.SelectedIndex == 0)
                {
                    mediaPlayer.MediaPlayer.Play();
                }
                //mediaPlayer.MediaPlayer?.Pause();
            }
        }

        private void MediaPlayer_Unloaded(object sender, RoutedEventArgs e)
        {
            var mediaPlayer = sender as MediaPlayerElement;
            if (mediaPlayer != null)
            {
                mediaPlayer.MediaPlayer?.Pause();

                var m = new MediaPlayer();
                //m.SetUriSource(new Uri("C:\\Users\\forDece\\source\\repos\\ZeroPlay\\ZeroPlay\\Assets\\video1.mp4"));
                //mediaPlayer.SetMediaPlayer(m);
                //mediaPlayer.MediaPlayer = ;

                //mediaPlayer.MediaPlayer?.Dispose();
            }
        }


        public static T FindChild<T>(DependencyObject parent) where T : DependencyObject
        {
            if (parent == null)
                return null;

            int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (child is T typedChild)
                    return typedChild;

                // Recursively search deeper into nested containers
                T result = FindChild<T>(child);
                if (result != null)
                    return result;
            }

            return null; // Child not found
        }


        private WebView2 FindWebView2(DependencyObject parent)
        {
            var childCount = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < childCount; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (child is WebView2 mediaPlayer)
                {
                    return mediaPlayer;
                }
                var result = FindWebView2(child);
                if (result != null)
                {
                    return result;
                }
            }
            return null;
        }

        private MediaPlayerElement FindMediaPlayerElement(DependencyObject parent)
        {

            var childCount = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < childCount; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (child is MediaPlayerElement mediaPlayer)
                {
                    return mediaPlayer;
                }
                var result = FindMediaPlayerElement(child);
                if (result != null)
                {
                    return result;
                }
            }
            return null;
        }
    }

}
