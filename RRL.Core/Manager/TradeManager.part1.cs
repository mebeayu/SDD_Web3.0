using Dapper;
using RRL.Config;
using RRL.Core.External;
using RRL.Core.Models;
using RRL.Core.Models.WxJSAPI;
using RRL.Core.Pay;
using RRL.Core.Tables;
using RRL.Core.Utility;
using RRL.DB;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Dynamic;
using System.Data.SqlClient;

namespace RRL.Core.Manager
{
    public partial class TradeManager
    {
        #region 可配置常量

        public static decimal DEFAULT_CASH_PAY_RATE = 5;
        public static decimal DEFAULT_BEANS_PAY_RATE = RRLConfig.RMB_To_GoldBean_Rate; // lcl 2018-09-08 Modify RMB兑换金豆比率由1:100修改为1:1
        public static int ZERO_HOLD_GOODS_TYPE = 895;
        ConfigManager configMgr = new ConfigManager();
        UserManager userManager = new UserManager();
        #endregion
        /// <summary>
        /// 通过商品生成订单
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="goods_id">商品id</param>
        /// <param name="goods_count">商品数量</param>
        /// <param name="receive_id">收货信息</param>
        /// <returns>生成结果</returns>
        public BussResult CreateOrderFromGoodsV3(int uid, int goods_id, int goods_count, string msg_leave_word,
            string msg_phone, string msg_realname, string msg_idcardno, out int? order_id, int order_type = 1, int spreader_uid = 0)
        {
            BussResult bussResult = new BussResult() { status = 0, message = "订单创建成功!" };
            if (spreader_uid != 0)
            {
                int t;
                AddToSpreader(uid, spreader_uid, out t);
            }
            //创建单一订单
            order_id = null;
            SqlDataBase db = new SqlDataBase();
            int effect = db.Execute(@"UPDATE  rrl_goods SET inv_count =inv_count-@goods_count, sell_count = sell_count + @goods_count WHERE id = @goods_id and inv_count>=@goods_count", new { goods_count = goods_count, goods_id = goods_id });
            if (effect == 0)
            {
                return new BussResult() { status = 99, message = "库存不足!" };

            }
            else
            {
                int? orderId = Order_Insert(goods_id, uid);
                if (orderId != null)
                {
                    int cnt = OrderInfoGoods_Insert(orderId.Value, goods_id, goods_count, msg_leave_word, msg_phone, msg_realname, msg_idcardno);
                    if (cnt == 0)
                    {
                        return new BussResult() { status = 99, message = "商品订单创建失败!" };
                    }


                    List<FeeConfig> list = db.Select<FeeConfig>("select buyer_recommend_award from rrl_goods a left join rrl_fee_config b on a.fee_id=b.id where a.id=@goods_id",
                        new { goods_id = goods_id });
                    double buyer_recommend_award = 0;
                    if (list != null && list.Count != 0)
                    {
                        buyer_recommend_award = list[0].buyer_recommend_award;
                    }
                    List<Spreader> list_spreader = db.Select<Spreader>("select spreader_uid,spreader_expire from rrl_user where id=@uid",
                        new { uid = uid });
                    if (list_spreader != null && list_spreader.Count > 0)
                    {
                        spreader_uid = list_spreader[0].spreader_uid;
                        DateTime spreader_expire = DateTime.Parse(list_spreader[0].spreader_expire);
                        if (DateTime.Now > spreader_expire) spreader_uid = 0;//过期
                    }
                    //订单上设置推荐人id，推荐奖励比例
                    int res = db.Execute("update rrl_order set order_type=970,spreader_uid=@spreader_uid,recommend_award_rate=@recommend_award_rate where id=@orderId",
                        new { spreader_uid = spreader_uid, orderId = orderId, recommend_award_rate = buyer_recommend_award });

                }
                else
                    return new BussResult() { status = 99, message = "主订单创建失败!" };
                order_id = orderId;
            }
            return bussResult;
        }
        public int AddToSpreader(int uid, int spreader_uid, out int new_spreader_uid)
        {
            new_spreader_uid = 0;
            RRLDB db = new RRLDB();
            DataSet ds = db.ExeQuery("select spreader_uid,spreader_expire from rrl_user where id=@uid", new SqlParameter("uid", uid));
            if (ds == null)
            {
                db.Close();
                return MessageCode.ERROR_EXECUTE_SQL;
            }
            if (ds.Tables[0].Rows.Count == 0)
            {
                db.Close();
                return MessageCode.ERROR_NO_UID;
            }
            DateTime spreader_expire = DateTime.Parse(ds.Tables[0].Rows[0][1].ToString());
            int old_spreader_uid = int.Parse(ds.Tables[0].Rows[0][0].ToString());
            if (spreader_uid != old_spreader_uid)
            {
                if (spreader_expire < DateTime.Now)//当前时间超过到期日
                {
                    DateTime new_spreader_expire = DateTime.Now.AddDays(30);
                    int res = db.ExeCMD("update rrl_user set spreader_uid=@spreader_uid,spreader_expire=@spreader_expire where id=@uid",
                        new SqlParameter("spreader_uid", spreader_uid),
                        new SqlParameter("spreader_expire", new_spreader_expire.ToString("yyyy-MM-dd HH:mm:ss")),
                        new SqlParameter("uid", uid));
                    if (res <= 0)
                    {
                        db.Close();
                        return MessageCode.ERROR_EXECUTE_SQL;
                    }
                    db.Close();
                    new_spreader_uid = spreader_uid;
                    return MessageCode.SUCCESS;
                }
                else
                {
                    new_spreader_uid = old_spreader_uid;
                    return MessageCode.ERROR_HAVE_SPREADER;
                }
            }
            else
            {
                db.Close();
                new_spreader_uid = old_spreader_uid;
                return MessageCode.SUCCESS;
            }
        }
        public DataSet QueryMemberOrder(int spreader_uid, int Page, int PageSize = 20)
        {
            string col = "a.id,a.ordercode,a.buyer_id,a.money,a.order_type,a.status,a.addtime,a.pay_bean,a.pay_money,a.recommend_award_rate,a.yj_tag,b.username,b.real_name";
            int PageID = Math.Max(Page, 1);
            int offset = (PageID - 1) * PageSize;
            string sql = string.Format(@" select top {0} * from 
                                        (select row_number() over(order by a.id desc) as rownumber,{2} from rrl_order a left join rrl_user b on a.buyer_id=b.id where 
                                         a.spreader_uid=@spreader_uid and a.status in(2,3,4,-3)) as s where s.rownumber>{1}",
            PageSize, offset, col);
            RRLDB db = new RRLDB();
            DataSet ds = db.ExeQuery(sql, new SqlParameter("spreader_uid", spreader_uid));
            return ds;
        }
        /// <summary>
        /// 佣金结算，收货7天后
        /// </summary>
        /// <returns></returns>
        public int SettleAccountsOfSpreader()
        {
            RRLDB db = new RRLDB();
            DateTime checktime = DateTime.Now.AddDays(-7);//七天前
            DataSet ds = db.ExeQuery(@"select a.id,a.buyer_id,a.pay_bean,a.pay_money,a.spreader_uid,a.recommend_award_rate,b.username,b.real_name from 
                                        rrl_order a left join rrl_user b on a.buyer_id=b.id where 
                                        a.yj_tag=0 and a.spreader_uid<>0 and a.checktime<=@checktime and a.status=3", new SqlParameter("checktime", checktime));
            int n = ds.Tables[0].Rows.Count;
            for (int i = 0; i < n; i++)
            {
                string order_id = ds.Tables[0].Rows[i]["id"].ToString();
                double pay_bean = double.Parse(ds.Tables[0].Rows[i]["pay_bean"].ToString());
                double pay_money = double.Parse(ds.Tables[0].Rows[i]["pay_money"].ToString());
                double recommend_award_rate = double.Parse(ds.Tables[0].Rows[i]["recommend_award_rate"].ToString());
                int buyer_id = int.Parse(ds.Tables[0].Rows[i]["pay_bean"].ToString());
                int spreader_uid= int.Parse(ds.Tables[0].Rows[i]["spreader_uid"].ToString());
                string username = ds.Tables[0].Rows[i]["username"].ToString();
                string real_name = ds.Tables[0].Rows[i]["real_name"].ToString();

                double y_money = pay_money * recommend_award_rate;
                double y_bean = pay_bean * recommend_award_rate;

                int res = db.ExeCMD("update rrl_user set x_money=x_money+@y_money,h_money=h_money+y_bean where id=@spreader_uid",
                    new SqlParameter("y_money", y_money),
                    new SqlParameter("y_bean", y_bean), 
                    new SqlParameter("spreader_uid", spreader_uid));
                if (y_money > 0)
                {
                    res = db.ExeCMD(@"insert into rrl_user_money_record(addtime,uid,order_id,money,type,remark) values(@addtime,@uid,@order_id,@money,201,@remark)",
                    new SqlParameter("addtime", DateTime.Now),
                    new SqlParameter("uid", spreader_uid),
                    new SqlParameter("order_id", order_id),
                    new SqlParameter("money", y_money),
                    new SqlParameter("remark", string.Format("{0}{1}佣金现金收入{2}",username,real_name, y_money)));
                }
                if (y_bean>0)
                {
                    res = db.ExeCMD(@"insert into rrl_user_money_record(addtime,uid,order_id,money,type,remark) values(@addtime,@uid,@order_id,@money,201,@remark)",
                    new SqlParameter("addtime", DateTime.Now),
                    new SqlParameter("uid", spreader_uid),
                    new SqlParameter("order_id", order_id),
                    new SqlParameter("money", y_bean),
                    new SqlParameter("remark", string.Format("{0}{1}佣金金豆收入{2}", username, real_name, y_bean)));
                }
                
            }
            db.Close();
            return n;
        }
        /// <summary>
        /// 更新是否豆支付
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="orderlist"></param>
        /// <param name="is_beans_pay"></param>
        /// <returns></returns>
        public int UpdateOrderIsBeansPayV3(int uid, string orderlist, string is_beans_pay, string discount_type)
        {
            if (!"0".Equals(is_beans_pay))
            {
                is_beans_pay = "1";
            }
            SqlDataBase db = new SqlDataBase();
            int effect = db.Execute($@"update rrl_order set is_beans_pay={is_beans_pay} where buyer_id={uid} and id in({orderlist}) and status=1 and is_paid=0 and isnull(is_paid_beans,0)=0 ");
            if (effect > 0)
            {
                if ("1" == discount_type)//3,7开
                {
                    decimal Order_Shared_discount_money_rate = 100 * configMgr.GetConfigValue("Order_Shared_discount_money_rate", 0.29m);
                    // lcl 2018-09-18 Modify （需改金豆比率规则）
                    decimal Order_Shared_discount_beans_rate = configMgr.GetConfigValue("Order_Shared_discount_beans_rate", 0.52m);
                    var sql = $@"update rrl_order_info_goods set cash_pay_rate=@cash_pay_rate,beans_pay_rate=@beans_pay_rate where order_id in({orderlist}) ";
                    effect = db.Execute(sql, new { cash_pay_rate = Order_Shared_discount_money_rate, beans_pay_rate = Order_Shared_discount_beans_rate });
                    return 0;
                }
                else
                {
                    var sql = $@"update  a 
                            set a.cash_pay_rate=isnull(b.cash_pay_rate,{DEFAULT_CASH_PAY_RATE}),a.beans_pay_rate=isnull(b.beans_pay_rate,{DEFAULT_BEANS_PAY_RATE})
                            from rrl_order_info_goods a inner join rrl_goods b on  a.goods_id=b.id
                            where a.order_id in ({orderlist})";//
                    effect = db.Execute(sql);
                    return 0;
                }
            }
            return 0;

        }


        /// <summary>
        /// 通过购物车生成订单
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="receive_id">收件地址id</param>
        /// <param name="cart_list">购物车列表</param>
        /// <returns>生成结果</returns>
        public BussResult CreateOrderFromCartV3(int uid, /*int receive_id,*/ List<int> cart_list, out List<int> order_list)
        {
            BussResult bussResult = new BussResult() { status = 0, message = "订单创建成功!" };
            //创建单一订单
            order_list = new List<int>();
            string cart_id_in_str = string.Join(",", cart_list.Select(v => v.ToString()));
            SqlDataBase db = new SqlDataBase();
            var conn = db.CreateConnection();
            DbTransaction tran = null;
            int count = 0;
            bool isSubInv_ok = false;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                var sql = $@"UPDATE a
            set a.inv_count=a.inv_count-b.goods_count,
              a.sell_count=a.sell_count+b.goods_count
            from rrl_goods as a
            inner join rrl_shop_cart as b on a.id=b.goods_id
            where a.inv_count-b.goods_count>=0 
            and b.isUsed=0
            AND b.uid = @uid
            AND b.deletemark IS NULL
            and b.id in({cart_id_in_str})";
                count = db.ExecuteTran(conn, sql, new { uid = uid }, tran);
                if (count != cart_list.Count)
                {
                    if (tran != null)
                    { tran.Rollback(); }
                    return new BussResult() { status = 99, message = "库存不足!" };
                }
                else
                {
                    string sql2 = $@"update rrl_shop_cart set isUsed=1 where id in({cart_id_in_str})";
                    int cnt2 = db.ExecuteTran(conn, sql2, null, tran);
                    isSubInv_ok = true;
                    tran.Commit();
                }
            }
            catch (Exception ex)
            {
                if (tran != null)
                    tran.Rollback();
                return new BussResult() { status = 99, message = "订单创建失败!" };
            }
            finally
            {
                if (conn != null)
                    conn.Close();
            }

            if (isSubInv_ok)
            {
                List<RRL_Shop_Cart> list = db.Select<RRL_Shop_Cart>($@"select * from rrl_shop_cart where uid=@uid and id in({cart_id_in_str})", new { uid = uid });
                foreach (RRL_Shop_Cart shopCart in list)
                {
                    int? orderId = Order_Insert(shopCart.goods_id, uid);
                    if (orderId != null)
                    {
                        int cnt = OrderInfoGoods_Insert(orderId.Value, shopCart.goods_id, shopCart.goods_count, "", "", "", "");
                        if (cnt != 0)
                        {
                            order_list.Add(orderId.Value);
                        }
                    }
                    else
                    {
                        //return new BussResult() { status = 99, message = "主订单创建失败!" };
                    }
                }
            }
            return bussResult;

        }

