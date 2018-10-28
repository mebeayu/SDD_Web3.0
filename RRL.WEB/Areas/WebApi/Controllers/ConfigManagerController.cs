using RRL.Core.Manager;
using RRL.Core.Utility;
using RRL.WEB.Models.Response;
using RRL.WEB.Models.ViewModel;
using RRL.WEB.Ulity;
using System.Collections.Generic;
using System.Data;
using System.Web.Http;
using WebApi.OutputCache.V2;

namespace RRL.WEB.Areas.WebApi.Controllers
{
    /// <summary>
    /// 配置管理接口
    /// </summary>
    public class ConfigManagerController : ApiController
    {
        private ConfigManager cm = new ConfigManager();

        /// <summary>
        /// 获取精选页面轮播图(已更新)
        /// </summary>
        /// <returns>结果对象</returns>
        [HttpGet]
        [ActionName("GoodsCarousel")]
        public ResultView<List<CarouslFigure>> GetGoodsCarousel()
        {
            DataSet ds = cm.GetGoodsCarousel();
            return ResultView<CarouslFigure>.InitFromDataSet(ds);
        }

        /// <summary>
        /// 获取主页面轮播图(已更新)
        /// </summary>
        /// <returns>结果对象</returns>
        [HttpGet]
        [ActionName("MainCarousel")]
        [CacheOutput(ServerTimeSpan = 7200, ClientTimeSpan = 7200)]
        public ResultView<List<CarouslFigure>> GetMainCarousel()
        {
            DataSet ds = cm.GetMainCarousel();
            return ResultView<CarouslFigure>.InitFromDataSet(ds);
        }


        [HttpGet]
        [Route("SWebApi/ConfigManager/MainCarousel.json")]
        [CacheOutput(ServerTimeSpan = 7200, ClientTimeSpan = 7200)]
        public ResultView<List<CarouslFigure>> GetMainCarousel_json()
        {
            return GetMainCarousel();
        }

        /// <summary>
        /// 获取活动轮播图(新增)
        /// </summary>
        /// <returns>结果对象</returns>
        [HttpGet]
        [ActionName("EvevtCarousel")]
        public ResultView<List<CarouslFigure>> GetEventCarousel()
        {
            DataSet ds = cm.GetEventCarousel();
            return ResultView<CarouslFigure>.InitFromDataSet(ds);
        }

        /// <summary>
        /// 获取兑换页轮播图(新增)
        /// </summary>
        /// <returns>结果对象</returns>
        [HttpGet]
        [ActionName("ExchangeCarousel")]
        public ResultView<List<CarouslFigure>> GetExchangeCarousel()
        {
            DataSet ds = cm.GetExchangeCarousel();
            return ResultView<CarouslFigure>.InitFromDataSet(ds);
        }

        /// <summary>
        /// 获取游戏分区
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ActionName("GameArea")]
        [CacheOutput(ServerTimeSpan = 7200, ClientTimeSpan = 7200)]
        public ResultView<int> GetGameArea()
        {
            ResultView<int> resault = ResultView<int>.InitFromMessageCode(MessageCode.SUCCESS);
            resault.data = cm.GetGameArea();
            return resault;
        }

        /// <summary>
        /// 获取金币分区
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ActionName("GoldenCoinArea")]
        [CacheOutput(ServerTimeSpan = 7200, ClientTimeSpan = 7200)]
        public ResultView<int> GetGoldenCoinArea()
        {
            ResultView<int> resault = ResultView<int>.InitFromMessageCode(MessageCode.SUCCESS);
            resault.data = cm.GetGoldenCoinArea();
            return resault;
        }

        /// <summary>
        /// 获取安卓APP版本升级信息(已更新)
        /// </summary>
        /// <returns>APP版本信息对象</returns>
        [HttpGet]
        [ActionName("AndroidVersionInfo")]
        public AppVersionObject GetAndroidVersionInfo()
        {
            DataSet ds = cm.GetAndroidAppVersionInfo();
            return DataSetHelper.ConvertDataRowToObj<AppVersionObject>(ds.Tables[0].Rows[0]);
        }


        /// <summary>
        /// 获取苹果APP版本升级信息(已更新)
        /// </summary>
        /// <returns>APP版本信息对象</returns>
        [HttpGet]
        [ActionName("AppleVersionInfo")]
        public AppleVersionObject GetAppleVersionInfo()
        {
            DataSet ds = cm.GetAppleAppVersionInfo();
            return DataSetHelper.ConvertDataRowToObj<AppleVersionObject>(ds.Tables[0].Rows[0]);
        }

        /// <summary>
        /// 获取APP简介(已更新)
        /// </summary>
        /// <returns>结果对象</returns>
        [HttpGet]
        [ActionName("AppSummary")]
        public ResultView<string> GetAppSummary()
        {
            ResultView<string> resault = ResultView<string>.InitFromMessageCode(MessageCode.SUCCESS);
            resault.data = cm.GetAppSummary();
            return resault;
        }


    }
}