using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeroPlay.Model;

namespace ZeroPlay.Interface
{
    public interface IChatService
    {
        /// <summary>
        /// 获取好友列表
        /// </summary>
        List<Friend>? GetFriendList(int userId, string token, out string errorMsg);

        /// <summary>
        /// 获取消息列表
        /// </summary>
        List<Message>? GetMessageList(int toUserId, string token, long preMsgTime, out string errorMsg);

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="toUserId">接收消息的用户ID</param>
        /// <param name="token">认证Token</param>
        /// <param name="content">消息内容</param>
        /// <param name="errorMsg">错误情况下返回错误原因</param>
        /// <returns>是否成功</returns>
        bool SendMessage(int toUserId, string token, string content, out string errorMsg);
    }
}
