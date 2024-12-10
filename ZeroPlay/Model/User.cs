using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZeroPlay.Model
{
        public class User
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public int FollowCount { get; set; }
            public int FollowerCount { get; set; }
            public bool IsFollow { get; set; }
            public string Avatar { get; set; }
            public string BackgroundImage { get; set; }
            public string Signature { get; set; }
            public int TotalFavorited { get; set; }
            public int WorkCount { get; set; }
            public int FavoriteCount { get; set; }
        }
}
