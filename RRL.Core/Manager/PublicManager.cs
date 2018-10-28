using QRCoder;
using RRL.Core.External;
using RRL.Core.Models;
using RRL.Core.Models.WxJSAPI;
using RRL.Core.Pay.WxPay;
using RRL.DB;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace RRL.Core.Manager
{
    public class PublicManager
    {
        /// <summary>
        /// 获取字符串SHA1值
        /// </summary>
        /// <param name="text">字符串</param>
        /// <returns>获取结果</returns>
        private static string SHA1(string text)
        {
            byte[] cleanBytes = Encoding.Default.GetBytes(text);
            byte[] hashedBytes = System.Security.Cryptography.SHA1.Create().ComputeHash(cleanBytes);
            return BitConverter.ToString(hashedBytes).Replace("-", "");
        }

        /// <summary>
        /// 获取微信JSAPI配置对象
        /// </summary>
        /// <returns></returns>
        public static JsConfigObject GetConfig(string url)
        {
            string noncestr = WxPayApi.GenerateNonceStr();
            WxJsAPICache cache = new WxJsAPICache();
            string ticket = cache.GetJsApiTicket();
            long timeStart = new DateTime(1970, 1, 1).Ticks;
            long timeEnd = DateTime.UtcNow.Ticks;
            long timestamp = (timeEnd - timeStart) / 10000000;
            string strres = "jsapi_ticket=" + ticket + "&noncestr=" + noncestr + "&timestamp=" + timestamp + "&url=" + url;
            //HelpManager.Mark(strres);
            string signature = SHA1(strres).ToLower();
            JsConfigObject Obj = new JsConfigObject();
            Obj.debug = false;
            Obj.appId = WxPayConfig.WxMPAPPID;
            Obj.timestamp = timestamp;
            Obj.nonceStr = noncestr;
            Obj.signature = signature;

            return Obj;
        }

        /// <summary>
        /// 获取图片二进制编码
        /// </summary>
        /// <param name="id">图片id</param>
        /// <returns>二进制码</returns>
        public byte[] getPicture(int id)
        {
            byte[] res = null;
            RRLPICDB db = new RRLPICDB();
            DataSet ds = db.ExeQuery("select pic_data from rrl_pics where id=@id", new SqlParameter("id", id));
            if (ds == null)
            {
                return res;
            }
            if (ds.Tables[0].Rows.Count == 0)
            {
                return res;
            }
            db.Close();
            res = (byte[])ds.Tables[0].Rows[0]["pic_data"];
            return res;
        }

        /// <summary>
        /// 获取区域字典
        /// </summary>
        /// <returns>区域字典</returns>
        public DataSet getzonecode()
        {
            return Dictionary.GetAreaCode();
        }

        /// <summary>
        /// 获取银行字典
        /// </summary>
        /// <returns>银行字典</returns>
        public DataSet getbankdic()
        {
            return Dictionary.GetBankMain();
        }

        /// <summary>
        /// 获取快递公司字典
        /// </summary>
        /// <returns>快递公司字典</returns>
        public DataSet gettrackcom()
        {
            return Dictionary.GetTrackCom();
        }

        /// <summary>
        /// 获取快递数据
        /// </summary>
        /// <param name="TrackCom">快递公司编码</param>
        /// <param name="TrackNum">快递单号</param>
        /// <returns>结果字符串</returns>
        public string GetTrackInfo(string TrackCom, string TrackNum)
        {
            return KuaiDi100.QueryData(TrackCom, TrackNum);
        }

        /// <summary>
        /// 获取推荐商品图片
        /// </summary>
        /// <param name="id">商品id</param>
        /// <param name="username">用户名</param>
        /// <returns></returns>
        public byte[] GetShareGoodsPic(int id, string username)
        {
            var baseDir = AppDomain.CurrentDomain.BaseDirectory;
            var filePath = $@"Assets\GoodsShare\Img\Single\{id}.jpg";
            var fileName = Path.Combine(baseDir, filePath);
            byte[] result = null;
            using (var baseImg = new Bitmap(fileName))
            {
                //var height = bitmap.Size.Height;
                //var width = bitmap.Size.Width;
                //计算QR偏移值
                //height - 114 - 280
                var y = baseImg.Height - 394;
                //width - 78 - 280
                var x = 722;
                using (var outImgG = Graphics.FromImage(baseImg))
                {
                    //获取QR
                    using (var qrGenerator = new QRCodeGenerator())
                    {
                        using (var qrCodeData = qrGenerator.CreateQrCode(@"https://e-shop.rrlsz.com.cn/Login?hash=/goodsdetail/" + $@"{id}/{username}", QRCodeGenerator.ECCLevel.Q))
                        {
                            using (var qrCode = new QRCode(qrCodeData))
                            {
                                using (var qrCodeG = qrCode.GetGraphic(20))
                                {
                                    //var width = qrCodeG.Width;
                                    //var height = qrCodeG.Height;
                                    //等比例缩放
                                    using (var qrImg = new Bitmap(280, 280))
                                    {
                                        using (var outQrG = Graphics.FromImage(qrImg))
                                        {
                                            outQrG.Clear(Color.Transparent);
                                            outQrG.CompositingQuality = CompositingQuality.HighQuality;
                                            outQrG.SmoothingMode = SmoothingMode.HighQuality;
                                            outQrG.InterpolationMode = InterpolationMode.HighQualityBicubic;
                                            outQrG.DrawImage(qrCodeG, new Rectangle(0, 0, 280, 280), new Rectangle(0, 0, qrCodeG.Width, qrCodeG.Height), GraphicsUnit.Pixel);
                                        }

                                        outImgG.DrawImage(qrImg, new Rectangle(x, y, qrImg.Width, qrImg.Height));
                                    }
                                }
                            }
                        }
                    }
                }

                using (var ep = new EncoderParameters())
                {
                    var qy = new long[1];
                    qy[0] = 75;

                    var eParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, qy);
                    ep.Param[0] = eParam;

                    try
                    {
                        var arrayIci = ImageCodecInfo.GetImageDecoders();
                        var jpegIcIinfo = arrayIci.FirstOrDefault(t => t.FormatDescription.Equals("JPEG"));

                        using (var ms = new MemoryStream())
                        {
                            if (jpegIcIinfo != null)
                            {
                                baseImg.Save(ms, jpegIcIinfo, ep);
                            }
                            else
                            {
                                baseImg.Save(ms, baseImg.RawFormat);
                            }
                            result = ms.ToArray();
                        }

                    }
                    catch
                    {
                        // ignored
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// 获取复数张推荐图片
        /// </summary>
        /// <param name="id">商品id</param>
        /// <returns></returns>
        public List<string> GetMutipleShareGoodsPics(int id)
        {
            var baseDir = AppDomain.CurrentDomain.BaseDirectory;
            var dirPath = $@"Assets\GoodsShare\Img\Mutiple\{id}\";
            var dirName = Path.Combine(baseDir, dirPath);
            var dirInfo = new DirectoryInfo(dirName);

            return dirInfo.GetFiles("*.jpg").Select(f => Regex.Replace(f.FullName.Remove(0, baseDir.Length), @"\\", @"/")).Take(5).ToList();
        }

        /// <summary>
        /// 获取推荐商品文字信息
        /// </summary>
        /// <param name="id">商品id</param>
        /// <returns></returns>
        public string GetShareGoodeText(int id)
        {
            var result = "";
            var baseDir = AppDomain.CurrentDomain.BaseDirectory;
            var filePath = $@"Assets\GoodsShare\Text\{id}.txt";
            var fileName = Path.Combine(baseDir, filePath);
            using (var sr = new StreamReader(fileName, Encoding.Default))
            {
                result = sr.ReadToEnd();
            }
            return result;
        }

        // STORY #18 lcl 20180518 Insert
        /// <summary>
        /// 生成二维码图像
        /// </summary>
        /// <param name="qrCodeContent">需要生成的内容</param>
        /// <returns>位图对象</returns>
        public Image GenerateQRCodeImage(string qrCodeContent)
        {
            using (var qrGenerator = new QRCodeGenerator())
            {
                using (var qrCodeData = qrGenerator.CreateQrCode(qrCodeContent, QRCodeGenerator.ECCLevel.Q))
                {
                    using (var qrCode = new QRCode(qrCodeData))
                    {
                        return qrCode.GetGraphic(20);
                    }
                }
            }
        }

        #region 图像处理

        // STORY #18 lcl 20180518 Insert
        /// <summary>
        /// 等比例裁剪为指定宽度和高度的图片
        /// </summary>
        /// <param name="sourceImage">原始图像</param>
        /// <param name="newWidth">裁剪后的图像宽度</param>
        /// <param name="newHeight">裁剪后的图像高度</param>
        /// <returns></returns>
        public Image CutToFixedSizeByEqualProportion(Image sourceImage, int newWidth, int newHeight)
        {
            var resultImage = new Bitmap(newWidth, newHeight);
            using (var resultG = Graphics.FromImage(resultImage))
            {
                resultG.Clear(Color.Transparent);
                resultG.CompositingQuality = CompositingQuality.HighQuality;
                resultG.SmoothingMode = SmoothingMode.HighQuality;
                resultG.InterpolationMode = InterpolationMode.HighQualityBicubic;
                resultG.DrawImage(sourceImage, new Rectangle(0, 0, newWidth, newHeight), new Rectangle(0, 0, sourceImage.Width, sourceImage.Height), GraphicsUnit.Pixel);
            }

            return resultImage;
        }

        #endregion
    }
}