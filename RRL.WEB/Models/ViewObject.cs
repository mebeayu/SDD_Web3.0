using RRL.Core.Models;
using RRL.WEB.Ulity;
using System;
using System.Collections.Generic;
using System.Data;

namespace RRL.WEB.Models.ViewModel
{
    #region 商店概览对象

    /// <summary>
    /// 商店概览（用于商户列表渲染）
    /// </summary>
    public partial class ShopSummary
    {
        /// <summary>
        /// 商店标识
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// 商店名称
        /// </summary>
        public string shop_name { get; set; }

        /// <summary>
        /// 商店区域编码
        /// </summary>
        public string area_code { get; set; }

        /// <summary>
        /// 商店坐标经度
        /// </summary>
        public string lng { get; set; }

        /// <summary>
        /// 商店坐标纬度
        /// </summary>
        public string lat { get; set; }

        /// <summary>
        /// 商店封面图
        /// </summary>
        public int pic_id { get; set; }
    }

    #endregion 商店概览对象

    #region 商品类别对象

    /// <summary>
    /// 基础商品分类对象
    /// </summary>
    public partial class SimpleGoodsType
    {
        /// <summary>
        /// 类别标识
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// 类别名称
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 类别图标
        /// </summary>
        public int icon { get; set; }


        /// <summary>
        /// 重定向类型0=不做url 跳转,1=跳转url ,按redirect_url 值做跳转
        /// </summary>
        public string redirect_type { get; set; }

        /// <summary>
        /// 跳转的url
        /// </summary>
        public string redirect_url { get; set; }

        /// <summary>
        /// 生成基础商品分类列表
        /// </summary>
        /// <param name="simpleTable"></param>
        /// <returns></returns>
        public static List<SimpleGoodsType> InitList(DataTable simpleTable)
        {
            var list = new List<SimpleGoodsType>();
            foreach (DataRow row in simpleTable.Rows)
            {
                var redirect_type_col= row["redirect_type"];
                var redirect_url_col = row["redirect_url"];
                string redirect_type = "0";
                string redirect_url = "";
                if (redirect_type_col==null|| redirect_type_col==DBNull.Value)
                {
                    redirect_type = "0";
                }else
                {
                    redirect_type = row["redirect_type"].ToString();
                }

                #region redirect_url
                if (redirect_url_col == null || redirect_url_col == DBNull.Value)
                {
                    redirect_url = "";
                }
                else
                {
                    redirect_url = row["redirect_url"].ToString();
                } 
                #endregion
                var type = new SimpleGoodsType
                {
                    id = Convert.ToInt32(row["id"]),
                    name = Convert.ToString(row["name"]),
                    icon = Convert.ToInt32(row["icon"]),
                    redirect_type = redirect_type,
                    redirect_url= redirect_url
                };

                list.Add(type);
            }
            return list;
        }
    }

    /// <summary>
    /// 商品类别对象
    /// </summary>
    /// 对应shop_cate
    public partial class GoodsType
    {
        private List<GoodsType> _nodes = new List<GoodsType>();
        private List<CarouslFigure> _carousl = new List<CarouslFigure>();

        /// <summary>
        /// 类别标识
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// 类别名称
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 类别图标
        /// </summary>
        public int icon { get; set; }

        /// <summary>
        /// 轮播图列表
        /// </summary>
        public List<CarouslFigure> carousl
        {
            get { return _carousl; }
            set { _carousl = value; }
        }

        ///// <summary>
        ///// 父类别标识
        ///// </summary>
        //public int pid { get; set; }

        /// <summary>
        /// 子分类节点
        /// </summary>
        public List<GoodsType> nodes
        {
            get { return _nodes; }
            set { _nodes = value; }
        }

