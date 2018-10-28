using System.Collections.Generic;

namespace RRL.Core.Models.WxJSAPI
{
    /// <summary>
    /// 调用AccessTonen成功
    /// </summary>
    public class SuccessAccessToken
    {
        /// <summary>
        /// token
        /// </summary>
        public string access_token { get; set; }

        /// <summary>
        /// 过期时间
        /// </summary>
        public int expires_in { get; set; }
    }

    /// <summary>
    /// 调用AccessToken失败
    /// </summary>
    public class ErrorAccessToken
    {
        /// <summary>
        /// 错误编码
        /// </summary>
        public int errcode { get; set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        public string errmsg { get; set; }
    }

    /// <summary>
    /// 微信请求票据
    /// </summary>
    public class JsApiTicket
    {
        /// <summary>
        /// 错误编码
        /// </summary>
        public int errcode { get; set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        public string errmsg { get; set; }

        /// <summary>
        /// 请求票据
        /// </summary>
        public string ticket { get; set; }

        /// <summary>
        /// 票据过期时间
        /// </summary>
        public int expires_in { get; set; }
    }

    /// <summary>
    /// 微信JsAPI配置对象
    /// </summary>
    public class JsConfigObject
    {
        /// <summary>
        /// 是否开启调试
        /// </summary>
        public bool debug { get; set; }

        /// <summary>
        /// 公众号AppID
        /// </summary>
        public string appId { get; set; }

        /// <summary>
        /// 签名时间戳
        /// </summary>
        public long timestamp { get; set; }

        /// <summary>
        /// 随机字符串
        /// </summary>
        public string nonceStr { get; set; }

        /// <summary>
        /// 签名
        /// </summary>
        public string signature { get; set; }

        /// <summary>
        /// 所需Api列表
        /// </summary>
        public List<string> jsApiList { get; set; }
    }

    public class JsPayConfigObject
    {
        /// <summary>
        /// 公众号ID
        /// </summary>
        public string appId { get; set; }

        /// <summary>
        /// 签名时间戳
        /// </summary>
        public long timeStamp { get; set; }

        /// <summary>
        /// 随机字符串
        /// </summary>
        public string nonceStr { get; set; }

        /// <summary>
        /// 预支付订单号
        /// </summary>
        public string package { get; set; }

        /// <summary>
        ///  签名方式
        /// </summary>
        public string signType { get; set; }

        /// <summary>
        /// 支付签名
        /// </summary>
        public string paySign { get; set; }
    }
}