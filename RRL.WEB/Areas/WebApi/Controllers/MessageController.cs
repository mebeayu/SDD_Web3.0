/*
 * lcl 20180509 Insert
 */

using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Web.Http;

using RRL.Config;
using RRL.Core.Manager;
using RRL.Core.Models;
using RRL.Core.Utility;
using RRL.DB;
using RRL.WEB.Models.Request;
using RRL.WEB.Models.Response;

namespace RRL.WEB.Areas.WebApi.Controllers
{
    /// <summary>
    /// 消息API
    /// </summary>
    public partial class MessageController : ApiController
    {
        /// <summary>
        /// 获取指定用户的消息的分页数据
        /// </summary>
        /// <param name="request">包含分页信息和token的请求</param>
        /// <returns></returns>
        [HttpPost]
        [HttpGet]
        public DataResult MessageByPage([FromBody]PaginateTokenRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.token))
            {
                // token字符串无效，不做处理
                return DataResult.InitFromMessageCode(MessageCode.ERROR_TOKEN_LENGTH);
            }

            // 获取token对象
            TokenObject tokenObj = TokenObject.InitTokenObjFromString(request.token);
            UserAuth User = new UserAuth();
            int res = User.Load(tokenObj.id);
            if (res != MessageCode.SUCCESS)
            {
                // 加载用户数据失败，返回错误信息
                return DataResult.InitFromMessageCode(res);
            }

            string strSql = @"select [id] ,[title] ,[content] ,[remark] ,[viewtimes] ,convert(varchar(19) ,[create_time] ,120) as create_time
                                from sys_msg_message(nolock)
                               where (receiver_id = @uid)
                                 and (is_enable = '1')
                               order by create_time desc";
            List<dynamic> resultList = new SqlDataBase().SelectPage(strSql, request.PageSize, request.Page, new { uid = User.id });


            DataResult dataResult = DataResult.InitFromMessageCode(MessageCode.SUCCESS);
            dataResult.data = resultList;

            return dataResult;
        }

        /// <summary>
        /// 获取指定用户未读消息总数
        /// </summary>
        /// <param name="request">基础token请求</param>
        /// <returns></returns>
        [HttpPost]
        [HttpGet]
        public DataResult UnreadMessageCount([FromBody]BaseTokenRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.token))
            {
                // token字符串无效，不做处理
                return DataResult.InitFromMessageCode(MessageCode.ERROR_TOKEN_LENGTH);
            }

            // 获取token对象
            TokenObject tokenObj = TokenObject.InitTokenObjFromString(request.token);
            UserAuth User = new UserAuth();
            int res = User.Load(tokenObj.id);
            if (res != MessageCode.SUCCESS)
            {
                // 加载用户数据失败，返回错误信息
                return DataResult.InitFromMessageCode(res);
            }

            int intUnreadCount = 0;
            string strSql = @"select count(0) as unread
                                from sys_msg_message(nolock)
                               where (receiver_id = @uid)
                                 and (viewtimes < 1)
                                 and (is_enable = '1')";
            List<dynamic> resultList = new SqlDataBase().Select(strSql, new { uid = User.id });
            if (resultList != null && resultList.Count > 0)
            {
                intUnreadCount = resultList.First().unread;
            }

            DataResult dataResult = DataResult.InitFromMessageCode(MessageCode.SUCCESS);
            dataResult.data = intUnreadCount;

            return dataResult;
        }

        /// <summary>
        /// 将指定用户的未读消息设置为“已读”
        /// </summary>
        /// <param name="request">基础token请求</param>
        /// <returns></returns>
        [HttpPost]
        [HttpGet]
        public DataResult SetAsHasBeenRead([FromBody]BaseTokenRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.token))
            {
                // token字符串无效，不做处理
                return DataResult.InitFromMessageCode(MessageCode.ERROR_TOKEN_LENGTH);
            }

            // 获取token对象
            TokenObject tokenObj = TokenObject.InitTokenObjFromString(request.token);
            UserAuth User = new UserAuth();
            int res = User.Load(tokenObj.id);
            if (res != MessageCode.SUCCESS)
            {
                // 加载用户数据失败，返回错误信息
                return DataResult.InitFromMessageCode(res);
            }

            string strSql = @"update sys_msg_message
                                 set viewtimes = viewtimes + 1
                               where (viewtimes = 0)
                                 and (is_enable = '1')
                                 and (receiver_id = @uid)";
            new SqlDataBase().Execute(strSql, new { uid = User.id });
            return DataResult.InitFromMessageCode(MessageCode.SUCCESS);
        }



    }
}