        /// <summary>
        ///  获取订单信息
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="order_arr_str"></param>
        /// <returns></returns>
        public List<dynamic> GetPreOrderListV3(int uid, string order_arr_str)
        {
            SqlDataBase db = new SqlDataBase();
            var sql = $@"SELECT
	         rrl_order.id,
	         rrl_order.ordercode AS order_code,
	         rrl_order.buyer_id AS uid,
	         rrl_order.shop_id,
             rrl_order_info_goods.goods_id as goods_id,
	         rrl_shop.shop_name,
	         rrl_order_info_goods.goods_price as money,
	         rrl_order_info_goods.pay_gold_coin*rrl_order_info_goods.goods_count  as pay_gold_coin,
             rrl_order_info_goods.return_gold_coin*rrl_order_info_goods.goods_count as return_gold_coin,
	         rrl_order_info_goods.postage AS postage,
             rrl_order_info_goods.goods_name as goods_name,
             rrl_order_info_goods.goods_pic_id as goods_pic_id,
             convert(DECIMAL(18,2), (isnull(rrl_goods.cash_pay_rate,{DEFAULT_CASH_PAY_RATE}) * (rrl_order_info_goods.goods_price*rrl_order_info_goods.goods_count+isnull(rrl_order_info_goods.postage,0)-isnull(rrl_order.user_coupons_countmoney,0))/100)) as need_pay_money,
             convert(DECIMAL(18,2),(isnull(rrl_goods.beans_pay_rate,{DEFAULT_BEANS_PAY_RATE}) * (rrl_order_info_goods.goods_price*rrl_order_info_goods.goods_count+isnull(rrl_order_info_goods.postage,0)-isnull(rrl_order.user_coupons_countmoney,0)))) as need_pay_beans,
	         rrl_order.status,
	         rrl_order_info_goods.goods_count AS goods_count,
	         rrl_order.addtime,
	         rrl_order.send_time,
	         rrl_order.track_com,
	         rrl_order.track_num,
	         rrl_order.receive_name,
	         rrl_order.receive_address,
	         rrl_order.receive_mobile,
	         rrl_order.check_code,
             isnull(rrl_order.user_coupons_countmoney,0) as user_coupons_countmoney,
             isnull(rrl_order.user_coupons_id,0) as user_coupons_id,
             (rrl_order_info_goods.goods_price*rrl_order_info_goods.goods_count+isnull(rrl_order_info_goods.postage,0)-isnull(rrl_order.user_coupons_countmoney,0))  as all_pay_cash,
             isnull(rrl_order.is_beans_pay,1) as is_beans_pay,
             ISNULL(rrl_goods.is_can_use_coupons,cast(1 as bit)) as is_can_use_coupons,
             ISNULL(rrl_goods.postage_free_total_price,cast(88 as DECIMAL(18,2))) as postage_free_total_price,
             rrl_order_info_goods.goods_type as goods_type
        FROM
	        rrl_order
        inner join rrl_order_info_goods on rrl_order_info_goods.order_id=rrl_order.id
        inner join  rrl_shop ON rrl_order.shop_id =rrl_shop.id
        inner join rrl_goods on rrl_goods.id=rrl_order_info_goods.goods_id
        where rrl_order.status=1 and rrl_order.buyer_id=@uid
        and rrl_order.id in ({order_arr_str})
        ";

            List<dynamic> list = db.Select<dynamic>(sql, new { uid = uid });
            return list;
        }



