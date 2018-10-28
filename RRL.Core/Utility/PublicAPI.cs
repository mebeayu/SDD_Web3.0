using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace RRL.Core.Utility
{
    public class PublicAPI
    {
        //public static string separator = "@rrl@";
        //public static string JoinString(string[] arr)
        //{
        //    return string.Join(separator, arr);
        //}
        //public static string GetArrayData(string status, string msg)
        //{
        //    return GetArrayData(status, msg, null);
        //}
        //public static string GetArrayData(string status, string msg, string data)
        //{
        //    JsonData json = new JsonData();
        //    json["status"] = status;
        //    json["message"] = msg;
        //    if (data != null && data != "") json["data"] = data;
        //    return json.ToJson();
        //}
        //public static string GenerateRandomCode(int codeCount)
        //{
        //    string str = string.Empty;
        //    int num2 = (int)DateTime.Now.Ticks;

        //    Random random = new Random(num2);
        //    for (int i = 0; i < codeCount; i++)
        //    {
        //        char ch;
        //        int num = random.Next();
        //        if ((num % 2) == 0)
        //        {
        //            ch = (char)(0x30 + ((ushort)(num % 10)));
        //        }
        //        else
        //        {
        //            ch = (char)(0x41 + ((ushort)(num % 0x1a)));
        //        }
        //        str = str + ch.ToString();
        //    }
        //    return str;
        //}
        ///// <summary>
        ///// SHA1 加密，返回大写字符串
        ///// </summary>
        ///// <param name="content">需要加密字符串</param>
        ///// <param name="encode">指定加密编码</param>
        ///// <returns>返回40位大写字符串</returns>
        //public static string SHA1(string content, Encoding encode)
        //{
        //    try
        //    {
        //        SHA1 sha1 = new SHA1CryptoServiceProvider();
        //        byte[] bytes_in = encode.GetBytes(content);
        //        byte[] bytes_out = sha1.ComputeHash(bytes_in);
        //        sha1.Dispose();
        //        string result = BitConverter.ToString(bytes_out);
        //        result = result.Replace("-", "");
        //        return result;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("SHA1加密出错：" + ex.Message);
        //    }
        //}

        public static List<int> StrToIntList(string liststr)
        {
            string[] StrArr = liststr.Split(',');
            int[] IntArr = new int[StrArr.Length];
            for (int i = 0; i < StrArr.Length; i++)
            {
                IntArr[i] = Convert.ToInt32(StrArr[i]);
            }
            List<int> IntList = new List<int>();
            IntList.AddRange(IntArr);
            return IntList;
        }

        public static string GetMD5Hash(string input)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] bs = Encoding.UTF8.GetBytes(input);
            bs = md5.ComputeHash(bs);
            StringBuilder s = new StringBuilder();
            foreach (byte b in bs)
            {
                s.Append(b.ToString("x2"));
            }
            string res = s.ToString();
            return res;
        }

        //public static string GetMD5HashFromFile(string filePath)
        //{
        //    try
        //    {
        //        FileStream file = new FileStream(filePath, FileMode.Open);

        //        System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();

        //        byte[] retVal = md5.ComputeHash(file);

        //        file.Close();

        //        StringBuilder sb = new StringBuilder();

        //        for (int i = 0; i < retVal.Length; i++)
        //        {
        //            sb.Append(retVal[i].ToString("x2"));

        //        }

        //        return sb.ToString().ToUpper();

        //    }

        //    catch (System.Exception ex)
        //    {
        //        throw new System.Exception("GetMD5HashFromFile() fail,error:" + ex.Message);

        //    }

        //}
        //public static string PostWebService(string URL, string MethodName, string Param)
        //{
        //    string strURL = URL + "/" + MethodName;
        //    HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(strURL);
        //    request.Method = "POST";
        //    request.ContentType = "application/x-www-form-urlencoded";
        //    request.Credentials = CredentialCache.DefaultCredentials;
        //    request.Timeout = 10000;
        //    //string content = string.Format("username={0}&password={1}", textUserName.Text, textPassword.Text);
        //    byte[] param = Encoding.UTF8.GetBytes(Param);
        //    request.ContentLength = param.Length;
        //    //获得请 求流
        //    System.IO.Stream writer = request.GetRequestStream();
        //    //将请求参数写入流
        //    writer.Write(param, 0, param.Length);
        //    // 关闭请求流
        //    writer.Close();
        //    System.Net.HttpWebResponse response;
        //    response = (System.Net.HttpWebResponse)request.GetResponse();
        //    System.IO.StreamReader myreader = new System.IO.StreamReader(response.GetResponseStream(), Encoding.UTF8);
        //    string responseText = myreader.ReadToEnd();
        //    myreader.Close();
        //    return responseText;
        //}
        public static string HttpPost(string strURL, byte[] postData)
        {
            System.Net.ServicePointManager.DefaultConnectionLimit = 512;
            string result = "";
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(strURL);
            req.Method = "POST";

            req.ContentType = "application/x-www-form-urlencoded";
            req.ContentLength = postData.Length;
            using (Stream reqStream = req.GetRequestStream())
            {
                reqStream.Write(postData, 0, postData.Length);
                reqStream.Flush();
                reqStream.Close();
            }
            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
            Stream stream = resp.GetResponseStream();
            //获取响应内容
            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
            {
                result = reader.ReadToEnd();
            }
            return result;
        }

        //public static string HttpPost(string strURL, string strParam)
        //{
        //    string result = "";
        //    HttpWebRequest req = (HttpWebRequest)WebRequest.Create(strURL);
        //    req.Method = "POST";
        //    req.ContentType = "application/x-www-form-urlencoded";
        //    byte[] data = Encoding.GetEncoding("UTF-8").GetBytes(strParam);
        //    req.ContentLength = data.Length;
        //    using (Stream reqStream = req.GetRequestStream())
        //    {
        //        reqStream.Write(data, 0, data.Length);
        //        reqStream.Flush();
        //        reqStream.Close();

        //    }
        //    HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
        //    Stream stream = resp.GetResponseStream();
        //    //获取响应内容
        //    using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
        //    {
        //        result = reader.ReadToEnd();
        //    }
        //    return result;

        //}
        //public static string HttpGet(string strURL)
        //{
        //    try
        //    {
        //        System.Net.HttpWebRequest request;
        //        // 创建一个HTTP请求
        //        request = (System.Net.HttpWebRequest)WebRequest.Create(strURL);
        //        //request.Method="get";
        //        System.Net.HttpWebResponse response;
        //        response = (System.Net.HttpWebResponse)request.GetResponse();
        //        System.IO.StreamReader myreader = new System.IO.StreamReader(response.GetResponseStream(), Encoding.UTF8);
        //        string responseText = myreader.ReadToEnd();
        //        myreader.Close();
        //        return responseText;
        //    }
        //    catch (Exception ex)
        //    {
        //        return "";
        //    }

        //}

        //public static XmlDocument DataSetToXml(DataSet ds)
        //{
        //    string xml = ds.GetXml();
        //    XmlDocument x = new XmlDocument();
        //    x.LoadXml(xml);
        //    return x;
        //}
        //public static int DateTimeToInt(DateTime Date)
        //{
        //    DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));

        //    TimeSpan toNow = Date - dtStart;
        //    string timeStamp = toNow.Ticks.ToString();
        //    timeStamp = timeStamp.Substring(0, timeStamp.Length - 7);
        //    return int.Parse(timeStamp);
        //}
        //public static int DateTimeToInt(string strDate)
        //{
        //    DateTime Date = DateTime.Parse(strDate);
        //    return DateTimeToInt(Date);
        //}
        //public static DateTime IntToDateTime(string timeStamp)
        //{
        //    DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
        //    long lTime = long.Parse(timeStamp + "0000000");
        //    TimeSpan toNow = new TimeSpan(lTime);
        //    DateTime dtResult = dtStart.Add(toNow);
        //    return dtResult;
        //}
        ///// <summary>
        ///// 生成一个自定长度的随机字符串
        ///// </summary>
        ///// <param name="Length">要返回的字符串长度</param>
        ///// <returns>指的长度的字符串</returns>
        //private static string MakeRandomString(int Length)
        //{     //声明要返回的字符串
        //    string tmpstr = "";
        //    //密码中包含的字符数组
        //    string strchars = "abcdefghijklmnopqrstuvwxyz0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        //    //数组索引随机数
        //    int iRandNum;
        //    //随机数生成器
        //    Random rnd = new Random();
        //    for (int i = 0; i < Length; i++)
        //    {      //Random类的Next方法生成一个指定范围的随机数
        //        iRandNum = rnd.Next(strchars.Length);
        //        //tmpstr随机添加一个字符
        //        tmpstr += strchars[iRandNum];
        //    }
        //    return tmpstr;
        //}
        //public static Bitmap GetImageFromBase64(string base64string)
        //{
        //    byte[] b = Convert.FromBase64String(base64string);
        //    MemoryStream ms = new MemoryStream(b);
        //    Bitmap bitmap = new Bitmap(ms);
        //    return bitmap;
        //}
        //public static string GetBase64FromImage(Bitmap bitmap)
        //{
        //    string strbaser64 = "";
        //    try
        //    {
        //        MemoryStream ms = new MemoryStream();
        //        bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
        //        byte[] arr = new byte[ms.Length];
        //        ms.Position = 0;
        //        ms.Read(arr, 0, (int)ms.Length);
        //        ms.Close();
        //        strbaser64 = Convert.ToBase64String(arr);
        //    }
        //    catch (Exception)
        //    {
        //        return "";
        //        //throw new Exception("Something wrong during convert!");
        //    }
        //    return strbaser64;
        //}
    }
}