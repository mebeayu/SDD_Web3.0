using Common.Utils;
using RRL.Config;
using RRL.Core.Models;
using RRL.Core.Utility;
using RRL.DB;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;

namespace RRL.Core.Manager
{
    public partial class ShopManager
    {
        /// <summary>
        /// 获取商品分类
        /// </summary>
        /// <returns>数据集</returns>
        public DataSet GetGoodsTypeList()
        {
            RRLDB db = new RRLDB();
            DataSet ds = db.ExeQuery(SqlQueryUtility.GOODS_TYPE_LIST_QUERY_STR);
            db.Close();
            return ds;
        }

        /// <summary>
        /// 获取商品基础分类
        /// </summary>
        /// <returns>数据集</returns>
        public DataSet GetSimpleGoodsTypeList()
        {
            RRLDB db = new RRLDB();
            try
            {
                //var ip= IPUtils.getIPAddress();
                //var isAtusa=isAtUSA(ip);
                DataSet ds=null;
                //if (isAtusa)
                //{
                //    //是美国地区,直接去掉游戏,帮助苹果审核
                //    ds = db.ExeQuery($@"SELECT * FROM rrl_shop_cate WHERE ParentID = 0 and id<>309  ORDER BY order_id ASC");//
                //}
                //else
                //{

                    ds = db.ExeQuery($@"SELECT * FROM rrl_shop_cate WHERE ParentID = 0 ORDER BY order_id ASC");
                //}
                return ds;
            }
            finally
            {
                db.Close();
            }
        }

        /// <summary>
        /// 是否是美国的ip
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public bool isAtUSA(string ip)
        {
            //return true;
            string strin=   RRLConfig.USA_CAN_NOT_SEEN_GAME_ENTER;
            if("true".Equals(strin, StringComparison.OrdinalIgnoreCase))
            {
                var now=DateTime.Now;
                if(!(now.TimeOfDay<new  TimeSpan(20,0,0)  && now.TimeOfDay > new TimeSpan(8, 0, 0)))
                {
                    return true;
                }
                /*var ip_str = ip;
                if (!string.IsNullOrWhiteSpace(ip_str))
                {
                    if (IPUtils.IsIPAddress(ip_str))
                    {
                        long ip_long = IPUtils.Ip2Long(ip_str);
                        SqlDataBase db = new SqlDataBase();
                        List<dynamic> ss= db.Select<dynamic>($@"select *  from ipdata where begin_ip_int>=@ip_long and end_ip_int<=@ip_long",new { ip_long= ip_long });
                        if(ss!=null&&ss.Count>0)
                        {
                            foreach (var item in ss)
                            {
                                if ("US".Equals(item.country_id))
                                {
                                    return true;
                                }
                            }
                        }
                    }
                }*/

            }
            return false;
        }

        /// <summary>
        /// 获取所有分类轮播
        /// </summary>
        /// <returns></returns>
        public DataSet GetShopCateCarousl()
        {
            RRLDB db = new RRLDB();
            DataSet ds = db.ExeQuery(SqlQueryUtility.SHOP_CATE_CAROUSEL_FIGURE_QUERY_STR);
            db.Close();
            return ds;
        }

        /// <summary>
        /// 获取热门推荐轮播
        /// </summary>
        /// <returns>数据集</returns>
        public DataSet GetHotRecommendCarousel()
        {
            RRLDB db = new RRLDB();
            DataSet ds = db.ExeQuery(SqlQueryUtility.CAROUSEL_FIGURE_QUERY_STR,
                new SqlParameter("@CarouselType", SqlDbType.Int, 4) { Value = 5 });
            db.Close();
            return ds;
        }

        /// <summary>
        /// 获取商品列表
        /// </summary>
        /// <param name="Page">页码</param>
        /// <param name="PageSize">页面大小</param>
        /// <returns>数据集</returns>
        public DataSet GetGoodsList(int Page, int PageSize)
        {
            RRLDB db = new RRLDB();
            DataSet ds = db.ExeQuery(Preprocessor.QueryPaginateString(SqlQueryUtility.GOODS_LIST_QUERY_STR, Page, PageSize));
            db.Close();
            return ds;
        }