        /// <summary>
        /// 获取邮费
        /// </summary>
        /// <param name="goods_id"></param>
        /// <param name="goods_count"></param>
        /// <returns></returns>
        private decimal GetGoodsPostage(int goods_id, int goods_count)
        {
            SqlDataBase db = new SqlDataBase();
            decimal default_postage = 8.0m;
            /* var sql = $@"select   
 case when (select top 1 convert(DECIMAL(18,2), value) from rrl_config where  item='minimum_money_for_nil_postage')<=price *@goods_count
 then 0 else 
     case when isnull(postage,0)=0 then {default_postage} else postage end
 end postage
 from rrl_goods where id =@goods_id";*/


            //            var sql = $@"select   
            //case when isnull(postage,0)=0 then 0 
            //else  
            //	case when (select top 1 convert(DECIMAL(18,2), value) from rrl_config where  item='minimum_money_for_nil_postage')<=price *@goods_count
            //	then 0 else {default_postage} end
            //end postage
            //from rrl_goods where id =@goods_id";

            //          var  sql = $@"select   
            //case when isnull(postage,0)=0 then 0 
            //else  
            //	case when (select top 1 convert(DECIMAL(18,2), value) from rrl_config where  item='minimum_money_for_nil_postage')<=price *@goods_count
            //	then 0 else isnull(postage,0) end
            //end postage
            //from rrl_goods where id =@goods_id";
            var sql = $@"select   
case when isnull(postage,0)=0 then 0 
else  
	case when  isnull(postage_free_total_price,(select top 1 convert(DECIMAL(18,2), value) from rrl_config where  item='minimum_money_for_nil_postage'))<=price *@goods_count
	then 0 else isnull(postage,0) end
end postage
from rrl_goods where id =@goods_id";

            decimal? postage = db.ExecuteScalar<decimal?>(sql, new { goods_id = goods_id, goods_count = goods_count });
            if (postage == null)
            {
                return default_postage;
            }
            else
            {
                return postage.Value;
            }

        }


        /// <summary>
        /// 添加订单主表
        /// </summary>
        /// <param name="goods_id"></param>
        /// <param name="buyer_id"></param>
        /// <returns></returns>
        private int? Order_Insert(int goods_id, int buyer_id)
        {
            SqlDataBase db = new SqlDataBase();
            var list = db.ExecuteStoredProcedure<int?>("sp_V3_order_insert", new { good_id = goods_id, buyer_id = buyer_id });
            if (list == null || list.Count == 0)
                return null;
            else
                return list[0].Value;
        }


        /// <summary>
        /// 添加订单_商品表
        /// </summary>
        /// <param name="order_id"></param>
        /// <param name="goods_id"></param>
        /// <param name="goods_count"></param>
        /// <returns></returns>
        private int OrderInfoGoods_Insert(int order_id, int goods_id, int goods_count, string msg_leave_word, string msg_phone, string msg_realname, string msg_idcardno)
        {
            SqlDataBase db = new SqlDataBase();
            var postage = GetGoodsPostage(goods_id, goods_count);
            #region Insert rrl_order_info_goods sql
            var sql = @"INSERT INTO  [rrl_order_info_goods]
           ([order_id]
           ,[goods_id]
           ,[goods_count]
           ,[goods_type]
           ,[goods_price]
           ,[goods_name]
           ,[goods_pic_id]
           ,[cash_pay_rate]
           ,[beans_pay_rate]
           ,[msg_leave_word]
           ,[msg_phone]
           ,[msg_realname]
           ,[msg_idcardno]
           ,[postage]
           ,[pay_gold_coin]
           ,[return_gold_coin]
           ,[card_goods_id]
           ,[card_goods_attr]
            )
            select @order_id,
            a.id as goods_id,
            @goods_count,
            a.goods_type,
            a.price as goods_price,
            a.name as goods_name,
            a.pic_id as goods_pic_id,
            isnull(a.cash_pay_rate,@default_cash_pay_rate) as cash_pay_rate,
            isnull(a.beans_pay_rate,@default_beans_pay_rate) as beans_pay_rate,
            @msg_leave_word,
            @msg_phone as msg_phone,
            @msg_realname as msg_realname,
            @msg_idcardno as msg_idcardno,
            @postage as postage,
            a.return_money_discount as pay_gold_coin,
            CONVERT(DECIMAL(18,2), isnull(b.return_money_rate,'0'))*a.price  as return_gold_coin,
            a.card_goods_id,
            a.card_goods_attr
            from rrl_goods a left join  rrl_fee_config b on a.fee_id=b.id
            where a.id=@goods_id";
            #endregion

            int effect = db.Execute(sql, new
            {
                order_id = order_id,
                goods_count = goods_count,
                default_cash_pay_rate = DEFAULT_CASH_PAY_RATE,
                default_beans_pay_rate = DEFAULT_BEANS_PAY_RATE,
                msg_leave_word = msg_leave_word,
                msg_phone = msg_phone,
                msg_realname = msg_realname,
                msg_idcardno = msg_idcardno,
                postage = postage,
                goods_id = goods_id
            });
            return effect;

        }

        /// <summary>
        /// 更新,不为null 就更新
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="order_id"></param>
        /// <param name="goods_id"></param>
        /// <param name="goods_count"></param>
        /// <param name="msg_leave_word"></param>
        /// <param name="msg_phone"></param>
        /// <param name="msg_realname"></param>
        /// <param name="msg_idcardno"></param>
        /// <returns></returns>
        public int OrderInfoGoods_Update(int uid, int order_id, int goods_id, int? goods_count, string msg_leave_word, string msg_phone, string msg_realname, string msg_idcardno, int? user_coupons_id)
        {
            SqlDataBase db = new SqlDataBase();

            #region update rrl_order_info_goods sql
            var sql = @"update [rrl_order_info_goods] set ";
            StringBuilder sb = new StringBuilder();
            var param = new DynamicParameters();
            if (goods_count != null)
            {
                sb.Append(",").Append("goods_count=@goods_count");
                param.Add("@goods_count", goods_count.Value);

                var postage = GetGoodsPostage(goods_id, goods_count.Value);
                sb.Append(",").Append("postage=@postage");
                param.Add("@postage", postage);
            }
            if (null != msg_leave_word)
            {
                sb.Append(",").Append("msg_leave_word=@msg_leave_word");
                param.Add("@msg_leave_word", msg_leave_word);
            }
            if (null != msg_phone)
            {
                sb.Append(",").Append("msg_phone=@msg_phone");
                param.Add("@msg_phone", msg_phone);
            }
            if (null != msg_realname)
            {
                sb.Append(",").Append("msg_realname=@msg_realname");
                param.Add("@msg_realname", msg_realname);
            }
            if (null != msg_idcardno)
            {
                sb.Append(",").Append("msg_idcardno=@msg_idcardno");
                param.Add("@msg_idcardno", msg_idcardno);
            }
            if (sb.Length == 0) return 1;
            sb.Remove(0, 1).Insert(0, sql);//去掉第一, 插入前面的update...  set
            sb.Append(" where  order_id=@order_id and goods_id=@goods_id and EXISTS(select 1 from rrl_order where id=@order_id and status=1 and is_paid=0 and isnull(is_paid_beans,0)=0 )");
            param.Add("@order_id", order_id);
            param.Add("@goods_id", goods_id);
            #endregion

            int effect = db.Execute(sb.ToString(), param);
            return effect;

        }

