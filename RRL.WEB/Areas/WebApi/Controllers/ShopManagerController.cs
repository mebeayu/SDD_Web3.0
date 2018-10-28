using Newtonsoft.Json;
using RedisHelp;
using RRL.Core.Manager;
using RRL.Core.Models;
using RRL.Core.Utility;
using RRL.DB;
using RRL.WEB.Models.Response;
using RRL.WEB.Models.ViewModel;
using RRL.WEB.Ulity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Web.Caching;
using System.Web.Http;
using System.Web.Script.Serialization;
using WebApi.OutputCache.V2;

namespace RRL.WEB.Areas.WebApi.Controllers
{
    /// <summary>
    /// 商户管理接口
    /// </summary>
    //[RoutePrefix("WebApi/ShopManager")]
    public class ShopManagerController : ApiController
    {
        private ShopManager sm = new ShopManager();

        /// <summary>
        /// 获取商品分类列表(已更新)
        /// </summary>
        /// <returns>结果对象</returns>
        [HttpGet]
        [ActionName("GoodsTypeList")]
        public ResultView<List<GoodsType>> GetGoodsTypeList()
        {
            DataSet ds = sm.GetGoodsTypeList();
            DataSet ca = sm.GetShopCateCarousl();
            DataTable dt = ds.Tables[0];
            DataTable ct = ca.Tables[0];

            GoodsType type = new GoodsType();

            #region HotRecommend
            //try
            //{
            //    GoodsType RecomendType = new GoodsType();
            //    RecomendType.name = "热门推荐";
            //    DataSet RTCds = sm.GetHotRecommendCarousel();
            //    RecomendType.carousl =
            //        (List<CarouslFigure>)DataSetHelper.ConvertDataTableToList<CarouslFigure>(RTCds.Tables[0]);

            //    GoodsType spRecommend = new GoodsType();
            //    spRecommend.name = "专场推荐";
            //    DataRow[] spRecommendRows = ds.Tables[0].Select("recommend_type = 1");
            //    DataTable spRecommendTable = DataSetHelper.ConvertRowsToTable(spRecommendRows);
            //    spRecommend.nodes = GoodsType.InitFromDataTable(spRecommendTable);
            //    RecomendType.nodes.Add(spRecommend);

            //    GoodsType xdRecommend = new GoodsType();
            //    xdRecommend.name = "小兜推荐";
            //    DataRow[] xdRecommendRows = ds.Tables[0].Select("recommend_type = 2");
            //    DataTable xdRecommendTable = DataSetHelper.ConvertRowsToTable(xdRecommendRows);
            //    xdRecommend.nodes = GoodsType.InitFromDataTable(xdRecommendTable);
            //    RecomendType.nodes.Add(xdRecommend);
            //    type.nodes.Add(RecomendType);
            //}
            //catch
            //{
            //    type = new GoodsType();
            //}
            #endregion


            type.BuildTree(dt, type, ct, 0);

            return ResultView<GoodsType>.InitFromList(type.nodes);
        }

        [HttpGet]
        [Route("SWebApi/ShopManager/GoodsTypeList.json")]
        [CacheOutput(ServerTimeSpan = 7200, ClientTimeSpan = 7200)]
        public ResultView<List<GoodsType>> GetGoodsTypeList_json()
        {
            return GetGoodsTypeList();
        }
        /// <summary>
        /// 获取第三级分类列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ActionName("SimpleGoodsTypeList")]
        //[CacheOutput(ServerTimeSpan = 600, ClientTimeSpan = 600)]
        public ResultView<List<SimpleGoodsType>> GetSimpleGoodsTypeList()
        {
            var ds = sm.GetSimpleGoodsTypeList();
            var result = new ResultView<List<SimpleGoodsType>>();
            result.status = MessageCode.ERROR_CODE;
            result.message = MessageCode.TranslateMessageCode(MessageCode.ERROR_CODE);
            if (ds != null)
            {
                var list = SimpleGoodsType.InitList(ds.Tables[0]);
                result.status = MessageCode.SUCCESS;
                result.message = MessageCode.TranslateMessageCode(MessageCode.SUCCESS);
                result.data = list;
            }
            return result;
        }

