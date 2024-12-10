using RestSharp;
using System;
using System.Collections.Generic;
using System.Text.Json.Nodes;
using ZeroPlay.Interface;
using ZeroPlay.Model;

namespace ZeroPlay.Service
{
    internal class CommentClient : ICommentService
    {
        private readonly RestClient _client = new RestClient(Constant.Constant.ZeroPlayServerHost);
        public CommentClient() { }
        public bool TryGetComments(string videoId, string token, out List<Comment> comments)
        {
            var req = new RestRequest("/douyin/comment/list", Method.Get)
                .AddQueryParameter("video_id", videoId)
                .AddQueryParameter("token", token);

            var resp = _client.Execute(req);

            comments = new List<Comment>();

            if (!resp.IsSuccessStatusCode)
            {
                return false;
            }

            if (resp.Content is null || string.IsNullOrWhiteSpace(resp.Content))
            {
                return false;
            }

            var respJson = JsonNode.Parse(resp.Content);

            if (respJson["status_code"]!.GetValue<int>() != 0)
            {
                return false;
            }

            var commentArray = respJson["comment_list"]?.AsArray();
            if (commentArray == null)
            {
                return false;
            }

            foreach (var item in commentArray)
            {
                comments.Add(new Comment
                {
                    Id = item["id"]!.GetValue<int>(),
                    Content = item["content"]!.GetValue<string>(),
                    CreateDate = item["create_date"]!.GetValue<string>(),
                    User = new User
                    {
                        Id = item["user"]["id"]!.GetValue<int>(),
                        Name = item["user"]["name"]!.GetValue<string>(),
                        FollowCount = item["user"]["follow_count"]!.GetValue<int>(),
                        FollowerCount = item["user"]["follower_count"]!.GetValue<int>(),
                        IsFollow = item["user"]["is_follow"]!.GetValue<bool>(),
                        Avatar = item["user"]["avatar"]!.GetValue<string>(),
                        BackgroundImage = item["user"]["background_image"]!.GetValue<string>(),
                        Signature = item["user"]["signature"]!.GetValue<string>(),
                        TotalFavorited = item["user"]["total_favorited"]!.GetValue<int>(),
                        WorkCount = item["user"]["work_count"]!.GetValue<int>(),
                        FavoriteCount = item["user"]["favorite_count"]!.GetValue<int>()
                    }
                });
            }

            return true;
        }


        public bool TryPostComment(string videoId, string token, string content, out string errorMessage)
        {
            var req = new RestRequest("/douyin/comment/action", Method.Post)
                .AddQueryParameter("video_id", videoId)
                .AddQueryParameter("token", token)
                .AddQueryParameter("action_type", "1")
                .AddQueryParameter("comment_text", content);

            var resp = _client.Execute(req);

            if (!resp.IsSuccessStatusCode)
            {
                errorMessage = "Request to server failed. Please check your network.";
                return false;
            }

            if (resp.Content is null || string.IsNullOrWhiteSpace(resp.Content))
            {
                errorMessage = "Server returned an unexpected response. Please try again later.";
                return false;
            }

            var respJson = JsonNode.Parse(resp.Content);

            if (respJson["status_code"]!.GetValue<int>() != 0)
            {
                errorMessage = respJson["status_msg"]!.GetValue<string>();
                return false;
            }

            errorMessage = string.Empty;
            return true;
        }
    }
}
