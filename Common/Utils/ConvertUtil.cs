using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace Common.Utils
{
    /// <summary>
    /// Powered by Rocby.com
    /// </summary>
    public class ConvertUtil
    {
        /// <summary>
        /// 转换为布尔类型（错误兼容）
        /// </summary>
        /// <param name="obj">要转换的对象</param>
        public static bool ToBoolean(object obj)
        {
            if (obj == null) { return false; }
            else
            {
                bool result = bool.TryParse(obj.ToString(), out result) ? result : false;
                return result;
            }
        }

        public static string HideStr(string src, int len)
        {
            if (string.IsNullOrEmpty(src))
            {
                return "";
            }

            if (src.Length <= len)
            {
                return src;
            }
            string hstr = "";
            int l = src.Length - len;
            for (int i = 0; i < l; i++)
            {
                hstr += "*";
            }
            if (len == 1)
            {
                return src.Substring(0, 1) + hstr;
            }
            int f1 = len / 2;
            int f2 = len - len / 2;
            string a = src.Substring(0, f1);
            return a + hstr + src.Substring(src.Length - f2);

        }

        /// <summary>
        /// 处理小数，截取小数位数
        /// </summary>
        /// <param name="v"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static decimal FixNumber(decimal v, int length)
        {
            if (length < 0)
            {
                return v;
            }
            string xsd = v.ToString();
            int i = length + 1;
            if (xsd.IndexOf(".") > 0)
            {
                int doti = xsd.LastIndexOf(".");
                int a = xsd.Length - doti > i ? i : (xsd.Length - doti);
                xsd = xsd.Substring(0, doti + a);
            }
            return Convert.ToDecimal(xsd);
        }

        /// <summary>
        /// 转换为日期（错误兼容）
        /// </summary>
        /// <param name="obj">要转换的对象</param>
        public static DateTime ToDateTime(object obj)
        {
            if (obj == null) { return DateTime.Now; }
            else
            {
                DateTime result = DateTime.TryParse(obj.ToString(), out result) ? result : DateTime.Now;
                return result;
            }
        }

        /// <summary>
        /// 转换为数字（错误兼容）
        /// </summary>
        /// <param name="obj">要转换的对象</param>
        public static int ToInt32(object obj)
        {
            if (obj == null) { return 0; }
            else
            {
                int result = int.TryParse(obj.ToString(), out result) ? result : 0;
                return result;
            }
        }

        /// <summary>
        /// 转换为价格（错误兼容）
        /// </summary>
        /// <param name="obj">要转换的对象</param>
        public static decimal ToDecimal(object obj)
        {
            if (obj == null) { return 0; }
            else
            {
                decimal result = decimal.TryParse(obj.ToString(), out result) ? result : 0;
                return result;
            }
        }

        /// <summary>
        /// 转换为浮点数（错误兼容）
        /// </summary>
        /// <param name="obj">要转换的对象</param>
        public static double ToDouble(object obj)
        {
            if (obj == null) { return 0; }
            else
            {
                double result = double.TryParse(obj.ToString(), out result) ? result : 0;
                return result;
            }
        }

        /// <summary>
        /// 将列表转换为列表
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static int[] ToListInt32(object[] s)
        {
            if (s == null) { return null; }
            int[] result = new int[s.Length];
            for (int i = 0; i < s.Length; i++) { result[i] = ToInt32(s[i]); }
            return result;
        }

        /// <summary>
        /// 将列表转换为列表
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static decimal[] ToListDecimal(object[] s)
        {
            if (s == null) { return null; }
            decimal[] result = new decimal[s.Length];
            for (int i = 0; i < s.Length; i++) { result[i] = ToDecimal(s[i]); }
            return result;
        }

        /// <summary>
        /// 将列表转换为列表
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool[] ToListBoolean(object[] s)
        {
            if (s == null) { return null; }
            bool[] result = new bool[s.Length];
            for (int i = 0; i < s.Length; i++) { result[i] = ToBoolean(s[i]); }
            return result;
        }

        /// <summary>
        /// 将列表转换为列表
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static DateTime[] ToListDateTime(object[] s)
        {
            if (s == null) { return null; }
            DateTime[] result = new DateTime[s.Length];
            for (int i = 0; i < s.Length; i++) { result[i] = ToDateTime(s[i]); }
            return result;
        }

        /// <summary>
        /// 将列表转换为列表
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static double[] ToListDouble(object[] s)
        {
            if (s == null) { return null; }
            double[] result = new double[s.Length];
            for (int i = 0; i < s.Length; i++) { result[i] = ToDouble(s[i]); }
            return result;
        }

        /// <summary>
        /// 转换人民币大小金额
        /// </summary>
        /// <param name="num">金额</param>
        /// <returns>返回大写形式</returns>
        public static string ToCmy(decimal num)
        {
            string str1 = "零壹贰叁肆伍陆柒捌玖"; //0-9所对应的汉字
            string str2 = "万仟佰拾亿仟佰拾万仟佰拾元角分"; //数字位所对应的汉字
            string str3 = ""; //从原num值中取出的值
            string str4 = ""; //数字的字符串形式
            string str5 = ""; //人民币大写金额形式
            int i; //循环变量
            int j; //num的值乘以100的字符串长度
            string ch1 = ""; //数字的汉语读法
            string ch2 = ""; //数字位的汉字读法
            int nzero = 0; //用来计算连续的零值是几个
            int temp; //从原num值中取出的值

            num = Math.Round(Math.Abs(num), 2); //将num取绝对值并四舍五入取2位小数
            str4 = ((long)(num * 100)).ToString(); //将num乘100并转换成字符串形式
            j = str4.Length; //找出最高位
            if (j > 15) { return "溢出"; }
            str2 = str2.Substring(15 - j); //取出对应位数的str2的值。如：200.55,j为5所以str2=佰拾元角分

            //循环取出每一位需要转换的值
            for (i = 0; i < j; i++)
            {
                str3 = str4.Substring(i, 1); //取出需转换的某一位的值
                temp = Convert.ToInt32(str3); //转换为数字
                if (i != (j - 3) && i != (j - 7) && i != (j - 11) && i != (j - 15))
                {
                    //当所取位数不为元、万、亿、万亿上的数字时
                    if (str3 == "0")
                    {
                        ch1 = "";
                        ch2 = "";
                        nzero = nzero + 1;
                    }
                    else
                    {
                        if (str3 != "0" && nzero != 0)
                        {
                            ch1 = "零" + str1.Substring(temp * 1, 1);
                            ch2 = str2.Substring(i, 1);
                            nzero = 0;
                        }
                        else
                        {
                            ch1 = str1.Substring(temp * 1, 1);
                            ch2 = str2.Substring(i, 1);
                            nzero = 0;
                        }
                    }
                }
                else
                {
                    //该位是万亿，亿，万，元位等关键位
                    if (str3 != "0" && nzero != 0)
                    {
                        ch1 = "零" + str1.Substring(temp * 1, 1);
                        ch2 = str2.Substring(i, 1);
                        nzero = 0;
                    }
                    else
                    {
                        if (str3 != "0" && nzero == 0)
                        {
                            ch1 = str1.Substring(temp * 1, 1);
                            ch2 = str2.Substring(i, 1);
                            nzero = 0;
                        }
                        else
                        {
                            if (str3 == "0" && nzero >= 3)
                            {
                                ch1 = "";
                                ch2 = "";
                                nzero = nzero + 1;
                            }
                            else
                            {
                                if (j >= 11)
                                {
                                    ch1 = "";
                                    nzero = nzero + 1;
                                }
                                else
                                {
                                    ch1 = "";
                                    ch2 = str2.Substring(i, 1);
                                    nzero = nzero + 1;
                                }
                            }
                        }
                    }
                }
                if (i == (j - 11) || i == (j - 3))
                {
                    //如果该位是亿位或元位，则必须写上
                    ch2 = str2.Substring(i, 1);
                }
                str5 = str5 + ch1 + ch2;

                if (i == j - 1 && str3 == "0")
                {
                    //最后一位（分）为0时，加上"整"
                    str5 = str5 + '整';
                }
            }
            if (num == 0)
            {
                str5 = "零元整";
            }
            return str5;
        }

        /// <summary>
        /// 0性别，1出生年月
        /// </summary>
        /// <param name="IDCard"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string ToBirthday(string IDCard, int type)
        {

            try
            {
                string identityCard = IDCard.Trim();
                if (!string.IsNullOrEmpty(identityCard))
                {
                    if (identityCard.Length != 15 && identityCard.Length != 18)
                    {
                        return "";
                    }
                }
                string birthday = "";
                string sex = "";
                if (identityCard.Length == 18)
                {
                    birthday = identityCard.Substring(6, 4) + "-" + identityCard.Substring(10, 2) + "-" + identityCard.Substring(12, 2);
                    sex = identityCard.Substring(14, 3);
                }
                if (identityCard.Length == 15)
                {
                    birthday = "19" + identityCard.Substring(6, 2) + "-" + identityCard.Substring(8, 2) + "-" + identityCard.Substring(10, 2);
                    sex = identityCard.Substring(12, 3);
                }
                if (int.Parse(sex) % 2 == 0)//性别代码为偶数是女性奇数为男性
                {
                    sex = "女";
                }
                else
                {
                    sex = "男";
                }
                if (type == 0)
                {
                    return sex;
                }
                else
                {
                    return birthday;
                }
            }
            catch (Exception ex)
            {
                return "";
            }

        }

    }
}
