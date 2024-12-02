using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
