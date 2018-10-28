namespace RRL.WEB.Models.Wx
{
    /// <summary>
    /// 微信认证通行令牌对象
    /// </summary>
    public class WxOauthAccessToken
    {
        /// <summary>
        /// 通行令牌
        /// </summary>
        public string access_token { get; set; }

        /// <summary>
        /// 超时时间
        /// </summary>
        public string expires_in { get; set; }

        /// <summary>
        /// 刷新令牌
        /// </summary>
        public string refresh_token { get; set; }

        /// <summary>
        /// open_id
        /// </summary>
        public string openid { get; set; }

        /// <summary>
        /// 授权作用域
        /// </summary>
        public string scope { get; set; }
    }
}