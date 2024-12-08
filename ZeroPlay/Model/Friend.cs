using System.ComponentModel;
using System.Text.Json.Serialization;

namespace ZeroPlay.Model
{
    public class Friend
    {

        [JsonPropertyName("id")]
        public int Id { get; set; }


        [JsonPropertyName("name")]
        public string Name {  get; set; }= string.Empty;

        [JsonPropertyName("follow_count")]
        public int FollowCount { get; set; }

        [JsonPropertyName("follower_count")] 
        public int FollowerCount { get; set;}

        [JsonPropertyName("is_follow")]
        public bool IsFollow { get; set; }

        [JsonPropertyName("avatar")]
        public string Avatar { get; set; } = string.Empty;

        [JsonPropertyName("background_image")]
        public string FollowImage { get; set; } = string.Empty ;


        [JsonPropertyName("signature")]
        public string Signature {  get; set; } = string.Empty ;

        [JsonPropertyName("total_favorited")]
        public int TotalFavorited { get; set; }

        [JsonPropertyName("work_count")]
        public int WorkCount { get; set; }

        [JsonPropertyName("favorite_count")]
        public int FavoriteCount { get; set; }


    }
}