        /// <summary>
        /// 创建树
        /// </summary>
        /// <returns>结果树</returns>
        public void BuildTree(DataTable GoodsTypeTable, GoodsType Parent, DataTable CarouslTable, int PId)
        {
            DataRow[] ChildRows = GoodsTypeTable.Select(string.Format("pid = {0}", PId));
            DataRow[] CarousRows = CarouslTable.Select(string.Format("cate_id = {0}", PId));
            DataTable Carousl = DataSetHelper.ConvertRowsToTable(CarousRows);
            if (Carousl != null)
            {
                Carousl.Columns.Remove("cate_id");
            }
            Parent.carousl = (List<CarouslFigure>)DataSetHelper.ConvertDataTableToList<CarouslFigure>(Carousl);
            if (ChildRows.Length > 0)
            {
                foreach (DataRow Row in ChildRows)
                {
                    GoodsType Child = new GoodsType();
                    Child.id = Convert.ToInt32(Row["id"]);
                    Child.name = Convert.ToString(Row["name"]);
                    int icon_try = 0;
                    int.TryParse(Row["icon"].ToString(), out icon_try);
                    Child.icon = icon_try;
                    Parent.nodes.Add(Child);
                    BuildTree(GoodsTypeTable, Child, CarouslTable, Child.id);
                }
            }
        }

        public static List<GoodsType> InitFromDataTable(DataTable Table)
        {
            List<GoodsType> List = new List<GoodsType>();
            GoodsType Type = new GoodsType();
            foreach (DataRow Row in Table.Rows)
            {
                Type.id = Convert.ToInt32(Row["id"]);
                Type.name = Convert.ToString(Row["name"]);
                Type.icon = Convert.ToInt32(Row["icon"]);
                List.Add(Type);
            }
            return List;
        }
    }

    #endregion 商品类别对象

    #region 商品概览对象

    /// <summary>
    /// 商品概览（用于商品列表渲染）
    /// </summary>
    /// 对应GOODS_LIST_VIEW
    public partial class GoodsSummary
    {
        /// <summary>
        /// 商品标识
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// 店铺类别:0.自营店，1:入驻店
        /// </summary>
        public int shop_type { get; set; }

        /// <summary>
        /// 平台价格
        /// </summary>
        public decimal price { get; set; }

        /// <summary>
        /// 商品价格
        /// </summary>
        public decimal local_price { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 商品类型
        /// </summary>
        public int goods_type { get; set; }

        /// <summary>
        /// 商品简介
        /// </summary>
        public string propaganda { get; set; }

        /// <summary>
        /// 商品图
        /// </summary>
        public long pic_id { get; set; }

        /// <summary>
        /// 商品返还率
        /// </summary>
        public string return_money_rate { get; set; }

        /// <summary>
        /// 兑换所需金币数
        /// </summary>
        public decimal discount { get; set; }

        /// <summary>
        /// 钱支付的
        /// </summary>
        public decimal cash_price { get; set; }
        /// <summary>
        /// 豆支付的钱
        /// </summary>
        public decimal beans_price { get; set; }
    }

    #endregion 商品概览对象

    #region 推荐商品对象

    /// <summary>
    /// 推荐商品对象
    /// </summary>
    /// 对应RECOMMEND_HOT_VIEW
    /// RECOMMEND_MAIN_VIEW
    public partial class GoodsRecommend
    {
        /// <summary>
        /// 商品标识
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// 平台价格
        /// </summary>
        public decimal price { get; set; }

        /// <summary>
        /// 商品价格
        /// </summary>
        public decimal local_price { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 推广语
        /// </summary>
        public string propaganda { get; set; }

        /// <summary>
        /// 商品图片id
        /// </summary>
        public long pic_id { get; set; }

        /// <summary>
        /// 商品返还率
        /// </summary>
        public string return_money_rate { get; set; }
    }

    #endregion 推荐商品对象

    #region 轮播图对象

    /// <summary>
    /// 轮播图对象
    /// </summary>
    /// 对应CAROUSEL_CHOSEN_LIST_VIEW
    /// CAROUSEL_MAIN_LIST_VIEW
    public partial class CarouslFigure
    {
        /// <summary>
        /// 轮播图标识
        /// </summary>
        public int id { get; set; }

