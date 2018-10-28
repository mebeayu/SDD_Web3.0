using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {

            //    string s = "13827470343";
            //    string sss=RRL.Core.Utility.DES.EncryptDES(s);

            //    string sbs=RRL.Core.Utility.DES.DecryptDES(sss);
            //    Console.WriteLine(sss);
            //    //ycQnXJaOGVA9anaBP3VM6gDENGHAODENGHAO
            //    //chuzkrRvv0DtHNumMhRWmgDENGHAODENGHAO

            //    Console.ReadKey();
            // var s=Convert.ToDecimal( DBNull.Value.ToString());
            // var cc= Convert.ToString(Convert.ToInt32((DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000));
            string input = "";
            var st = GetMD5Hash(input);
            Console.WriteLine(st);
            Console.ReadKey();

        }
        static string GetMD5Hash(string input)
        {
            var md5 = new MD5CryptoServiceProvider();
            var bs = Encoding.UTF8.GetBytes(input);
            bs = md5.ComputeHash(bs);
            var s = new StringBuilder();
            foreach (var b in bs)
            {
                s.Append(b.ToString("x2"));
            }
            var res = s.ToString();
            return res;
        }

    }
}