        /// <summary>
        /// 券应用到订单上
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="order_id"></param>
        /// <param name="user_coupons_id"></param>
        /// <returns></returns>
        public int UserCouponsApplyOrder(int uid, int order_id, int? user_coupons_id)
        {

            if (user_coupons_id == null)
                return 0;
            SqlDataBase db = new SqlDataBase();
            if (user_coupons_id == 0)
            {
                db.Execute($@"update rrl_coupons set pay_order_id=null,is_used=0 where  id in (select user_coupons_id from rrl_order where id=@order_id  and  status=1 and  user_coupons_id is not null)", new { order_id = order_id });
                db.Execute($@"update rrl_order  set user_coupons_id=null,user_coupons_countmoney=null where id=@order_id  and  status=1 and  user_coupons_id is not null", new { order_id = order_id });
                return 0;
            }

            decimal? all_price = db.ExecuteScalar<decimal?>(@"select (b.goods_count*b.goods_price) all_price from 
                rrl_order a inner join rrl_order_info_goods b on a.id=b.order_id 
                where a.id = @order_id and a.status = 1 and a.is_paid = 0 and a.is_paid_beans=0", new { order_id = order_id });
            //EXISTS(select 1 from rrl_order where id = @order_id and status = 1 and is_paid = 0 and isnull(is_paid_beans, 0) = 0)
            if (all_price == null)
            {
                return 1;
            }
            if (user_coupons_id > 0)//清空券选中
            {
                db.Execute($@"update rrl_coupons set pay_order_id=null,is_used=0 where  id in (select user_coupons_id from rrl_order where id=@order_id  and  status=1 and  user_coupons_id is not null)", new { order_id = order_id });
                db.Execute($@"update rrl_order  set user_coupons_id=null,user_coupons_countmoney=null where id=@order_id  and  status=1 and  user_coupons_id is not null", new { order_id = order_id });

            }

            int coupons_id_int = user_coupons_id.Value;

            string str_sql = "select count(*) from rrl_goods where ISNULL(dbo.rrl_goods.is_can_use_coupons,cast(1 as bit))=0 and id in (select goods_id from rrl_order_info_goods where order_id=@order_id)";
            int cnt = db.ExecuteScalar<int>(str_sql, new { order_id = order_id });
            if (cnt > 0)//购物卡类型不允许加券支付
            {
                return 0;
            }


            /*string  goods_type =  "317,171";//购物卡分类id
            string sql = $@"select count(*) from rrl_goods where goods_type in({goods_type}) and id in (select goods_id from rrl_order_info_goods where order_id=@order_id)";
            int cnt = db.ExecuteScalar<int>(sql, new { goods_type = goods_type, order_id =  order_id  });
            if (cnt > 0)//购物卡类型不允许加券支付
            {
                return 0;
            }*/

            string sql_cou = $@"select countmoney,isnull(leastconsume,0) as leastconsume from rrl_coupons c where  c.uid=@uid  and is_used=0 and is_paid=1  
  and  id=@coupons_id and(('1900-01-01' = c.starttime and '1900-01-01' = c.endtime) or(c.starttime < GETDATE() and c.endtime > GETDATE()) ) ";
            var cuntMM = db.Select<ApplyCouponsPay_countmoney>(sql_cou, new { coupons_id = coupons_id_int, uid = uid });
            if (cuntMM == null || cuntMM.Count == 0)
            {

                return 0;
            }
            else
            {

                var countmoney = cuntMM[0].countmoney;
                var leastconsume = cuntMM[0].leastconsume;
                if (all_price >= leastconsume)
                {
                    //用券
                    db.Execute($@"update rrl_coupons set pay_order_id=@order_id,is_used=1 where  id=@coupons_id ", new { order_id = order_id, coupons_id = coupons_id_int });
                    db.Execute($@"update rrl_order set user_coupons_id=@coupons_id,user_coupons_countmoney=@countmoney where  id=@order_id", new { coupons_id = coupons_id_int, countmoney = countmoney, order_id = order_id });
                }

            }
            return 1;
        }


        /// <summary>
        /// 核心支付接口
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="order_arr_str"></param>
        /// <param name="trans_type"></param>
        /// <param name="plate_to_return_money"></param>
        /// <param name="IP"></param>
        /// <param name="query_resault"></param>
        /// <returns></returns>
        public string ApplyPayV3(UserAuth user, string order_arr_str, int trans_type, string IP, out int query_resault)
        {

            string is_beans_pay = "1";

            string PayRequestString = "SUCCESS";
            ApplyPayV3_SumPay applyPayV3_SumPay = null;
            PayBody body = null;
            string recordId = Inner_ApplyPayV3(user, order_arr_str, trans_type, out is_beans_pay, out query_resault, out PayRequestString, out applyPayV3_SumPay, out body);
            if (query_resault != 0)
            {
                return PayRequestString;
            }
            //微信
            if (trans_type == 1)
            {
                #region 微信支付,扣豆和金币,并返回支付字符串
                var bussResult = ReduceBeansAndGoldIcon(user, applyPayV3_SumPay, order_arr_str, is_beans_pay);
                if (bussResult.status == 0)
                {
                    PayRequestString = GeneratorPay.BuildWxPay(recordId, body.order_code, body.money + "", body.order_code, IP);
                    query_resault = 0;
                    return PayRequestString;
                }
                else
                {
                    query_resault = bussResult.status;
                    return bussResult.message;
                }
                #endregion
            }
            //支付宝
            else if (trans_type == 2)
            {
                #region 支付宝,扣豆和金币,并返回支付字符串
                var bussResult = ReduceBeansAndGoldIcon(user, applyPayV3_SumPay, order_arr_str, is_beans_pay);
                if (bussResult.status == 0)
                {
                    PayRequestString = GeneratorPay.BuildAliPay(recordId, body.order_code, body.money + "", body.order_code);
                    query_resault = 0;
                    return PayRequestString;
                }
                else
                {
                    query_resault = bussResult.status;
                    return bussResult.message;
                }
                #endregion
            }
            //现金账户支付
            else if (trans_type == 3)
            {
                #region 现金账户支付,扣豆和金
                var bussResult = ReduceBeansAndGoldIcon(user, applyPayV3_SumPay, order_arr_str, is_beans_pay);
                if (bussResult.status == 0)
                {
                    var bussREX = ReduceXmoney(user, applyPayV3_SumPay, order_arr_str);
                    if (bussREX.status == 0)
                    {
                        query_resault = 0;
                        PayRequestString = "SUCCESS";
                    }
                    else
                    {
                        query_resault = bussResult.status;
                        return bussResult.message;
                    }
                }
                else
                {
                    query_resault = bussResult.status;
                    return bussResult.message;
                }
                #endregion
            }
            query_resault = 0;
            return PayRequestString;
        }


        private string Inner_ApplyPayV3(UserAuth user, string order_arr_str, int trans_type, out string is_beans_pay, out int query_resault, out string message, out ApplyPayV3_SumPay applyPayV3_SumPay, out PayBody body)
        {
            List<int> orderListInt = PublicAPI.StrToIntList(order_arr_str);
            query_resault = 0;
            message = "ok";
            body = null;
            SqlDataBase sqldb = new SqlDataBase();
            #region 没设置收货地址不给下单
            var areaCodeDs = sqldb.Select<string>($@"select area_code from rrl_order where id in ({order_arr_str})");
            foreach (var area_code in areaCodeDs)
            {
                if (string.IsNullOrWhiteSpace(area_code))
                {
                    query_resault = MessageCode.ERROR_ORDER_NO_ADDRESS;
                    message = "订单无收货地址";
                }
            }
            #endregion


            #region 获取订单商品的信息
            // convert(DECIMAL(18,2), (isnull(rrl_order_info_goods.cash_pay_rate,{DEFAULT_CASH_PAY_RATE}) * (rrl_order_info_goods.goods_price*rrl_order_info_goods.goods_count+isnull(rrl_order_info_goods.postage,0)-isnull(rrl_order.user_coupons_countmoney,0))/100)) as need_pay_money,
            //convert(DECIMAL(18, 2), (isnull(rrl_order_info_goods.beans_pay_rate,{ DEFAULT_BEANS_PAY_RATE}) *(rrl_order_info_goods.goods_price * rrl_order_info_goods.goods_count + isnull(rrl_order_info_goods.postage, 0) - isnull(rrl_order.user_coupons_countmoney, 0)))) as need_pay_beans
            var sql_sum = $@"SELECT
	         sum(b.goods_price*b.goods_count) as sum_money,
	         sum(isnull(b.pay_gold_coin,0)*b.goods_count)  as sum_pay_gold_coin,
	         isnull(sum(b.postage),0) AS sum_postage,
             sum(convert(DECIMAL(18,2), (isnull(b.cash_pay_rate,{DEFAULT_CASH_PAY_RATE}) * ((b.goods_price*b.goods_count+isnull(b.postage,0))-isnull(a.user_coupons_countmoney,0))/100))) as sum_need_pay_money,
             sum(convert(DECIMAL(18,2),(isnull(b.beans_pay_rate,{DEFAULT_BEANS_PAY_RATE}) * ((b.goods_price*b.goods_count+isnull(b.postage,0))-isnull(a.user_coupons_countmoney,0))))) as sum_need_pay_beans,
             sum(isnull(a.user_coupons_countmoney,0)) as sum_user_coupons_countmoney,
             max(a.ordercode) as max_ordercode,
             max(isnull(CONVERT(varchar,a.is_beans_pay),'1')) as is_beans_pay
        FROM
	        rrl_order a
        inner join rrl_order_info_goods b on b.order_id=a.id
        where a.status=1 and a.buyer_id=@uid
        and a.id in ({order_arr_str})";
            applyPayV3_SumPay = sqldb.Single<ApplyPayV3_SumPay>(sql_sum, new { uid = user.id });
            is_beans_pay = applyPayV3_SumPay.is_beans_pay;



            if (applyPayV3_SumPay == null)
            {
                query_resault = 99;
                message = "订单不存在或者已被处理!";
                return "FAIL";
            }
            if ("0".Equals(applyPayV3_SumPay.is_beans_pay))//现金支付
            {
                applyPayV3_SumPay.sum_need_pay_beans = 0;
                applyPayV3_SumPay.sum_need_pay_money = applyPayV3_SumPay.sum_money + applyPayV3_SumPay.sum_postage - applyPayV3_SumPay.sum_user_coupons_countmoney;
            }
            else//豆支付+钱
            {
                is_beans_pay = "1";
            }
            #endregion

            decimal PLATFORM_HOLD_H_MONEY = userManager.GetUserHoldMoney(user);//门槛

            #region 产品分类判断&0门槛
            var sql_goods_type_sql = $@"select b.goods_type from rrl_order a
        inner join rrl_order_info_goods b on b.order_id=a.id
        where a.status=1 and a.buyer_id=@uid and a.id in ({order_arr_str})";
            var goods_type_list = sqldb.Select<dynamic>(sql_goods_type_sql, new { uid = user.id });
            bool is_zero_hold_good = false;//是否都是0门槛商品
            Dictionary<int, string> dic = new Dictionary<int, string>();
            foreach (var goods in goods_type_list)
            {
                if (!dic.ContainsKey(goods.goods_type))
                {
                    dic.Add(goods.goods_type, "1");
                }
            }
            if (dic.Count == 1 && dic.ContainsKey(ZERO_HOLD_GOODS_TYPE))
            {
                PLATFORM_HOLD_H_MONEY = 0;
                is_zero_hold_good = true;
            }

            #endregion

            #region 新用户限制消费
            decimal news_user_limit_beans = 0m; // 新用户每天限制消费的金豆数
            decimal.TryParse(configMgr.GetConfigValue("NewUser_MaxBeanNum_Daily"), out news_user_limit_beans);
            if (userManager.IfNewUser(user) && is_beans_pay == "1")
            {
                // 如果是新用户，获取新用户当天下单总共已经抵扣的金豆数
                decimal dBeanTotalDaily = GetOrderGoldBeansTodayByUser(user.id);
                if (applyPayV3_SumPay.sum_need_pay_beans + dBeanTotalDaily > news_user_limit_beans && !is_zero_hold_good)
                {
                    query_resault = 99;
                    message = $@"新用户每天限制最多只能消费{news_user_limit_beans}金豆";
                    return "FAIL";
                }
            }
            #endregion


            #region 门槛无限高
            if (PLATFORM_HOLD_H_MONEY >= 10000 * RRLConfig.RMB_To_GoldBean_Rate && is_beans_pay == "1")
            {
                query_resault = 99;
                message = $@"您只能现金购物或者在0门槛金豆区购物";
                return "FAIL";
            }

            #endregion


            #region 限制门槛
            if (is_beans_pay == "1")
            {
                if ((decimal)user.h_money - PLATFORM_HOLD_H_MONEY < 0)
                {
                    #region 调试用日志
                    try
                    {
                        string strRemark = $@"金豆底分--订单ID：【{order_arr_str}】；用户金豆：【{user.h_money}】；用户金豆转换：【{(decimal)user.h_money}】；注册时间：【{user.addtime.ToLongTimeString()}】";
                        LogOrderPay(user.id, strRemark);
                    }
                    catch
                    {
                        string strRemark = $@"金豆底分-- 订单ID：【{order_arr_str}】。写日志异常！";
                        LogOrderPay(user.id, strRemark);
                    }
                    #endregion


                    query_resault = 99;
                    message = $@"金豆必须保留{(int)PLATFORM_HOLD_H_MONEY}底分!";
                    return "FAIL";
                }
            }
            #endregion

            #region 判断金币/金豆/帐号/不足,直接返回
            if (applyPayV3_SumPay.sum_pay_gold_coin < 0)
            {
                query_resault = 99;
                message = "金币不足,交易失败!";
                return "FAIL";
            }

            if (applyPayV3_SumPay.sum_need_pay_beans < 0)
            {
                query_resault = 99;
                message = "需要支付的金豆不能小于等于0!";
                return "FAIL";
            }

            if (applyPayV3_SumPay.sum_need_pay_money <= 0)
            {
                #region 调试用日志
                try
                {
                    var orderStatus = sqldb.ExecuteScalar<int>(@"select status from rrl_order(nolock) where (id = @orderId)", new { orderId = order_arr_str });
                    var orderInfoGoodsCount = sqldb.ExecuteScalar<int>(@"select count(0) from rrl_order_info_goods(nolock) where (order_id = @orderId)", new { orderId = order_arr_str });
                    string strRemark = $@"订单ID：【{order_arr_str}】；需要支付的现金：【{applyPayV3_SumPay.sum_need_pay_money}】；订单状态：【{orderStatus}】；订单明细表行数：【{orderInfoGoodsCount}】";
                    LogOrderPay(user.id, strRemark);
                }
                catch
                {
                    string strRemark = $@"订单ID：【{order_arr_str}】。写日志异常！";
                    LogOrderPay(user.id, strRemark);
                }
                #endregion


                query_resault = 99;
                message = "需要支付的金额不能小于等于0!";
                return "FAIL";
            }



            if ((decimal)user.plate_to_return_money < applyPayV3_SumPay.sum_pay_gold_coin)
            {
                query_resault = 99;
                message = "金币不足,交易失败!";
                return "FAIL";
            }

            if ((decimal)user.h_money < applyPayV3_SumPay.sum_need_pay_beans)
            {
                #region 调试用日志
                try
                {
                    var orderStatus = sqldb.ExecuteScalar<int>(@"select status from rrl_order(nolock) where (id = @orderId)", new { orderId = order_arr_str });
                    var orderInfoGoodsCount = sqldb.ExecuteScalar<int>(@"select count(0) from rrl_order_info_goods(nolock) where (order_id = @orderId)", new { orderId = order_arr_str });
                    string strRemark = $@"金豆余额检查--订单ID：【{order_arr_str}】；需要支付的金豆：【{applyPayV3_SumPay.sum_need_pay_beans}】；订单状态：【{orderStatus}】；订单明细表行数：【{orderInfoGoodsCount}】";
                    LogOrderPay(user.id, strRemark);
                }
                catch
                {
                    string strRemark = $@"金豆余额检查--订单ID：【{order_arr_str}】。写日志异常！";
                    LogOrderPay(user.id, strRemark);
                }
                #endregion

                query_resault = 99;
                message = "金豆不足,交易失败!";
                return "FAIL";
            }
            if (trans_type == 3 && (decimal)user.x_money < applyPayV3_SumPay.sum_need_pay_money)//3,退款账户余额支付
            {
                query_resault = 99;
                message = "现金账户余额不足,交易失败!";
                return "FAIL";
            }
            #endregion

            #region 判断购物券抵扣金额是否够
            var sql_yh = $@"select count(*) as cnt from (
                            select isnull(a.user_coupons_countmoney,0) as user_countmoney,b.goodsid,isnull(b.leastconsume,0) as leastconsume,convert(decimal(18,2),c.goods_count*c.goods_price) as  total_price
                            from rrl_order a inner join rrl_coupons b on a.user_coupons_id=b.id
                            inner join rrl_order_info_goods c on c.order_id=a.id
                            where a.id in({order_arr_str}) and isnull(a.user_coupons_id,0)>0 ) xx
                            where (goodsid=0 and total_price<leastconsume) ";
            int cnt_coupons = sqldb.ExecuteScalar<int>(sql_yh);
            if (cnt_coupons != 0)
            {
                query_resault = 99;
                message = "券抵扣金额还不够,请多添商品数量!";
                return "FAIL";
            }
            #endregion


            int effect_c = sqldb.Execute($@"UPDATE rrl_order SET is_beans_pay={is_beans_pay}, trans_type=@trans_type,combine_code=@combine_code WHERE is_paid = 0 AND id IN ({order_arr_str}) AND status = 1"
                , new { trans_type = trans_type, combine_code = applyPayV3_SumPay.max_ordercode });//修改交易类型
            if (effect_c < 1)
            {
                query_resault = 99;
                message = "订单不存在或者已被处理!";
                return "FAIL";
            }

            body = new PayBody();
            body.uid = user.id;
            body.order_code = applyPayV3_SumPay.max_ordercode + "";
            body.order_list = order_arr_str;
            body.bean = (double)applyPayV3_SumPay.sum_need_pay_beans;
            body.money = (double)applyPayV3_SumPay.sum_need_pay_money;
            body.postage = (double)applyPayV3_SumPay.sum_postage;
            body.discount = (double)applyPayV3_SumPay.sum_pay_gold_coin;
            string recordId = PayBody.EncryptBody(body);
            return recordId;
        }




