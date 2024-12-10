using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeroPlay.Model;

namespace ZeroPlay.Interface
{
    public interface IZeroPlayService
    {
        /// <summary>
        /// 尝试登录
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="tokenOrRetMsg">成功情况下返回 Token，失败的情况下返回错误的原因</param>
        /// <returns></returns>
        bool TryLogin(string username, string password, out string tokenOrRetMsg);
        /// <summary>
        /// 尝试注册
        /// 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="message">错误的情况下返回原因</param>
        /// <returns></returns>
        bool TryRegister(string username, string password, out string message);
        /// <summary>
        /// 判断提供的 Token 是否有效
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        bool IsTokenValid(string token);

		/// <summary>
		/// 尝试获取用户信息
		/// </summary>
		/// <param name="uid"></param>
		/// <param name="token"></param>
		/// <param name="message">成功时返回user属性下的json字符串，失败的时候返回错误原因</param>
		bool TryGetUserData(int uid, string token, out string message);

		/// <summary>
		/// 尝试关注用户
		/// </summary>
		/// <param name="uid"/>
		/// <param name="token"/>
		/// <param name="follow">设置关注或取消关注</param>
		/// <param name="message"/>
		bool TrySetFollow(int uid, string token, bool follow, out string message);

		bool TryGetFollowList(int uid, string token, out string message);

		bool TryGetPostList(int uid, string token, out string message);





        bool TryFetchVideo(out List<VideoResp> videos);
    }
}