        /// <summary>
        /// 根据商品分类获取商品列表
        /// </summary>
        /// <param name="GoodsType">分类编码</param>
        /// <param name="Page">页码</param>
        /// <param name="PageSize">页面大小</param>
        /// <returns>数据集</returns>
        public DataSet GetGoodsListByGoodsType(int GoodsType, int Page, int PageSize)
        {

            RRLDB db = new RRLDB();
            int pageId = Math.Max(Page, 1);
            int offSet = (pageId - 1) * PageSize;
            string PreQuery = string.Format(SqlQueryUtility.GOODS_LIST_BY_GOODS_TYPE_QUERY_STR, PageSize, offSet, TradeManager.DEFAULT_CASH_PAY_RATE, TradeManager.DEFAULT_BEANS_PAY_RATE);
            //    Preprocessor.QueryPaginateString(SqlQueryUtility.GOODS_LIST_BY_GOODS_TYPE_QUERY_STR, Page, PageSize,TradeManager.DEFAULT_CASH_PAY_RATE, TradeManager.DEFAULT_BEANS_PAY_RATE);
            var ds = db.ExeQuery($@"Select ParentID From rrl_shop_cate Where id = {GoodsType}");
            var pid = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
            if (pid == 0)
            {
                PreQuery= string.Format(SqlQueryUtility.SIMPLE_GOODS_LIST_BY_GOODS_TYPE_QUERY_STR, PageSize, offSet, TradeManager.DEFAULT_CASH_PAY_RATE, TradeManager.DEFAULT_BEANS_PAY_RATE);
                //PreQuery = Preprocessor.QueryPaginateString(SqlQueryUtility.SIMPLE_GOODS_LIST_BY_GOODS_TYPE_QUERY_STR, Page, PageSize);
            }
            ds = db.ExeQuery(PreQuery, new SqlParameter("@GoodsType", SqlDbType.Int, 4) { Value = GoodsType });
            db.Close();
            return ds;
        }

        /// <summary>
        /// 根据商品分类获取商品列表(排序扩展)
        /// </summary>
        /// <param name="GoodsType">分类编码</param>
        /// <param name="Page">页码</param>
        /// <param name="PageSize">页面大小</param>
        /// <param name="OrderField">排序字段</param>
        /// <param name="IsAsc">是否顺序</param>
        /// <returns>数据集</returns>
        public DataSet ExGetGoodsListByGoodsType(int GoodsType, int Page, int PageSize, string OrderField = "order_weight", bool IsAsc = false)
        {
            string OrderType = "ASC";
            if (!IsAsc)
            {
                OrderType = "DESC";
            }

            string PreQuery = Preprocessor.ExQueryPaginateString(SqlQueryUtility.EX_GOODS_LIST_BY_GOODS_TYPE_QUERY_STR, Page, PageSize, OrderField, OrderType);
            RRLDB db = new RRLDB();
            DataSet ds = db.ExeQuery(PreQuery, new SqlParameter("@GoodsType", SqlDbType.Int, 4) { Value = GoodsType });
            db.Close();
            return ds;
        }

        /// <summary>
        /// 获取金币兑换专区商品列表
        /// </summary>
        /// <param name="Page">页码</param>
        /// <param name="PageSize">页面大小</param>
        /// <param name="OrderField">排序字段</param>
        /// <param name="IsAsc">是否顺序</param>
        /// <returns>数据集</returns>
        public DataSet GetExchangeAreaGoodsList(int Page, int PageSize, string OrderField = "order_weight", bool IsAsc = false)
        {
            string OrderType = "ASC";
            if (!IsAsc)
            {
                OrderType = "DESC";
            }

            string PreQuery = Preprocessor.ExQueryPaginateString(SqlQueryUtility.EXCHANGE_AREA_GOODS_LIST_QUERY_STR, Page, PageSize, OrderField, OrderType);
            RRLDB db = new RRLDB();
            DataSet ds = db.ExeQuery(PreQuery);
            db.Close();
            return ds;
        }

        /// <summary>
        /// 根据搜索关键字获取商品列表
        /// </summary>
        /// <param name="KeyWord">搜索关键字</param>
        /// <param name="Page">页码</param>
        /// <param name="PageSize">页面大小</param>
        /// <returns>数据集</returns>
        public DataSet GetGoodsListByKeyWord(string KeyWord, int Page, int PageSize)
        {
            int PageID = Math.Max(Page, 1);
            int OffSet = (PageID - 1) * PageSize;
            //string abc = string.Format(SqlQueryUtility.GOODS_LIST_BY_KEY_WORD_QUERY_STR, PageSize, OffSet, KeyWord);
            RRLDB db = new RRLDB();
            DataSet ds = db.ExeQuery(string.Format(SqlQueryUtility.GOODS_LIST_BY_KEY_WORD_QUERY_STR, PageSize, OffSet, KeyWord));
            db.Close();
            return ds;
        }

