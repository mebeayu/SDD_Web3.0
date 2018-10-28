using System;
using RRL.WEB.Ulity;
using System.Web.Mvc;
using RRL.Core.Models;
using RRL.Core.Utility;
using RRL.DB;
using RRL.Core.Manager;
using Common;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Common.Utils;

namespace RRL.WEB.Controllers
{
    public class VerCodeController : Controller
    {
        public RdSession rdsession = new RdSession(System.Web.HttpContext.Current);
        public static string IMAGEKEY_SUBFIX="_Image_Code";
        // GET: Game
        public FileContentResult Picture()
        {
            
            YZMHelper yzm = new YZMHelper();
            yzm.CreateImage();
            var image = yzm.Image;
            var code = yzm.Text;
            var data = ConvertImage(image);
            //var ip_str=IPUtils.getIPAddress();
            //double currTimes=rdsession.Redis.StringIncrement("IP_ref"+ip_str);
            //if(currTimes<=1)
            //{
            //    rdsession.Redis.KeyExpire("IP_ref"+ip_str  , new TimeSpan(6, 0, 0));
            //}
            //if (currTimes>20)
            //{
            //    return File((byte[])null, "image/jpeg");
            //}
            rdsession.Redis.StringSet(rdsession.SessionId+IMAGEKEY_SUBFIX, code, new TimeSpan(0, 20, 0));
            return File(data, "image/jpeg");
            
        }

         

        private byte[] ConvertImage(Image image)
        {

            MemoryStream ms = new MemoryStream();
            image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            return ms.ToArray();

              
        }

    }
}