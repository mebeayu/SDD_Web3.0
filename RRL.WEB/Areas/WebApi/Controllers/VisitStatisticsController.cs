/*
 *  lcl 20180526 Insert
 */

using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Http;

using RRL.Config;
using RRL.Core.Manager;
using RRL.Core.Models;
using RRL.Core.Utility;
using RRL.DB;
using RRL.WEB.Models.Request;
using RRL.WEB.Models.Response;
using RRL.WEB.Ulity;
using Common.Utils;

namespace RRL.WEB.Areas.WebApi.Controllers
{
    /// <summary>
    /// 访问统计的WebApi
    /// </summary>
    public partial class VisitStatisticsController : ApiController
    {
        private RdSession rdsession = new RdSession(HttpContext.Current);

        /// <summary>
        /// 记录访问数据
        /// </summary>
        /// <param name="request">包含token的请求</param>
        /// <returns></returns>
        [HttpGet]
        public DataResult RecordsVisitLog([FromUri]VisitDataRequest request)
        {
            int intUserId = 0;

            if (request != null && !string.IsNullOrWhiteSpace(request.token))
            {
                // 获取token对象
                TokenObject tokenObject = TokenObject.InitTokenObjFromString(request.token);
                UserAuth user = new UserAuth();
                if (user.Load(tokenObject.id) == MessageCode.SUCCESS)
                {
                    // 如果用户身份验证通过，则获取用户ID
                    intUserId = user.id;
                }
            }

            // 转换页面访问时间
            DateTime dtVisitTime = DatetimeHelper.JsTimeStampConvertDateTime(request.visitTime);
            // 转换页面离开时间
            DateTime dtLeaveTime = DatetimeHelper.JsTimeStampConvertDateTime(request.leaveTime);

            string strSql = $@"insert into rrl_app_visit_stat_data
                                     ([uid] ,[session_id] ,[visit_time] ,[leave_time] ,[visit_interval] 
                                     ,[page_type] ,[goods_id] ,[visitor_ip] ,[visit_path] ,[deep])
                               values (@uid ,@session_id ,@visit_time ,@leave_time ,@visit_interval
                                      ,@page_type ,@goods_id ,@visitor_ip ,@visit_path ,@deep)";
            new SqlDataBase().Execute(strSql,
                new
                {
                    uid = intUserId,
                    session_id = rdsession.SessionId,
                    visit_time = dtVisitTime,
                    leave_time = dtLeaveTime,
                    visit_interval = request.visitInterval,
                    page_type = request.pageType,
                    goods_id = request.goodsId,
                    visitor_ip = IPUtils.getIPAddress(),
                    visit_path = request.visitPath,
                    deep = request.deep
                });


            return DataResult.InitFromMessageCode(MessageCode.SUCCESS);
        }
    }
}