using RRL.Core.Models;
using RRL.Core.Models.WxJSAPI;
using RRL.Core.Pay;
using RRL.Core.Pay.WxPay;
using RRL.Core.Utility;
using RRL.DB;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace RRL.Core.Manager
{
    public partial class TradeManager
    {
        /// <summary>
        /// 获取购物车列表
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <returns>数据集</returns>
        public DataSet GetShopCartList(int uid)
        {
            //SHOP_CART_LIST_VIEW
            RRLDB db = new RRLDB();
            DataSet ds = db.ExeQuery(@"SELECT * FROM SHOP_CART_LIST_VIEW WHERE uid=@uid",
                new SqlParameter("@uid", SqlDbType.Int, 4) { Value = uid });
            db.Close();
            return ds;
        }

        /// <summary>
        /// 向购物车中添加或减少商品
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="goods_id">商品id</param>
        /// <param name="goods_count">商品数量</param>
        /// <returns>添加结果</returns>
        public int AddGoodsToCart(int uid, int goods_id, int goods_count)
        {
            RRLDB db = new RRLDB();
            int res = db.ExecSP(@"sp_CART_GOODS_ADD",
                new SqlParameter("@uid", SqlDbType.Int, 4) { Value = uid },
                new SqlParameter("@goods_id", SqlDbType.Int, 4) { Value = goods_id },
                new SqlParameter("@goods_count", SqlDbType.Int, 4) { Value = goods_count });
            int one = Convert.ToInt32(db.sm.Parameters["@return"].Value);
            db.Close();
            if (one == -2)
            {
                return MessageCode.ERROR_MORE_ONE_YUAN_GOODS_IN_CART;
            }
            if (res < 0)
                return MessageCode.ERROR_EXECUTE_SQL;
            if (res == 0)
                return MessageCode.ERROR_UNKONWN;
            return MessageCode.SUCCESS;
        }

        /// <summary>
        /// 从购物车中删除商品
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="cart_id">购物车内容序列标识</param>
        /// <returns>删除结果</returns>
        public int RemoveGoodsInCart(int uid, string cart_id)
        {
            var cartIds= PublicAPI.StrToIntList(cart_id);
            RRLDB db = new RRLDB();
            foreach (var id in cartIds)
            {
                int res = db.ExecSP(@"sp_CART_GOODS_REMOVE",  new SqlParameter("@cart_id", SqlDbType.Int, 4) { Value = id },
                new SqlParameter("@uid", SqlDbType.Int, 4) { Value = uid });
            }
            db.Close();
            //if (res < 0)
            //    return MessageCode.ERROR_EXECUTE_SQL;
            //if (res == 0)
            //    return MessageCode.ERROR_UNKONWN;
            return MessageCode.SUCCESS;
        }

        /// <summary>
        /// 通过商品生成订单
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="goods_id">商品id</param>
        /// <param name="goods_count">商品数量</param>
        /// <param name="receive_id">收货信息</param>
        /// <returns>生成结果</returns>
        public int CreateOrderFromGoods(int uid, int goods_id, int goods_count, /*int receive_id, */out List<int> order_list)
        {
            //创建单一订单
            int res, order_id;
            order_list = new List<int>();
            RRLDB db = new RRLDB();
            //db.BeginTran();
            res = db.ExecSP(@"sp_ORDER_CREATE_BY_GOODS",
                new SqlParameter("@uid", SqlDbType.Int, 4) { Value = uid },
                new SqlParameter("@goods_id", SqlDbType.Int, 4) { Value = goods_id },
                new SqlParameter("@goods_count", SqlDbType.Int, 4) { Value = goods_count });
            //new SqlParameter("@receive_id", SqlDbType.Int, 4) { Value = receive_id }
            order_id = Convert.ToInt32(db.sm.Parameters["@return"].Value);
            db.Close();
            if (order_id == -2)
            {
                return MessageCode.ERROR_USER_HAVE_ATTEND_ONE_YUAN;
            }
            if (order_id == -1)
            {
                return MessageCode.ERROR_GOODS_COUNT_LIMIT;
            }
            if (res <= 0)
            {
                return MessageCode.ERROR_EXECUTE_SQL;
            }
            order_list.Add(order_id);
            return MessageCode.SUCCESS;
        }

        /// <summary>
        /// 通过购物车生成订单
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="receive_id">收件地址id</param>
        /// <param name="cart_list">购物车列表</param>
        /// <returns>生成结果</returns>
        public int CreateOrderFromCart(int uid, /*int receive_id,*/ List<int> cart_list, out List<int> order_list)
        {
            //创建多个订单
            var count = cart_list.Count;
            var db = new RRLDB();
            int res, orderId;
            var orderlist = new List<int>();
            order_list = new List<int>();
            db.BeginTran();
            for (var i = 0; i < count; i++)
            {
                res = db.ExecSP(@"sp_ORDER_CREATE_BY_CART",
                    new SqlParameter("@uid", SqlDbType.Int, 4) { Value = uid },
                    new SqlParameter("@cart_id", SqlDbType.Int, 4) { Value = cart_list[i] });
                //new SqlParameter("@receive_id", SqlDbType.Int, 4) { Value = receive_id }
                orderId = Convert.ToInt32(db.sm.Parameters["@return"].Value);
                switch (orderId)
                {
                    case -2:
                        db.RollBackTran();
                        db.Close();
                        return MessageCode.ERROR_USER_HAVE_ATTEND_ONE_YUAN;
                    case -1:
                        db.RollBackTran();
                        db.Close();
                        return MessageCode.ERROR_GOODS_COUNT_LIMIT;
                }

                if (res <= 0)
                {
                    db.RollBackTran();
                    db.Close();
                    return MessageCode.ERROR_EXECUTE_SQL;
                }

                res = db.ExecSP(@"sp_ORDER_CREATE_COMPLETE",
                    new SqlParameter("@order_id", SqlDbType.Int, 4) { Value = orderId });
                if (res <= 0)
                {
                    db.RollBackTran();
                    db.Close();
                    return MessageCode.ERROR_EXECUTE_SQL;
                }
                orderlist.Add(orderId);
            }
            var newlist = orderlist.Distinct().ToList();
            //count = newlist.Count;
            //for (var j = 0; j < count; j++)
            //{
            //    res = db.ExecSP(@"sp_ORDER_CREATE_COMPLETE",
            //        new SqlParameter("@order_id", SqlDbType.Int, 4) { Value = newlist[j] });
            //    if (res <= 0)
            //    {
            //        db.RollBackTran();
            //        db.Close();
            //        return MessageCode.ERROR_EXECUTE_SQL;
            //    }
            //}
            db.CommitTran();
            db.Close();
            order_list = newlist;
            return MessageCode.SUCCESS;
        }

        /// <summary>
        /// 获取订单列表
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="page">页码</param>
        /// <param name="pagesize">页面大小</param>
        /// <returns>order集合</returns>
        public List<ListOrder> GetOrderList(int uid, int page, int pagesize)
        {
            int PageID = Math.Max(page, 1);
            int OffSet = (PageID - 1) * pagesize;
            //ORDER_LIST_VIEW
            RRLDB db = new RRLDB();
            string sql = string.Format(@"SELECT TOP {0} * FROM(SELECT ROW_NUMBER()OVER(ORDER BY id DESC)AS RowNum ,* FROM ORDER_LIST_VIEW WHERE uid=@uid)AS TEMP WHERE TEMP.RowNum > {1}",
                pagesize.ToString(), OffSet.ToString());
            DataSet ds = db.ExeQuery(sql, new SqlParameter("@uid", SqlDbType.Int, 4) { Value = uid });
            db.Close();
            List<ListOrder> list = new List<ListOrder>();
            int count = ds.Tables[0].Rows.Count;
            for (int i = 0; i < count; i++)
            {
                ListOrder Order = new ListOrder(ds.Tables[0].Rows[i]);
                list.Add(Order);
            }
            return list;
        }

        /// <summary>
        /// 获取用户订单分类统计数量
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <returns>数据集</returns>
        public DataSet GetOrderStatusCount(int uid)
        {
            //ORDER_STATUS_COUNT
            RRLDB db = new RRLDB();
            DataSet ds = db.ExeQuery(@"SELECT * FROM ORDER_STATUS_COUNT WHERE uid=@uid",
                new SqlParameter("@uid", SqlDbType.Int, 4) { Value = uid });
            db.Close();
            return ds;
        }

        /// <summary>
        /// 通过订单状态获取订单列表
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="status">订单状态码</param>
        /// <param name="page">页码</param>
        /// <param name="pagesize">页面大小</param>
        /// <returns>order集合</returns>
        public List<ListOrder> GetOrderListByStatus(int uid, int status, int page, int pagesize)
        {
            int PageID = Math.Max(page, 1);
            int OffSet = (PageID - 1) * pagesize;
            //ORDER_LIST_VIEW
            RRLDB db = new RRLDB();
            string sql = string.Format(@"SELECT TOP {0} * FROM(SELECT ROW_NUMBER()OVER(ORDER BY id DESC)AS RowNum ,* FROM ORDER_LIST_VIEW WHERE uid=@uid AND status = @status)AS TEMP WHERE TEMP.RowNum > {1}",
                pagesize.ToString(), OffSet.ToString());
            DataSet ds = db.ExeQuery(sql,
                new SqlParameter("@uid", SqlDbType.Int, 4) { Value = uid },
                new SqlParameter("@status", SqlDbType.Int, 4) { Value = status });
            db.Close();
            List<ListOrder> list = new List<ListOrder>();
            int count = ds.Tables[0].Rows.Count;
            for (int i = 0; i < count; i++)
            {
                ListOrder Order = new ListOrder(ds.Tables[0].Rows[i]);
                list.Add(Order);
            }
            return list;
        }

        /// <summary>
        /// 通过订单标识获取订单详情
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="order_id">订单id</param>
        /// <returns>订单对象</returns>
        public ListOrder GetOrderDetail(int uid, int order_id)
        {
            RRLDB db = new RRLDB();
            DataSet ds = db.ExeQuery(@"SELECT * FROM ORDER_LIST_VIEW WHERE id=@order_id AND uid=@uid",
                new SqlParameter("@order_id", SqlDbType.Int, 4) { Value = order_id },
                new SqlParameter("@uid", SqlDbType.Int, 4) { Value = uid });
            db.Close();
            ListOrder Order = new ListOrder(ds.Tables[0].Rows[0]);
            return Order;
        }

        /// <summary>
        /// 根据订单标识数组获取预处理订单列表
        /// </summary>
        /// <param name="uid">用户标识</param>
        /// <param name="order_arr_str">订单id数组</param>
        /// <returns>order集合</returns>
        public List<ListOrder> GetPreOrderList(int uid, string order_arr_str)
        {
            RRLDB db = new RRLDB();
            string sql = string.Format(@"SELECT * FROM dbo.ORDER_LIST_VIEW WHERE id IN({0})AND uid=@uid AND status=1", order_arr_str);
            DataSet ds = db.ExeQuery(sql, new SqlParameter("@uid", SqlDbType.Int, 4) { Value = uid });
            db.Close();
            List<ListOrder> list = new List<ListOrder>();
            int count = ds.Tables[0].Rows.Count;
            for (int i = 0; i < count; i++)
            {
                ListOrder Order = new ListOrder(ds.Tables[0].Rows[i]);
                list.Add(Order);
            }
            return list;
        }

        /// <summary>
        /// 补充订单收货信息
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="order_id">订单id</param>
        /// <param name="receive_id">收货信息id</param>
        /// <returns>补充结果</returns>
        public int FillOrderReceiveInfo(int uid, List<int> order_id_list, int receive_id)
        {
            RRLDB db = new RRLDB();
            foreach (var order_id in order_id_list)
            {
                int res = db.ExecSP(@"sp_ORDER_FILL_RECEIVE_INFO",
               new SqlParameter("@uid", SqlDbType.Int, 4) { Value = uid },
               new SqlParameter("@order_id", SqlDbType.Int, 4) { Value = order_id },
               new SqlParameter("@receive_id", SqlDbType.Int, 4) { Value = receive_id });
            }

            db.Close();
            //if (res <= 0)
            //{
            //    return MessageCode.ERROR_EXECUTE_SQL;
            //}
            return MessageCode.SUCCESS;
        }

        /// <summary>
        /// 取消未支付订单
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="order_id">订单id</param>
        /// <returns></returns>
        public int CancelOlder(int uid, int order_id)
        {
            RRLDB db = new RRLDB();
            int res = db.ExecSP(@"sp_ORDER_CANCEL",
                new SqlParameter("@order_id", SqlDbType.Int, 4) { Value = order_id },
                new SqlParameter("@uid", SqlDbType.Int, 4) { Value = uid });
            db.Close();
            if (res <= 0)
            {
                return MessageCode.ERROR_EXECUTE_SQL;
            }else
            {
                
            }
            return MessageCode.SUCCESS;
        }

        /// <summary>
        /// 申请退货
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="order_id">订单号</param>
        /// <returns>申请结果</returns>
        public int OrderRefundApply(int uid, int order_id)
        {
            RRLDB db = new RRLDB();
            int res = db.ExecSP(@"sp_ORDER_REFUND",
                new SqlParameter("@uid", SqlDbType.Int, 4) { Value = uid },
                new SqlParameter("@order_id", SqlDbType.Int, 4) { Value = order_id });
            db.Close();
            if (res < 0)
                return MessageCode.ERROR_EXECUTE_SQL;
            if (res == 0)
                return MessageCode.ERROR_UNKONWN;
            return MessageCode.SUCCESS;
        }

        /// <summary>
        /// 结算订单
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="order_id">订单号</param>
        /// <returns>结算结果</returns>
        public int OrderSettlement(int uid, int order_id)
        {
            RRLDB db = new RRLDB();
            int res = db.ExecSP(@"sp_ORDER_SETTLEMENT",
                new SqlParameter("@uid", SqlDbType.Int, 4) { Value = uid },
                new SqlParameter("@order_id", SqlDbType.Int, 4) { Value = order_id });
            db.Close();
            if (res < 0)
                return MessageCode.ERROR_EXECUTE_SQL;
            if (res == 0)
                return MessageCode.ERROR_UNKONWN;
            return MessageCode.SUCCESS;
        }

        /// <summary>
        /// 申请支付
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="order_arr_str">订单列表数组</param>
        /// <param name="discount">可抵扣金额</param>
        /// <param name="trans_type">支付类型:1微信，2:支付宝，3:退款账户余额支付,5=金豆支付</param>
        /// <param name="plate_to_return_money">用户金币余额</param>
        /// <param name="query_resault">查询结果</param>
        /// <param name="IP">客户端ip</param>
        /// <returns>支付申请字符串</returns>
        public string ApplyPay(int uid, string order_arr_str, double discount, int trans_type, double plate_to_return_money, string IP, out int query_resault)
        {
            //HelpManager.Mark($@"First-Step || uid:{uid},order_arr_str:{order_arr_str},discount:{discount},transtype:{trans_type},platetoreturnmoeny:{plate_to_return_money},ip:{IP}");
            //SELECT
            //   COUNT(DISTINCT buyer_id) AS user_count,
            //   MAX (buyer_id)AS uid,
            //   COUNT(DISTINCT status) AS status_count,
            //   MAX(status) AS status,
            //   MAX(ordercode) AS ordercode,
            //   SUM(money) AS money,
            //   SUM(dbo.fn_GET_ORDER_DISCOUNT(id))AS discount,
            //   SUM (dbo.fn_GET_ORDER_POST_AGE(id)) AS postage
            //FROM
            //   rrl_order
            //WHERE
            //   id IN({0})
            string PayRequestString = "";
            RRLDB db = new RRLDB();
            try
            {
                var areaCodeDs = db.ExeQuery($@"select area_code from rrl_order where id in ({order_arr_str})");
                foreach (DataRow row in areaCodeDs.Tables[0].Rows)
                {
                    if (string.IsNullOrWhiteSpace(row["area_code"].ToString()))
                    {
                        query_resault = MessageCode.ERROR_ORDER_NO_ADDRESS;
                        return "订单无收货地址";
                    }
                }

                string presql = @"SELECT COUNT(DISTINCT buyer_id)AS user_count,MAX(buyer_id)AS uid,COUNT(DISTINCT status)AS status_count,MAX(status)AS status,MAX(ordercode)AS ordercode,SUM(money)AS money,SUM(dbo.fn_GET_ORDER_DISCOUNT(id))AS discount,SUM (dbo.fn_GET_ORDER_POST_AGE(id)) AS postage,dbo.fn_GET_CONFIG_MINIMUM_MONEY_FOR_NIL_POSTAGE() AS min_money,sum(isnull(user_coupons_countmoney,0)) as coupons_money FROM rrl_order WHERE id IN ({0})";
                //查询请求订单用户数，订单号，用户id
                string sql = string.Format(presql, order_arr_str);
                DataSet ds = db.ExeQuery(sql);
                int ds_user_count = Convert.ToInt32(ds.Tables[0].Rows[0]["user_count"]);//下订单的用户总数
                int ds_uid = Convert.ToInt32(ds.Tables[0].Rows[0]["uid"]);//最大的用户id
                int ds_status_count = Convert.ToInt32(ds.Tables[0].Rows[0]["status_count"]);//订单状态数量
                int ds_status = Convert.ToInt32(ds.Tables[0].Rows[0]["status"]);//订单状态最大值
                string ds_ordercode = Convert.ToString(ds.Tables[0].Rows[0]["ordercode"]);//最大的订单号
                double ds_money = Convert.ToDouble(ds.Tables[0].Rows[0]["money"]);//订单金额总和
                double ds_discount = Convert.ToDouble(ds.Tables[0].Rows[0]["discount"]);//需要总的金币数量
                double ds_postage = Convert.ToDouble(ds.Tables[0].Rows[0]["postage"]);//所有商户总的邮费
                double min_money = Convert.ToDouble(ds.Tables[0].Rows[0]["min_money"]);//最小的包邮88元
                #region 判断金币不足,直接返回
                if(plate_to_return_money < ds_discount)
                {
                    query_resault = 99;
                    return "金币不足,交易失败!";
                }
                #endregion


                #region 券抵扣的金额zetee
                double coupons_money = 0d;//券抵扣的金额
                double coupons_money_try = 0;
                if (double.TryParse(ds.Tables[0].Rows[0]["coupons_money"].ToString(), out coupons_money_try))
                {
                    coupons_money = coupons_money_try;
                }
                #endregion
                ds = db.ExeQuery($@"Select h_money From rrl_user Where id = {uid}");//金豆账余额

                //一元专区172
                var dsa = db.ExeQuery($@"SELECT
	(
		SELECT
			COUNT (rrl_goods.goods_type)
		FROM
			rrl_order_info_goods
		LEFT JOIN rrl_order ON rrl_order.id = rrl_order_info_goods.order_id
		LEFT JOIN rrl_goods ON rrl_order_info_goods.goods_id = rrl_goods.id
		WHERE
			rrl_order.id IN ({order_arr_str})
		AND rrl_goods.goods_type = dbo.fn_GET_CONFIG_ONE_YUAN_AREA ()
	) AS a,
	(
		SELECT
			rrl_user.one_yuan_remark
		FROM
			rrl_user
		WHERE
			id = {uid}
	) AS b");
                var a = Convert.ToInt32(dsa.Tables[0].Rows[0][0]);
                var b = Convert.ToInt32(dsa.Tables[0].Rows[0][1]);
                var one = false;
                if (b == 0)
                {
                    one = true;
                }
                else
                {
                    if (a == 0)
                    {
                        one = true;
                    }
                }
                //卡券区
                /*
                var dsb = db.ExeQuery($@"SELECT
        COUNT(rrl_goods.goods_type)
    FROM
        rrl_order_info_goods
    LEFT JOIN rrl_order ON rrl_order.id = rrl_order_info_goods.order_id
    LEFT JOIN rrl_goods ON rrl_order_info_goods.goods_id = rrl_goods.id
    WHERE
        rrl_order.id IN ({order_arr_str})
    AND rrl_goods.goods_type IN (
        SELECT
            id
        FROM
            rrl_shop_cate
        WHERE
            ParentID = (
                SELECT
                    [Value]
                FROM
                    rrl_config
                WHERE
                    [Item] = 'tickets_area'
            )
    )");*/

                var dsb = db.ExeQuery($@"
SELECT
	ISNULL(
		SUM (
			[dbo].[fn_COUNT_GOODS_MONEY] (
				rrl_order_info_goods.goods_price,
				rrl_order_info_goods.goods_count,
				rrl_goods.goods_type
			)
		),
		0
	)
FROM
	rrl_order_info_goods
JOIN rrl_order ON rrl_order_info_goods.order_id = rrl_order.id
JOIN rrl_goods ON rrl_order_info_goods.goods_id = rrl_goods.id
WHERE
	rrl_order.id IN ({order_arr_str})");
                var c = Convert.ToDouble(dsb.Tables[0].Rows[0][0]);//总价格=goods_price*goods_count*rate
                var hMoneyDis = Convert.ToInt32((ds_money - c) * 100);//折扣后的金额
                var isHasTickets = false;
                //if (ds_money > c)
                //{
                //    ds_money = c;
                //    isHasTickets = true;
                //}

                //提交处理的订单必须满足如下条件
                //只有一个买家
                //订单状态全部为待支付
                /////可抵扣金额大于或等于实际抵扣金额
                //用户可用抵金币大于实际兑换金币
                if (ds_user_count == 1 && ds_uid == uid && ds_status_count == 1 && ds_status == 1 &&  plate_to_return_money >= ds_discount && one)
                   // if (ds_user_count == 1 && ds_uid == uid && ds_status_count == 1 && ds_status == 1 && ds_discount == discount && plate_to_return_money >= discount && one)
                 {
                    //修改订单属性
                    //UPDATE
                    //    rrl_order
                    //SET trans_type = {1}, combine_codde = {2}
                    //WHERE
                    //    id IN({ 0})
                    //AND is_paid = 0
                    //AND status = 1
                    presql = @"UPDATE rrl_order SET trans_type={1},combine_code='{2}' WHERE is_paid = 0 AND id IN ({0}) AND status = 1";//修改交易类型
                    sql = string.Format(presql, order_arr_str, trans_type, ds_ordercode);
                    var res = db.ExeCMD(sql);
                    if (res <= 0)
                    {
                        query_resault = MessageCode.ERROR_EXECUTE_SQL;
                    }
                    else
                    {
                        query_resault = MessageCode.SUCCESS;
                        PayBody body = new PayBody();
                        body.uid = uid;
                        body.order_code = ds_ordercode;
                        body.order_list = order_arr_str;
                        body.money = ds_money;
                        body.postage = ds_postage;
                        body.discount = ds_discount;

                        //前后端统一支付金额计算:
                        //sum(money)+sum(postage)-discount
                        //包邮
                        if (ds_money >= min_money)
                        {
                            ds_postage = 0;
                        }
                        if (isHasTickets && trans_type != 5)
                        {
                            body.isticket = true;
                            body.ticketHmoney = hMoneyDis;
                            var userc = new UserAuth(body.uid);
                            if (userc.h_money < hMoneyDis)
                            {
                                query_resault = MessageCode.ERROR_ORDER;
                                return "交易异常";
                            }
                            //try
                            //{

                            //    db.ExeCMD($@"UPDATE rrl_user SET h_money = h_money - {ds_money * 100} WHERE id = {uid}");
                            //    db.ExeCMD(
                            //        $@"INSERT INTO rrl_user_money_record (uid,order_id,money,type,remark) VALUES({uid},{
                            //                order_arr_str.Split(',')[0]
                            //            },-{ds_money * 100},13,'金豆购物消费')");
                            //}
                            //catch
                            //{
                            //    query_resault = MessageCode.ERROR_ORDER;
                            //    return "交易异常";
                            //}

                        }
                        //HelpManager.Mark($@"First Step || body:{JsonConvert.SerializeObject(body)}");
                        double applyMoney = ds_money + ds_postage;//应支付总金额
                        applyMoney = applyMoney - coupons_money;//券逻辑抵扣掉一部分的钱 zetee 2018-02-12 17:13:21
                        if(applyMoney<0.0000001)//抵扣完了.那么直接就交易成功
                        {
                            query_resault = MessageCode.SUCCESS;
                            return "SUCCESS";
                        }

                        string recordId = PayBody.EncryptBody(body);
                        //微信
                        if (trans_type == 1)
                        {
                            PayRequestString = GeneratorPay.BuildWxPay(recordId, ds_ordercode, applyMoney.ToString(), ds_ordercode, IP);
                        }
                        //支付宝
                        if (trans_type == 2)
                        {
                            //ds_money + ds_postage - discount;
                            PayRequestString = GeneratorPay.BuildAliPay(recordId, ds_ordercode, applyMoney.ToString(), ds_ordercode);
                        }
                        //现金账户支付
                        if (trans_type == 3)
                        {
                            query_resault = MessageCode.SUCCESS;
                            PayRequestString = "SUCCESS";
                            //todo 退款账户扣除
                            //PayBody body_confirm = PayBody.DecryptBody(record_id, ds_ordercode);
                            if (body.uid == 0)
                            {
                                //标记订单异常
                                res = PayBody.MarkOrderExce(body.order_list);
                                if (res != MessageCode.SUCCESS)
                                {
                                    query_resault = MessageCode.ERROR_UNKONWN;
                                    PayRequestString = "FAIL";
                                }
                            }
                            else
                            {
                                //处理订单:改变订单状态，添加现金记录，修改用户账户，添加交易记录
                                try
                                {
                                    // HelpManager.Mark("Second Step Confirm");
                                    res = PayBody.MarkOrderConfirm(body);
                                    //HelpManager.Mark("Third Step Record");
                                    res = db.ExeCMD($@"UPDATE rrl_user SET x_money = x_money - {applyMoney} WHERE id = {uid}");
                                    res = db.ExeCMD($@"INSERT INTO rrl_user_money_record (uid,order_id,money,type,remark) VALUES({uid},{order_arr_str.Split(',')[0]},-{applyMoney},12,'现金账户消费')");
                                    //HelpManager.Mark($@"Mark Step res = {res}");
                                    if (res == 1)
                                    {
                                        res = MessageCode.SUCCESS;
                                    }
                                    //if (body.isticket)
                                    //{
                                    //    db.ExeCMD($@"UPDATE rrl_user SET h_money = h_money - { body.money * 100 } WHERE id = {body.uid}");
                                    //    db.ExeCMD($@"INSERT INTO rrl_user_money_record (uid,order_id,money,type,remark) VALUES({body.uid},{body.order_list.Split(',')[0]},-{ body.money * 100 },13,'金豆购物消费')");
                                    //}
                                }
                                catch
                                {
                                    // ignored
                                }
                                if (res != MessageCode.SUCCESS)
                                {
                                    //HelpManager.Mark("Fail Step Record");
                                    query_resault = MessageCode.ERROR_UNKONWN;
                                    PayRequestString = "FAIL";
                                }
                            }
                        }
                        //二维码扫码支付
                        if (trans_type == 4)
                        {
                            PayRequestString = GeneratorPay.BuildWxNativePayUrl(recordId, ds_ordercode, applyMoney.ToString(CultureInfo.InvariantCulture), ds_ordercode, IP);
                        }
                        //金豆支付
                        if (trans_type == 5)
                        {
                            if (!isHasTickets)
                            {
                                query_resault = MessageCode.SUCCESS;
                                PayRequestString = "SUCCESS";
                                //todo 退款账户扣除
                                //PayBody body_confirm = PayBody.DecryptBody(record_id, ds_ordercode);
                                if (body.uid == 0)
                                {
                                    //标记订单异常
                                    res = PayBody.MarkOrderExce(body.order_list);
                                    if (res != MessageCode.SUCCESS)
                                    {
                                        query_resault = MessageCode.ERROR_UNKONWN;
                                        PayRequestString = "FAIL";
                                    }
                                }
                                else
                                {
                                    var user = new UserAuth(body.uid);
                                    if (user.h_money >= applyMoney * 100)
                                    {//处理订单:改变订单状态，添加现金记录，修改用户账户，添加交易记录
                                        try
                                        {
                                            res = PayBody.MarkOrderConfirm(body);
                                            res = db.ExeCMD($@"UPDATE rrl_user SET h_money = h_money - { applyMoney * 100 } WHERE id = {uid}");
                                            res = db.ExeCMD($@"INSERT INTO rrl_user_money_record (uid,order_id,money,type,remark) VALUES({uid},{order_arr_str.Split(',')[0]},-{ applyMoney * 100 },13,'金豆购物消费')");
                                            if (res == 1)
                                            {
                                                res = MessageCode.SUCCESS;
                                            }
                                        }
                                        catch
                                        {
                                            // ignored
                                        }
                                    }
                                    else
                                    {
                                        query_resault = MessageCode.ERROR_UNKONWN;
                                        PayRequestString = "金豆余额不足";
                                    }


                                    if (res != MessageCode.SUCCESS)
                                    {
                                        query_resault = MessageCode.ERROR_UNKONWN;
                                        PayRequestString = "FAIL";
                                    }
                                }
                            }
                            else
                            {
                                query_resault = MessageCode.ERROR_ORDER;
                                return "交易异常";
                            }

                        }
                    }
                }
                else
                {
                    query_resault = MessageCode.ERROR_ORDER;
                }
                db.Close();
                return PayRequestString;
            }
            finally
            {
                db.Close();
            }
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
        public JsPayConfigObject ApplyWxJsPay(int uid, string order_arr_str, double discount, double plate_to_return_money, string IP, string openid, out int query_resault, string sperador = null)
        {
            string prepay_id = "";
            RRLDB db = new RRLDB();
            string presql = @"SELECT COUNT(DISTINCT buyer_id)AS user_count,MAX(buyer_id)AS uid,COUNT(DISTINCT status)AS status_count,MAX(status)AS status,MAX(ordercode)AS ordercode,SUM(money)AS money,SUM(dbo.fn_GET_ORDER_DISCOUNT(id))AS discount,SUM (dbo.fn_GET_ORDER_POST_AGE(id)) AS postage,dbo.fn_GET_CONFIG_MINIMUM_MONEY_FOR_NIL_POSTAGE() AS min_money,sum(isnull(user_coupons_countmoney,0)) as coupons_money  FROM rrl_order WHERE id IN ({0})";
            //查询请求订单用户数，订单号，用户id
            string sql = string.Format(presql, order_arr_str);
            DataSet ds = db.ExeQuery(sql);
            int ds_user_count = Convert.ToInt32(ds.Tables[0].Rows[0]["user_count"]);
            int ds_uid = Convert.ToInt32(ds.Tables[0].Rows[0]["uid"]);
            int ds_status_count = Convert.ToInt32(ds.Tables[0].Rows[0]["status_count"]);
            int ds_status = Convert.ToInt32(ds.Tables[0].Rows[0]["status"]);
            string ds_ordercode = Convert.ToString(ds.Tables[0].Rows[0]["ordercode"]);
            double ds_money = Convert.ToDouble(ds.Tables[0].Rows[0]["money"]);
            double ds_discount = Convert.ToDouble(ds.Tables[0].Rows[0]["discount"]);
            double ds_postage = Convert.ToDouble(ds.Tables[0].Rows[0]["postage"]);
            double min_money = Convert.ToDouble(ds.Tables[0].Rows[0]["min_money"]);

            #region 券抵扣的金额zetee
            double coupons_money = 0d;//券抵扣的金额
            double coupons_money_try = 0d;
            if (double.TryParse(ds.Tables[0].Rows[0]["coupons_money"].ToString(),out   coupons_money_try))
            {
                coupons_money = coupons_money_try;
            }
             
            #endregion
            //一元专区
            var dsa = db.ExeQuery($@"SELECT
	(
		SELECT
			COUNT (rrl_goods.goods_type)
		FROM
			rrl_order_info_goods
		LEFT JOIN rrl_order ON rrl_order.id = rrl_order_info_goods.order_id
		LEFT JOIN rrl_goods ON rrl_order_info_goods.goods_id = rrl_goods.id
		WHERE
			rrl_order.id IN ({order_arr_str})
		AND rrl_goods.goods_type = dbo.fn_GET_CONFIG_ONE_YUAN_AREA ()
	) AS a,
	(
		SELECT
			rrl_user.one_yuan_remark
		FROM
			rrl_user
		WHERE
			id = {uid}
	) AS b");
            var a = Convert.ToInt32(dsa.Tables[0].Rows[0][0]);
            var b = Convert.ToInt32(dsa.Tables[0].Rows[0][1]);
            var one = false;
            if (b == 0)
            {
                one = true;
            }
            else
            {
                if (a == 0)
                {
                    one = true;
                }
            }

            var dsb = db.ExeQuery($@"
SELECT
	ISNULL(
		SUM (
			[dbo].[fn_COUNT_GOODS_MONEY] (
				rrl_order_info_goods.goods_price,
				rrl_order_info_goods.goods_count,
				rrl_goods.goods_type
			)
		),
		0
	)
FROM
	rrl_order_info_goods
JOIN rrl_order ON rrl_order_info_goods.order_id = rrl_order.id
JOIN rrl_goods ON rrl_order_info_goods.goods_id = rrl_goods.id
WHERE
	rrl_order.id IN ({order_arr_str})");
            var c = Convert.ToDouble(dsb.Tables[0].Rows[0][0]);
            var hMoneyDis = Convert.ToInt32((ds_money - c) * 100);
            var isHasTickets = false;
            if (ds_money > c)
            {
                ds_money = c;
                isHasTickets = true;
            }
            //提交处理的订单必须满足如下条件
            //只有一个买家
            //订单状态全部为待支付
            //可抵扣金额大于或等于实际抵扣金额
            //用户可用抵扣金大于实际抵扣金
            if (ds_user_count == 1 && ds_uid == uid && ds_status_count == 1 && ds_status == 1 && ds_discount == discount && plate_to_return_money >= discount && one)
            {
                //修改订单属性
                presql = @"UPDATE rrl_order SET trans_type={1},combine_code={2} WHERE is_paid = 0 AND id IN ({0}) AND status = 1";
                sql = string.Format(presql, order_arr_str, "1", ds_ordercode);
                int res = db.ExeCMD(sql);
                if (res <= 0)
                {
                    query_resault = MessageCode.ERROR_EXECUTE_SQL;
                }
                else
                {
                    query_resault = MessageCode.SUCCESS;
                    PayBody body = new PayBody();
                    body.uid = uid;
                    body.order_code = ds_ordercode;
                    body.order_list = order_arr_str;
                    body.money = ds_money;
                    body.postage = ds_postage;
                    body.discount = ds_discount;
                    //前后端统一支付金额计算:
                    //sum(money)+sum(postage)-discount
                    //包邮
                    if (ds_money >= min_money)
                    {
                        ds_postage = 0;
                    }
                    if (isHasTickets)
                    {
                        body.isticket = true;
                        body.ticketHmoney = hMoneyDis;
                        var userc = new UserAuth(body.uid);
                        if (userc.h_money < hMoneyDis)
                        {
                            query_resault = MessageCode.ERROR_ORDER;
                            return null;
                        }
                        //try
                        //{

                        //    db.ExeCMD($@"UPDATE rrl_user SET h_money = h_money - {ds_money * 100} WHERE id = {uid}");
                        //    db.ExeCMD(
                        //        $@"INSERT INTO rrl_user_money_record (uid,order_id,money,type,remark) VALUES({uid},{
                        //                order_arr_str.Split(',')[0]
                        //            },-{ds_money * 100},13,'金豆购物消费')");
                        //}
                        //catch
                        //{
                        //    query_resault = MessageCode.ERROR_ORDER;
                        //    return "交易异常";
                        //}

                    }
                    double ApplyMoney = ds_money + ds_postage;
                    ApplyMoney = ApplyMoney - coupons_money;//券逻辑抵扣掉一部分的钱 zetee 2018-02-12 17:13:21
                    if (ApplyMoney < 0.0000001)//抵扣完了.那么直接就交易成功
                    {
                        ApplyMoney = 0.01;
                    }
                    string record_id = PayBody.EncryptBody(body);
                    //微信JS支付 todo 获取prepay_id
                    prepay_id = GeneratorPay.BuildWxJsPrepayID(record_id, ds_ordercode, ApplyMoney.ToString(), ds_ordercode, IP, openid);
                }
            }
            else
            {
                query_resault = MessageCode.ERROR_ORDER;
            }
            db.Close();

            return GetPayConfig(prepay_id);
        }

        /// <summary>
        /// 游戏券申请微信JS支付
        /// </summary>
        /// <param name="list">订单列表数组</param>
        /// <param name="openid"></param>
        /// <param name="queryResault">查询结果</param>
        /// <param name="ip"></param>
        /// <returns>支付申请字符串</returns>
        public JsPayConfigObject ApplyWxJsCouponPay(string list, string ip, string openid, out int queryResault, out double pay_money)
        {
            pay_money = 0;
            var prepayId = "";
            var db = new RRLDB();
            var presql = $@"SELECT * FROM rrl_coupons WHERE id IN({list})";
            var ds = db.ExeQuery(presql);

            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                var combine = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
                //修改订单属性
                presql = $@"UPDATE rrl_coupons SET combine_id={combine} WHERE id IN ({list})";
                var res = db.ExeCMD(presql);

                if (res <= 0)
                {
                    queryResault = MessageCode.ERROR_EXECUTE_SQL;
                }
                else
                {
                    var stamp = Convert.ToString(Convert.ToInt32((DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000));
                    var prestr = $@"{stamp}CouponsId{combine}";
                    string ordecode;
                    using (var md5 = new MD5CryptoServiceProvider())
                    {
                        var data = md5.ComputeHash(Encoding.UTF8.GetBytes(prestr));
                        var builder = new StringBuilder();
                        foreach (var b in data)
                        {
                            builder.Append(b.ToString("x2"));
                        }
                        ordecode = builder.ToString();
                    }
                    ds = db.ExeQuery($@"SELECT SUM(moeny) FROM rrl_coupons WHERE id IN({list})");
                    db.ExeCMD($@"UPDATE rrl_coupons SET order_code = '{ordecode}' WHERE id IN({list})");
                    var money = ds.Tables[0].Rows[0][0].ToString();
                    pay_money = Convert.ToDouble(money);
                    queryResault = MessageCode.SUCCESS;
                    //微信JS支付 todo 获取prepay_id
                    prepayId = GeneratorPay.BuildWxJsPrepayID($@"CouponsId", "购买商城抵扣券", money, ordecode, ip, openid);
                }
            }
            else
            {
                queryResault = MessageCode.ERROR_ORDER;
            }
            db.Close();

            return GetPayConfig(prepayId);
        }

        /// <summary>
        /// app购买卡券
        /// </summary>
        /// <param name="list">预购买卡券列表</param>
        /// <param name="ip">ip</param>
        /// <param name="transType">支付方式1：微信，2：支付宝</param>
        /// <param name="queryResault">查询结果</param>
        /// <param name="payMoney">支付金额</param>
        /// <returns></returns>
        public string ApplyAppCouponPay(string list, string ip, int transType, out int queryResault, out double payMoney)
        {
            string PayRequestString = "";
            payMoney = 0;
            var prepayId = "";
            var db = new RRLDB();
            var presql = $@"SELECT * FROM rrl_coupons WHERE id IN({list})";
            var ds = db.ExeQuery(presql);

            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                var combine = Convert.ToInt32(ds.Tables[0].Rows[0][0]);//第一券的id
                //修改订单属性
                presql = $@"UPDATE rrl_coupons SET combine_id={combine} WHERE id IN ({list})";//全部combine_id  全部变成第一的id
                var res = db.ExeCMD(presql);

                if (res <= 0)
                {
                    queryResault = MessageCode.ERROR_EXECUTE_SQL;
                }
                else
                {
                    var stamp = Convert.ToString(Convert.ToInt32((DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000));
                    var prestr = $@"{stamp}CouponsId{combine}";//1524122121CouponsId3254
                    string ordecode;//1524122121CouponsId3254 的MD5值
                    using (var md5 = new MD5CryptoServiceProvider())
                    {
                        var data = md5.ComputeHash(Encoding.UTF8.GetBytes(prestr));
                        var builder = new StringBuilder();
                        foreach (var b in data)
                        {
                            builder.Append(b.ToString("x2"));
                        }
                        ordecode = builder.ToString();
                    }
                    ds = db.ExeQuery($@"SELECT SUM(moeny) FROM rrl_coupons WHERE id IN({list})");
                    db.ExeCMD($@"UPDATE rrl_coupons SET order_code = '{ordecode}' WHERE id IN({list})");
                    var money = ds.Tables[0].Rows[0][0].ToString();//需要支付的钱总和
                    payMoney = Convert.ToDouble(money);
                    queryResault = MessageCode.SUCCESS;
                    //微信JS支付 todo 获取prepay_id
                    //微信
                    if (transType == 1)
                    {
                        PayRequestString = GeneratorPay.BuildWxPay($@"CouponsId", "购买商城抵扣券", money, ordecode, ip);
                    }
                    //支付宝
                    if (transType == 2)
                    {
                        //ds_money + ds_postage - discount;
                        PayRequestString = GeneratorPay.BuildAliPay($@"CouponsId", "购买商城抵扣券", money, ordecode);
                    }
                }
            }
            else
            {
                queryResault = MessageCode.ERROR_ORDER;
            }
            db.Close();

            return PayRequestString;
        }

        ///// <summary>
        ///// 申请微信扫码支付
        ///// </summary>
        ///// <param name="uid">用户id</param>
        ///// <param name="order_arr_str">订单列表数组</param>
        ///// <param name="discount">可抵扣金额</param>
        ///// <param name="trans_type">支付类型:1微信，2:支付宝</param>
        ///// <param name="r_money">用户红包账户余额</param>
        ///// <param name="query_resault">查询结果</param>
        ///// <returns>支付申请字符串</returns>
        //public string ApplyWxNativePay(int uid, string order_arr_str, double discount, double r_money, string IP, out int query_resault, string sperador = null)
        //{
        //    string url = "";
        //    RRLDB db = new RRLDB();
        //    string presql = @"SELECT COUNT(DISTINCT buyer_id)AS user_count,MAX(buyer_id)AS uid,COUNT(DISTINCT status)AS status_count,MAX(status)AS status,MAX(ordercode)AS ordercode,SUM(money)AS money,SUM(dbo.fn_GET_ORDER_DISCOUNT(id))AS discount,SUM (dbo.fn_GET_ORDER_POST_AGE(id)) AS postage FROM rrl_order WHERE id IN ({0})";
        //    //查询请求订单用户数，订单号，用户id
        //    string sql = string.Format(presql, order_arr_str);
        //    DataSet ds = db.ExeQuery(sql);
        //    int ds_user_count = Convert.ToInt32(ds.Tables[0].Rows[0]["user_count"]);
        //    int ds_uid = Convert.ToInt32(ds.Tables[0].Rows[0]["uid"]);
        //    int ds_status_count = Convert.ToInt32(ds.Tables[0].Rows[0]["status_count"]);
        //    int ds_status = Convert.ToInt32(ds.Tables[0].Rows[0]["status"]);
        //    string ds_ordercode = Convert.ToString(ds.Tables[0].Rows[0]["ordercode"]);
        //    double ds_money = Convert.ToDouble(ds.Tables[0].Rows[0]["money"]);
        //    double ds_discount = Convert.ToDouble(ds.Tables[0].Rows[0]["discount"]);
        //    double ds_postage = Convert.ToDouble(ds.Tables[0].Rows[0]["postage"]);

        //    //提交处理的订单必须满足如下条件
        //    //只有一个买家
        //    //订单状态全部为待支付
        //    //可抵扣金额大于或等于实际抵扣金额
        //    //用户可用抵扣金大于实际抵扣金
        //    if (ds_user_count == 1 && ds_uid == uid && ds_status_count == 1 && ds_status == 1 && ds_discount >= discount && r_money >= discount)
        //    {
        //        //修改订单属性
        //        presql = @"UPDATE rrl_order SET trans_type={1},combine_code={2} WHERE is_paid = 0 AND id IN ({0}) AND status = 1";
        //        sql = string.Format(presql, order_arr_str, "1", ds_ordercode);
        //        int res = db.ExeCMD(sql);
        //        if (res <= 0)
        //        {
        //            query_resault = MessageCode.ERROR_EXECUTE_SQL;
        //        }
        //        else
        //        {
        //            query_resault = MessageCode.SUCCESS;
        //            PayBody body = new PayBody();
        //            body.uid = uid;
        //            body.order_code = ds_ordercode;
        //            body.order_list = order_arr_str;
        //            body.money = ds_money;
        //            body.postage = ds_postage;
        //            body.discount = discount;
        //            if (sperador != null)
        //            {
        //                body.spreador = sperador;
        //            }
        //            string record_id = PayBody.EncryptBody(body);
        //            //前后端统一支付金额计算:
        //            //sum(money)+sum(postage)-discount
        //            double ApplyMoney = ds_money + ds_postage - discount;
        //            //微信JS支付 todo 获取prepay_id
        //            url = GeneratorPay.BuildWxNativePayUrl(record_id, ds_ordercode, ApplyMoney.ToString(), ds_ordercode, IP);
        //        }
        //    }
        //    else
        //    {
        //        query_resault = MessageCode.ERROR_ORDER;
        //    }
        //    db.Close();

        //    return url;
        //}

        private JsPayConfigObject GetPayConfig(string prepay_id)
        {
            string timestamp = WxPayApi.GenerateTimeStamp();
            string noncestr = WxPayApi.GenerateNonceStr();
            WxPayData jsApiParam = new WxPayData();
            jsApiParam.SetValue("appId", WxPayConfig.WxMPAPPID);
            jsApiParam.SetValue("timeStamp", timestamp);
            jsApiParam.SetValue("nonceStr", noncestr);
            jsApiParam.SetValue("package", "prepay_id=" + prepay_id);
            jsApiParam.SetValue("signType", "MD5");
            jsApiParam.SetValue("paySign", jsApiParam.MakeSign(WxPayConfig.WxMPKEY));
            string parameters = jsApiParam.ToJson();
            return Newtonsoft.Json.JsonConvert.DeserializeObject<JsPayConfigObject>(parameters);
        }


        #region 一键转卖
        /// <summary>
        ///  一键转卖
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="username"></param>
        /// <param name="order_id"></param>
        /// <param name="goods_id"></param>
        /// <returns></returns>
        public OneKeyResell_Resp OneKeyResell(int userid, string username, int order_id, int goods_id)
        {
            //http://e-shop.szrundao.com/xianzhuan/product/publish?tel=对称加密后的手机号码&title=标题&picurl=urlencode后的图片地址&orgprice=原价

            var oneKeyResell_Resp = new OneKeyResell_Resp();
            string jujuHost = "https://e-shop.szrundao.com/Shendoudou";
            string url = "";
            string username_Decry = HttpUtility.UrlEncode(Utility.DES.EncryptDES(username));
            url += jujuHost;
            url += ("?tel=" + username_Decry);
            RRLDB db = new RRLDB();
            try
            {
//                var goods_ds = db.ExeQuery(@"select id,isnull(goods_name,'') as name,
//case when is_beans_pay = 1 then CONVERT(dec(18, 2), isnull(need_pay_money, 0) + isnull(need_pay_beans / 100, 0)) else 
//isnull(all_pay_cash, 0.00) end as price,
//isnull(goods_pic_id, 0) as pic_id,
//isnull(postage,0.00) as postage,
//status,
//goods_count
//  from v_v3_order_price where id=@id", new SqlParameter("@id", order_id));

                var goods_ds = db.ExeQuery(@"select id,isnull(goods_name,'') as name,all_pay_cash  as price,
isnull(goods_pic_id, 0) as pic_id,
isnull(postage,0.00) as postage,
status,
goods_count
  from v_v3_order_price where id=@id", new SqlParameter("@id", order_id));
                if (goods_ds == null || goods_ds.Tables.Count == 0 || goods_ds.Tables[0].Rows.Count == 0)
                {
                    oneKeyResell_Resp.errorMessage = "商品不存在!";
                    return oneKeyResell_Resp;
                }

                var order_ds = db.ExeQuery("select ordercode,status from rrl_order where id=@id", new SqlParameter("@id", order_id));
                if (order_ds == null || order_ds.Tables.Count == 0 || order_ds.Tables[0].Rows.Count == 0)
                {
                    oneKeyResell_Resp.errorMessage = "订单不存在!";
                    return oneKeyResell_Resp;
                }
                string order_status = order_ds.Tables[0].Rows[0]["status"].ToString();
                string ordercode= order_ds.Tables[0].Rows[0]["ordercode"].ToString();
                if (("3"!= order_status&& "2" != order_status))
                {
                    oneKeyResell_Resp.errorMessage = "订单状态不符合要求!";
                    return oneKeyResell_Resp;
                }
                string sh_sell_token = Guid.NewGuid().ToString("N");

                var ds_info_goods=db.ExeQuery("select sh_sell_token from rrl_order_info_goods where order_id=@order_id  ", new SqlParameter("@order_id", order_id));
                if (ds_info_goods == null || ds_info_goods.Tables.Count == 0 || ds_info_goods.Tables[0].Rows.Count == 0)
                {
                    oneKeyResell_Resp.errorMessage = "订单商品不存在!";
                    return oneKeyResell_Resp;
                }else
                {
                    string org_sh_sell_token=ds_info_goods.Tables[0].Rows[0]["sh_sell_token"].ToString();
                    if(!string.IsNullOrWhiteSpace(org_sh_sell_token))
                    {
                        sh_sell_token = org_sh_sell_token;
                    }
                }
                int effect=db.ExeCMD("update  rrl_order_info_goods set sh_sell_token=@sh_sell_token,sh_sell_status=0  where order_id=@order_id and   EXISTS (select 1 from rrl_order where id=@order_id and status in(2,3))",
                    new SqlParameter("@sh_sell_token", sh_sell_token),
                    new SqlParameter("@order_id", order_id));
                if(effect<1)
                {
                    oneKeyResell_Resp.errorMessage = "订单状态不符合要求!";
                    return oneKeyResell_Resp;
                }


                string good_name = goods_ds.Tables[0].Rows[0]["name"].ToString();
                string encoe_good_name = HttpUtility.UrlEncode(good_name);
                string good_price = goods_ds.Tables[0].Rows[0]["price"].ToString();
                string good_pic_id = goods_ds.Tables[0].Rows[0]["pic_id"].ToString();
                string goods_count = goods_ds.Tables[0].Rows[0]["goods_count"].ToString();
                //(3=待收,4=待评价)
                string goods_status = (Convert.ToInt32(goods_ds.Tables[0].Rows[0]["status"].ToString())+1).ToString();
                string goods_postage = goods_ds.Tables[0].Rows[0]["postage"].ToString();

                url += ("&title=" + encoe_good_name);
                url += ("&orgprice=" + good_price);
                url += ("&picurl=" + HttpUtility.UrlEncode(@"https://e-shop.rrlsz.com.cn/WebApi/Public/picture/" + good_pic_id));
                url += ("&action=1");//一键转卖
                url += ("&order_id="+ ordercode);// 
                url += ("&sell_token=" + sh_sell_token);// 
                url += ("&count=" + goods_count);// 
                url += ("&sdd_order_status=" + goods_status);// 
                url += ("&postage=" + goods_postage);// 
                oneKeyResell_Resp.IsSuccess = true;
                oneKeyResell_Resp.Url = url;
                return oneKeyResell_Resp;
            }
            finally
            {
                db.Close();
            }


        }

        public class OneKeyResell_Resp
        {
            public string Url { get; set; }
            public bool IsSuccess { get; set; } = false;
            public string errorMessage { get; set; }
        }
        #endregion

        #region 转卖详情
        /// <summary>
        /// 转卖详情
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public OneKeyResell_Resp ReSellDetails(int userid, string username)
        {


            var oneKeyResell_Resp = new OneKeyResell_Resp();
            string jujuHost = "https://e-shop.szrundao.com/ShendoudouW";
            string url = "";
            string username_Decry = HttpUtility.UrlEncode(Utility.DES.EncryptDES(username));
            url += jujuHost;
            url += ("?tel=" + username_Decry);
            //RRLDB db = new RRLDB();
            try
            {
                url += ("&title=");
                url += ("&orgprice=");
                url += ("&picurl=");
                url += ("&act=2");//代表转卖详情
                url += ("&redirect="+ HttpUtility.UrlEncode("https://e-shop.rrlsz.com.cn/"));//代表转卖详情
                oneKeyResell_Resp.IsSuccess = true;
                oneKeyResell_Resp.Url = url;
                return oneKeyResell_Resp;
            }
            finally
            {
                //db.Close();
            }


        }
        #endregion


        #region 我的优惠券
        /// <summary>
        /// 我已支付成功优惠券
        /// </summary>
        /// <param name="id"></param>
        /// <param name="order_list"></param>
        /// <param name="page_index"></param>
        /// <param name="page_size"></param>
        /// <param name="errorCode"></param>
        /// <param name="errMsg"></param>
        public List<MyCoupons> GetAvailableCoupons(int user_id, string order_list, int page_index, int page_size, out int errorCode, out string errMsg)
        {
            errorCode =0;
            errMsg = "";
            int OFFSET = 0;

            SqlDataBase db = new SqlDataBase();
            string str_sql = "select count(*) from rrl_goods where ISNULL(dbo.rrl_goods.is_can_use_coupons,cast(1 as bit))=0 and id in (select goods_id from rrl_order_info_goods where order_id in({order_list}))";
            int cnt = db.ExecuteScalar<int>(str_sql);
            if (cnt > 0)//购物卡类型不允许加券支付
            {
                errorCode = 99;
                errMsg = "该订单存在不支持券支付的商品!";
                return new List<MyCoupons>();
            }



            /*int goods_type = 317;//购物卡分类id
            string sql_gw = $@"select count(*) from rrl_goods where goods_type=@goods_type and id in (select goods_id from rrl_order_info_goods where order_id in({order_list}))";
            int cnt = db.ExecuteScalar<int>(sql_gw, new { goods_type = goods_type  });
            if (cnt > 0)//购物卡类型不允许加券支付
            {
                errorCode = 99;
                errMsg = "购物卡不能用券支付!";
                return new List<MyCoupons>();
            }*/

            OFFSET = (page_index - 1)* page_size;
            
            List<MyCoupons> list = new List<MyCoupons>();
            string sql = $@"
select * from (
select * from rrl_coupons c where  c.uid={user_id} and is_used=0 and is_paid=1 and  c.goodsid=0
  and isnull(c.leastconsume,0.0)<=(select sum(isnull(a.goods_count*a.goods_price,0)) as totalPrice from rrl_order_info_goods a  where a.order_id in ({order_list}))
and  ( ('1900-01-01'=c.starttime and '1900-01-01'=c.endtime)  
or  (c.starttime<GETDATE() and c.endtime >GETDATE()) )  
union ALL
select * from rrl_coupons c where  c.uid={user_id} and is_used=0 and is_paid=1 and  c.goodsid in (
select goods_id from rrl_order_info_goods a  where a.order_id in ({order_list})
)
 and isnull(c.leastconsume,0.0)<=(select sum(isnull(a.goods_count*a.goods_price,0)) as totalPrice from rrl_order_info_goods a  where a.order_id in ({order_list}))
and  ( ('1900-01-01'=c.starttime and '1900-01-01'=c.endtime)  
or  (c.starttime<GETDATE() and c.endtime >GETDATE()) )
) aa
order by aa. endtime asc
OFFSET {OFFSET} ROWS
FETCH NEXT {page_size} ROWS ONLY
";
            
            list = db.Select<MyCoupons>(sql, null);


            return list;

        }
        /// <summary>
        /// 我的优惠券
        /// </summary>
        public class MyCoupons
        {
            /// <summary>
            ///  
            /// </summary>

            public int id { get; set; }
            /// <summary>
            ///  
            /// </summary>

            public int goodsid { get; set; }
            /// <summary>
            ///  
            /// </summary>

            public decimal moeny { get; set; }
            /// <summary>
            ///  
            /// </summary>

            public decimal countmoney { get; set; }
            /// <summary>
            ///  
            /// </summary>

            public int goldbean { get; set; }
            /// <summary>
            ///  
            /// </summary>

            public DateTime starttime { get; set; }
            /// <summary>
            ///  
            /// </summary>

            public DateTime endtime { get; set; }
            /// <summary>
            ///  
            /// </summary>

            public int uid { get; set; }
            /// <summary>
            ///  
            /// </summary>

            public bool is_used { get; set; }
            /// <summary>
            ///  
            /// </summary>

            public DateTime addtime { get; set; }
            /// <summary>
            ///  
            /// </summary>

            public bool is_paid { get; set; }
            /// <summary>
            ///  
            /// </summary>

            public int combine_id { get; set; }
            /// <summary>
            ///  
            /// </summary>

            public string order_code { get; set; }
            /// <summary>
            ///  
            /// </summary>

            public int goldcoin { get; set; }
            /// <summary>
            ///送的红包数量  
            /// </summary>

            public decimal? redpacket { get; set; }
            /// <summary>
            ///最少消费可抵用  countmoney
            /// </summary>

           // public decimal? leastconsume { get; set; }

            private decimal? _leastconsume;

            public decimal? leastconsume
            {
                get
                {
                    if (_leastconsume == null)
                    {
                        return countmoney;
                    }
                    else
                    {
                        if (_leastconsume.Value < countmoney)
                        {
                            return countmoney;
                        }
                        return _leastconsume.Value;
                    }

                }
                set { _leastconsume = value; }
            }


            /// <summary>
            /// 是否过期,1=过期,0=没过期
            /// </summary>
            public string is_outdate { get; set; }
            /// <summary>
            /// 券名称
            /// </summary>
            public string name { get
                {
                    if(string.IsNullOrWhiteSpace(goodsname) )
                    {
                        if (goodsid == 0)
                            return "通用商品使用券";
                        else
                            return "特定商品使用券";
                    }
                    return goodsname;
                }
            }

            public string goodsname { get; set; }

            public string starttimestr
            {
                get {
                    if(starttime==null||starttime == new DateTime(1900,1,1))
                    {
                        return "2016-01-01";
                    }else
                    {
                        return starttime.ToString("yyyy-MM-dd");
                    }
                }
                
            }

            public string endtimestr
            {
                get
                {
                    if (endtime == null || endtime == new DateTime(1900, 1, 1))
                    {
                        return "永久有效";
                    }
                    else
                    {
                        return endtime.ToString("yyyy-MM-dd");
                    }
                }

            }

            public string addtimestr
            {
                get
                {
                    if (addtime == null || addtime == new DateTime(1900, 1, 1))
                    {
                        return "2016-01-01";
                    }
                    else
                    {
                        return addtime.ToString("yyyy-MM-dd");
                    }
                }

            }



        }


        #endregion


        #region 应用券支付
        /// <summary>
        /// 应用券支付
        /// </summary>
        /// <param name="user_id"></param>
        /// <param name="order_id"></param>
        /// <param name="coupons_id"></param>
        /// <param name="errorCode"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public int ApplyCouponsPay(int user_id, string order_id, string coupons_id, out int errorCode, out string errMsg)
        {
            errorCode = 0;
            errMsg = "ok";
            int coupons_id_int = 0;
            if (!string.IsNullOrWhiteSpace(coupons_id))
            {
                var isParseOk = int.TryParse(coupons_id, out coupons_id_int);
                if (!isParseOk)
                {
                    return 1;
                }
            }
            SqlDataBase db = new SqlDataBase();
            string goods_type = "317,171";//购物卡分类id,和金币专区id
            string sql = $@"select count(*) from rrl_goods where goods_type=in({goods_type}) and id in (select goods_id from rrl_order_info_goods where order_id=@order_id)";
            int cnt= db.ExecuteScalar<int>(sql, new {   order_id= Convert.ToInt32(order_id) });
            if(cnt>0)//购物卡类型不允许加券支付
            {
                errorCode = 99;
                errMsg = "购物卡不能用券支付和金币商城!";
                return 1;
            }

            string sql_cou=$@"select countmoney,isnull(leastconsume,0) as leastconsume from rrl_coupons c where  c.uid={user_id} and is_used=0 and is_paid=1  
  and  id={coupons_id} and(('1900-01-01' = c.starttime and '1900-01-01' = c.endtime) or(c.starttime < GETDATE() and c.endtime > GETDATE()) ) ";
           var cuntMM=  db.Select<ApplyCouponsPay_countmoney>(sql_cou,null);
            if(cuntMM==null|| cuntMM.Count==0)
            {
                errorCode = 99;
                errMsg = "优惠券已经失效!";
                return 1;
            }else
            {
               var countmoney= cuntMM[0].countmoney;
                db.Execute($@"update rrl_coupons set pay_order_id={order_id},is_used=1 where  id={coupons_id} ",null);
                db.Execute($@"update rrl_order set user_coupons_id={coupons_id},user_coupons_countmoney={countmoney} where  id={order_id} ", null);
            }
            return 1;
        }
        
        public class ApplyCouponsPay_countmoney
        {
            public decimal countmoney { get; set; }
            public decimal  leastconsume { get; set; }
        }
        #endregion


    }
}