        ///// <summary>
        ///// 轮播图类型:1.主页轮播,2:精选页轮播
        ///// </summary>
        //public byte type { get; set; }

        /// <summary>
        /// 图片标识
        /// </summary>
        public int pic_id { get; set; }

        /// <summary>
        /// 跳转URL:1.goods_id;2.shop_id;3.url
        /// </summary>
        public string url { get; set; }

        /// <summary>
        /// 跳转类型:1.跳转到商品,2.跳转到商店,3.跳转到一个url
        /// </summary>
        public int direct_type { get; set; }
        public int type { get; set; }
    }

    #endregion 轮播图对象

    #region 地理编码对象

    /// <summary>
    /// 地理编码对象
    /// </summary>
    public partial class AreaCode
    {
        /// <summary>
        /// 区域编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 区域名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 区域等级:1.省,2.市,3.区
        /// </summary>
        public string Class { get; set; }

        /// <summary>
        /// 父编码
        /// </summary>
        public string Parent { get; set; }
    }

    #endregion 地理编码对象

    #region 银行卡编码对象

    /// <summary>
    /// 银行卡编码对象
    /// </summary>
    public partial class BankMain
    {
        /// <summary>
        /// 银行英文编码
        /// </summary>
        public string bank_en { get; set; }

        /// <summary>
        /// 银行名称
        /// </summary>
        public string bank_name { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        public string bank_icon { get; set; }
    }

    #endregion 银行卡编码对象

    #region 快递公司编码对象

    /// <summary>
    /// 快递公司编码对象
    /// </summary>
    public partial class TrackCom
    {
        /// <summary>
        /// 标识
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// 快递公司名
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 快递公司编码
        /// </summary>
        public string code { get; set; }
    }

    #endregion 快递公司编码对象

    #region 用户收益记录对象

    /// <summary>
    /// 用户收益记录
    /// </summary>
    public partial class UserIncomeRecord
    {
        /// <summary>
        /// 标识
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// 记录添加时间
        /// </summary>
        public DateTime addtime { get; set; }

        /// <summary>
        /// 用户id
        /// </summary>
        public int uid { get; set; }

        /// <summary>
        /// 订单id
        /// </summary>
        public int order_id { get; set; }

        /// <summary>
        /// 收益数额
        /// </summary>
        public decimal money { get; set; }

        /// <summary>
        /// 收益类型
        /// </summary>
        public int type { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string remark { get; set; }

        /// <summary>
        /// 记录类型
        /// 1:现金收益记录，2:金币收益记录
        /// </summary>
        public int record_type { get; set; }
        /// <summary>
        /// 推荐等级
        /// </summary>
        public int rec_class { get; set; }
    }

    #endregion 用户收益记录对象

    #region 用户账户对象

    /// <summary>
    /// 用户账户对象
    /// </summary>
    public partial class UserAccount
    {
        /// <summary>
        /// 用户id
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// 用户金币
        /// </summary>
        public decimal plate_to_return_money { get; set; }
        /// <summary>
        /// 不参与分配的金币
        /// </summary>
        public decimal ex_plate_to_return_money { get; set; }
        /// <summary>
        /// 用户历史积分
        /// </summary>
        public int point { get; set; }
        /// <summary>
        /// 用户消费账户
        /// </summary>
        public decimal r_money { get; set; }
        /// <summary>
        /// 用户现金账户
        /// </summary>
        public decimal x_money { get; set; }
        /// <summary>
        /// 用户推荐账户
        /// </summary>
        public decimal y_money { get; set; }
        /// <summary>
        /// 用户推荐账户冻结
        /// </summary>
        public decimal y_money_frz { get; set; }
        /// <summary>
        /// 金豆账户
        /// </summary>
        public decimal h_money { get; set; }
        /// <summary>
        /// 用户分配权重
        /// </summary>
        public int plate_to_return_weight { get; set; }

