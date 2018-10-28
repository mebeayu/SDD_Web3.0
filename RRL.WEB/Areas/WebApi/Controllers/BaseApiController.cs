/*
 * STORY #18 lcl 20180515 Insert
 */
using System;
using System.Web.Http;
using RRL.Core.Models;
using RRL.Core.Utility;
using RRL.WEB.Models.Request;

namespace RRL.WEB.Areas.WebApi.Controllers
{
    /// <summary>
    /// Web API基类
    /// </summary>
    public partial class BaseApiController : ApiController
    {
        /// <summary>
        /// Token校验
        /// </summary>
        /// <param name="request">从客户端提交的包含token信息的参数</param>
        /// <param name="user">返回用户基础信息对象</param>
        /// <returns></returns>
        public int CheckToken(BaseTokenRequest request, out UserAuth user)
        {
            user = new UserAuth();

            if (request == null || string.IsNullOrWhiteSpace(request.token))
            {
                // token字符串无效
                return MessageCode.ERROR_TOKEN_LENGTH;
            }

            // 获取token对象
            TokenObject tokenObject = TokenObject.InitTokenObjFromString(request.token);
            return user.Load(tokenObject.id);
        }
    }
}