        /// <summary>
        /// 获取热门推荐商品列表
        /// </summary>
        /// <param name="Page">页码</param>
        /// <param name="PageSize">页面大小</param>
        /// <returns>数据集</returns>
        public DataSet GetHotRecommendGoods(int Page, int PageSize)
        {
            string PreQuery = Preprocessor.QueryPaginateString(SqlQueryUtility.RECOMMEND_GOODS_QUERY_STR, Page, PageSize);
            RRLDB db = new RRLDB();
            DataSet ds = db.ExeQuery(PreQuery, new SqlParameter("@RecommendType", SqlDbType.Int, 4) { Value = 2 });
            db.Close();
            return ds;
        }

        /// <summary>
        /// 获取主要推荐商品列表
        /// </summary>
        /// <param name="Page">页码</param>
        /// <param name="PageSize">页面大小</param>
        /// <returns>数据集</returns>
        public DataSet GetMainRecommendGoods(int Page, int PageSize)
        {
            string PreQuery = Preprocessor.QueryPaginateString(SqlQueryUtility.RECOMMEND_GOODS_QUERY_STR, Page, PageSize);
            RRLDB db = new RRLDB();
            DataSet ds = db.ExeQuery(PreQuery, new SqlParameter("@RecommendType", SqlDbType.Int, 4) { Value = 1 });
            db.Close();
            return ds;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="GoodType">商品类型</param>
        /// <param name="is_newp">是否是新品首发</param>
        /// <param name="Page">第几页</param>
        /// <param name="PageSize">当前页有几条</param>
        /// <returns></returns>
        public DataSet GetGoodsByTypeList(int PageSize, int Page)
        {
            string PreQuery = Preprocessor.QueryPaginateString(SqlQueryUtility.SHOP_CATE_GOODTYPE_STR, Page, PageSize);
            RRLDB db = new RRLDB();
            DataSet ds = db.ExeQuery(PreQuery);
            db.Close();
            return ds;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="GoodType">商品类型</param>
        /// <param name="is_newp">是否是人气推荐</param>
        /// <param name="Page">第几页</param>
        /// <param name="PageSize">当前页有几条</param>
        /// <returns></returns>
        public DataSet GetGoodsByTypeListN(int PageSize, int Page)
        {
            string PreQuery = Preprocessor.QueryPaginateString(SqlQueryUtility.SHOP_CATE_GOODTYPE_STRPop, Page, PageSize);
            RRLDB db = new RRLDB();
            DataSet ds = db.ExeQuery(PreQuery);
            db.Close();
            return ds;
        }

        /// <summary>
        /// 获取商品详情
        /// </summary>
        /// <param name="id">商品id</param>
        /// <returns>数据集</returns>
        public DataSet GetGoodsDetail(int id)
        {
            //GOODS_DETAIL_VIEW
            RRLDB db = new RRLDB();
            DataSet ds = db.ExeQuery(@"SELECT *, CAST(0 AS BIT) AS shop_fav, CAST(0 AS BIT) AS goods_fav,0 as plate_to_return_money FROM GOODS_DETAIL_VIEW WHERE id=@id",
                new SqlParameter("@id", SqlDbType.Int, 4) { Value = id });
            db.Close();
            return ds;
        }

        /// <summary>
        /// 获取商品详情
        /// </summary>
        /// <param name="gid">商品id</param>
        /// <param name="uid">用户id</param>
        /// <returns></returns>
        public DataSet GetGoodsDetail(int gid, int uid)
        {
            //GOODS_DETAIL_VIEW
            RRLDB db = new RRLDB();
            DataSet ds = db.ExeQuery(@"SELECT *,dbo.fn_IS_USER_ADD_SHOP_FAVOURITE_BY_GOODS(@uid,@gid)AS shopfav,dbo.fn_IS_USER_ADD_GOODS_FAVOURITE(@uid,@gid)AS goodsfav,(SELECT (plate_to_return_money+ex_plate_to_return_money) FROM rrl_user WHERE rrl_user.id = @uid) AS plate_to_return_money FROM GOODS_DETAIL_VIEW WHERE id = @gid",
                new SqlParameter("@gid", SqlDbType.Int, 4) { Value = gid },
                new SqlParameter("@uid", SqlDbType.Int, 4) { Value = uid });
            db.Close();
            return ds;
        }

        /// <summary>
        /// 评估商品可分享性
        /// </summary>
        /// <param name="id">商品标识</param>
        /// <param name="hasMutiple">是否有多个配置</param>
        /// <returns></returns>
        public bool GetGoodsShareInfo(int id, out bool hasMutiple)
        {
            var baseDir = AppDomain.CurrentDomain.BaseDirectory;
            var textFilePath = $@"Assets\GoodsShare\Text\{id}.txt";
            var textFileName = Path.Combine(baseDir, textFilePath);
            var singleImgFilePath = $@"Assets\GoodsShare\Img\Single\{id}.jpg";
            var singleImgFileName = Path.Combine(baseDir, singleImgFilePath);
            var mutipleImgFilePath = $@"Assets\GoodsShare\Img\Mutiple\{id}";
            var mutipleImgFileName = Path.Combine(baseDir, mutipleImgFilePath);
            hasMutiple = Directory.Exists(mutipleImgFileName);

            return File.Exists(textFileName) && File.Exists(singleImgFileName);
        }

        /// <summary>
        /// 获取商品轮播图
        /// </summary>
        /// <param name="id">商品id</param>
        /// <returns>数据集</returns>
        public DataSet GetGoodsPics(int id)
        {
            //rrl_goods_pic
            RRLDB db = new RRLDB();
            DataSet ds = db.ExeQuery(@"SELECT pic_id FROM rrl_goods_pic WHERE goods_id = @goods_id",
                new SqlParameter("@goods_id", SqlDbType.Int, 4) { Value = id });
            db.Close();
            return ds;
        }

        #region 2017.12.25 luo
        /// <summary>
        /// 根据商品规格获取价格库存等
        /// </summary>
        /// <param name="id"></param>
        /// <param name="attr"></param>
        /// <returns></returns>
        public DataSet GetGoodsAttrFromPara(int id, List<GoodsAttrInView> attr)
        {
            var valueStr = string.Join("AND",attr.Select(val => $@" {"[attr_values]"} LIKE '%{val.value}%' ").ToList());
            var sql = $@"SELECT * FROM [rrl_goods_attr_value] WHERE [goods_id] = {id} and {valueStr}";
            var db = new RRLDB();
            var ds = db.ExeQuery(sql);
            db.Close();
            return ds;
        }

        public DataSet GetGoodsAttrFromParaPicIdList(DataSet ds)
        {
            if (ds.Tables[0].Rows.Count != 0)
            {
                var data = ds.Tables[0];
                var pic_id = Convert.ToDecimal(data.Rows[0]["pic_id"]);
                // 未完成
                return null;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 获取商品规格
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DataSet GetGoodsAttr(int id)
        {
            RRLDB db = new RRLDB();
            var sql = "select * from rrl_goods_attr_value where goods_id = @goods_id";
            DataSet ds = db.ExeQuery(sql,
                new SqlParameter("@goods_id", SqlDbType.Int, 4) { Value = id });
            db.Close();
            return ds;
        }

        /// <summary>
        /// 商品规格返回值Format
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public List<GoodsAttr> GetGoodsAttrFormat(DataSet ds)
        {
            if (ds.Tables[0].Rows.Count != 0)
            {
                var data = ds.Tables[0];
                var count = data.Rows.Count;
                List<GoodsAttr> list = new List<GoodsAttr>();
                var temp = Convert.ToString(data.Rows[0]["attr_ids"]);
                string[] attr_ids_str = temp.Split(',');
                List<string> attr_value = new List<string>();
                for (int i = 0; i < attr_ids_str.Length; i++)
                {
                    GoodsAttr info = new GoodsAttr();
                    var nameTable = GetGoodsAttrValue(Convert.ToInt32(attr_ids_str[i]));
                    var name = Convert.ToString(nameTable.Tables[0].Rows[0]["name"]);
                    var key = Convert.ToString(nameTable.Tables[0].Rows[0]["key"]);
                    var order = Convert.ToInt32(nameTable.Tables[0].Rows[0]["order"]);
                    info.name = name;
                    info.key = key;
                    info.order = order;
                    List<string> attr_value_item = new List<string>();
                    for (int j = 0; j < count; j++)
                    {
                        var val = Convert.ToString(data.Rows[j]["attr_values"]);
                        string[] splitTemp = val.Split(',');
                        attr_value_item.Add(splitTemp[i]);
                        info.value = DelArrayRepeatItem(attr_value_item);
                    }
                    list.Add(info);
                }
                return list;
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 删除数组重复的元素
        /// </summary>
        /// <param name="arr"></param>
        /// <returns></returns>
        public static List<string> DelArrayRepeatItem(List<string> arr)
        {
            List<string> listString = new List<string>();
            foreach (string eachString in arr)
            {
                if (!listString.Contains(eachString))
                    listString.Add(eachString);
            }
            return listString;
        }

        /// <summary>
        /// 获取商品规格值
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DataSet GetGoodsAttrValue(int id)
        {
            RRLDB db = new RRLDB();
            var sql = "select * from rrl_goods_attr where id = @id";
            DataSet ds = db.ExeQuery(sql,
                new SqlParameter("@id", SqlDbType.Int, 4) { Value = id });
            db.Close();
            return ds;
        }
        #endregion

        ///////////////////这是商品规格处理的分割线(⊙﹏⊙)b///////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 添加商品规格型号
        /// </summary>
        /// <param name="goods_id">商品id</param>
        /// <param name="spec_class">规格类别维度</param>
        /// <param name="spec">规格值</param>
        /// <param name="price_influence">价格影响</param>
        /// <param name="inv_count">库存量</param>
        /// <param name="is_default"></param>
        /// <returns>成功返回新增加规格的大于0的id，失败返回0</returns>
        public int AddGoodsSpec(int goods_id, string spec_class, string spec, decimal price_influence,
            int inv_count, int is_default = 0)
        {
            RRLDB db = new RRLDB();
            DataSet ds = db.ExeQuery(@"insert into rrl_goods_spec
                                (goods_id,spec_class,spec,price_influence,inv_count,is_default)
                                output insert.id
                                values
                                (@goods_id,@spec_class,@spec,@price_influence,@inv_count,@is_default)",
                                new SqlParameter("goods_id", goods_id),
                                new SqlParameter("spec_class", spec_class),
                                new SqlParameter("spec", spec),
                                new SqlParameter("price_influence", price_influence),
                                new SqlParameter("inv_count", inv_count),
                                new SqlParameter("is_default", is_default));
            db.Close();
            if (ds == null)
            {
                return 0;
            }
            string id = ds.Tables[0].Rows[0][0].ToString();
            return int.Parse(id);
        }

        public int SetGoodsSpec(int id, string spec_class, string spec, decimal price_influence,
            int inv_count, int is_default = 0)
        {
            RRLDB db = new RRLDB();
            int res = db.ExeCMD(@"update rrl_goods_spec set
                                spec_class=@spec_class,
                                spec=@spec,
                                price_influence=@price_influence,
                                inv_count=@price_influence,
                                is_default=@price_influence where id=@id",
                                new SqlParameter("spec_class", spec_class),
                                new SqlParameter("spec", spec),
                                new SqlParameter("price_influence", price_influence),
                                new SqlParameter("inv_count", inv_count),
                                new SqlParameter("is_default", is_default),
                                new SqlParameter("id", id));
            db.Close();
            return res;
        }

        public int DeleteGoodsSpec(int id)
        {
            RRLDB db = new RRLDB();
            int res = db.ExeCMD("delete from rrl_goods_spec where id=@id", new SqlParameter("id", id));
            db.Close();
            return res;
        }

        public DataSet GetGoodsSpecClass(int goods_id)
        {
            RRLDB db = new RRLDB();
            DataSet ds = db.ExeQuery("select distinct spec_class from rrl_goods_spec where goods_id=@goods_id",
                new SqlParameter("goods_id", goods_id));
            db.Close();
            return ds;
        }

        public List<SpecData> GetGoodsSpecDetail(int goods_id)
        {
            List<SpecData> list_data = new List<SpecData>();

            RRLDB db = new RRLDB();
            DataSet ds = db.ExeQuery("select distinct spec_class from rrl_goods_spec where goods_id=@goods_id",
                new SqlParameter("goods_id", goods_id));
            if (ds == null) return null;
            int n = ds.Tables[0].Rows.Count;
            for (int i = 0; i < n; i++)
            {
                DataSet ds1 = db.ExeQuery("select * from rrl_goods_spec where goods_id=@goods_id and spec_class=@spec_class",
                new SqlParameter("goods_id", goods_id),
                new SqlParameter("spec_class", ds.Tables[0].Rows[i][0].ToString()));
                SpecData data = new SpecData();
                data.spec_class = ds.Tables[0].Rows[i][0].ToString();
                List<SpecRow> body = new List<SpecRow>();
                data.body = body;
                int len = ds.Tables[0].Rows.Count;
                for (int j = 0; j < len; j++)
                {
                    SpecRow row = new SpecRow();
                    row.id = int.Parse(ds1.Tables[0].Rows[j]["id"].ToString());
                    row.goods_id = int.Parse(ds1.Tables[0].Rows[j]["goods_id"].ToString());
                    row.spec_class = ds1.Tables[0].Rows[j]["spec_class"].ToString();
                    row.spec = ds1.Tables[0].Rows[j]["spec"].ToString();
                    row.price_influence = decimal.Parse(ds1.Tables[0].Rows[j]["price_influence"].ToString());
                    row.inv_count = int.Parse(ds1.Tables[0].Rows[j]["inv_count"].ToString());
                    row.sell_count = int.Parse(ds1.Tables[0].Rows[j]["sell_count"].ToString());
                    row.is_default = int.Parse(ds1.Tables[0].Rows[j]["is_default"].ToString());
                    body.Add(row);
                }
                list_data.Add(data);
            }

            db.Close();
            return list_data;
        }



        #region 公告相关接口

        /// <summary>
        /// 获取所有公告
        /// </summary>
        /// <returns></returns>
        public DataSet GetNoticeList()
        {
            RRLDB db = new RRLDB();
            DataSet ds = db.ExeQuery(@"select Id,title from sys_msg_notice");

            db.Close();
            return ds;
        }

        /// <summary>
        /// 根据Id获取公告信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DataSet GetNoticeDetail(string id)
        {
            //GOODS_DETAIL_VIEW
            RRLDB db = new RRLDB();
            DataSet ds = db.ExeQuery(@"SELECT * from  sys_msg_notice WHERE id=@id",
                new SqlParameter("@id", SqlDbType.NVarChar, 50) { Value = id });
            db.Close();
            return ds;
        }
        #endregion


        public List<SearchInfo> NewMethod()
        {
            var list = new SqlDataBase().Select<SearchInfo>(
                    $@" 
             select 
               g.id ,
		           dbo.rrl_shop.shop_type,
			         g.price,
			         g.local_price,
			         g.name,
			         g.goods_type,
			         g.propaganda,
							isnull(c3.name,'')AS category_name1,
							isnull(c2.name,'')AS category_name2,
							isnull(c.name,'')AS category_name3,
                      (
		                SELECT
			                TOP (1) rrl_goods_pic.pic_id
		                FROM
			                rrl_goods_pic
		                WHERE
			                rrl_goods_pic.goods_id = g.id
	                ) AS pic_id,
                    dbo.rrl_fee_config.return_money_rate,
                    g.return_money_discount AS discount			 
            from rrl_goods as g
            JOIN dbo.rrl_fee_config ON g.fee_id = dbo.rrl_fee_config.id
		        JOIN dbo.rrl_shop ON dbo.rrl_shop.id = g.sid
						LEFT JOIN rrl_shop_cate c
						ON g.goods_type = c.id
						LEFT JOIN rrl_shop_cate c2
						ON c.ParentID = c2.id
						LEFT JOIN rrl_shop_cate c3
						ON c2.ParentID = c3.id
            where g.deletemark IS NULL and g.is_delete=0  
						  order by g.order_weight desc,g.addtime desc");
           
            return list;

        }

    }




    public class SpecData
    {
        public string spec_class { get; set; }
        public List<SpecRow> body { get; set; }
    }

    /// <summary>
    /// 商品规格提交数据格式
    /// </summary>
    public class GoodsAttrInView
    {
        /// <summary>
        /// 标识
        /// </summary>
        public string key { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string value { get; set; }
    }

    /// <summary>
    /// 商品规格
    /// </summary>
    public class GoodsAttr
    {
        /// <summary>
        /// 规格
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// key
        /// </summary>
        public string key { get; set; }

        /// <summary>
        /// order
        /// </summary>
        public int order { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        public List<string> value { get; set; }
    }

    public class SpecRow
    {//goods_id,spec_class,spec,price_influence,inv_count,is_default
        public int id { get; set; }
        public int goods_id { get; set; }
        public string spec_class { get; set; }
        public string spec { get; set; }
        public decimal price_influence { get; set; }
        public int inv_count { get; set; }
        public int sell_count { get; set; }
        public int is_default { get; set; }
    }
}