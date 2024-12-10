using System.Collections.Generic;
using ZeroPlay.Model;

namespace ZeroPlay.Interface
{
    public interface ICommentService
    {
        /// <summary>
        /// 获取视频的评论列表
        /// </summary>
        /// <param name="videoId">视频 ID</param>
        /// <param name="token">用户 Token</param>
        /// <param name="comments">成功时返回评论列表</param>
        /// <returns>是否获取成功</returns>
        bool TryGetComments(string videoId, string token, out List<Comment> comments);

        /// <summary>
        /// 尝试提交评论
        /// </summary>
        /// <param name="videoId">视频 ID</param>
        /// <param name="token">用户 Token</param>
        /// <param name="content">评论内容</param>
        /// <param name="errorMessage">失败时返回的错误信息</param>
        /// <returns>是否提交成功</returns>
        bool TryPostComment(string videoId, string token, string content, out string errorMessage);
    }
}
