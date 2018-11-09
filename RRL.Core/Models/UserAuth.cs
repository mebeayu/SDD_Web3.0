using RRL.Core.Utility;
using RRL.DB;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;

namespace RRL.Core.Models
{
    public class TokenObject
    {
        public const string ErrorToken = "ErrorToken";
        public const string LongTimeToken = "LongTimeToken";//长效TOKEN头
        public const string ShortTimeToken = "ShortTimeToken";//短效TOKEN头
        //错误TOKEN头

        public DateTime Expirtime
        {
            get; set;
        }

        public int id
        {
            get; set;
        }

        public string Prefix
        {
            get; set;
        }

        public string Username
        {
            get; set;
        }

        /// <summary>
        /// 通过Token字符串初始化Token对象
        /// </summary>
        /// <param name="tokenString">Token字符串</param>
        public static TokenObject InitTokenObjFromString(string tokenString)
        {
            tokenString = DES.DecryptDES(tokenString);
            var token = new TokenObject();
            string[] tokenArray;
            try
            {
                tokenArray = tokenString.Split(',');
            }
            catch
            {
                return token;
            }

            if (tokenArray.Length == 4)
            {
                token.Prefix = tokenArray[0];
                token.id = Convert.ToInt32(tokenArray[1]);
                token.Username = tokenArray[2];
                token.Expirtime = Convert.ToDateTime(tokenArray[3]);
            }
            else
            {
                token.Prefix = ErrorToken;
                token.id = -1;
                token.Username = "";
                token.Expirtime = DateTime.Now;
            }
            return token;
        }

        /// <summary>
        /// 编码Token字符串
        /// </summary>
        /// <returns>Token字符串</returns>
        public string EncodeTokenObjToString()
        {
            var tokenArray = new[]
            {
                Prefix,
                id.ToString(),
                Username,
                Expirtime.ToString(CultureInfo.InvariantCulture)
            };
            return DES.EncryptDES(string.Join(",", tokenArray));
        }
    }

    public class UserAuth
    {
        private static readonly List<string> WhiteList = new List<string>
        {
 
            "13668795584",//伟哥
            "10000000001",//注册测试1
            "10000000002",//注册测试2
            "99999999999",
            "13827470343",//王龙
            "18788458013",//尧山
            "15887055073",//小罗
            "13769150280",//测试王强
            "13688719310", //尚国永
            //"13658835081",
            "13428711799" ,// 刘楚龙 lcl 2018-04-17 Insert
            "10000000003",
            //"13888118063",
            "18888888888",//推广 wx
            "18888888887",//推广 wx
            "18888888886",//推广 wx
            "18888888885",//推广 app
            "18888888884",//推广 app
            "18888888883",//推广 app
            "14444444444"
            ,"19000000001"
            ,"19000000002"
            ,"19000000003"
            ,"19000000004"
            ,"19000000005"
            ,"19000000006"
            ,"19000000007"
            ,"19000000008"
            ,"19000000009"
            ,"19000000010"
            ,"19000000011"
            ,"19000000012"
            ,"19000000013"
            ,"19000000014"
            ,"19000000015"
            ,"19000000016"
            ,"19000000017"
            ,"19000000018"
            ,"19000000019"
            ,"19000000020"
        };


        #region 字段定义

        private int _id = 0;

        private string _long_time_token = "";

        private DateTime _long_time_token_expir = DateTime.Now;

        private string _short_time_token = "";

        private DateTime _short_time_token_expir = DateTime.Now;

        private string _username = "";

        private double _r_money = 0;

        private double _x_money = 0;

        private double _h_money = 0;

        private double _h_money_free = 0;

        private double _h_money_free_frz = 0;

        private DateTime _h_money_free_frz_expire = DateTime.Now;

        private bool _has_received_free_money_default = false;

        private bool _has_received_spreader_h_money = false;

        private bool _has_received_daily_free_h_money = false;

        private bool _has_received_h_money_five_group_spreade = true;

        private DateTime _last_spreader_h_money_time = DateTime.Now;

        private DateTime _last_random_h_money_time = DateTime.Now;

