using RedisHelp;
using RRL.Config;
using RRL.Core.External;
using RRL.Core.Manager;
using RRL.Core.Models;
using RRL.Core.Models.WxJSAPI;
using RRL.Core.Utility;
using RRL.WEB.Models.Response;
using RRL.WEB.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web.Http;
using WebApi.OutputCache.V2;

namespace RRL.WEB.Areas.WebApi.Controllers
{
    /// <summary>
    /// 开放WebApi
    /// </summary>
    public class PublicController : ApiController
    {
        private PublicManager pm = new PublicManager();

        /// <summary>
        /// 获取图片数据库中的图片(没想好怎么改)
        /// </summary>
        /// <param name="id">图片索引</param>
        /// <returns>图片响应</returns>
        [HttpGet]
        [ActionName("picture")]
        public HttpResponseMessage picture(int id)
        {
            try
            {
                var imgByte = pm.getPicture(id);
                var imgStream = new MemoryStream(imgByte);
                var resp = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StreamContent(imgStream)
                };
                resp.Content.Headers.ContentType = new MediaTypeHeaderValue("image/jpg");
                resp.Content.Headers.Expires = new DateTimeOffset(DateTime.Now.AddDays(5));
                return resp;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 静态化-资源
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("SWebApi/Public/picture/{id}.jpg")]
        [CacheOutput(ServerTimeSpan = 7200, ClientTimeSpan = 604800, ExcludeQueryStringFromCacheKey = true)]
        public HttpResponseMessage picture_jpg(int id)
        {

            try
            {
                var imgByte = pm.getPicture(id);
                var imgStream = new MemoryStream(imgByte);
                var resp = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StreamContent(imgStream)
                };
                resp.Content.Headers.ContentType = new MediaTypeHeaderValue("image/jpg");
                //resp.Content.Headers.Expires = new DateTimeOffset(DateTime.Now.AddDays(5));
                return resp;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 获取商品分享图片
        /// </summary>
        /// <param name="id">商品索引(测试id:1)</param>
        /// <param name="token">短效token</param>
        /// <returns>图片响应</returns>
        [HttpGet]
        [ActionName("ShareGoodsPic")]
        public HttpResponseMessage GetShareGoodsPic(int id, string token = null)
        {
            var username = "null";
            if (token == null)
            {
                username = "name!";
            }
            else
            {
                var tokenObj = TokenObject.InitTokenObjFromString(token);
                if (string.Equals(TokenObject.ShortTimeToken, tokenObj.Prefix))
                {
                    var user = new UserAuth();
                    var res = user.Load(tokenObj.id);
                    if (res == MessageCode.SUCCESS)
                    {
                        username = user.username;
                    }
                }
            }
            var imgByte = pm.GetShareGoodsPic(id, username);
            try
            {
                var imgStream = new MemoryStream(imgByte);
                var resp = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StreamContent(imgStream)
                };
                resp.Content.Headers.ContentType = new MediaTypeHeaderValue("image/png");
                return resp;
            }
            catch (Exception e)
            {
                var resp = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(e.Message, Encoding.UTF8, "text/plain")
                };

                return resp;
            }
        }

        /// <summary>
        /// 获取复数图片分享
        /// </summary>
        /// <param name="goods_id">商品id</param>
        /// <returns></returns>
        [HttpGet]
        [ActionName("ShareGoodsPics")]
        public DataResult GetMutipleShareGoodsPic(int goods_id)
        {
            var piclist = pm.GetMutipleShareGoodsPics(goods_id).Select(s => $@"{PlatFormConfig.BaseUrl}{s}").ToList();

            var result = DataResult.InitFromMessageCode(MessageCode.SUCCESS);
            result.data = piclist;

            return result;
        }

        /// <summary>
        /// 获取商品分享文字
        /// </summary>
        /// <param name="id">商品索引(测试id:1)</param>
        /// <returns></returns>
        [HttpGet]
        [ActionName("ShareGoodsText")]
        public DataResult GetShareGoodsText(int id)
        {
            var text = pm.GetShareGoodeText(id);
            if (string.IsNullOrEmpty(text))
            {
                return DataResult.InitFromMessageCode(MessageCode.ERROR_UNKONWN);
            }
            var result = DataResult.InitFromMessageCode(MessageCode.SUCCESS);
            result.data = text;
            return result;
        }