        [HttpGet]
        [Route("SWebApi/ShopManager/SimpleGoodsTypeList.json")]
        [CacheOutput(ServerTimeSpan = 3600*24, ClientTimeSpan = 3600*24)]
        public ResultView<List<SimpleGoodsType>> GetSimpleGoodsTypeList_json()
        {
            return GetSimpleGoodsTypeList();
        }

        /// <summary>
        /// 获取商品列表(已更新)
        /// </summary>
        /// <param name="Page">页码</param>
        /// <param name="PageSize">页面大小</param>
        /// <returns>结果对象</returns>
        [HttpGet]
        [ActionName("GoodsList")]
        public ResultView<List<GoodsSummary>> GetGoodsList(int Page, int PageSize)
        {
            DataSet ds = sm.GetGoodsList(Page, PageSize);
            return ResultView<GoodsSummary>.InitFromDataSet(ds);
        }

        [HttpGet]
        [Route("SWebApi/ShopManager/GoodsList/{Page}.json")]
        [CacheOutput(ServerTimeSpan = 3600*24, ClientTimeSpan = 3600*24)]
        public ResultView<List<GoodsSummary>> GetGoodsList_json(int Page, int PageSize)
        {
            return GetGoodsList(Page, PageSize);
        }

        /// <summary>
        /// 根据商品类型获取商品列表(已更新)
        /// </summary>
        /// <param name="GoodsType">商品类型编码</param>
        /// <param name="Page">页码</param>
        /// <param name="PageSize">页面大小</param>
        /// <returns>结果对象</returns>
        [HttpGet]
        [ActionName("GoodsListByType")]
        public ResultView<List<GoodsSummary>> GetGoodsListByType(int GoodsType, int Page, int PageSize)
        {
            DataSet ds = sm.GetGoodsListByGoodsType(GoodsType, Page, PageSize);
            return ResultView<GoodsSummary>.InitFromDataSet(ds);
        }


        [HttpGet]
        [Route("SWebApi/ShopManager/GoodsListByType/{GoodsType}/{Page}.json")]
        [CacheOutput(ServerTimeSpan = 7200, ClientTimeSpan = 7200, ExcludeQueryStringFromCacheKey = true)]
        public ResultView<List<GoodsSummary>> GetGoodsListByType_json(int GoodsType, int Page, int PageSize)
        {
            return GetGoodsListByType(GoodsType, Page, PageSize);
        }

        /// <summary>
        /// 根据商品类型获取商品列表(排序扩展)
        /// </summary>
        /// <param name="GoodsType">商品类型编码</param>
        /// <param name="Page">页码</param>
        /// <param name="PageSize">页面大小</param>
        /// <param name="OrderField">排序字段 默认:order_weight;可用字段:return_money_discount(根据兑所需金币),sell_count(兑换量)</param>
        /// <param name="IsAsc">是否顺序 默认:false</param>
        /// <returns>结果对象</returns>
        [HttpGet]
        [ActionName("ExGoodsListByType")]
        public ResultView<List<GoodsSummary>> ExGetGoodsListByType(int GoodsType, int Page, int PageSize,string OrderField = "order_weight", bool IsAsc = false)
        {
            DataSet ds = sm.ExGetGoodsListByGoodsType(GoodsType, Page, PageSize,OrderField,IsAsc);
            return ResultView<GoodsSummary>.InitFromDataSet(ds);
        }

