using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.Media.Core;
using Windows.Media.Playback;
using ZeroPlay.Interface;
using ZeroPlay.Model;
using ZeroPlay.Service;

namespace ZeroPlay.ViewModel
{
    public partial class HomeViewModel : ObservableRecipient
    {
        [ObservableProperty]
        private ObservableCollection<VideoItem> videos = [];

        [ObservableProperty]
        private int currentIndex;

        public static string VideoFilePath = "C:\\Users\\forDece\\source\\repos\\ZeroPlay\\ZeroPlay\\Assets\\video1.mp4";

        private int count = 0;
        public HomeViewModel()
        {
            // 初始化视频列表，这里先用测试数据

            //this.videos = new ObservableCollection<VideoItem>
            //{
            //    new VideoItem
            //    {
            //        VideoUri = MediaSource.CreateFromUri(new Uri(VideoFilePath)),
            //        Title = "Video 1",
            //        Description = "Description 1"
            //    },
            //    new VideoItem
            //    {
            //        VideoUri =  MediaSource.CreateFromUri(new Uri(VideoFilePath)),
            //        Title = "Video 2",
            //        Description = "Description 2"
            //    },
            //    // 可以添加更多测试数据
            //};
            //count = 2;



            currentIndex = 0;
        }

        public void AddVideo(VideoItem video)
        {
            this.Videos.Add(video);
        }

        public int GetSize()
        {
            return Videos.Count;
        }
    }

    public class VideoItem
    {
        public MediaSource VideoUri { get; set; }

        public ulong VideoId { get; set; }

        public string PlayUrl { get; set; }

        public string LikeNumStr { get; set; }


        public string CommentNumStr { get; set; }


        public string Title { get; set; }
        public string Description { get; set; }

        public ulong AuthorId { get; set; }

        public ImageSource AuthorAvatar { get; set; }


        public string AuthorName { get; set; }

    }
}

