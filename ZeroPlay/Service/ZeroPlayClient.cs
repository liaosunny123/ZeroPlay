using Microsoft.Extensions.Hosting;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using ZeroPlay.Interface;
using ZeroPlay.Model;

namespace ZeroPlay.Service
{
    internal class ZeroPlayClient : IZeroPlayService
    {
        private RestClient client = new RestClient(Constant.Constant.ZeroPlayServerHost);
        public ZeroPlayClient() { }

        public bool IsTokenValid(string token)
        {
            var req = new RestRequest("/about", Method.Get)
                .AddQueryParameter("token", token);

            var resp = client.Execute(req);

            if (!resp.IsSuccessStatusCode)
            {
                return false;
            }

            if (resp.Content is null || String.IsNullOrWhiteSpace(resp.Content))
            {
                return false;
            }

            var respJson = JsonNode.Parse(resp.Content)!;

            if (respJson["time_stamp"]!.GetValue<long>() != 0L)
            {
                return false;
            }

            return true;
        }

        public bool TryLogin(string username, string password, out string retMsg)
        {
            var req = new RestRequest("/douyin/user/login/", Method.Post)
                .AddQueryParameter("username", username)
                .AddQueryParameter("password", password);
            var resp = client.Execute(req);

            if (!resp.IsSuccessStatusCode)
            {
				retMsg = "Request to Zeroplay is not successful, check your own network environment please.";
                return false;
            }

            if (resp.Content is null || String.IsNullOrWhiteSpace(resp.Content))
            {
				retMsg = "Unexpected Zeroplay response, please wait for a minute and try again.";
                return false;
            }

            var respJson = JsonNode.Parse(resp.Content)!;

            if (respJson["status_code"]!.GetValue<int>() != 0)
            {
				retMsg = respJson["status_msg"]!.GetValue<string>();
                return false;
            }

			retMsg = respJson.ToString();
            return true;
        }

        public bool TryRegister(string username, string password, out string message)
        {
            var req = new RestRequest("/douyin/user/register/", Method.Post)
                .AddQueryParameter("username", username)
                .AddQueryParameter("password", password);
            var resp = client.Execute(req);

            if (!resp.IsSuccessStatusCode)
            {
                message = "Request to Zeroplay is not successful, check your own network environment please.";
                return false;
            }

            if (resp.Content is null || String.IsNullOrWhiteSpace(resp.Content))
            {
                message = "Unexpected Zeroplay response, please wait for a minute and try again.";
                return false;
            }

            var respJson = JsonNode.Parse(resp.Content)!;

            if (respJson["status_code"]!.GetValue<int>() != 0)
            {
                message = respJson["status_msg"]!.GetValue<string>();
                return false;
            }

            message = respJson.ToString();
            return true;
        }

		public bool TryGetUserData(int uid, string token, out string message)
		{
			var req = new RestRequest("/douyin/user/", Method.Get)
				.AddQueryParameter("user_id", uid)
				.AddQueryParameter("token", token);
			var resp = client.Execute(req);

			if (!resp.IsSuccessStatusCode)
			{
				message = "Request to Zeroplay is not successful, check your network environment please.";
				return false;
			}

			if (resp.Content is null || String.IsNullOrWhiteSpace(resp.Content))
			{
				message = "Unexpected Zeroplay response, please wait for a minute and try again.";
				return false;
			}

			var respJson = JsonNode.Parse(resp.Content)!;
			if (respJson["status_code"]!.GetValue<int>() != 0)
			{
				message = respJson["status_msg"]!.GetValue<string>();
				return false;
			}

			message = respJson["user"]!.ToString();
			return true;
		}

		public bool TrySetFollow(int uid, string token, bool follow, out string message)
		{
			var req = new RestRequest("/douyin/relation/action/", Method.Post)
				.AddQueryParameter("token", token)
				.AddQueryParameter("to_user_id", uid)
				.AddQueryParameter("action_type", follow ? 1 : 2);
			var resp = client.Execute(req);

			if (!resp.IsSuccessStatusCode)
			{
				message = "Request to Zeroplay is not successful, check your network environment please.";
				return false;
			}

			if (resp.Content is null || String.IsNullOrWhiteSpace(resp.Content))
			{
				message = "Unexpected Zeroplay response, please wait for a minute and try again.";
				return false;
			}

			var respJson = JsonNode.Parse(resp.Content)!;
			message = respJson["status_msg"]!.GetValue<string>();
			if (respJson["status_code"]!.GetValue<int>() != 0)
			{
				return false;
			}
			return true;
		}

		public bool TryGetFollowList(int uid, string token, out string message)
		{
			var req = new RestRequest("/douyin/relation/follow/list/", Method.Get)
				.AddParameter("user_id", uid)
				.AddParameter("token", token);
			var resp = client.Execute(req);
			if (!resp.IsSuccessStatusCode)
			{
				message = "Request to Zeroplay is not successful, check your network environment please.";
				return false;
			}

			if (resp.Content is null || String.IsNullOrWhiteSpace(resp.Content))
			{
				message = "Unexpected Zeroplay response, please wait for a minute and try again.";
				return false;
			}

			var respJson = JsonNode.Parse(resp.Content)!;
			if (respJson["status_code"]!.GetValue<int>() != 0)
			{
				message = respJson["status_msg"]!.GetValue<string>();
				return false;
			}
			message = respJson["user_list"]!.ToString();
			return true;
		}

		public bool TryGetPostList(int uid, string token, out string message)
		{
			var req = new RestRequest("/douyin/publish/list/", Method.Get)
				.AddParameter("user_id", uid)
				.AddParameter("token", token);
			var resp = client.Execute(req);
			if (!resp.IsSuccessStatusCode)
			{
				message = "Request to Zeroplay is not successful, check your network environment please.";
				return false;
			}

			if (resp.Content is null || String.IsNullOrWhiteSpace(resp.Content))
			{
				message = "Unexpected Zeroplay response, please wait for a minute and try again.";
				return false;
			}

			var respJson = JsonNode.Parse(resp.Content)!;
			if (respJson["status_code"]!.GetValue<int>() != 0)
			{
				message = respJson["status_msg"]!.GetValue<string>();
				return false;
			}
			message = respJson["video_list"]!.ToString();
			return true;
		}
	}
}