        /// <summary>
        /// 申请微信JS支付
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="order_arr_str">订单列表数组</param>
        /// <param name="discount">可抵扣金额</param>
        /// <param name="r_money">用户红包账户余额</param>
        /// <param name="query_resault">查询结果</param>
        /// <returns>支付申请字符串</returns>
        public JsPayConfigObject ApplyWxJsPayV3(UserAuth user, string order_arr_str, string IP, string openid, out int query_resault, out string errer_message, string sperador = null)
        {
            string is_beans_pay = "1";
            string PayRequestString = "SUCCESS";
            errer_message = "ok";
            ApplyPayV3_SumPay applyPayV3_SumPay = null;
            PayBody body = null;
            string recordId = Inner_ApplyPayV3(user, order_arr_str, 1, out is_beans_pay, out query_resault, out PayRequestString, out applyPayV3_SumPay, out body);
            //扣豆

            if (query_resault != 0)
            {
                errer_message = PayRequestString;
                return null;
            }
            else
            {
                var bussResult = ReduceBeansAndGoldIcon(user, applyPayV3_SumPay, order_arr_str, is_beans_pay);
                if (bussResult.status != 0)
                {
                    query_resault = bussResult.status;
                    errer_message = bussResult.message;
                    return null;
                }
                var prepay_id = GeneratorPay.BuildWxJsPrepayID(recordId, applyPayV3_SumPay.max_ordercode + "", applyPayV3_SumPay.sum_need_pay_money + "", applyPayV3_SumPay.max_ordercode + "", IP, openid);
                return GetPayConfig(prepay_id);
            }
            //微信JS支付 todo 获取prepay_id

        }