        /// <summary>
        /// 获取兑换专区商品列表
        /// </summary>
        /// <param name="Page">页码</param>
        /// <param name="PageSize">页面大小</param>
        /// <param name="OrderField">排序字段 默认:order_weight;可用字段:return_money_discount(根据兑所需金币),sell_count(兑换量)</param>
        /// <param name="IsAsc">是否顺序 默认:false</param>
        /// <returns>结果对象</returns>
        [HttpGet]
        [ActionName("ExChangeAreaGoodsList")]
        public ResultView<List<GoodsSummary>> GetExchangeAreaGoodsList(int Page, int PageSize, string OrderField = "order_weight", bool IsAsc = false)
        {
            DataSet ds = sm.GetExchangeAreaGoodsList(Page, PageSize, OrderField, IsAsc);
            return ResultView<GoodsSummary>.InitFromDataSet(ds);
        }


        /// <summary>
        /// 根据关键字获取商品列表(新发布)
        /// </summary>
        /// <param name="KeyWord">搜索关键字</param>
        /// <param name="Page">页码</param>
        /// <param name="PageSize">页面大小</param>
        /// <returns>结果对象</returns>
        [HttpGet]
        [ActionName("GoodsListByKeyWord")]
        public ResultView<List<SearchInfo>> GetGoodsListByKeyWord(string KeyWord, int Page, int PageSize)
        {
            //DataSet ds = sm.GetGoodsListByKeyWord(KeyWord, Page, PageSize);
            List<SearchInfo> info = SetCaChe(KeyWord,Page,PageSize);
            return ResultView<SearchInfo>.InitFromList(info);
        }

        static int m_clearTimeSecond = 600;
        static object lockObject = new object();
        static string SearchInfo_Key = "SearchInfo_Cache";
        //static List<SearchInfo> m_SearchInfolist = null;
        public static List<SearchInfo> m_SearchInfolist
        {
            get {
                return (List<SearchInfo>)System.Web.HttpContext.Current.Cache[SearchInfo_Key];
            }
            set
            {
                System.Web.HttpContext.Current.Cache.Insert(SearchInfo_Key, value,null, DateTime.Now.AddSeconds(m_clearTimeSecond), System.Web.Caching.Cache.NoSlidingExpiration);

               // Cache.Insert("SearchInfo_Key", value, null, DateTime.Now.AddDays(m_clearTimeSecond),Cache.NoSlidingExpiration);
            }
        }

        // <summary>
        /// 缓存商品列表
        /// </summary>
        public List<SearchInfo> SetCaChe(string key,int Page,int PageSize)
        {

            //RedisHelper redis = new RedisHelper();
            // var data = redis.ListGet<SearchInfo>("rrl_SearchList");
            //List<SearchInfo> list = null;// new List<SearchInfo>();
            if (m_SearchInfolist == null)
            {
                lock (lockObject)
                {
                    if (m_SearchInfolist == null)
                    {
                        m_SearchInfolist = sm.NewMethod();
                    }
                }
            }
              
            int pageOffest = (Page - 1) * PageSize;
            List<SearchInfo> pageList = new List<SearchInfo>();
            if (!string.IsNullOrWhiteSpace(key))
            {
                int hadSearch_Index = 0;
                for (int i = 0; i < m_SearchInfolist.Count; i++)
                {
                    var p = m_SearchInfolist[i];
                    if (p.name.Contains(key) || p.propaganda.Contains(key))
                    {
                        hadSearch_Index++;
                        if (hadSearch_Index > pageOffest)
                        {
                            pageList.Add(p);
                            if (pageList.Count >= PageSize)
                            {
                                break;
                            }
                        }
                    }
                }
            }
             
            return  pageList.ToList() ;
        }

         
        /// <summary>
        /// 获取热门推荐商品列表(已更新)
        /// </summary>
        /// <param name="Page">页码</param>
        /// <param name="PageSize">页面大小</param>
        /// <returns>结果对象</returns>
        [HttpGet]
        [ActionName("HotRecommend")]
        [CacheOutput(ServerTimeSpan = 7200, ClientTimeSpan =7200, ExcludeQueryStringFromCacheKey = true)]
        public ResultView<List<GoodsRecommend>> GetHotRecommendGoods(int Page, int PageSize)
        {
            DataSet ds = sm.GetHotRecommendGoods(Page, PageSize);
            return ResultView<GoodsRecommend>.InitFromDataSet(ds);
        }


