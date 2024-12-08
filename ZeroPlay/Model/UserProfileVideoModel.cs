using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace ZeroPlay.Model
{
    public class UserProfileVideoModel
    {
		public string CoverSrc { get; set; } = string.Empty;
		public string Title { get; set; } = string.Empty;

		public void InitFromJson(JsonNode json)
		{
			CoverSrc = json["cover_url"]!.ToString();
			Title = json["title"]!.ToString();
		}

		public UserProfileVideoModel() { }
		public UserProfileVideoModel(JsonNode jsonNode)
		{
			InitFromJson(jsonNode);
		}
    }
}
