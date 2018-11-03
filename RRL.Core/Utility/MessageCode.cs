namespace RRL.Core.Utility
{
    public class MessageCode
    {
        public const int SUCCESS = 0;//成功
        public const int ERROR_TOO_MANY_DEVICE = 90;//过多客户使用该设备
        public const int ERROR_NO_SPERATER = 91;//没有推荐人异常
        public const int ERROR_SMS_CLIENT = 92;//短信客户端异常
        public const int ERROR_SMS_SERVER = 93;//短信服务器异常
        public const int ERROR_SEND_CODE = 94;//发送失败
        public const int ERROR_BAD_MOBILE = 95;//错误的手机号码
        public const int HAS_REMOVE = 96;//已不存在
        public const int HAS_RECORD = 97;//已有记录
        public const int ERROR_TOKEN_VALIDATE = 98;//Token验证失败
        public const int ERROR_UNKONWN = 99;//自定义错误
        public const int ERROR_TOKEN_LENGTH = 100;//Token长度错误
        public const int ERROR_TOKEN_PSW = 101;//Token用户密码验证错误
        public const int ERROR_TOKEN_TIMESTAMP = 102;//Token时间戳错误
        public const int ERROR_TOKEN_RELOGIN = 103;//Token在其他地方登录
        public const int ERROR_NOUSERNAME = 104;//用户名不存在
        public const int ERROR_PASSWORD = 105;//密码错误
        public const int ERROR_EXECUTE_SQL = 106;//执行SQL错误
        public const int ERROR_NULL_UID = 107;//用户名或密码为空
        public const int ERROR_NO_UID = 108;//登录失败:用户名不存在
        public const int ERROR_BAD_PSW = 109;//登录失败:密码错误
        public const int ERROR_BAD_SMS = 110;//短信代码验证失败
        public const int ERROR_EXIST_USER = 111;//用户已经存在
        public const int ERROR_PASSWORD_LENGTH = 112;//密码长度错误
        public const int ERROR_CREATE_DIR = 113;//创建文件夹错误
        public const int ERROR_SAVE_FILE = 114;//保存文件错误
        public const int ERROR_NO_DATA = 115;//未查询到相关数据
        public const int ERROR_NO_AUTH = 116;//没有操作权限
        public const int ERROR_NO_POINT = 117;//积分不足
        public const int ERROR_TRANS_TYPE = 118;//错误的交易方式
        public const int ERROR_CREATE_ORDER = 119;//订单创建失败
        public const int ERROR_ORDER_STATUS = 120;//错误的订单状态
        public const int ERROR_NO_RENYUAN = 121;//人元不足
        public const int ERROR_NO_MONEY = 122;//金额不足
        public const int ERROR_ACOUNT_TYPE = 123;//错误的账户类型
        public const int ERROR_CODE = 124;//错误的验证码
        public const int ERROR_MIN = 125;//未达到数量或金额要求
        public const int ERROR_EXEIST = 126;//交易密码已经存在
        public const int ERROR_ORDER_RECEIVE = 128;//订单接收状态错误
        public const int ERROR_ORDER = 129;//订单类型错误
        public const int ERROR_GOODS_COUNT = 130;//商品库存数量不足
        public const int ERROR_ALLOWED_MODE = 131;//未开通的交易模式
        public const int ERROR_SAVE_PIC = 132;//保存图片失败
        public const int ERROR_TOOMANEY_PIC = 133;//图片超过数量限制
        public const int ERROR_ORDER_CANNOT_REFUND = 134;//订单不能退款
        public const int ERROR_ORDER_VALID_DATE = 135;//订单或商品过期
        public const int ERROR_TGSELLER_ORDER = 136;//非注册商家不能下单
        public const int ERROR_GOODS_COUNT_LIMIT = 137;//用户购买数量超过商品最大购买数量限制
        public const int ERROR_USER_HAVE_ATTEND_ONE_YUAN = 139;//用户已经参与过一元购活动
        public const int ERROR_MORE_ONE_YUAN_GOODS_IN_CART = 140;//已参与过一元购或尝试向购物车中添加过多的一元购商品
        public const int ERROR_HAVE_CHECKIN = 138;//今天已经签到

        public const int ERROR_USER_LOCKED = 141;//用户被所锁定
        public const int ERROR_ORDER_NO_ADDRESS = 200;//订单无收货地址
        public const int ERROR_HAVE_SPREADER = 201;//已有在安全期内的推荐人

        public static string TranslateMessageCode(int Code)
        {
            string result = "";
            switch (Code)
            {
                
                case SUCCESS:
                    result = "成功";
                    break;
                case ERROR_HAVE_SPREADER:
                    result = "已有在安全期内的推荐人";
                    break;
                case ERROR_ORDER_NO_ADDRESS:
                    result = "订单无收货地址";
                    break;
                case ERROR_TOO_MANY_DEVICE:
                    result = "过多用户使用该设备";
                    break;

                case ERROR_NO_SPERATER:
                    result = "没有推荐人或推荐人不存在(或被锁定)";
                    break;

                case ERROR_SMS_CLIENT:
                    result = "短信客户端异常";
                    break;

                case ERROR_SMS_SERVER:
                    result = "短信服务器异常";
                    break;

                case ERROR_SEND_CODE:
                    result = "发送失败";
                    break;

                case ERROR_BAD_MOBILE:
                    result = "手机号码错误";
                    break;

                case HAS_REMOVE:
                    result = "没有记录";
                    break;

                case HAS_RECORD:
                    result = "已有记录";
                    break;

                case ERROR_TOKEN_VALIDATE:
                    result = "Token验证失败";
                    break;

                case ERROR_UNKONWN:
                    result = "自定义错误";
                    break;

                case ERROR_TOKEN_LENGTH:
                    result = "Token长度错误";
                    break;

                case ERROR_TOKEN_PSW:
                    result = "Token用户密码验证错误";
                    break;

                case ERROR_TOKEN_TIMESTAMP:
                    result = "Token时间戳错误";
                    break;

                case ERROR_TOKEN_RELOGIN:
                    result = "在其他地方登录";
                    break;

                case ERROR_NOUSERNAME:
                    result = "用户名不存在";
                    break;

                case ERROR_PASSWORD:
                    result = "密码错误";
                    break;

                case ERROR_EXECUTE_SQL:
                    result = "执行SQL错误";
                    break;

                case ERROR_NULL_UID:
                    result = "用户名或密码为空";
                    break;

                case ERROR_NO_UID:
                    result = "登录失败:用户名不存在";
                    break;

                case ERROR_BAD_PSW:
                    result = "登录失败:密码错误";
                    break;

                case ERROR_BAD_SMS:
                    result = "短信代码验证失败";
                    break;

                case ERROR_EXIST_USER:
                    result = "用户已经存在";
                    break;

                case ERROR_PASSWORD_LENGTH:
                    result = "密码长度错误";
                    break;

                case ERROR_CREATE_DIR:
                    result = "创建文件夹错误";
                    break;

                case ERROR_SAVE_FILE:
                    result = "保存文件错误";
                    break;

                case ERROR_NO_DATA:
                    result = "未查询到相关数据";
                    break;

                case ERROR_NO_AUTH:
                    result = "没有操作权限";
                    break;

                case ERROR_NO_POINT:
                    result = "没有足够的积分";
                    break;

                case ERROR_TRANS_TYPE:
                    result = "错误的交易方式";
                    break;

                case ERROR_CREATE_ORDER:
                    result = "订单创建失败";
                    break;

                case ERROR_ORDER_STATUS:
                    result = "错误的订单状态";
                    break;

                case ERROR_NO_RENYUAN:
                    result = "人元不足";
                    break;

                case ERROR_NO_MONEY:
                    result = "金额不足";
                    break;

                case ERROR_ACOUNT_TYPE:
                    result = "错误的账户类型";
                    break;

                case ERROR_CODE:
                    result = "错误的验证码";
                    break;

                case ERROR_MIN:
                    result = "未达到数量或金额要求";
                    break;

                case ERROR_EXEIST:
                    result = "交易密码已经存在";
                    break;

                case ERROR_ORDER_RECEIVE:
                    result = "订单接收状态错误";
                    break;

                case ERROR_ORDER:
                    result = "订单异常";
                    break;

                case ERROR_GOODS_COUNT:
                    result = "商品库存数量不足";
                    break;

                case ERROR_ALLOWED_MODE:
                    result = "未开通的交易模式";
                    break;

                case ERROR_SAVE_PIC:
                    result = "保存图片失败";
                    break;

                case ERROR_TOOMANEY_PIC:
                    result = "图片超过数量限制";
                    break;

                case ERROR_ORDER_CANNOT_REFUND:
                    result = "订单不能退款";
                    break;

                case ERROR_ORDER_VALID_DATE:
                    result = "订单或商品过期";
                    break;

                case ERROR_TGSELLER_ORDER:
                    result = "非注册商家不能下单";
                    break;

                case ERROR_GOODS_COUNT_LIMIT:
                    result = "超过商品最大购买数量限制";
                    break;

                case ERROR_USER_HAVE_ATTEND_ONE_YUAN:
                    result = "您已参与过一元购，请勿重复参与";
                    break;

                case ERROR_MORE_ONE_YUAN_GOODS_IN_CART:
                    result = "已参与过一元购或尝试向购物车中添加过多的一元购商品";
                    break;

                case ERROR_HAVE_CHECKIN:
                    result = "今天已经签到";
                    break;

                case ERROR_USER_LOCKED:
                    result = "用户被锁定!请用注册手机号码联系客服!";
                    break;

                default:
                    result = "未定义消息";
                    break;
            }
            return result;
        }
    }
}