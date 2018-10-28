using RRL.Core.Models;
using RRL.Core.Utility;
using RRL.WEB.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using RRL.WEB.Models.Request;
using RRL.Config;
using System.Dynamic;
using RRL.DB;
using System.Text;
using RRL.WEB.Ulity;
using Newtonsoft.Json;

namespace RRL.WEB.Areas.WebApi.Controllers
{
    public partial class TradeManagerController  
    {
        /// <summary>
        /// 获取订单分享得次数
        /// </summary>
        /// <param name="orderlist"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpGet]
        [ActionName("GetOrderSharedTimes")]
        public DataResult GetOrderSharedTimes(string orderlist,   string token)
        {
            List<int> order_list =  PublicAPI.StrToIntList(orderlist);
            int res;
            TokenObject Token = TokenObject.InitTokenObjFromString(token);
            if (!string.Equals(TokenObject.ShortTimeToken, Token.Prefix))
            {
                res = MessageCode.ERROR_TOKEN_VALIDATE;
            }
            else
            {
                UserAuth User = new UserAuth();
                res = User.Load(Token.id);
                if (res == MessageCode.SUCCESS)
                {
                    int count= tm.GetOrderSharedTimes(order_list[0]);
                    var result = new DataResult() { status = 0, message ="ok" }; 
                    result.data = count;
                    return result;
                }
            }
            DataResult Resault = DataResult.InitFromMessageCode(res);
            Resault.data = 0;
            return Resault;
        }
         

    }
}