        /// <summary>
        /// 交易总额
        /// </summary>
        public decimal total_trans { get; set; }
        /// <summary>
        /// 总返还额
        /// </summary>
        public decimal total_return { get; set; }
        /// <summary>
        /// 总权重
        /// </summary>
        public int total_weight { get; set; }
        /// <summary>
        /// 每权平均返还
        /// </summary>
        public decimal return_ave_weight { get; set; }
        /// <summary>
        /// 信息是否已阅读
        /// </summary>
        public bool if_info_has_read { get; set; }



        public int coupons_count { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public UserAccount(DataRow accountRow)
        {
            var plateData = PlateStatistic.GetData()?.Tables[0].Rows[0];
            if (plateData != null)
            {
                total_trans = Convert.ToDecimal(plateData["total_trans"]);
                total_return = Convert.ToDecimal(plateData["total_return"]);
                total_weight = Convert.ToInt32(plateData["total_weight"]);
            }
            return_ave_weight = Math.Round(total_return / total_weight,3);

            id = Convert.ToInt32(accountRow["id"]);
            plate_to_return_money = Convert.ToDecimal(accountRow["plate_to_return_money"]);
            ex_plate_to_return_money = Convert.ToDecimal(accountRow["ex_plate_to_return_money"]);
            point = Convert.ToInt32(accountRow["point"]);
            r_money = Convert.ToDecimal(accountRow["r_money"]);
            x_money = Convert.ToDecimal(accountRow["x_money"]);
            y_money = Convert.ToDecimal(accountRow["y_money"]);
            y_money_frz = Convert.ToDecimal(accountRow["y_money_frz"]);
            h_money = Convert.ToDecimal(accountRow["h_money"]);
            plate_to_return_weight = Convert.ToInt32(accountRow["plate_to_return_weight"]);
            if_info_has_read = Convert.ToBoolean(accountRow["if_info_has_read"]);
            coupons_count = Convert.ToInt32(accountRow["coupons_count"]);
        }
    }

    #endregion 用户账户对象

    #region 用户下线对象

    /// <summary>
    /// 用户下线对象
    /// </summary>
    public partial class UserReferral
    {
        /// <summary>
        /// 用户id
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string username { get; set; }
        /// <summary>
        /// 绑定时间
        /// </summary>
        public DateTime addtime { get; set; }
    }

    #endregion 用户下线对象

    #region 用户推荐统计对象