        /// <summary>
        /// 减豆和金币
        /// </summary>
        /// <param name="user"></param>
        /// <param name="sumPay"></param>
        /// <param name="order_arr_str"></param>
        /// <param name="trans_type"></param>
        /// <param name="is_beans_pay"></param>
        private BussResult ReduceBeansAndGoldIcon(UserAuth user, ApplyPayV3_SumPay sumPay, string order_arr_str, String is_beans_pay)
        {
            List<int> orderListInt = PublicAPI.StrToIntList(order_arr_str);
            if (!"1".Equals(is_beans_pay))
            {
                return new BussResult() { status = 0, message = "纯现金支付,不用扣豆" };
            }
            SqlDataBase db = new SqlDataBase();
            DbConnection conn = null;
            DbTransaction tran = null;
            try
            {
                conn = db.CreateConnection();
                conn.Open();
                tran = conn.BeginTransaction();
                int effect_paid_beans = db.ExecuteTran(conn, $@"update rrl_order set is_paid_beans=1 where id in({order_arr_str}) and isnull(is_paid_beans,0)=0", null, tran);
                if (effect_paid_beans == orderListInt.Count)
                {
                    int effect = db.ExecuteTran(conn, @"update rrl_user  set h_money=h_money-@pay_beans
                                                ,plate_to_return_money=plate_to_return_money-@pay_coin 
                                                where id=@uid and h_money>=@pay_beans  and plate_to_return_money>=@pay_coin",
                                                    new { pay_beans = sumPay.sum_need_pay_beans, uid = user.id, pay_coin = sumPay.sum_pay_gold_coin }, tran);
                    if (effect > 0)
                    {
                        int effmc = db.ExecuteTran(conn, @"INSERT INTO rrl_user_money_record (uid,order_id,money,type,remark) VALUES(@uid,@order_id,@money,13,'金豆购物消费')", new { uid = user.id, order_id = orderListInt.Max(), money = -sumPay.sum_need_pay_beans }, tran);
                        if (effmc > 0)
                        {
                            tran.Commit();
                        }
                        else
                        {
                            tran.Rollback();
                            return new BussResult() { status = 99, message = "插入消费记录表失败!" };
                        }
                    }
                    else
                    {
                        tran.Rollback();
                        return new BussResult() { status = 99, message = "金豆或金币不足" };
                    }
                }
                else
                {
                    tran.Rollback();
                }
            }
            catch (Exception ex)
            {
                if (tran != null)
                    tran.Rollback();
                return new BussResult() { status = 99, message = ex.Message };
            }
            finally
            {
                if (conn != null)
                    conn.Close();
            }

            return new BussResult() { status = 0, message = "ok" };

        }


        /// <summary>
        /// 减现金帐号
        /// </summary>
        /// <param name="user"></param>
        /// <param name="sumPay"></param>
        /// <param name="order_arr_str"></param>
        /// <param name="trans_type"></param>
        /// <param name="is_beans_pay"></param>
        private BussResult ReduceXmoney(UserAuth user, ApplyPayV3_SumPay sumPay, string order_arr_str)
        {
            List<int> orderListInt = PublicAPI.StrToIntList(order_arr_str);

            SqlDataBase db = new SqlDataBase();
            DbConnection conn = null;
            DbTransaction tran = null;
            try
            {
                conn = db.CreateConnection();
                conn.Open();
                tran = conn.BeginTransaction();
                int effect = db.ExecuteTran(conn, @"update rrl_user  set x_money=x_money-@pay_money,pay_order_total_count=isnull(pay_order_total_count,0)+1
                                                where id=@uid  and x_money>=@pay_money",
                                                new { uid = user.id, pay_money = sumPay.sum_need_pay_money }, tran);
                if (effect > 0)
                {
                    int eff_order = db.ExecuteTran(conn, $@"update rrl_order set is_paid=1,status=2
                                                where id in ({order_arr_str})  and is_paid=0 and status=1",
                        null, tran);
                    if (eff_order != orderListInt.Count)
                    {
                        tran.Rollback();
                        return new BussResult() { status = 99, message = "更新订单状态失败!" };
                    }
                    int effmc = db.ExecuteTran(conn, @"INSERT INTO rrl_user_money_record (uid,order_id,money,type,remark) VALUES(@uid,@order_id,@money,12,'现金账户消费')",
                        new { uid = user.id, order_id = orderListInt.Max(), money = -sumPay.sum_need_pay_money }, tran);
                    if (effmc > 0)
                    {
                        tran.Commit();
                    }
                    else
                    {
                        tran.Rollback();
                        return new BussResult() { status = 99, message = "插入消费记录表失败!" };
                    }
                }
                else
                {
                    tran.Rollback();
                    return new BussResult() { status = 99, message = "现金帐号不足" };
                }
            }
            catch (Exception ex)
            {
                if (tran != null)
                    tran.Rollback();
                return new BussResult() { status = 99, message = ex.Message };
            }
            finally
            {
                if (conn != null)
                    conn.Close();
            }

            return new BussResult() { status = 0, message = "ok" };

        }



        private void SendWxMessage(dynamic s)// UserAuth user,PayBody body)
        {
            UserAuth user = s.user;
            PayBody body = s.body;
            var db = new RRLDB();
            try
            {

                SendSMS(user);
                if (!string.IsNullOrWhiteSpace(user.wx_mp_open_id))
                {

                    var ds = db.ExeQuery($@"SELECT rrl_goods.name,rrl_order_info_goods.goods_price,rrl_order_info_goods.goods_count FROM rrl_order_info_goods LEFT JOIN rrl_goods ON rrl_order_info_goods.goods_id = rrl_goods.id WHERE rrl_order_info_goods.order_id IN ({body.order_list})");
                    var goodsName = ds.Tables[0].Rows[0]["name"];
                    if (ds.Tables[0].Rows.Count > 1)
                    {
                        goodsName = $@"{goodsName}等";
                    }

                    var messageobj = new
                    {
                        touser = user.wx_mp_open_id,
                        template_id = "eMeVsZWC0B1i0GU9mbhFf80iIrmf49F0Tm81Mxyl04U",
                        url = "https://e-shop.rrlsz.com.cn/#/order/2",
                        data = new
                        {
                            orderMoneySum = new
                            {
                                value = (object)body.money,
                                color = "#CC0000"
                            },
                            orderProductName = new
                            {
                                value = goodsName,
                                color = "#0000CC"
                            }
                        }
                    };

                    var wxMpMessage = new WxMpMessage();
                    wxMpMessage.SendTemplateMessage(messageobj);
                }
            }
            catch
            {
                var sdf = "";
            }
            finally
            {
                db.Close();
            }
        }