        [HttpGet]
        [Route("SWebApi/ShopManager/HotRecommend/{Page}.json")]
        [CacheOutput(ServerTimeSpan = 3600*24, ClientTimeSpan = 3600*24, ExcludeQueryStringFromCacheKey = true)]
        public ResultView<List<GoodsRecommend>> GetHotRecommendGoods_json(int Page, int PageSize)
        {
            return GetHotRecommendGoods(Page, PageSize);
        }

        /// <summary>
        /// 获取主要推荐商品列表(已更新)
        /// </summary>
        /// <param name="Page">页码</param>
        /// <param name="PageSize">页面大小</param>
        /// <returns>结果对象</returns>
        [HttpGet]
        [ActionName("MainRecommend")]
        public ResultView<List<GoodsRecommend>> GetMainRecommendGoods(int Page, int PageSize)
        {
            DataSet ds = sm.GetMainRecommendGoods(Page, PageSize);
            return ResultView<GoodsRecommend>.InitFromDataSet(ds);
        }

        /// <summary>
        /// 获取商品详情
        /// </summary>
        /// <param name="id">商品标识</param>
        /// <param name="token">用户令牌</param>
        /// <returns>结果对象</returns>
        [HttpGet]
        [ActionName("GoodsDetail")]
        public DataResult GetGoodsDetail(int id, string token = null)
        {
            if (token == null)
            {
                token = "";
            }
            HelpManager.MarkPageView(string.Format("/#/goodsdetail/{0}", id));
            DataResult Resault;
            TokenObject Token = TokenObject.InitTokenObjFromString(token);
            if (!string.Equals(TokenObject.ShortTimeToken, Token.Prefix))
            {
                DataSet ds = sm.GetGoodsDetail(id);
                Resault = DataResult.InitFromDataSet(ds);
                Resault.additional_data = DataResult.SingleColumnDataSetToList(sm.GetGoodsPics(id));
                Resault.add_data = sm.GetGoodsAttrFormat(sm.GetGoodsAttr(id));
            }
            else
            {
                UserAuth User = new UserAuth();
                int res = User.Load(Token.id);
                if (res != MessageCode.SUCCESS)
                    return DataResult.InitFromMessageCode(res);
                DataSet ds = sm.GetGoodsDetail(id, User.id);
                Resault = DataResult.InitFromDataSet(ds);
                Resault.additional_data = DataResult.SingleColumnDataSetToList(sm.GetGoodsPics(id));
                Resault.add_data = sm.GetGoodsAttrFormat(sm.GetGoodsAttr(id));
            }
            return Resault;
        }


        [HttpGet]
        [Route("SWebApi/ShopManager/GoodsDetail/{id}.json")]
        [CacheOutput(ServerTimeSpan = 7200, ClientTimeSpan = 7200, ExcludeQueryStringFromCacheKey = true)]
        public DataResult GetGoodsDetail_json(int id)
        {
            return GetGoodsDetail(id, null);
        }
        /// <summary>
        /// 根据商品规格获取库存价格等
        /// </summary>
        /// <param name="id"></param>
        /// <param name="token"></param>
        /// <param name="attr"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("GoodsAttr")]
        public DataResult GetGoodsAttr(int id, List<GoodsAttrInView> attr, string token = null)
        {
            if (token == null)
            {
                token = "";
            }
            DataResult Resault = new DataResult();
            TokenObject Token = TokenObject.InitTokenObjFromString(token);
            if (!string.Equals(TokenObject.ShortTimeToken, Token.Prefix))
            {
                DataSet ds = sm.GetGoodsAttrFromPara(id, attr);
                Resault = DataResult.InitFromDataSet(ds);
                Resault.additional_data = DataResult.SingleColumnDataSetToList(sm.GetGoodsAttrFromParaPicIdList(ds));
            }
            return Resault;
        }

