using RRL.Config;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace RRL.Core.External
{
    public class KuaiDi100
    {
        private static readonly string DefaultUserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";

        private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true; //总是接受
        }

        public static HttpWebResponse CreatePostHttpResponse(string url, IDictionary<string, string> parameters, Encoding charset)
        {
            HttpWebRequest request;
            //HTTPSQ请求
            ServicePointManager.ServerCertificateValidationCallback = CheckValidationResult;
            request = WebRequest.Create(url) as HttpWebRequest;
            request.ProtocolVersion = HttpVersion.Version10;
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.UserAgent = DefaultUserAgent;
            //如果需要POST数据
            if (!(parameters == null || parameters.Count == 0))
            {
                var buffer = new StringBuilder();
                var i = 0;
                foreach (var key in parameters.Keys)
                {
                    if (i > 0)
                    {
                        buffer.AppendFormat("&{0}={1}", key, parameters[key]);
                    }
                    else
                    {
                        buffer.AppendFormat("{0}={1}", key, parameters[key]);
                    }
                    i++;
                }
                var data = charset.GetBytes(buffer.ToString());
                using (var stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
            }
            return request.GetResponse() as HttpWebResponse;
        }

        public static string QueryData(string trackCom, string trackNum)
        {
            var url = "http://poll.kuaidi100.com/poll/query.do";
            var encoding = Encoding.GetEncoding("utf-8");

            //参数
            var param = $"{{\"com\":\"{trackCom}\",\"num\":\"{trackNum}\",\"from\":\"\",\"to\":\"\"}}";
            var customer = RRLConfig.Kuaidi100customer;
            var key = RRLConfig.Kuaidi100key;
            var md5 = new MD5CryptoServiceProvider();
            var inBytes = Encoding.GetEncoding("UTF-8").GetBytes(param + key + customer);
            var outBytes = md5.ComputeHash(inBytes);

            var outString = outBytes.Aggregate("", (current, t) => current + t.ToString("x2"));

            var sign = outString.ToUpper();
            IDictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("param", param);
            parameters.Add("customer", customer);
            parameters.Add("sign", sign);
            var response = CreatePostHttpResponse(url, parameters, encoding);
            //打印返回值
            var stream = response.GetResponseStream();   //获取响应的字符串流
            var sr = new StreamReader(stream); //创建一个stream读取流
            var html = sr.ReadToEnd();   //从头读到尾，放到字符串html
            return html;
        }
    }
}