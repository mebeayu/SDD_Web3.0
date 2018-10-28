using System.Web.Http;
using RRL.Core.Manager;
using RRL.Core.Utility;
using RRL.WEB.Models.Response;

namespace RRL.WEB.Areas.WebApi.Controllers
{
    /// <summary>
    /// 活动API
    /// </summary>
    public class EventManagerController : ApiController
    {
        EventManager em = new EventManager();

        /// <summary>
        /// 记录活动8
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="mobile">联系电话</param>
        /// <param name="area">区域</param>
        /// <param name="address">地址</param>
        /// <returns>结果对象</returns>
        [HttpGet]
        [ActionName("Eight")]
        private DataResult Eight(string username,string mobile,string area, string address)
        {
            string text = $@"{username},{mobile},{area},{address}";
            int res = em.EventMark(text,8);
            if (res== 0)
            {
                res = MessageCode.SUCCESS;
            }
            else
            {
                res = MessageCode.ERROR_UNKONWN;
            }
            
            return DataResult.InitFromMessageCode(res);
        }
    }
}