        private int _spreader_count = 0;

        private int _daily_free_h_money_count = 1;

        private double _plate_to_return_money = 0;

        private double _ex_plate_to_return_money = 0;

        private string _wx_open_id = "";

        private string _wx_mp_open_id = "";

        private string _device_code = "";

        public int pay_order_total_count { get; set; } =0;

        public decimal pay_coupons_total_money { get; set; } = 0m;




        /// <summary>
        /// 小红包
        /// </summary>
        public decimal h_money_pay { get; set; } = 0;

        public int id
        {
            get { return _id; }
            private set { _id = value; }
        }

        public string long_time_token
        {
            get { return _long_time_token; }
            set { _long_time_token = value; }
        }

        public DateTime long_time_token_expir
        {
            get { return _long_time_token_expir; }
            set { _long_time_token_expir = value; }
        }

        public string short_time_token
        {
            get { return _short_time_token; }
            set { _short_time_token = value; }
        }

        public DateTime short_time_token_expir
        {
            get { return _short_time_token_expir; }
            set { _short_time_token_expir = value; }
        }

        public string username
        {
            get { return _username; }
            private set { _username = value; }
        }

        public double r_money
        {
            get { return _r_money; }
            private set { _r_money = value; }
        }
        
        public double x_money
        {
            get { return _x_money; }
            private set { _x_money = value; }
        }

        public double h_money
        {
            get { return _h_money; }
            private set { _h_money = value; }
        }

        public double h_money_free
        {
            get { return _h_money_free; }
            private set { _h_money_free = value; }
        }

        public double h_money_free_frz
        {
            get { return _h_money_free_frz; }
            private set { _h_money_free_frz = value; }
        }

        public DateTime h_money_free_frz_expire
        {
            get { return _h_money_free_frz_expire; }
            private set { _h_money_free_frz_expire = value; }
        }

        public bool has_received_free_money_default
        {
            get { return _has_received_free_money_default; }
            private set { _has_received_free_money_default = value; }
        }

        public bool has_received_spreader_h_money
        {
            get { return _has_received_spreader_h_money; }
            private set { _has_received_spreader_h_money = value; }
        }

        public bool has_received_daily_free_h_money
        {
            get { return _has_received_daily_free_h_money; }
            private set { _has_received_daily_free_h_money = value; }
        }

        public bool has_received_h_money_five_group_spreade
        {
            get { return _has_received_h_money_five_group_spreade; }
            private set { _has_received_h_money_five_group_spreade = value; }
        }

        public int daily_free_h_money_count
        {
            get { return _daily_free_h_money_count; }
            private  set { _daily_free_h_money_count = value; }
        }

        public DateTime last_spreader_h_money_time
        {
            get { return _last_spreader_h_money_time; }
            private set { _last_spreader_h_money_time = value; }
        }

        public DateTime last_random_h_money_time
        {
            get { return _last_random_h_money_time; }
            private set { _last_random_h_money_time = value; }
        }

        public int spreader_count
        {
            get { return _spreader_count; }
            private set { _spreader_count = value; }
        }
        /// <summary>
        /// 金币
        /// </summary>
        public double plate_to_return_money
        {
            get { return _plate_to_return_money; }
            private set { _plate_to_return_money = value; }
        }
        /// <summary>
        /// 其他用处的金币??
        /// </summary>
        public double ex_plate_to_return_money
        {
            get { return _ex_plate_to_return_money; }
            private set { _ex_plate_to_return_money = value; }
        }

        public string wx_open_id
        {
            get { return _wx_open_id; }
            private set { _wx_open_id = value; }
        }

        public string wx_mp_open_id
        {
            get { return _wx_mp_open_id; }
            private set { _wx_mp_open_id = value; }
        }

        public string device_code
        {
            get { return _device_code; }
            private set { _device_code = value; }
        }


        public string is_locked_login { get; set; }


        public string is_locked_trade { get; set; }

        /// <summary>
        /// 随机支付的红包
        /// </summary>
        public decimal rnd_pay_redpacket { get; set; }

