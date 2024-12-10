using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZeroPlay.Model
{
    public class VideoResp
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("id")]
        public ulong Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("author")]
        public Author Author { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("play_url")]
        public string PlayUrl { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("cover_url")]
        public string CoverUrl { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("favorite_count")]
        public ulong FavoriteCount { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("comment_count")]
        public ulong CommentCount { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("is_favorite")]
        public bool IsFavorite { get; set; }

        /// <summary>
        /// 华为三折叠，怎么折都有面！
        /// </summary>
        [JsonProperty("title")]
        public string Title { get; set; }
    }

    public class Author
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("id")]
        public ulong Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("follow_count")]
        public ulong FollowCount { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("follower_count")]
        public ulong FollowerCount { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("is_follow")]
        public bool IsFollow { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("avatar")]
        public string Avatar { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("background_image")]
        public string BackgroundImage { get; set; }

        /// <summary>
        /// 越想贴近事实，不明白的事情就越多。
        /// </summary>
        [JsonProperty("signature")]
        public string Signature { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("total_favorited")]
        public ulong TotalFavorited { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("work_count")]
        public ulong WorkCount { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("favorite_count")]
        public ulong FavoriteCount { get; set; }
    }
}