        /// <summary>
        /// 获取商品可分享评估信息
        /// </summary>
        /// <param name="id">商品id</param>
        /// <returns></returns>
        [HttpGet]
        [ActionName("GoodsShareInfo")]
        public DataResult GetGoodsShareInfo(int id)
        {
            bool hasMutiple;

            var result = DataResult.InitFromMessageCode(sm.GetGoodsShareInfo(id, out hasMutiple) ? MessageCode.SUCCESS : MessageCode.ERROR_UNKONWN);

            result.additional_data = new
            {
                mutiple = hasMutiple
            };

            return result;
        }


        [HttpGet]
        [Route("SWebApi/ShopManager/GoodsShareInfo/{id}.json")]
        [CacheOutput(ServerTimeSpan = 7200, ClientTimeSpan = 7200, ExcludeQueryStringFromCacheKey = true)]
        public DataResult GetGoodsShareInfo_json(int id)
        {
            return GetGoodsShareInfo(id);
        }
        //[HttpGet]
        //[ActionName("AddGoodsSpec")]
        //public DataResault AddGoodsSpec(string token,int goods_id,string spec_class,string spec,decimal price_influence,
        //    int inv_count,int is_default=0)
        //{
        //    int res;
        //    TokenObject Token = TokenObject.InitTokenObjFromString(token);
        //    if (!string.Equals(TokenObject.SHORT_TIME_TOKEN, Token.prefix))
        //    {
        //        res = MessageCode.ERROR_TOKEN_VALIDATE;
        //        return DataResault.InitFromMessageCode(res);
        //    }
        //    res = sm.AddGoodsSpec(goods_id, spec_class, spec, price_influence,
        //    inv_count, is_default);
        //    DataResault Resault = new DataResault();
        //    if(res==0)
        //    {
        //        Resault = DataResault.InitFromMessageCode(MessageCode.ERROR_EXECUTE_SQL);
        //        return Resault;
        //    }
        //    else
        //    {
        //        Resault = DataResault.InitFromMessageCode(MessageCode.SUCCESS);
        //        Resault.data = res;
        //        return Resault;
        //    }
        //}

        /// <summary>
        /// 获得商品规格型号数据
        /// </summary>
        /// <param name="goods_id">商品id</param>
        /// <returns></returns>
        [HttpGet]
        [ActionName("GetGoodsSpecDetail")]
        public DataResult GetGoodsSpecDetail(int goods_id)
        {
            List<SpecData> list_data = sm.GetGoodsSpecDetail(goods_id);
            if (list_data == null)
            {
                return DataResult.InitFromMessageCode(MessageCode.ERROR_EXECUTE_SQL);
            }
            if (list_data.Count == 0)
            {
                return DataResult.InitFromMessageCode(MessageCode.ERROR_NO_DATA);
            }
            DataResult Resault = new DataResult();
            Resault.data = list_data;
            Resault.status = MessageCode.SUCCESS;
            Resault.message = MessageCode.TranslateMessageCode(MessageCode.SUCCESS);
            return Resault;
        }

        [HttpGet]
        [ActionName("GetGoodsTypeList")]
        public DataResult GetGoodsTypeList(int PageSize, int Page)
        {

           DataSet ds = sm.GetGoodsByTypeList(PageSize, Page);
           DataSet dd = sm.GetGoodsByTypeListN(PageSize, Page);
           DataResult dt = new DataResult();
            dt.data = new { NewData =  (ds == null ? null : ds.Tables[0])
, PopData = (dd == null ? null : dd.Tables[0])
            };
            //dt.add_data = DataResult.InitFromDataSet(ds).data;//人气推荐
            //dt.additional_data = DataResult.InitFromDataSet(dd).data;
            //新品首发
            return dt;
        }