    /// <summary>
    /// 用户推荐统计对象
    /// </summary>
    public partial class UserRecommandCount
    {
        /// <summary>
        /// 用户标识
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// 是否是掌柜
        /// </summary>
        public bool is_shop_keeper { get; set; }
        /// <summary>
        /// 推荐用户数合计
        /// </summary>
        public int total_recommand_user_count { get; set; }
        /// <summary>
        /// 第一级推荐用户
        /// </summary>
        public int recommand_user_first_class { get; set; }
        /// <summary>
        /// 第二级推荐用户
        /// </summary>
        public int recommand_user_second_class { get; set; }
        /// <summary>
        /// 第三级推荐用户
        /// </summary>
        public int recommand_user_third_class { get; set; }
        /// <summary>
        /// 已购物用户数合计
        /// </summary>
        public int recommand_user_consumed { get; set; }
        /// <summary>
        /// 未购物用户数合计
        /// </summary>
        public int recommand_user_not_consumed { get; set; }
        /// <summary>
        /// 推荐收益合计
        /// </summary>
        public decimal total_recommand_award { get; set; }
        /// <summary>
        /// 本月推荐收益合计
        /// </summary>
        public decimal month_recommand_award { get; set; }
        /// <summary>
        /// 推荐订单数合计
        /// </summary>
        public int total_recommand_order_count { get; set; }
        /// <summary>
        /// 本月推荐订单数量合计
        /// </summary>
        public int month_recommand_order_count { get; set; }
        // lcl 20180426 Insert
        /// <summary>
        /// 本月推荐用户数合计
        /// </summary>
        public int month_recommand_user_count { get; set; }
        // lcl 20180426 Insert
        /// <summary>
        /// 分级推荐人数统计数据
        /// </summary>
        public dynamic[] each_level_recommand_user_count { get; set; }
        // lcl 20180426 Insert
        /// <summary>
        /// 本月推荐人数排名
        /// </summary>
        public int month_recommand_user_count_ranking { get; set; }
        // lcl 20180427 Insert
        /// <summary>
        /// 本月推荐奖励排名
        /// </summary>
        public int month_recommand_rewards_ranking { get; set; }
        // lcl 20180427 Insert
        /// <summary>
        /// 本月待确认奖励金额
        /// </summary>
        public decimal month_unconfirm_order_sum_reward { get; set; }


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="countRow"></param>
        public UserRecommandCount(DataRow countRow)
        {
            id = Convert.ToInt32(countRow["id"]);
            is_shop_keeper = Convert.ToBoolean(countRow["is_shop_keeper"]);

            total_recommand_user_count = Convert.ToInt32(countRow["total_recommand_user_count"]);
            recommand_user_first_class = Convert.ToInt32(countRow["recommand_user_first_class"]);
            recommand_user_second_class = Convert.ToInt32(countRow["recommand_user_second_class"]);
            recommand_user_third_class = Convert.ToInt32(countRow["recommand_user_third_class"]);
            recommand_user_consumed = Convert.ToInt32(countRow["recommand_user_consumed"]);
            recommand_user_not_consumed = Convert.ToInt32(countRow["recommand_user_not_consumed"]);

            total_recommand_award = Convert.ToDecimal(countRow["total_recommand_award"]);
            month_recommand_award = Convert.ToDecimal(countRow["month_recommand_award"]);

            total_recommand_order_count = Convert.ToInt32(countRow["total_recommand_order_count"]);
            month_recommand_order_count = Convert.ToInt32(countRow["month_recommand_order_count"]);
        }
    }

    #endregion

    #region 月度用户推荐数据对象
    /// <summary>
    /// 用户月度推荐数据
    /// </summary>
    public partial class MonthUserRecommandData
    {
        /// <summary>
        /// 用户id
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// 是否是掌柜
        /// </summary>
        public bool is_shop_keeper { get; set; }
        /// <summary>
        /// 月度推荐用户总数
        /// </summary>
        public int month_total_recommand_user { get; set; }
        /// <summary>
        /// 月度推荐第一层级用户
        /// </summary>
        public int month_first_class_recommand_user { get; set; }
        /// <summary>
        /// 月度推荐第二层级用户
        /// </summary>
        public int month_second_class_recommand_user { get; set; }
        /// <summary>
        /// 月度第三层级用户
        /// </summary>
        public int month_third_class_recommand_user { get; set; }
        /// <summary>
        /// 月度推荐已购买用户
        /// </summary>
        public int month_consumed_recommand_user { get; set; }
        /// <summary>
        /// 月度推荐未购买用户
        /// </summary>
        public int month_consumed_not_consumed_user { get; set; }
        /// <summary>
        /// 月度推荐用户列表
        /// </summary>
        public List<MonthUserRecommandObj> list { get; set; } = new List<MonthUserRecommandObj>();

        public MonthUserRecommandData(DataRow mainRow , DataTable recommandTable)
        {
            id = Convert.ToInt32(mainRow["id"]);
            is_shop_keeper = Convert.ToBoolean(mainRow["is_shop_keeper"]);
            month_total_recommand_user = Convert.ToInt32(mainRow["month_total_recommand_user"]);
            month_first_class_recommand_user = Convert.ToInt32(mainRow["month_first_class_recommand_user"]);
            month_second_class_recommand_user = Convert.ToInt32(mainRow["month_second_class_recommand_user"]);
            month_third_class_recommand_user = Convert.ToInt32(mainRow["month_third_class_recommand_user"]);
            month_consumed_recommand_user = Convert.ToInt32(mainRow["month_consumed_recommand_user"]);
            month_consumed_not_consumed_user = Convert.ToInt32(mainRow["month_consumed_not_consumed_user"]);

            foreach (DataRow row in recommandTable.Rows)
            {
                list.Add(new MonthUserRecommandObj(row));
            }
        }
    }

