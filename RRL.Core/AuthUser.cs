using Common.Utils;
using System.Web;
using System.Web.Security;

namespace RRL.Core
{
    /// <summary>
    /// 用户中心认证
    /// </summary>
    public class AuthUser
    {
        private int userid;

        private string username;

        private string truename;

        private int usertype;

        private int status;

        private bool isauthenticated = false;

        /// <summary>
        /// 实例化AuthUser对象后可获得用户认证信息
        /// </summary>

        public AuthUser()
        {
            if (HttpContext.Current.User.Identity.IsAuthenticated)
            {
                string userName = HttpContext.Current.User.Identity.Name;
                string[] parts = userName.Split(':');
                if (parts.Length < 1)
                {
                    isauthenticated = false;

                    return;
                }
                userid = ConvertUtil.ToInt32(parts[0]);

                username = parts[1];
                status = ConvertUtil.ToInt32(parts[2]);
                truename =parts[3];

                isauthenticated = true;
                return;
            }
        }

        public static int LoginUser(int userid)
        {

            if (userid > 0)
            {
                DbService db = new DbService();
                var user = db.GetUserAccountById(userid);
                if (user.UserID > 0)
                {
                    string name = user.UserID + ":" + user.UserName +  ":" + user.Status + ":" + user.TrueName;
                    FormsAuthentication.SetAuthCookie(name, false);
                    return 1;
                }
                else
                {
                    return 0;
                }

            }
            else
            {
                return 0;
            }


        }


        public static void LoginGuestUser()
        {
            try
            {
                string name = "-1:Guest:-1:1:游客";
                FormsAuthentication.SetAuthCookie(name, false);
            }
            catch { }
        }

        /// <summary>
        /// 用户登录认证
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static int AuthenticateUser(string username, string password)
        {
            DbService db = new DbService();
            int userid = db.AuthUser(username, password);
            if (userid > 0)
            {
                var user = db.GetUserAccountById(userid);
                string name = user.UserID + ":" + user.UserName +  ":" + user.Status + ":" + user.TrueName;
                FormsAuthentication.SetAuthCookie(name, false);
                return user.UserID;
            }
            else
            {
                return 0;
            }


        }

        /// <summary>
        /// 用户ID
        /// </summary>
        public int UserID
        {
            get { return userid; }
            set { userid = value; }
        }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName
        {
            get { return username; }
            set { username = value; }
        }

        /// <summary>
        /// 真实姓名
        /// </summary>
        public string TrueName
        {
            get { return truename; }
            set { truename = value; }
        }

        /// <summary>
        /// 账户类型
        /// </summary>
        public int UserType
        {
            get { return usertype; }
            set { usertype = value; }
        }

        public int Status
        {
            get { return status; }
            set { status = value; }
        }


        /// <summary>
        /// 是否登录
        /// </summary>
        public bool IsAuth
        {
            get { return isauthenticated; }
            set { isauthenticated = value; }
        }
    }
}
