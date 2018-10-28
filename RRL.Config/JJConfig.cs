namespace RRL.Config
{
    public class JJConfig
    {
        #region DataBaseConfig

        //公网
        public const string DBHost = "120.77.100.120";

        public const string DBPicServer = "JJ_pic_storage";
        public const string DBServer = "JJ_three";
        public const string DBUser = "sa";
        public const string DBPassWord = "rrl-123456";

        #endregion DataBaseConfig

        #region DocConfig

        public const string DocName = "聚e聚接口测试工具";
        public const string PlateName = "聚e聚";
        public const string WxJSPayViewTitle = "聚e聚微信支付";
        public const string PlateSpreaderCode = "juju";

        #endregion DocConfig

        #region SMSConfig

        public const string SMSproduct = "Dysmsapi";
        public const string SMSdomain = "dysmsapi.aliyuncs.com";
        public const string SMSaccessKeyId = "LTAIi02YucCih8vK";
        public const string SMSaccessKeySecret = "520YP832pbUcMrYhLCNAZqEnGe8vmO";
        public const string SMSRequestSignName = "聚e聚";
        public const string SMSRequestOutId = "JJ-SMS";

        #endregion SMSConfig

        #region SMSTemplate

        //验证码短信
        public const string SMSTemplate_AUTH_SMS = "SMS_91895085";

        //下单成功推广//非企业用户，无法使用
        public const string SMSTemplate_ORDER_PAYMENT_SUCCESS_SMS = "";

        //提现申请通知短信//非企业用户，无法使用
        public const string SMSTemplate_CASH_APPLY_HANDLE_SMS = "";

        #endregion SMSTemplate

        #region TrackConfig

        public const string Kuaidi100customer = "";
        public const string Kuaidi100key = "";

        #endregion TrackConfig

        #region AliPayConfig

        public const string AliPay_app_id = "2017032206343911";
        public const string AliPay_partner = "2088621281918420";
        public const string AliPay_seller_id = "15288167190@sina.cn";
        public const string AliPay_key = "g4yf9xmv4px5j132ukp055t9ux189wbc";
        public const string AliPay_notify_url = "https://e-shop.szrundao.com/PayCallBack/alipay_notify";
        public const string AliPay_return_url = "";
        public const string AliPay_private_key_path = "CERT\\alipaykey\\JJ\\rsa_private_key.pem";

        #endregion AliPayConfig

        #region WxPayConfig

        public const string WxPayAPPID = "wxce5b34b5fc822977";
        public const string WxPayMCHID = "1480799852";
        public const string WxPayKEY = "JL3P85bcQwFKKJO7Vg1mZ6SgFDfBx6wF";
        public const string WxPayAPPSECRET = "9eb9fb8901280227fc5424ec8d879232";
        public const string WxPayNOTIFY_URL = "https://e-shop.szrundao.com/PayCallBack/wxpay_notify";

        #endregion WxPayConfig

        #region WxMPConfig

        public const string WxMPAPPID = "wx45433edc50e68210";
        public const string WxMPMCHID = "1445439002";
        public const string WxMPKEY = "JL3P85bcQwFKKJO7Vg1mZ6SgFDfBx6wF";
        public const string WxMPAPPSECRET = "383c472b8fc83de92fd7f4c5de319e4b";
        public const string WxMPSPAURI = "https://e-shop.szrundao.com/";

        #endregion WxMPConfig
    }
}