        // lcl 20180428 Insert
        /// <summary>
        /// 是否为合伙人(1：是合伙人)
        /// </summary>
        public int is_partner { get; set; }

        // STORY #18 lcl 20180517 Insert
        /// <summary>
        /// 头像ID
        /// </summary>
        public int head_pic { get; set; }

        // lcl 20180627 Insert (支付门槛逻辑变更，用于判断是否是新用户)
        /// <summary>
        /// 注册时间
        /// </summary>
        public DateTime addtime { get; set; }

        // lcl 20180627 Insert (支付门槛逻辑变更。用于确定不同的用户等级类型，对应于不同的门槛规则)
        /// <summary>
        /// 用户等级类型
        /// </summary>
        public string level_category { get; set; }

        // lcl 20180717 Insert
        /// <summary>
        /// 是否是代理商(1=是，0=否)
        /// </summary>
        public int is_agent { get; set; }

        #endregion 字段定义

        /// <summary>
        /// 构造UserAuth
        /// </summary>
        public UserAuth()
        {
        }

        /// <summary>
        /// 通过id构造UserAuth
        /// </summary>
        /// <param name="id">用户id</param>
        public UserAuth(int id)
        {
            Load(id);
        }

        /// <summary>
        /// 通过用户名构造UserAuth
        /// </summary>
        /// <param name="username">用户名</param>
        public UserAuth(string username)
        {
            Load(username);
        }

        /// <summary>
        /// 通过Openid构造UserAuth
        /// </summary>
        /// <param name="openid"></param>
        /// <param name="type"></param>
        public UserAuth(string openid, string type)
        {
            Load(openid, type);
        }

        /// <summary>
        /// 验证Token
        /// </summary>
        /// <param name="token">Token对象</param>
        /// <returns>验证结果</returns>
        public int CheckToken(TokenObject token)
        {
            int res = Load(token.id);
            if (res != MessageCode.SUCCESS)
                return res;
            if (string.Equals(TokenObject.LongTimeToken, token.Prefix))
            {
                if (DateTime.Compare(long_time_token_expir, DateTime.Now) <= 0)
                {
                    return MessageCode.ERROR_TOKEN_TIMESTAMP;
                }
            }
            else if (string.Equals(TokenObject.ShortTimeToken, token.Prefix))
            {
                if (DateTime.Compare(short_time_token_expir, DateTime.Now) <= 0)
                {
                    return MessageCode.ERROR_TOKEN_TIMESTAMP;
                }
            }
            else
            {
                return MessageCode.ERROR_TOKEN_VALIDATE;
            }
            return MessageCode.SUCCESS;
        }

        /// <summary>
        /// 加密长效Token
        /// </summary>
        public string EncryptLongTimeToken()
        {
            long_time_token_expir = DateTime.Now.AddMonths(6);
            var token = new TokenObject
            {
                Prefix = TokenObject.LongTimeToken,
                id = id,
                Username = username,
                Expirtime = long_time_token_expir
            };
            long_time_token = token.EncodeTokenObjToString();
            return long_time_token;
        }

        /// <summary>
        /// 加密短效Token
        /// </summary>
        public string EncryptShortTimeToken()
        {
            short_time_token_expir = DateTime.Now.AddDays(1);
            var token = new TokenObject
            {
                Prefix = TokenObject.ShortTimeToken,
                id = id,
                Username = username,
                Expirtime = short_time_token_expir
            };
            short_time_token = token.EncodeTokenObjToString();
            return short_time_token;
        }

        /// <summary>
        /// 是否存在记录
        /// </summary>
        /// <param name="id">用户id</param>
        /// <returns>判断结果</returns>
        public static bool Exists(int id)
        {
            RRLDB db = new RRLDB();
            DataSet ds = db.ExeQuery(@"select count(1) from [rrl_user] where id = @id",
                new SqlParameter("@id", SqlDbType.Int, 4) { Value = id });
            db.Close();
            int count = int.Parse(ds.Tables[0].Rows[0][0].ToString());
            if (count == 0) return false;
            else return true;
        }

