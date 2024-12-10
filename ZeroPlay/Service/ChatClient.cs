using RestSharp;
using System;
using System.Text.Json.Nodes;
using ZeroPlay.Interface;
using System.Collections.Generic;
using System.Text.Json;
using ZeroPlay.Model;

namespace ZeroPlay.Service
{
    internal class ChatClient : IChatService
    {
        private RestClient client = new RestClient(Constant.Constant.ZeroPlayServerHost);
        public ChatClient() { }
        public List<Friend>? GetFriendList(int userId, string token, out string errorMsg)
        {
            var req = new RestRequest("/douyin/relation/friend/list", Method.Get)
                .AddQueryParameter("user_id", userId.ToString())
                .AddQueryParameter("token", token);

            var resp = client.Execute(req);

            if (!resp.IsSuccessStatusCode)
            {
                errorMsg = "请求失败，请检查网络环境或服务状态。";
                return null;
            }

            if (string.IsNullOrWhiteSpace(resp.Content))
            {
                errorMsg = "服务器返回了空的响应，请稍后重试。";
                return null;
            }

            var respJson = JsonNode.Parse(resp.Content)!;

            if (respJson["status_code"]!.GetValue<int>() != 0)
            {
                errorMsg = respJson["status_msg"]!.GetValue<string>();
                return null;
            }

            var userList = respJson["user_list"]!.Deserialize<List<Friend>>();
            errorMsg = string.Empty;
            return userList;
        }

        public List<Message>? GetMessageList(int toUserId, string token, long preMsgTime, out string errorMsg)
        {
            var req = new RestRequest("/douyin/message/chat", Method.Get)
                .AddQueryParameter("to_user_id", toUserId.ToString())
                .AddQueryParameter("token", token)
                .AddQueryParameter("pre_msg_time", preMsgTime.ToString());

            var resp = client.Execute(req);

            if (!resp.IsSuccessStatusCode)
            {
                errorMsg = "请求失败，请检查网络环境或服务状态。";
                return null;
            }

            if (string.IsNullOrWhiteSpace(resp.Content))
            {
                errorMsg = "服务器返回了空的响应，请稍后重试。";
                return null;
            }

            var respJson = JsonNode.Parse(resp.Content)!;

            if (respJson["status_code"]!.GetValue<int>() != 0)
            {
                errorMsg = respJson["status_msg"]!.GetValue<string>();
                return null;
            }

            var messageList = respJson["message_list"]!.Deserialize<List<Message>>();
            errorMsg = string.Empty;
            return messageList;
        }

        public bool SendMessage(int toUserId, string token, string content, out string errorMsg)
        {
            var req = new RestRequest("/douyin/message/action/", Method.Post)
                .AddQueryParameter("token", token)
                .AddQueryParameter("to_user_id", toUserId.ToString())
                .AddQueryParameter("action_type", "1") // 固定为1表示发送消息
                .AddQueryParameter("content", content);

            var resp = client.Execute(req);

            if (!resp.IsSuccessStatusCode)
            {
                errorMsg = "请求失败，请检查网络环境或服务状态。";
                return false;
            }

            if (string.IsNullOrWhiteSpace(resp.Content))
            {
                errorMsg = "服务器返回了空的响应，请稍后重试。";
                return false;
            }

            var respJson = JsonNode.Parse(resp.Content)!;

            if (respJson["status_code"]!.GetValue<int>() != 0)
            {
                errorMsg = respJson["status_msg"]!.GetValue<string>();
                return false;
            }

            errorMsg = string.Empty;
            return true;
        }
    }
}
