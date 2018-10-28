using Aliyun.Acs.Core;
using Aliyun.Acs.Core.Exceptions;
using Aliyun.Acs.Core.Profile;
using Aliyun.Acs.Dysmsapi.Model.V20170525;
using Newtonsoft.Json;
using RRL.Config;
using RRL.Core.Utility;

namespace RRL.Core.External
{
    public class AliSMS
    {
        #region 字段定义

        //短信API产品名称
        private string product = RRLConfig.SMSproduct;

        //短信API产品域名
        private string domain = RRLConfig.SMSdomain;

        //你的accessKeyId
        private string accessKeyId = RRLConfig.SMSaccessKeyId;

        //你的accessKeySecret
        private string accessKeySecret = RRLConfig.SMSaccessKeySecret;

        private IClientProfile profile;

        //private IAcsClient acsClient;

        private SendSmsResponse _sendSmsResponse;

        private SendSmsRequest request;

        #endregion 字段定义

        public AliSMS()
        {
            profile = DefaultProfile.GetProfile("cn-hangzhou", accessKeyId, accessKeySecret);
            DefaultProfile.AddEndpoint("cn-hangzhou", "cn-hangzhou", product, domain);
            //acsClient = new DefaultAcsClient(profile);
            request = new SendSmsRequest
            {
                SignName = RRLConfig.SMSRequestSignName,
                OutId = RRLConfig.SMSRequestOutId
            };
        }

        public int SendSMS(string mobile, string tempCode, object tempParam)
        {
            request.PhoneNumbers = mobile;
            request.TemplateCode = tempCode;
            request.TemplateParam = JsonConvert.SerializeObject(tempParam, Formatting.Indented);
            IAcsClient acsClient = new DefaultAcsClient(profile);
            try
            {
                _sendSmsResponse = acsClient.GetAcsResponse(request);
                var res = _sendSmsResponse.Message;
                return string.Equals(res, "OK") ? MessageCode.SUCCESS : MessageCode.ERROR_SEND_CODE;
            }
            catch (ServerException)
            {
                return MessageCode.ERROR_SMS_SERVER;
            }
            catch (ClientException)
            {
                return MessageCode.ERROR_SMS_CLIENT;
            }
        }
    }
}