        /// <summary>
        /// 是否存在记录
        /// </summary>
        /// <param name="username">用户名</param>
        /// <returns>判断结果</returns>
        public static bool Exists(string username)
        {
            RRLDB db = new RRLDB();
            DataSet ds = db.ExeQuery(@"select count(1) from [rrl_user] where username = @username",
                new SqlParameter("@username", username));
            db.Close();
            int count = int.Parse(ds.Tables[0].Rows[0][0].ToString());
            if (count == 0) return false;
            else return true;
        }

        /// <summary>
        /// 检查用户手机验证码
        /// </summary>
        /// <param name="mobile">手机号码</param>
        /// <param name="code">验证码</param>
        /// <returns>检查结果</returns>
        public static bool CheckSMS(string mobile, string code)
        {
            // lcl 2018-08-29 阿里云短信异常临时解决
            string strMobilePart = "9999";
            if (mobile.Length == 11)
            {
                strMobilePart = mobile.Substring(mobile.Length - 4);
            }

            if (WhiteList.Contains(mobile) || code == $@"{DateTime.Now.ToString("yyyyMMdd")}{strMobilePart}")
            {
                return true;
            }

            //if (WhiteList.Contains(mobile))
            //{
            //    return true;
            //}
            var db = new RRLDB();
            var sql = $@"select id,code,addtime from [rrl_sms] where mobile = '{mobile}'"/* AND is_used = 0*/;
            var ds = db.ExeQuery(sql);
            db.Close();
            if(ds.Tables[0].Rows.Count == 0 || ds == null)
            {
                return false;
            }
            var id = Convert.ToInt32(ds.Tables[0].Rows[0]["id"]);
            var smscode = ds.Tables[0].Rows[0]["code"].ToString();
            var addtime = new TimeSpan(Convert.ToDateTime(ds.Tables[0].Rows[0]["addtime"].ToString()).Ticks);
            var now = new TimeSpan(DateTime.Now.Ticks);
            var ts = now.Subtract(addtime).Duration();
            var interval = ts.Minutes;
            if (interval > 15)
            {
                return false;
            }
            if (!string.Equals(code, smscode))
            {
                return false;
            }
            SetSMSUsed(id);
            return true;
        }

        /// <summary>
        /// 验证获取已注册用户发送的验证码
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="code">验证码</param>
        /// <returns>验证结果</returns>
        public static bool CheckSMSForExistsUser(string username, string code)
        {
            // lcl 2018-08-29 阿里云短信异常临时解决
            string strMobilePart = "9999";
            if (username.Length == 11)
            {
                strMobilePart = username.Substring(username.Length - 4);
            }

            if (WhiteList.Contains(username) || code == $@"{DateTime.Now.ToString("yyyyMMdd")}{strMobilePart}")
            {
                return true;
            }

            //if (WhiteList.Contains(username))
            //{
            //    return true;
            //}
            var db = new RRLDB();
            var ds = db.ExeQuery(@"select id,code,addtime from [USER_SMS_VALI_VIEW] where username = @username",
                new SqlParameter("@username", username));
            db.Close();
            if (ds.Tables[0].Rows.Count == 0)
            {
                return false;
            }
            var id = Convert.ToInt32(ds.Tables[0].Rows[0]["id"]);
            var smscode = ds.Tables[0].Rows[0]["code"].ToString();
            var addtime = new TimeSpan(Convert.ToDateTime(ds.Tables[0].Rows[0]["addtime"].ToString()).Ticks);
            var now = new TimeSpan(DateTime.Now.Ticks);
            var ts = now.Subtract(addtime).Duration();
            var interval = ts.Minutes;
            if (interval > 15)
            {
                return false;
            }
            if (!string.Equals(code, smscode))
            {
                return false;
            }
            SetSMSUsed(id);
            return true;
        }

        /// <summary>
        /// 设置验证码已使用
        /// </summary>
        /// <param name="smsId">验证码标识</param>
        public static void SetSMSUsed(int smsId)
        {
            return;
            var db = new RRLDB();
            db.ExecSP(@"sp_SMS_MARKUSED", new SqlParameter("@sms_id", SqlDbType.Int, 4) { Value = smsId });
            db.Close();
        }

