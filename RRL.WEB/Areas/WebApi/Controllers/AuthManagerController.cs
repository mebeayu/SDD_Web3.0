using RRL.Core.External;
using RRL.Core.Manager;
using RRL.Core.Utility;
using RRL.WEB.Models.Response;
using System.Web.Http;

namespace RRL.WEB.Areas.WebApi.Controllers
{
    /// <summary>
    /// 认证API
    /// </summary>
    public class AuthManagerController : ApiController
    {
        private AuthManager am = new AuthManager();

        /// <summary>
        /// 发送短信验证码（已脱离过程）
        /// </summary>
        /// <param name="mobile">手机号码</param>
        /// <returns>结果对象</returns>
        [HttpGet]
        [ActionName("SendSMS")]
        public DataResult SendSMS(string mobile)
        {
            int res;
            if (mobile.Trim() == "" || mobile.Length != 11)
            {
                res = MessageCode.ERROR_BAD_MOBILE;
                return DataResult.InitFromMessageCode(res);
            }
            string code = Rand.Number(6);
            AliSMS sms = new AliSMS();
            res = sms.SendSMS(mobile, SMSTemplate.AUTH_SMS, new AuthSMS { code = code });
            ////触发发送并获取发送状态
            //send_status = 1;
            if (res == MessageCode.SUCCESS)
            {
                res = am.RecordSMS(mobile, code);
            }
            else
            {
                res = MessageCode.ERROR_SEND_CODE;
            }
            return DataResult.InitFromMessageCode(res);
        }
    }
}