    /// <summary>
    /// 推荐用户对象
    /// </summary>
    public partial class MonthUserRecommandObj
    {
        /// <summary>
        /// 用户标识
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string username { get; set; }
        /// <summary>
        /// 用户昵称
        /// </summary>
        public string real_name { get; set; }
        /// <summary>
        /// 注册时间
        /// </summary>
        public DateTime addtime { get; set; }
        /// <summary>
        /// 累计消费
        /// </summary>
        public decimal total_cash { get; set; }
        /// <summary>
        /// 用户推荐层级
        /// </summary>
        public int recommand_class { get; set; }
        /// <summary>
        /// 用户子用户数
        /// </summary>
        public int recommand_count { get; set; }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="row">数据行</param>
        public MonthUserRecommandObj(DataRow row)
        {
            id = Convert.ToInt32(row["id"]);
            username = Convert.ToString(row["username"]);
            real_name = Convert.ToString(row["real_name"]);
            addtime = Convert.ToDateTime(row["addtime"]);
            total_cash = Convert.ToDecimal(row["total_cash"]);
            recommand_class = Convert.ToInt32(row["recommand_class"]);
            recommand_count = Convert.ToInt32(row["recommand_count"]);
        }
    }

    #endregion

    #region 月度订单推荐数据对象

    /// <summary>
    /// 订单月度推荐数据
    /// </summary>
    public partial class MonthOrderRecommandData
    {
        /// <summary>
        /// 用户id
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// 是否是掌柜
        /// </summary>
        public bool is_shop_keeper { get; set; }
        /// <summary>
        /// 年度推荐总订单数
        /// </summary>
        public int year_total_order { get; set; }
        /// <summary>
        /// 年度已确认订单数
        /// </summary>
        public int year_settled_order { get; set; }
        /// <summary>
        /// 月度推荐总订单数
        /// </summary>
        public int month_total_order { get; set; }
        /// <summary>
        /// 月度已确认订单数
        /// </summary>
        public int month_settled_order { get; set; }
        /// <summary>
        /// 月度推荐用户列表
        /// </summary>
        public List<MonthOrderRecommandObj> list { get; set; } = new List<MonthOrderRecommandObj>();

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="mainRow"></param>
        /// <param name="recommandTable"></param>
        public MonthOrderRecommandData(DataRow mainRow, DataTable recommandTable)
        {
            id = Convert.ToInt32(mainRow["id"]);
            is_shop_keeper = Convert.ToBoolean(mainRow["is_shop_keeper"]);
            year_total_order = Convert.ToInt32(mainRow["year_total_order"]);
            year_settled_order = Convert.ToInt32(mainRow["year_settled_order"]);
            month_total_order = Convert.ToInt32(mainRow["month_total_order"]);
            month_settled_order = Convert.ToInt32(mainRow["month_settled_order"]);

            foreach (DataRow row in recommandTable.Rows)
            {
                list.Add(new MonthOrderRecommandObj(row));
            }
        }
    }

