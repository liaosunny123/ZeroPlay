using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
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

            //var mediaPlayer = new MediaPlayer();
            //mediaPlayer.Source = MediaSource.CreateFromUri(new Uri("ms-appx:///Assets/example_video.mkv"));
            //mediaPlayer.Play();

            ViewModel = new HomeViewModel();

            //VideoFlipView.Items.Add(new VideoItem
            //{
            //    VideoUri = Windows.Media.Core.MediaSource.CreateFromUri(new Uri("C:\\Users\\forDece\\source\\repos\\ZeroPlay\\ZeroPlay\\Assets\\video1.mp4")),
            //    Title = "Video 1",
            //    Description = "Description 1"
            //});

            ViewModel.AddVideo(new VideoItem
            {
                VideoUri = Windows.Media.Core.MediaSource.CreateFromUri(new Uri(VideoFilePath)),
                Title = "Video " + (ViewModel.GetSize() + 1),
                Description = "Description " + (ViewModel.GetSize() + 1)
            });


            //VideoFlipView.ItemsSource = ViewModel.Videos;

        }

        private void VideoFlipView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {



            // 停止之前的视频
            _currentMediaPlayer?.Pause();

            var curIdx = VideoFlipView.SelectedIndex;
            if (curIdx >= 0 && curIdx + 1 >= ViewModel.GetSize())
            {

                Debug.WriteLine("add.........");
                ViewModel.AddVideo(new VideoItem
                {
                    VideoUri = Windows.Media.Core.MediaSource.CreateFromUri(new Uri(VideoFilePath)),
                    Title = "Video " + (ViewModel.GetSize() + 1),
                    Description = "Description " + (ViewModel.GetSize() + 1)
                });
            };

            Debug.WriteLine(
         sender.GetType()

                );

            //var c = VideoFlipView.SelectedItem.GetType();




            //var b = a.FindName("MediaPlayer").GetType();

            //Debug.WriteLine(b);

            // 获取当前选中项的MediaPlayerElement
            var container = VideoFlipView.ContainerFromIndex(VideoFlipView.SelectedIndex) as FlipViewItem;
            if (container != null)
            {
                _currentMediaPlayer = FindMediaPlayerElement(container)?.MediaPlayer;
                if (_currentMediaPlayer != null)
                {

                    //_currentMediaPlayer.MediaPlayer?.Pause();
                    _currentMediaPlayer?.Play();
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