        private void SendSMS(UserAuth user)
        {
            var mobile = user.username;
            var sms = new AliSMS();
            sms.SendSMS(mobile, SMSTemplate.ORDER_PAYMENT_SUCCESS_SMS, new OrderPaymentSuccessSMS());
        }

        /// <summary>
        /// 处理完成交易的订单
        /// </summary>
        /// <param name="body"></param>
        /// <param name="Transaction_id"></param>
        /// <param name="trans_type"></param>
        /// <returns></returns>
        public int DealWithPayCompleteTrade(PayBody body, string Transaction_id, string notify_id, string trans_type, string three_pay_type, DateTime three_completed_trans_time, string order_code = "")
        {
            List<int> listInt = PublicAPI.StrToIntList(body.order_list);
            SqlDataBase db = new SqlDataBase();
            List<int?> list = db.ExecuteStoredProcedure<int?>("sp_V3_order_pay_complete", new
            {
                order_list = body.order_list,
                trans_id = Transaction_id,
                notify_id = notify_id,
                uid = body.uid,
                money = -body.money,
                type = 19,
                remark = string.Format("现金消费,交易类型={0},订单列表={1}", trans_type, body.order_list),
                three_pay_type = three_pay_type,
                three_completed_trans_time = @three_completed_trans_time
            });

            UserAuth user = new UserAuth(body.uid);
            ThreadPool.QueueUserWorkItem(new WaitCallback(SendWxMessage), new { user = user, body = body });
            //更新订单金豆支付和现金支付实际金额，更新推荐人安全期
            RRLDB rrldb = new RRLDB();
            int res = rrldb.ExeCMD("update rrl_order set pay_bean=@pay_bean,pay_money=@pay_money where ordercode=@order_code",
                new SqlParameter("pay_bean", body.bean),
                new SqlParameter("pay_money", body.money),
                new SqlParameter("order_code", order_code));
            DataSet ds = rrldb.ExeQuery("select spreader_uid,order_type,buyer_id from rrl_order where ordercode=@order_code",
                new SqlParameter("order_code", order_code));
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                int spreader_uid = int.Parse(ds.Tables[0].Rows[0]["spreader_uid"].ToString());
                int order_type = int.Parse(ds.Tables[0].Rows[0]["order_type"].ToString());
                int buyer_id = int.Parse(ds.Tables[0].Rows[0]["buyer_id"].ToString());
                if (order_type == 970 && spreader_uid != 0)
                {
                    DateTime spreader_expire = DateTime.Now.AddDays(30);
                    res = rrldb.ExeCMD("update rrl_user set spreader_expire=@spreader_expire",
                        new SqlParameter("spreader_expire", spreader_expire.ToString("yyyy-MM-dd HH:mm:ss")));
                }
            }
            rrldb.Close();
            return MessageCode.SUCCESS;
        }

        /// <summary>
        /// 二手转卖回调
        /// </summary>
        /// <param name="order_code"></param>
        /// <param name="status">订单状态 
        /// 0=发布转卖成功(订单重新上架) ,
        /// 1=商品转卖支付成功,
        /// 2=转卖商品下架 ,
        /// 99=转卖发货完成(客户确认订单,或7天后自动确认订单)</param>
        /// <param name="sell_token"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        public int ReSellCallBack(string order_code, string status, string sell_token, out string errorMessage)
        {
            //status  0-> 对应数据库的sh_sell_status  =1
            //status  1-> 对应数据库的sh_sell_status  =2
            //status  2-> 对应数据库的sh_sell_status  =0
            //status  99-> 对应数据库的sh_sell_status  =99
            errorMessage = "";
            SqlDataBase db = new SqlDataBase();
            string sh_sell_status = "1";
            if ("0".Equals(status))
            {
                sh_sell_status = "1";

            }
            else if ("1".Equals(status))
            {
                sh_sell_status = "2";

            }
            else if ("2".Equals(status))
            {
                sh_sell_status = "0";

            }
            else if ("99".Equals(status))
            {
                sh_sell_status = "99";
            }
            else
            {
                sh_sell_status = "1";
            }
            int effect = db.Execute("update rrl_order_info_goods set sh_sell_status=@sh_sell_status where sh_sell_token=@sh_sell_token", new { sh_sell_status = sh_sell_status, sh_sell_token = sell_token });
            if (effect > 0)
            {
                if (sh_sell_status == "99")
                {
                    TradeManager tm = new TradeManager();//99的要确认订单
                    var oneD = db.Single<dynamic>($@"select b.buyer_id as uid,b.id as order_id from rrl_order_info_goods a inner join rrl_order b on a.order_id=b.id where sh_sell_token=@sh_sell_token", new { sh_sell_token = sell_token });
                    if (oneD != null)
                    {
                        tm.OrderSettlement(oneD.uid, oneD.order_id);
                        //db.Execute("update rrl_order_info_goods set sh_sell_status=@sh_sell_status where sh_sell_token=@sh_sell_token", new { sh_sell_status = "99", sh_sell_token = sell_token });
                    }
                }
                return 0;
            }
            else
            {
                errorMessage = "更新订单商品状态失败!";
                return 99;

            }
        }

        /// <summary>
        /// 获取指定用户当天有效的订单总共使用的金豆数量 
        /// </summary>
        /// <param name="uid">用户ID</param>
        /// <returns></returns>
        public decimal GetOrderGoldBeansTodayByUser(int uid)
        {
            DateTime dt = DateTime.Now.Date;
            string strSql = $@"select isnull(sum(convert(decimal(18,2), (isnull(g.beans_pay_rate, {DEFAULT_BEANS_PAY_RATE}) * ((g.goods_price * g.goods_count + isnull(g.postage,0)) - isnull(ord.user_coupons_countmoney,0))))), 0)
                                 from rrl_order(nolock) ord
                                inner join rrl_order_info_goods(nolock) g
                                   on g.order_id = ord.id
                                where (ord.buyer_id = @buyer_id)
                                  and (ord.[status] in (2, 3, 4))
                                  and (is_paid_beans = 1)
                                  and (is_paid = 1)
                                  and (ord.addtime >= convert(date ,getdate()))
                                  and (ord.addtime < convert(date ,dateadd(dd, 1 ,getdate())))
                                  and ((g.goods_type < {ZERO_HOLD_GOODS_TYPE}) or (g.goods_type > {ZERO_HOLD_GOODS_TYPE}))";

            return new SqlDataBase().ExecuteScalar<decimal>(strSql, new { buyer_id = uid });
        }

        // lcl 2018-07-28  Insert
        public dynamic SecondHandBuyCardCalculate(decimal goldBeans)
        {
            decimal dCard100_GoldBean = RRLConfig.Card100_GoldBean; // 100元卡对应的金豆数
            decimal dCard10_GoldBean = RRLConfig.Card10_GoldBean; // 10元卡对应的金豆数
            decimal dGoldBean_To_RMB_Rate = RRLConfig.GoldBean_To_RMB_Rate; // 金豆兑换人民币的比率
            decimal dPlatform_Fee_Rate = RRLConfig.Platform_Fee_Rate; // 平台现金手续费比率
            decimal dActualPayGoldBeans = 0m; // 购卡实际需要支付的金豆数
            decimal dHoldMoney = GetSecondHandHoldMoney(); // 购卡门槛

            dynamic result = new ExpandoObject();
            result.isCanBuy = 0; // 是否能买卡（1：可以；0：不可以）
            result.holdMoney = Convert.ToInt32(dHoldMoney); // 购卡门槛
            result.canBuyQty_Card100 = 0; // 能购买的100元卡的数量
            result.canBuyQty_Card10 = 0; // 能购买的10元卡的数量
            result.actualPayGoldBeans = 0; // 购卡实际需要支付的金豆数
            result.actualGetCash = 0; // 用户实际可以获得的现金

            if (Math.Truncate((goldBeans - dHoldMoney) / dCard10_GoldBean) <= 0)
            {
                // 如果减去门槛后的金豆余额连最低面额的卡都不能购买，则认为是金豆不足
                return result;
            }

            // 能用于购买卡的金豆额度
            decimal dCanBuyCardGoldBeans = goldBeans - dHoldMoney;
            // 优先购买大面额的，100元的卡。计算能够购买100元卡的数量
            result.canBuyQty_Card100 = Convert.ToInt32(Math.Truncate(dCanBuyCardGoldBeans / dCard100_GoldBean));
            if (result.canBuyQty_Card100 > 0)
            {
                dActualPayGoldBeans += dCard100_GoldBean * result.canBuyQty_Card100;
            }
            // 计算余额
            dCanBuyCardGoldBeans = dCanBuyCardGoldBeans % dCard100_GoldBean;
            if (dCanBuyCardGoldBeans >= dCard10_GoldBean)
            {
                // 如果余额能购买10元卡，则计算购卡数量
                result.canBuyQty_Card10 = Convert.ToInt32(Math.Truncate(dCanBuyCardGoldBeans / dCard10_GoldBean));
                // 累加实际需要支付金豆
                dActualPayGoldBeans += dCard10_GoldBean * result.canBuyQty_Card10;
            }

            result.actualPayGoldBeans = dActualPayGoldBeans;
            result.actualGetCash = dActualPayGoldBeans * dGoldBean_To_RMB_Rate * (1 - dPlatform_Fee_Rate);
            result.actualGetCash = Math.Round(result.actualGetCash, 2, MidpointRounding.AwayFromZero);

            // 设置为金豆额度可以购买卡
            result.isCanBuy = 1;

            return result;
        }