        [HttpGet]
        [Route("SWebApi/ShopManager/GetGoodsTypeList/{Page}.json")]
        [CacheOutput(ServerTimeSpan = 7200, ClientTimeSpan = 7200, ExcludeQueryStringFromCacheKey = true)]
        public DataResult GetGoodsTypeList_json(  int Page,int PageSize=6)
        {
            return GetGoodsTypeList(PageSize, Page);
        }

        /// <summary>
        /// 秒杀正在进行时
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public DataResult GetSecKillActivity()
        {
            var db = new SqlDataBase();
            var model = db.Select<dynamic>(@"
select * from SHOP_SEC_KILL_ACTIVITY WHERE deletemark IS NULL 
AND is_seckill_activity =1 order by order_weight desc");
            return new DataResult { status = 0, message = "成功", data = model };
        }


        /// <summary>
        /// 特惠商品区间值
        /// </summary>
        /// <param name="SectionStr"></param>
        /// <returns></returns>
        [HttpGet]
        [ActionName("ShopOneSectionInfo")]
        public DataResult GetOnepreferentialList(string SectionStr)
        {
           
            //SectionStr = "1-1000";
            if (!string.IsNullOrEmpty(SectionStr))
                {
                    var ser = JsonConvert.DeserializeObject<dynamic>(SectionStr);
                    var lsit = ser.list.Value;
                    var listSection = lsit.Split(',');
                var db = new SqlDataBase();
                List<dynamic> listdata = new List<dynamic>();
                for (int i = 0; i <listSection.Length; i++)
                    {
                    if (listSection[i].Contains("-"))
                    {
                        var arr = listSection[i].Split('-');
                        var model = db.Select<dynamic>($@"
                        select * from SHOP_PREFERENTTIAL_LIST WHERE deletemark IS NULL and 	is_preferential=1
                        and beans_price between '{arr[0]}' and '{arr[1]}'
                        order by order_weight desc");
                        listdata.Add(new { model = model, Section = listSection[i] });
                    }
                    else
                    {
                        if (!string.IsNullOrWhiteSpace(listSection[i]))
                        {
                            var model = db.Select<dynamic>($@"
                        select * from SHOP_PREFERENTTIAL_LIST WHERE deletemark IS NULL and 	is_preferential=1
                        and beans_price>'{listSection[i]}'
                        order by order_weight desc");
                         listdata.Add(new { model=model,Section= listSection[i] });
                        }
                      
                    }
                }

                  return new DataResult { status = 0, message = "成功", data = listdata };
            }
                 
            
            else
            {
                return new DataResult { status = 99, message = "区间值为null", data = null };
            }
        }

        /// <summary>
        /// 特惠商品区间值
        /// </summary>
        /// <param name="SectionStr"></param>
        /// <returns></returns>
        [HttpGet]
        [ActionName("ShopSectionInfo")]
        public DataResult GetPreferentialList(string SectionStr)
        {
            
            //SectionStr = "1-1000";
            if (!string.IsNullOrEmpty(SectionStr))
            {
                if (SectionStr.Contains('-'))
                {
                    var arr = SectionStr.Split('-');
                    var db = new SqlDataBase();
                    var model = db.Select<dynamic>($@"
                    select * from SHOP_PREFERENTTIAL_LIST WHERE deletemark IS NULL and 	is_preferential=1
                    and beans_price between '{arr[0]}' and '{arr[1]}'
                    order by order_weight desc");
                    return new DataResult { status = 0, message = "成功", data = model };
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(SectionStr))
                    {
                        var db = new SqlDataBase();
                        var model = db.Select<dynamic>($@"
                        select * from SHOP_PREFERENTTIAL_LIST WHERE deletemark IS NULL and 	is_preferential=1
                        and beans_price > '{SectionStr}'
                        order by order_weight desc");
                        return new DataResult { status = 0, message = "成功", data = model};
                    }
                    return new DataResult { status = 0, message = "" };
                }
            }
            else
            {
                return new DataResult { status = 99, message = "区间值为null", data = null };
            }
        }

        /// <summary>
        /// 秒杀下期预告
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public DataResult GetNextSecKillActvity(string token)
        {
            if (!string.IsNullOrWhiteSpace(token))
            {
                TokenObject Token = TokenObject.InitTokenObjFromString(token);
                 
                    var db = new SqlDataBase();
                    var online_time = DateTime.Now.Date.AddDays(2).AddSeconds(-1);
                 
                    var model = db.ExecuteStoredProcedure<dynamic>("sp_SHOP_SEC_KILL_ACTIVITY", new {uid=Token.id,online_time = online_time });
                    return new DataResult { status = 0, message = "成功", data = model };
                

            }
            else
            {
                return new DataResult { status = 0, message = "token为空" };

            }

        }

        /// <summary>
        /// 秒杀提醒
        /// </summary>
        /// <returns>token</returns>
        /// <returns>商品id</returns>
        [HttpGet]
        public DataResult Set_Skill_Remind(string token=null,int goods_id=0)
        {
            if (!string.IsNullOrWhiteSpace(token))
            {
                TokenObject Token = TokenObject.InitTokenObjFromString(token);
                if (!string.Equals(TokenObject.ShortTimeToken, Token.Prefix))
                {
                    return new DataResult { status = MessageCode.ERROR_TOKEN_VALIDATE, message = "Token验证失败" };
                }

                else
                {
                    if (goods_id > 0)
                    {
                        var db = new SqlDataBase();
                        var model = db.Select<dynamic>($@"select online_time,name from rrl_goods where id='{goods_id}'");
                        if (model != null)
                        {
                          var remindList=db.Select<dynamic>($@"select * from rrl_skill_remind where goods_id='{goods_id}'and uid='{Token.id}'");
                            if (remindList.Count == 0)
                            {
                                string id = Guid.NewGuid().ToString();
                                int effct = db.Execute(@"insert into rrl_skill_remind(id,goods_id,uid,addtime,online_time)VALUES(@id,@goods_id,@uid,@addtime,@online_time)",
                                  new
                                  {
                                      id = id,
                                      goods_id = goods_id,
                                      uid = Token.id,
                                      addtime = DateTime.Now,
                                      online_time = model[0].online_time ?? "1999-12-12 12:12:12"
                                  });

                                #region 插入消息推送
                                string goods_name = model[0].name;
                                MessageManager messageManager = new MessageManager();
                                messageManager.InsertPushMessage(Token.id, $@"您关注的商品{goods_name}即将开抢!", "您关注的商品还有5分钟就开抢了.请尽快登陆APP或者公众号开抢![首页]->[1元秒杀]->[马上抢]", id, model[0].online_time??DateTime.Now );
                                #endregion

                                if (effct > 0)
                                {
                                    var data=db.Select<dynamic>($@"SELECT count(*)as number, goods_id FROM [dbo].[rrl_skill_remind] group by goods_id having goods_id='{goods_id}'");
                                    return new DataResult { status = 0, message = "加入成功",data= data };
                                }
                            }
                            else
                            {
                                var remindL = db.Select<dynamic>($@"SELECT count(*)as number, goods_id FROM [dbo].[rrl_skill_remind] group by goods_id having goods_id='{goods_id}'");
                                return new DataResult { status = 0, message = "已提醒", data= remindL };
                            }
                        }
                        else
                        {
                            return new DataResult { status = 99, message = "没查到相关商品信息" };
                        }
                    }
                    else
                    {
                        return new DataResult { status = 99, message = "没有商品id" };
                    }

                    return null;
                }
               
            }
            else
            {
                return new DataResult { status = 99, message = "token为空" };
            }
        }
    }
}