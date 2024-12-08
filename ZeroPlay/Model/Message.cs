using System;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace ZeroPlay.Model
{
    public class Message 
    {

        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("content")]
        public string Content { get; set; } = string.Empty;

        [JsonPropertyName("create_time")]
        public string CreateTime { get; set; } = string.Empty;

        [JsonPropertyName("from_user_id")]
        public int FromUserId { get; set; }

        [JsonPropertyName("to_user_id")]
        public int ToUserId { get; set; }

        public DateTime FormattedTime
        {
            get
            {
                long timestamp = long.Parse(CreateTime);
                return DateTimeOffset.FromUnixTimeMilliseconds(timestamp).ToLocalTime().DateTime;
            }
        }

        public bool IsSentByMe { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; } = string.Empty ;
    }
}
