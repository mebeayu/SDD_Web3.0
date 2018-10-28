namespace RRL.Config
{
    public class RRLConfig
    {
        /// <summary>
        /// 平台拥有的豆数量
        /// </summary>
        public static decimal PLATFORM_HOLE_H_MONEY = 30000m;


        // lcl 2018-09-08 Insert
        /// <summary>
        /// 人民币兑换金豆的比率
        /// </summary>
        public static decimal RMB_To_GoldBean_Rate = 1m;

        #region RedisConfig

        public static string RedisConn = System.Configuration.ConfigurationManager.AppSettings["RedisConn"]; // System.Configuration."127.0.0.1:6379";
        public const int RedisDbNum = 0;

        #endregion RedisConfig

        #region DataBaseConfig


        public static string DBHost = System.Configuration.ConfigurationManager.AppSettings["DBHost"];
        public static string DBPicHost = System.Configuration.ConfigurationManager.AppSettings["DBPicHost"];
        public static string DBServer = System.Configuration.ConfigurationManager.AppSettings["DBServer"];
        public static string DBPicServer = System.Configuration.ConfigurationManager.AppSettings["DBPicServer"];
        public static string DBUser = System.Configuration.ConfigurationManager.AppSettings["DBUser"];
        public static string DBPicUser = System.Configuration.ConfigurationManager.AppSettings["DBPicUser"];
        public static string DBPassWord = System.Configuration.ConfigurationManager.AppSettings["DBPassWord"];
        public static string DBPicPassWord = System.Configuration.ConfigurationManager.AppSettings["DBPicPassWord"];

        public static string DB_ConnectString = string.Format(@"Data Source={0};Initial Catalog={1};Persist Security Info=True;User ID={2};Password={3};Pooling=true;min pool size=5;max pool size=512", DBHost, DBServer, DBUser, DBPassWord);
        #endregion DataBaseConfig

        #region DocConfig

        public const string DocName = "省兜兜接口测试工具";
        public const string PlateName = "省兜兜";
        public const string WxJSPayViewTitle = "省兜兜微信支付";
        public const string PlateSpreaderCode = "shengdoudou";
        public const string BaseUrl = "https://e-shop.rrlsz.com.cn/";

        #endregion DocConfig

        /// <summary>
        /// 锁定交易描述
        /// </summary>
        public static string LOCKED_TRADE_DESCRIPTION = System.Configuration.ConfigurationManager.AppSettings["locked_trade_description"];
        public static string USA_CAN_NOT_SEEN_GAME_ENTER = System.Configuration.ConfigurationManager.AppSettings["usa_can_not_seen_game_enter"];


        #region SMSConfig

        public const string SMSproduct = "Dysmsapi";
        public const string SMSdomain = "dysmsapi.aliyuncs.com";
        public const string SMSaccessKeyId = "LTAI6JqvZ5uAVIWY";
        public const string SMSaccessKeySecret = "A6PyFpSmLbyL9AkwiYq8eenY7OElTV";
        public const string SMSRequestSignName = "省兜兜";
        public const string SMSRequestOutId = "RRL-SMS";

        #endregion SMSConfig

        #region SMSTemplate

        //验证码短信
        public const string SMSTemplate_AUTH_SMS = "SMS_80210001";

        //下单成功通知（弃用）
        //public const string SMSTemplate_ORDER_PAYMENT_SUCCESS_SMS = "SMS_85410052";
        //下单成功推广
        public const string SMSTemplate_ORDER_PAYMENT_SUCCESS_SMS = "SMS_90115018";

        //提现申请通知短信
        public const string SMSTemplate_CASH_APPLY_HANDLE_SMS = "SMS_90835064";

        #endregion SMSTemplate

        #region TrackConfig

        public const string Kuaidi100customer = "2EA27A1409EB4443803F7634C1A79BA2";
        public const string Kuaidi100key = "oNwiQFIe1942";

        #endregion TrackConfig

        #region AliPayConfig

        public const string AliPay_app_id = "2016082501802210";
        public const string AliPay_partner = "2088421594878857";
        public const string AliPay_seller_id = "rrl_2016@sohu.com";
        public const string AliPay_key = "mr0f77rdav822wjport77awu38ennf8t";
        public static string AliPay_notify_url = System.Configuration.ConfigurationManager.AppSettings["AliPay_notify_url"];// "https://e-shop.rrlsz.com.cn/PayCallBack/alipay_notify";
        public const string AliPay_return_url = "";
        public const string AliPay_private_key_path = "CERT\\alipaykey\\RRL\\rsa_private_key.pem";


        #endregion AliPayConfig

        #region WxPayConfig

        public const string WxPayAPPID = "wx9032832bc5c4500d";
        public const string WxPayMCHID = "1384905102";
        public const string WxPayKEY = "5a4d390c775e13d683679eb3823e3b80";
        public const string WxPayAPPSECRET = "3a0a733599b20b39d9013a7b553c2009";
        public static string WxPayNOTIFY_URL = System.Configuration.ConfigurationManager.AppSettings["WxPayNOTIFY_URL"];// "https://e-shop.rrlsz.com.cn/PayCallBack/wxpay_notify";

        #endregion WxPayConfig

        #region WxMPConfig

        public const string WxMPAPPID = "wx69446e15eb276240";
        public const string WxMPMCHID = "1384964702";
        public const string WxMPKEY = "5a4d390c775e13d683679eb3823e3b80";
        public const string WxMPAPPSECRET = "33aaaff7acba76d4c5f9c72a6e65d303";//"dd33eb209f2bdc82cc96e0fcf927903b";
        public const string WxMPSPAURI = "https://e-shop.rrlsz.com.cn/";

        #endregion WxMPConfig

        #region 加密解密
        // STORY #6 lcl 20180419 Insert
        /// <summary>
        /// DES方式加密解密的秘钥
        /// </summary>
        public const string DES_SECRET_KEY = "rrl@2334";
        #endregion

        // PROJECT #8 lcl 20180425 Insert
        /// <summary>
        /// 免费红包签到周期
        /// </summary>
        public const int CNT_SIGN_CYCLES_REDPACKAGE_FREE = 7;

        // lcl 20180504 Insert
        /// <summary>
        /// 小红包签到周期
        /// </summary>
        public const int CNT_SIGN_CYCLES_MONEY_PAY = 7;

        // STORY #18 lcl 20180523 Insert
        /// <summary>
        /// 正式站点地址
        /// </summary>
        public static string LiveSiteUrl = System.Configuration.ConfigurationManager.AppSettings["LiveSiteUrl"]; // "https://e-shop.rrlsz.com.cn";

        // lcl 2018-06-22 Insert
        /// <summary>
        /// 疯狂转转赚的盘面配置数据列表的接口地址
        /// </summary>
        public static string CrazyCircleConfigList_Url = System.Configuration.ConfigurationManager.AppSettings["CrazyCircleConfigList_Url"];

        // lcl 2018-06-22 Insert
        /// <summary>
        /// 疯狂转转赚的盘面配置数据中权重更新的接口地址
        /// </summary>
        public static string UpdateWeightForCrazyCircle_Url = System.Configuration.ConfigurationManager.AppSettings["UpdateWeightForCrazyCircle_Url"];

        // lcl 2018-06-22 Insert
        /// <summary>
        /// 疯狂转转赚的调控配置数据列表的接口地址
        /// </summary>
        public static string CrazyCircleWaveConfigList_Url = System.Configuration.ConfigurationManager.AppSettings["CrazyCircleWaveConfigList_Url"];

        // lcl 2018-06-22 Insert
        /// <summary>
        /// 疯狂转转赚的调控配置数据更新的接口地址
        /// </summary>
        public static string UpdateWaveConfigForCrazyCircle_Url = System.Configuration.ConfigurationManager.AppSettings["UpdateWaveConfigForCrazyCircle_Url"];

        #region 二手一键转卖 lcl 2018-07-28 Insert

        public static string SecondHand_PreProduct_Url = System.Configuration.ConfigurationManager.AppSettings["SecondHand_PreProduct_Url"];

        public static string SecondHand_ComProduct_Url = System.Configuration.ConfigurationManager.AppSettings["SecondHand_ComProduct_Url"];

        public static string SecondHand_Card100_GoodsId = System.Configuration.ConfigurationManager.AppSettings["SecondHand_Card100_GoodsId"];

        public static string SecondHand_Card10_GoodsId = System.Configuration.ConfigurationManager.AppSettings["SecondHand_Card10_GoodsId"];

        public static string SecondHand_DES_Key = "@sddq!$1";

        /// <summary>
        /// 100元卡对应的金豆数
        /// </summary>
        public static decimal Card100_GoldBean = 100m * RMB_To_GoldBean_Rate;

        /// <summary>
        /// 10元卡对应的金豆数
        /// </summary>
        public static decimal Card10_GoldBean = 10m * RMB_To_GoldBean_Rate;

        /// <summary>
        /// 金豆兑换人民币的比率
        /// </summary>
        //public static decimal GoldBean_To_RMB_Rate = 0.01m;
        public static decimal GoldBean_To_RMB_Rate = 1m;

        /// <summary>
        /// 平台现金手续费比率
        /// </summary>
        public static decimal Platform_Fee_Rate = 0.02m;

        #endregion

        #region 红包裂变

        public static int ReapackageCountForFinishFissionTask = 10;

        #endregion

        // lcl 2018-10-21 Insert
        public static string Card_Platform_Pay_Url = System.Configuration.ConfigurationManager.AppSettings["Card_Platform_Pay_Url"];
    }
}