    /// <summary>
    /// 推荐订单对象
    /// </summary>
    public partial class MonthOrderRecommandObj
    {
        /// <summary>
        /// 订单号
        /// </summary>
        public string ordercode { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string username { get; set; }
        /// <summary>
        /// 订单创建时间
        /// </summary>
        public DateTime addtime { get; set; }
        /// <summary>
        /// 订单确认时间
        /// </summary>
        public DateTime? checktime { get; set; }
        /// <summary>
        /// 订单额
        /// </summary>
        public decimal money { get; set; }
        /// <summary>
        /// 订单状态2:未确认，3:已确认
        /// </summary>
        public int status { get; set; }
        /// <summary>
        /// 推荐层级
        /// </summary>
        public int recommand_class { get; set; }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="row">数据行</param>
        public MonthOrderRecommandObj(DataRow row)
        {
            ordercode = Convert.ToString(row["ordercode"]);
            username = Convert.ToString(row["username"]);
            addtime = Convert.ToDateTime(row["addtime"]);
            if (row["checktime"] == DBNull.Value)
            {
                checktime = null;
            }
            else
            {
                checktime = Convert.ToDateTime(row["checktime"]);
            }
            money = Convert.ToDecimal(row["money"]);
            status = Convert.ToInt32(row["status"]);
            recommand_class = Convert.ToInt32(row["recommand_class"]);
        }
    }

    #endregion

    #region 月度推荐奖励金数据对象

    /// <summary>
    /// 月度推荐收益数据
    /// </summary>
    public partial class MonthRecommandMoneyData
    {
        /// <summary>
        /// 用户id
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// 是否是掌柜
        /// </summary>
        public bool is_shop_keeper { get; set; }
        /// <summary>
        /// 年度推荐收益总额
        /// </summary>
        public decimal year_total_money { get; set; }
        /// <summary>
        /// 年度推荐收益冻结
        /// </summary>
        public decimal year_frz_money { get; set; }
        /// <summary>
        /// 月度推荐收益总额
        /// </summary>
        public decimal month_total_money { get; set; }
        /// <summary>
        /// 月度推荐收益冻结
        /// </summary>
        public decimal month_frz_money { get; set; }
        /// <summary>
        /// 月度推荐用户列表
        /// </summary>
        public List<MonthRecommandMoneyObj> list { get; set; } = new List<MonthRecommandMoneyObj>();

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="mainRow"></param>
        /// <param name="recommandTable"></param>
        public MonthRecommandMoneyData(DataRow mainRow, DataTable recommandTable)
        {
            id = Convert.ToInt32(mainRow["id"]);
            is_shop_keeper = Convert.ToBoolean(mainRow["is_shop_keeper"]);
            year_total_money = Convert.ToDecimal(mainRow["year_total_money"]);
            year_frz_money = Convert.ToDecimal(mainRow["year_frz_money"]);
            month_total_money = Convert.ToDecimal(mainRow["month_total_money"]);
            month_frz_money = Convert.ToDecimal(mainRow["month_frz_money"]);

            foreach (DataRow row in recommandTable.Rows)
            {
                list.Add(new MonthRecommandMoneyObj(row));
            }
        }
    }

    /// <summary>
    /// 推荐收益对象
    /// </summary>
    public partial class MonthRecommandMoneyObj
    {
        /// <summary>
        /// 入账时间
        /// </summary>
        public DateTime addtime { get; set; }
        /// <summary>
        /// 推荐入账金额
        /// </summary>
        public decimal recommand_money { get; set; }
        /// <summary>
        /// 购买人用户名
        /// </summary>
        public string buyer_name { get; set; }
        /// <summary>
        /// 是否冻结状态
        /// </summary>
        public bool is_frz { get; set; }
        /// <summary>
        /// 订单额
        /// </summary>
        public decimal order_money { get; set; }
        /// <summary>
        /// 推荐层级
        /// </summary>
        public int recommand_class { get; set; }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="row">数据行</param>
        public MonthRecommandMoneyObj(DataRow row)
        {
            recommand_money = Convert.ToDecimal(row["recommand_money"]);
            buyer_name = Convert.ToString(row["buyer_name"]);
            addtime = Convert.ToDateTime(row["addtime"]);
            order_money = Convert.ToDecimal(row["order_money"]);
            is_frz = Convert.ToBoolean(row["is_frz"]);
            recommand_class = Convert.ToInt32(row["recommand_class"]);
        }
    }

    #endregion
}