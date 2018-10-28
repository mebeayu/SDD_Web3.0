using EasyHttp.Http;
using Newtonsoft.Json;

namespace RRL.Core.Models
{
    public class WxMpMessage
    {
        private WxJsAPICache Cache { get; } = new WxJsAPICache();

        private dynamic WxMpCoreBasePostRequest(string requestUrl, object requestObject = null)
        {
            var http = new HttpClient();
            var result = http.Post(requestUrl, JsonConvert.SerializeObject(requestObject), HttpContentTypes.ApplicationJson);
            return result.Server;
        }

        public dynamic SendTemplateMessage(object templateObject)
        {
            var token = Cache.GetAccessToken();
            var result =
                WxMpCoreBasePostRequest($@"https://api.weixin.qq.com/cgi-bin/message/template/send?access_token={token}",
                    templateObject);

            return result;
        }
    }
}