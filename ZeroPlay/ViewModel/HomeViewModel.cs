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

namespace ZeroPlay.ViewModel
{
    public class HomeViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<MediaItem> _mediaItems;
        public ObservableCollection<MediaItem> MediaItems
        {
            get { return _mediaItems; }
            set
            {
                _mediaItems = value;
                OnPropertyChanged();
            }
        }

        public HomeViewModel()
        {
            // 初始化数据集合并添加示例数据
            MediaItems = new ObservableCollection<MediaItem>();

            string pathPrefix = "C:\\Users\\forDece\\source\\repos\\ZeroPlay\\ZeroPlay\\";

            MediaItems.Add(new MediaItem { Image = pathPrefix + "Assets/video1.mp4", Name = "First Media" });
            MediaItems.Add(new MediaItem { Image = pathPrefix + "Assets/img1.png", Name = "Second Media" });
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public class MediaItem
        {
            public string Image { get; set; }
            public string Name { get; set; }
        }

    }
}
