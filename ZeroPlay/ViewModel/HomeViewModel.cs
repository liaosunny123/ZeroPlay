using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
            // 初始化数据集合并添加示例数据q
            MediaItems = new ObservableCollection<MediaItem>();
            MediaItems.Add(new MediaItem { Image = "Assets/Image1.jpg", Name = "First Media" });
            MediaItems.Add(new MediaItem { Image = "Assets/Image2.jpg", Name = "Second Media" });
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