        /// <summary>
        /// 获取通知消息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ActionName("InfoMessage")]
        public DataResult GetInfoMessage()
        {
            var result = DataResult.InitFromMessageCode(MessageCode.SUCCESS);
            result.data = new
            {
                length = 0,
                infoArray = new List<object>
                {
                    //new
                    //{
                    //    title = "title1",
                    //    content = "content1",
                    //    pic = 1
                    //},
                    //new
                    //{
                    //    title = "title2",
                    //    content = "content2",
                    //    pic = 1
                    //},
                    //new
                    //{
                    //    title = "title3",
                    //    content = "content3",
                    //    pic = 1
                    //},
                    //new
                    //{
                    //    title = "title4",
                    //    content = "content4",
                    //    pic = 1
                    //}
                }
            };
            return result;
        }

        /// <summary>
        /// 获取区域编码字典(已更新)
        /// </summary>
        /// <returns>结果对象</returns>
        [HttpGet]
        [ActionName("zonecode")]
        public ResultView<List<AreaCode>> zonecode()
        {
            DataSet ds = pm.getzonecode();
            return ResultView<AreaCode>.InitFromDataSet(ds);
        }

        /// <summary>
        /// 获取银行卡编码字典(已更新)
        /// </summary>
        /// <returns>结果对象</returns>
        [HttpGet]
        [ActionName("bankdic")]
        public ResultView<List<BankMain>> bankdic()
        {
            DataSet ds = pm.getbankdic();
            return ResultView<BankMain>.InitFromDataSet(ds);
        }

        /// <summary>
        /// 获取快递公司中字典(已更新)
        /// </summary>
        /// <returns>结果对象</returns>
        [HttpGet]
        [ActionName("trackcom")]
        public ResultView<List<TrackCom>> trcakcom()
        {
            DataSet ds = pm.gettrackcom();
            return ResultView<TrackCom>.InitFromDataSet(ds);
        }

        /// <summary>
        /// 获取快递信息(不需更新)
        /// </summary>
        /// <param name="TrackCom">快递公司编码,测试:shunfeng</param>
        /// <param name="TrackNum">快递单号,测试:236560068552</param>
        /// <returns>物流信息对象</returns>
        [HttpGet]
        [ActionName("TrackInfo")]
        public TrackResult gettrackinfo(string TrackCom, string TrackNum)
        {
            string json = pm.GetTrackInfo(TrackCom, TrackNum);

            TrackResult Track = Newtonsoft.Json.JsonConvert.DeserializeObject<TrackResult>(json);
            return Track;
        }

        /// <summary>
        /// 获取最近一条快递信息(不需更新)
        /// </summary>
        /// <returns>物流信息对象</returns>
        [HttpGet]
        [ActionName("RecentTrackInfo")]
        public TrackNode getRecentTrackInfo(string TrackCom, string TrackNum)
        {
            string json = pm.GetTrackInfo(TrackCom, TrackNum);

            TrackResult Track = Newtonsoft.Json.JsonConvert.DeserializeObject<TrackResult>(json);
            return Track.GetRecent();
        }

        /// <summary>
        /// 获取微信JsAPI配置,SPA外部不可使用(不需更新)
        /// </summary>
        /// <param name="url">生成签名的链接</param>
        /// <returns></returns>
        [HttpGet]
        [ActionName("WxJsApiConfig")]
        public JsConfigObject getWxJsConfig(string url = null)
        {
            string uri = PlatFormConfig.WxMPSPAURI;
            return PublicManager.GetConfig(uri);
        }

        /// <summary>
        /// 获取微信JsAPI配置(不需更新)
        /// </summary>
        /// <param name="url">请求令牌的URL</param>
        /// <returns></returns>
        [HttpGet]
        [ActionName("WxJsApiConfigFroPage")]
        public JsConfigObject getWxJsConfigFroPage(string url)
        {
            //HelpManager.Mark("debug")
            return PublicManager.GetConfig(url);
        }

        /// <summary>
        /// 发送公共短信（无参数）
        /// </summary>
        /// <param name="mobile">手机号码</param>
        /// <param name="templateCode">短信模板编码</param>
        /// <returns>结果对象</returns>
        [HttpGet]
        [ActionName("SendSMS")]
        public DataResult SendPublicSms(string mobile, string templateCode)
        {
            AliSMS sms = new AliSMS();
            int res = sms.SendSMS(mobile, templateCode, new PublicSMS());
            return DataResult.InitFromMessageCode(res);
        }
    }
}