        /// <summary>
        /// 写与二手平台订单交易的日志
        /// </summary>
        /// <param name="secondHandOrderId">在SDD平台发起的与二手平台交易的ID</param>
        /// <param name="remark">日志内容</param>
        public void WriteSecondHandOrderLog(string secondHandOrderId, string remark)
        {
            string strSql = @"insert into second_hand_order_log (second_hand_order_id ,remark ,addtime) values (@second_hand_order_id ,@remark ,getdate())";
            new SqlDataBase().Execute(strSql, new { second_hand_order_id = secondHandOrderId, remark = remark });
        }

        /// <summary>
        /// 生成与二手平台的交易订单
        /// </summary>
        /// <param name="secondHandOrderId">在SDD平台发起的与二手平台交易的ID</param>
        /// <param name="uid">用户ID</param>
        /// <param name="goodsId">商品ID</param>
        /// <param name="tranMoney">交易金额</param>
        /// <param name="payAccount">收款账号</param>
        /// <param name="name">真实姓名</param>
        /// <returns></returns>
        public int CreateSecondHandOrder(string secondHandOrderId, int uid, int goodsId, decimal tranMoney, string payAccount, string name)
        {
            string strSql = $@"insert into second_hand_order 
                                      (id ,uid ,goods_id ,tran_money ,pay_account ,real_name ,addtime)
                               values (@id ,@uid ,@goods_id ,@tran_money ,@pay_account ,@real_name ,getdate())";
            return new SqlDataBase().Execute(strSql, new { id = secondHandOrderId, uid = uid, goods_id = goodsId, tran_money = tranMoney, pay_account = payAccount, real_name = name });
        }

        /// <summary>
        /// “购物换钱”支付金豆
        /// </summary>
        /// <param name="secondHandOrderId">在SDD平台发起的与二手平台交易的ID</param>
        /// <param name="uid">用户ID</param>
        /// <param name="orderId">SDD平台上的订单ID</param>
        /// <param name="secondHandTranId">二手平台的交易ID</param>
        /// <param name="shSellToken">转到二手上卖的token</param>
        /// <param name="payGoldBeanTotal">需要支付的金豆总额</param>
        /// <param name="platformFeeMoney">平台需要收取的手续费总金额</param>
        /// <returns></returns>
        public dynamic SecondHandPay(string secondHandOrderId, int uid, int orderId, string secondHandTranId, string shSellToken, decimal payGoldBeanTotal, decimal platformFeeMoney)
        {
            dynamic result = new ExpandoObject();
            decimal dHoldMoney = GetSecondHandHoldMoney(); // 购卡门槛
            SqlDataBase sqlDB = new SqlDataBase();

            string strSql = $@"update rrl_user 
                                 set h_money = h_money - @pay_h_money
                                where (id = @uid) and (h_money - @hold_money - @pay_h_money >= 0) 
                                  and (isnull(is_locked_login, '0') = '0')
                                  and (isnull(is_locked_trade, '0') = '0')";
            int intExecResult = sqlDB.Execute(strSql, new { pay_h_money = payGoldBeanTotal, uid = uid, hold_money = dHoldMoney });
            if (intExecResult < 1)
            {
                result.status = 99;
                result.message = "可用于支付的金豆余额不足，或者账号被锁定";
                return result;
            }

            // 写资金流水记录
            sqlDB.Execute($@"insert into rrl_user_money_record (uid ,order_id ,money ,type ,remark) values ({uid} ,{orderId} ,-{payGoldBeanTotal} ,307 ,N'金豆购物换钱消费')");
            // 更新订单主表，状态设置为：3 -- 待评价
            strSql = $@"update rrl_order 
                           set status = 3 ,is_paid = 1 ,is_settled = 1 ,is_beans_pay = 1 ,is_paid_beans = 1
                              ,checktime = getdate() ,message = N'转卖金额扣除{platformFeeMoney.ToString("0.##")}元'
                         where (id = @order_id)";
            sqlDB.Execute(strSql, new { order_id = orderId });
            // 更新订单商品表(sh_sell_status=99表示已经成功地转到二手)
            strSql = $@"update rrl_order_info_goods
                           set sh_sell_token = @sh_sell_token ,sh_sell_status = 99
                         where (order_id = @order_id)";
            sqlDB.Execute(strSql, new { sh_sell_token = shSellToken, order_id = orderId });
            // 更新二手订单表
            strSql = $@"update second_hand_order
                           set second_hand_tran_id = @second_hand_tran_id ,sdd_order_id = @order_id ,sdd_account_pay_time = getdate()
                         where (id = @id)";
            sqlDB.Execute(strSql, new { second_hand_tran_id = secondHandTranId, order_id = orderId, id = secondHandOrderId });

            result.status = 0;
            result.message = "";
            return result;
        }

        /// <summary>
        /// 二手商品确认成功后，更新SDD中的二手订单表
        /// </summary>
        /// <param name="secondHandOrderId">在SDD平台发起的与二手平台交易的ID</param>
        /// <returns></returns>
        public int SecondHandConfirm(string secondHandOrderId)
        {
            string strSql = @"update second_hand_order set second_hand_confirm_time = getdate() where (id = @id)";
            return new SqlDataBase().Execute(strSql, new { id = secondHandOrderId });
        }

        /// <summary>
        /// 获取“购物换钱”门槛
        /// </summary>
        /// <returns></returns>
        private decimal GetSecondHandHoldMoney()
        {
            decimal dHoldMoney = 0;
            decimal.TryParse(configMgr.GetConfigValue("SecondHand_Hold_Money"), out dHoldMoney);
            if (dHoldMoney == 0)
            {
                dHoldMoney = 11000;
            }

            return dHoldMoney;
        }

        /// <summary>
        /// 获取订单编号
        /// </summary>
        /// <param name="orderId">订单ID</param>
        /// <returns></returns>
        public string GetOrderCode(int orderId)
        {
            string strSql = $@"select ordercode from rrl_order(nolock) where (id = @order_id)";
            return new SqlDataBase().ExecuteScalar<string>(strSql, new { order_id = orderId });
        }

        /// <summary>
        /// 检查用户和支付账号是否为一一对应的关系
        /// </summary>
        /// <param name="uid">用户ID</param>
        /// <param name="payAccount">支付账号</param>
        /// <returns>true:是；false：否</returns>
        public bool CheckPayAccount(int uid, string payAccount)
        {
            string strSql = $@"select count(0) from (
                                   select uid ,pay_account from second_hand_order(nolock) where (uid = @uid)
                                    union
                                   select uid ,pay_account from second_hand_order(nolock) where (pay_account = @pay_account)
                                    union
                                   select @uid ,@pay_account
                               ) as t";
            var recordCount = new SqlDataBase().ExecuteScalar<int>(strSql, new { uid = uid, pay_account = payAccount });

            if (recordCount == 1)
            {
                // 如果只有1条记录，说明是一一对应
                return true;
            }

            return false;
        }

        public void LogOrderPay(int uid, string remark)
        {
            string strSql = $@"insert into rrl_order_pay_log 
                                          (uid ,remark)
                                   values (@uid ,@remark)";
            new SqlDataBase().Execute(strSql, new { uid = uid, remark = remark });
        }
    }
}
