using System;
using System.Security.Cryptography;
using System.Text;

namespace Common.Utils
{
    /// <summary>
    /// 
    /// </summary>
    public class PasswordUtil
    {
        
        public static string AccountPassword(string pwd)
        {
            string result = Encode(pwd);
            result = Encode(result);
            return result;
        }
        
        /// <summary>
        /// 加密密码
        /// </summary>
        /// <param name="pwd">密码明文</param>
        /// <returns></returns>
        public static string Encode(string pwd)
        {
            string result;
            using (MD5 md5 = new MD5CryptoServiceProvider())
            {
                byte[] palindata = Encoding.UTF8.GetBytes(pwd);
                byte[] encryptdata = md5.ComputeHash(palindata);
                result = Convert.ToBase64String(encryptdata);
            }
            return result;
        }

        /// <summary>
        /// 密码比对
        /// </summary>
        /// <param name="dbPwdCode">数据库里的加密后密码</param>
        /// <param name="clientPwd">客户端输入的密码明文</param>
        /// <returns></returns>
        public static bool CheckPassword(string dbPwdCode, string clientPwd)
        {
            return dbPwdCode == Encode(clientPwd);
        }
    }
}