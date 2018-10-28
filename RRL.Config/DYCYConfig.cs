namespace RRL.Config
{
    public class DYCYConfig
    {
        #region DataBaseConfig

        //公网
        //public const string DBHost = "39.108.130.27";
        //内网
        public const string DBHost = "127.0.0.1";
        public const string DBPicServer = "pic_storage";
        public const string DBServer = "dycy";
        public const string DBUser = "sa";
        public const string DBPassWord = "dy-123";

        #endregion DataBaseConfig

        #region DocConfig

        public const string DocName = "DYCY接口测试工具";
        public const string PlateName = "端意创源";
        public const string WxJSPayViewTitle = "端意创源微信支付";
        public const string PlateSpreaderCode = "dycy";

        #endregion DocConfig

        #region SMSConfig

        public const string SMSproduct = "Dysmsapi";
        public const string SMSdomain = "dysmsapi.aliyuncs.com";
        public const string SMSaccessKeyId = "LTAIjlejVant5lBe";
        public const string SMSaccessKeySecret = "qpxHqiGyhWwlt6DBPtqdX6BeJh0OGG";
        public const string SMSRequestSignName = "端意创源";
        public const string SMSRequestOutId = "DYCY-SMS";

        #endregion SMSConfig

        #region SMSTemplate

        //验证码短信
        public const string SMSTemplate_AUTH_SMS = "SMS_94775118";

        //下单成功通知（弃用）
        //public const string SMSTemplate_ORDER_PAYMENT_SUCCESS_SMS = "SMS_85410052";
        //下单成功推广
        public const string SMSTemplate_ORDER_PAYMENT_SUCCESS_SMS = "";

        //提现申请通知短信
        public const string SMSTemplate_CASH_APPLY_HANDLE_SMS = "";

        #endregion SMSTemplate

        #region TrackConfig

        public const string Kuaidi100customer = "";
        public const string Kuaidi100key = "";

        #endregion TrackConfig

        #region AliPayConfig

        public const string AliPay_app_id = "";
        public const string AliPay_partner = "";
        public const string AliPay_seller_id = "";
        public const string AliPay_key = "";
        public const string AliPay_notify_url = "";
        public const string AliPay_return_url = "";
        public const string AliPay_private_key_path = "";

        #endregion AliPayConfig

        #region WxPayConfig

        public const string WxPayAPPID = "";
        public const string WxPayMCHID = "";
        public const string WxPayKEY = "";
        public const string WxPayAPPSECRET = "";
        public const string WxPayNOTIFY_URL = "http://www.gyypcdw.com/PayCallBack/wxpay_notify";

        #endregion WxPayConfig

        #region WxMPConfig

        public const string WxMPAPPID = "wx24c6c582a1e1ccba";
        public const string WxMPMCHID = "1487941802";
        public const string WxMPKEY = "5a4d390c775e13d683679eb3823e3b80";
        public const string WxMPAPPSECRET = "1aa178f9662900d5fb62544db5c13950";
        public const string WxMPSPAURI = "http://www.gyypcdw.com/";

        #endregion WxMPConfig
    }
}