        /// <summary>
        /// 是否超过设备数
        /// </summary>
        /// <param name="deviceCode"></param>
        /// <returns></returns>
        public bool CheckDeviceCode(string deviceCode)
        {
            if (string.IsNullOrWhiteSpace(deviceCode))
            {
                return true;
            }

            if(deviceCode.Equals(device_code))
            {
                return true;
            }
            var db = new RRLDB();
            db.ExeCMD($@"UPDATE rrl_user SET device_code = '{deviceCode}' WHERE id = {id}");
            var ds = db.ExeQuery($@"SELECT COUNT(*) FROM rrl_user WHERE [device_code] = '{deviceCode}'");
            db.Close();
            if (ds == null || Convert.ToInt32(ds.Tables[0].Rows[0][0])>=3)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 强制过期长效Token
        /// </summary>
        public void ExpirLongTimeToken()
        {
            long_time_token_expir = DateTime.Now;
        }

        /// <summary>
        /// 强制过期短效Token
        /// </summary>
        public void ExpirShortTimeToken()
        {
            short_time_token_expir = DateTime.Now;
        }
        public static Dictionary<int, DataRow> ListUserRow = new Dictionary<int, DataRow>();
        /// <summary>
        /// 根据用户id载入数据
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <returns>载入结果</returns>
        public int Load(int uid)
        {
            DataRow user = null;
            try
            {
                var db = new RRLDB();
                var ds = db.ExeQuery("select * from [rrl_user](nolock) where id = @id",
                    new SqlParameter("@id", SqlDbType.Int, 4) { Value = uid });
                db.Close();
                if (ds == null)
                    return MessageCode.ERROR_EXECUTE_SQL;//执行查询失败
                if (ds.Tables[0].Rows.Count == 0)
                    return MessageCode.ERROR_NO_UID;//没查到数据
                user = ds.Tables[0].Rows[0];
                if (ListUserRow.ContainsKey(uid) == false) ListUserRow.Add(uid, user);
                else ListUserRow[uid] = user;
                InitUser(user);
            }
            catch (Exception ex)
            {

                if(ListUserRow.ContainsKey(uid)&& ListUserRow[uid]!=null)
                {
                    InitUser(ListUserRow[uid]);
                }
                else
                {
                    id = 0;
                    username = "";
                    long_time_token = "";
                    long_time_token_expir = DateTime.Now;
                    short_time_token = "";
                    short_time_token_expir = DateTime.Now;
                    r_money = 0;
                    x_money = 0;
                    h_money = 0;
                    h_money_free = 0;
                    h_money_free_frz = 0;
                    h_money_free_frz_expire = DateTime.Now;
                    has_received_free_money_default =false;
                    has_received_spreader_h_money = false;
                    has_received_daily_free_h_money = false;
                    has_received_h_money_five_group_spreade = false;
                    last_spreader_h_money_time = DateTime.Now; 
                    last_random_h_money_time = DateTime.Now;
                    spreader_count = 0;
                    daily_free_h_money_count = 0;
                    plate_to_return_money =0;
                    ex_plate_to_return_money = 0;
                    wx_open_id = "";
                    wx_mp_open_id = "";
                    device_code ="";
                    decimal rnd_pay_redpacket_try = 0;
                    
                    decimal h_money_pay_try = 0;
                   

                    this.is_locked_login = "0";
                    
                    this.is_locked_trade = "0";

                    // lcl 20180428 Insert
                    // 是否为合伙人
                    is_partner = 0;
                    // STORY #18 lcl 20180517 Insert
                    head_pic = 0;

                    int pay_order_total_count_try = 0;
                    

                    decimal pay_coupons_total_money_try = 0m;
                    

                    // lcl 20180627 Insert
                    this.addtime =DateTime.Now;
                    // lcl 20180627 Insert
                    this.level_category = "";

                    // lcl 20180717 Insert
                    this.is_agent = 0;
                }
            }
            
           
            
            return MessageCode.SUCCESS;
        }

        /// <summary>
        /// 根据用户名载入数据
        /// </summary>
        /// <param name="uname">用户名</param>
        /// <returns>载入结果</returns>
        public int Load(string uname)
        {
            var db = new RRLDB();
            var ds = db.ExeQuery("select * from [rrl_user] where username = @username",
                new SqlParameter("@username", uname));
            db.Close();
            if (ds == null)
                return MessageCode.ERROR_EXECUTE_SQL;//执行查询失败
            if (ds.Tables[0].Rows.Count == 0)
                return MessageCode.ERROR_NO_UID;//没查到数据
            var user = ds.Tables[0].Rows[0];
            InitUser(user);
            return MessageCode.SUCCESS;
        }

        /// <summary>
        /// 根据Openid载入数据
        /// </summary>
        /// <param name="openid">openid</param>
        /// <param name="type">类型</param>
        /// <returns></returns>
        public int Load(string openid, string type)
        {
            var db = new RRLDB();
            DataSet ds = null;
            if (type.Equals("WxOpen"))
            {
                ds = db.ExeQuery("select * from [rrl_user] where wx_open_id = @wx_open_id", new SqlParameter("@wx_open_id", openid));
            }
            if (type.Equals("WxMP"))
            {
                ds = db.ExeQuery("select * from [rrl_user] where wx_mp_open_id = @wx_mp_open_id", new SqlParameter("@wx_mp_open_id", openid));
            }
            db.Close();
            if (ds == null)
                return MessageCode.ERROR_EXECUTE_SQL;//执行查询失败
            if (ds.Tables[0].Rows.Count == 0)
                return MessageCode.ERROR_NO_UID;//没查到数据
            var user = ds.Tables[0].Rows[0];
            InitUser(user);
            return MessageCode.SUCCESS;
        }

        /// <summary>
        /// 保存用户数据
        /// </summary>
        /// <returns>保存结果</returns>
        public int Save()
        {
            var db = new RRLDB();
            var res = db.ExeCMD(@"update rrl_user set
                                    long_time_token = @long_time_token,
                                    long_time_token_expir = @long_time_token_expir,
                                    short_time_token = @short_time_token,
                                    short_time_token_expir = @short_time_token_expir where id=@id",
                                 new SqlParameter("@long_time_token", long_time_token),
                                 new SqlParameter("@long_time_token_expir", long_time_token_expir),
                                 new SqlParameter("@short_time_token", short_time_token),
                                 new SqlParameter("@short_time_token_expir", short_time_token_expir),
                                 new SqlParameter("@id", SqlDbType.Int, 4) { Value = id });
            db.Close();
            return res == 1 ? MessageCode.SUCCESS : MessageCode.ERROR_EXECUTE_SQL;
        }

        /// <summary>
        /// 保存用户openid
        /// </summary>
        /// <returns></returns>
        public int SaveOpenid(string openid, string type)
        {
            if (type.Equals("WxOpen") && string.IsNullOrEmpty(wx_open_id))
            {
                wx_open_id = openid;
            }
            if (type.Equals("WxMP") && string.IsNullOrEmpty(wx_mp_open_id))
            {
                wx_mp_open_id = openid;
            }

            var db = new RRLDB();
            var res = db.ExeCMD(@"update rrl_user set
                                    wx_open_id = @wx_open_id,
                                    wx_mp_open_id = @wx_mp_open_id where id=@id",
                new SqlParameter("@wx_open_id", wx_open_id),
                new SqlParameter("@wx_mp_open_id", wx_mp_open_id),
                new SqlParameter("@id", SqlDbType.Int, 4) { Value = id });
            db.Close();
            return res == 1 ? MessageCode.SUCCESS : MessageCode.ERROR_EXECUTE_SQL;
        }

        /// <summary>
        /// 通过数据行初始化用户数据
        /// </summary>
        /// <param name="userRow">用户数据行</param>
        private void InitUser(DataRow userRow)
        {
            id = Convert.ToInt32(userRow["id"]);
            username = Convert.ToString(userRow["username"]);
            long_time_token = Convert.ToString(userRow["long_time_token"]);
            long_time_token_expir = Convert.ToDateTime(userRow["long_time_token_expir"]);
            short_time_token = Convert.ToString(userRow["short_time_token"]);
            short_time_token_expir = Convert.ToDateTime(userRow["short_time_token_expir"]);
            r_money = Convert.ToDouble(userRow["r_money"]);
            x_money = Convert.ToDouble(userRow["x_money"]);
            h_money = Convert.ToDouble(userRow["h_money"]);
            h_money_free = Convert.ToDouble(userRow["h_money_free"]);
            h_money_free_frz = Convert.ToDouble(userRow["h_money_free_frz"]);
            h_money_free_frz_expire = Convert.ToDateTime(userRow["h_money_free_frz_expire"]);
            has_received_free_money_default = Convert.ToBoolean(userRow["has_received_free_money_default"]);
            has_received_spreader_h_money = Convert.ToBoolean(userRow["has_received_spreader_h_money"]);
            has_received_daily_free_h_money = Convert.ToBoolean(userRow["has_received_daily_free_h_money"]);
            has_received_h_money_five_group_spreade = Convert.ToBoolean(userRow["has_received_h_money_five_group_spreade"]);
            last_spreader_h_money_time = Convert.ToDateTime(userRow["last_spreader_h_money_time"]);
            last_random_h_money_time = Convert.ToDateTime(userRow["last_random_h_money_time"]);
            spreader_count = Convert.ToInt32(userRow["spreader_count"]);
            daily_free_h_money_count = Convert.ToInt32(userRow["daily_free_h_money_count"]);
            plate_to_return_money = Convert.ToDouble(userRow["plate_to_return_money"]);
            ex_plate_to_return_money = Convert.ToDouble(userRow["ex_plate_to_return_money"]);
            wx_open_id = Convert.ToString(userRow["wx_open_id"]);
            wx_mp_open_id = Convert.ToString(userRow["wx_mp_open_id"]);
            device_code = Convert.ToString(userRow["device_code"]);
            decimal rnd_pay_redpacket_try = 0;
            if (decimal.TryParse(userRow["rnd_pay_redpacket"].ToString(), out rnd_pay_redpacket_try))
            {
                rnd_pay_redpacket = rnd_pay_redpacket_try;
            }
            decimal h_money_pay_try = 0;
            if (decimal.TryParse(userRow["h_money_pay"].ToString(), out h_money_pay_try))
            {
                h_money_pay = h_money_pay_try;
            }

            this.is_locked_login = userRow["is_locked_login"].ToString();
            if (string.IsNullOrWhiteSpace(this.is_locked_login))
            {
                this.is_locked_login = "0";
            }
            this.is_locked_trade = userRow["is_locked_trade"].ToString();
            if (string.IsNullOrWhiteSpace(this.is_locked_trade))
            {
                this.is_locked_trade = "0";
            }
            // lcl 20180428 Insert
            // 是否为合伙人
            is_partner = userRow["is_partner"] == DBNull.Value ? 0 : Convert.ToInt32(userRow["is_partner"]);
            // STORY #18 lcl 20180517 Insert
            head_pic = userRow["head_pic"] == DBNull.Value ? 0 : Convert.ToInt32(userRow["head_pic"]);

            int pay_order_total_count_try = 0;
            if (int.TryParse(userRow["pay_order_total_count"].ToString(), out pay_order_total_count_try))
            {
                this.pay_order_total_count = pay_order_total_count_try;
            }

            decimal pay_coupons_total_money_try = 0m;
            if (decimal.TryParse(userRow["pay_coupons_total_money"].ToString(), out pay_coupons_total_money_try))
            {
                this.pay_coupons_total_money = pay_coupons_total_money_try;
            }

            // lcl 20180627 Insert
            this.addtime = Convert.ToDateTime(userRow["addtime"]);
            // lcl 20180627 Insert
            this.level_category = userRow["level_category"] == DBNull.Value ? "A" : Convert.ToString(userRow["level_category"]);

            // lcl 20180717 Insert
            this.is_agent = userRow["is_agent"] == DBNull.Value ? 0 : Convert.ToInt32(userRow["is_agent"]);
        }
    }
}