using Newtonsoft.Json;
using RRL.Config;
using RRL.Core.Manager;
using RRL.Core.Models;
using RRL.Core.Pay.AliPay;
using RRL.Core.Pay.WxPay;
using RRL.Core.Utility;
using RRL.DB;
using RRL.WEB.Models;
using RRL.WEB.Ulity;
using System;
using System.Globalization;
using System.Text;
using System.Web.Mvc;
using System.Xml.Linq;

namespace RRL.WEB.Controllers
{
    /// <summary>
    /// 支付回调
    /// </summary>
    public class PayCallBackController : Controller
    {
        TradeManager tradeBiz = new TradeManager();
        /// <summary>
        /// 支付宝回调
        /// </summary>
        /// <returns>回调结果</returns>
        [HttpPost]
        public string alipay_notify()
        {
            var alipayNotifyURL = "https://www.alipay.com/cooperate/gateway.do?service=notify_verify";
            //此路径是在上面链接地址无法起作用时替换使用。
            //string alipayNotifyURL = "http://notify.alipay.com/trade/notify_query.do?";
            var partner = AliPayConfig.partner; //partner合作伙伴id（必须填写）
            string notify_id = Request.Form["notify_id"];
            alipayNotifyURL = alipayNotifyURL + "&partner=" + partner + "&notify_id=" + notify_id;
            //获取支付宝ATN返回结果，true是正确的订单信息，false 是无效的
            var responseTxt = AliPay.Get_Http(alipayNotifyURL, 120000);
            //HelpManager.Mark($@"ALI-res-out_trade_no-{Request.Form["out_trade_no"]}");
            //HelpManager.Mark($@"ALI-res-body-{Request.Form["body"]}");
           
            if (responseTxt == "true")
            {
                if (Request.Form["trade_status"] == "TRADE_FINISHED" || Request.Form["trade_status"] == "TRADE_SUCCESS")
                {
                    var record_id = Request.Form["body"];
                    var order_code = Request.Form["out_trade_no"];
                    var trade_no = Request.Form["trade_no"];
                    DateTime completed_time=DateTime.Now  ;
                    string gmt_payment = Request.Form["gmt_payment"];
                    if (!string.IsNullOrWhiteSpace(gmt_payment))
                    {
                        completed_time=DateTime.Parse(gmt_payment);
                    }
                    else
                    {
                        string gmt_create = Request.Form["gmt_create"];
                        if (!string.IsNullOrWhiteSpace(gmt_create))
                        {
                            completed_time = DateTime.Parse(gmt_create);
                        }
                    }
                    PayNotifyModel payNotifyModel = new PayNotifyModel() { Notify_id= notify_id, IsPaySuccess = true,
                        Transaction_id = trade_no, AttachData = record_id, Out_Trade_No = order_code
                    , Three_Pay_Type= "ALIPAY_APP", Completed_Trans_Time= completed_time
                    };

                    if (record_id.Equals("CouponsId"))
                    {
                        return DealWithCoupons(payNotifyModel);
                    }
                    else
                    {
                        PayBody body = PayBody.DecryptBody(record_id, order_code);
                        //订单校验完成，进行订单处理
                        int intResult = tradeBiz.DealWithPayCompleteTrade(body, payNotifyModel.Transaction_id, payNotifyModel.Notify_id, "Alipay",payNotifyModel.Three_Pay_Type, payNotifyModel.Completed_Trans_Time);
                        // lcl 2018-10-21 Insert
                        if (intResult == 0)
                        {
                            try
                            {
                                // 调用卡券平台的支付接口
                                CallPayFromCardPlatform(body.uid, body.order_list.Split(',')[0]);
                            }
                            catch (Exception ex)
                            {
                                tradeBiz.LogOrderPay(body.uid, $@"调用卡券支付接口(支付宝) -- {ex.StackTrace}");
                            }
                        }


                        return "success";
                    }
                }
            }
            return "failed";
        }

        /// <summary>
        /// 微信APP回调
        /// </summary>
        /// <returns>回调结果</returns>
        [HttpPost]
        public string wxpay_notify()
        {
            var intLen = Convert.ToInt32(Request.InputStream.Length);
            var b = new byte[intLen];
            Request.InputStream.Read(b, 0, intLen);
            var resultFromWx = Encoding.UTF8.GetString(b);
            var result = XDocument.Parse(resultFromWx);
            var return_code = result.Element("xml").Element("return_code").Value;//返回的状态SUCCESS为支付成功
            var result_code = result.Element("xml").Element("result_code").Value;//返回的状态SUCCESS为支付成功

            if (return_code == "SUCCESS" && result_code == "SUCCESS")
            {
                var transaction_id = result.Element("xml").Element("transaction_id").Value;
                var record_id = result.Element("xml").Element("attach").Value;
                var trade_type = result.Element("xml").Element("trade_type").Value;
                var order_code = result.Element("xml").Element("out_trade_no").Value;
                DateTime completed_time = DateTime.Now;
                string time_end = result.Element("xml").Element("time_end").Value;
                if (!string.IsNullOrWhiteSpace(time_end))
                {
                    completed_time = DateTime.ParseExact(time_end, "yyyyMMddHHmmss",CultureInfo.InvariantCulture );
                }

                PayNotifyModel payNotifyModel = new PayNotifyModel() { IsPaySuccess = true, Transaction_id = transaction_id,
                    AttachData = record_id, Out_Trade_No = order_code,
                    Three_Pay_Type = "WX_" + trade_type.ToUpper(),
                    Completed_Trans_Time = completed_time
                };
                if (record_id.Equals("CouponsId"))
                {
                    return DealWithCoupons(payNotifyModel);
                }
                else
                {
                    var req = new WxPayData();
                    WxPayData data = null;
                    //string tradeReturnCode;
                    //string tradeResultCode;
                    req.SetValue("transaction_id", transaction_id);
                    int intResult = -99;
                    try
                    {
                        var body = PayBody.DecryptBody(record_id, order_code);
                        intResult = tradeBiz.DealWithPayCompleteTrade(body, payNotifyModel.Transaction_id, "", "WX_" + trade_type, payNotifyModel.Three_Pay_Type, payNotifyModel.Completed_Trans_Time);
                        //int res;
                        //{
                        //    //处理订单:改变订单状态，添加现金记录，修改用户账户，添加交易记录
                        //    res = PayBody.MarkOrderConfirm(body);
                        //}
                        data = WxPayApi.OrderQuery(req);
                        //tradeReturnCode = data.GetValue("return_code").ToString();
                        //tradeResultCode = data.GetValue("result_code").ToString();

                        // lcl 2018-10-21 Insert
                        if (intResult == 0)
                        {
                            try
                            {
                                // 调用卡券平台的支付接口
                                CallPayFromCardPlatform(body.uid, body.order_list.Split(',')[0]);
                            }
                            catch (Exception ex)
                            {
                                tradeBiz.LogOrderPay(body.uid, $@"调用卡券支付接口(微信) -- {ex.StackTrace}");
                            }
                        }
                    }
                    catch
                    {
                        //tradeReturnCode = "SUCCESS";
                        //tradeResultCode = "SUCCESS";
                    }
                    return "success";
                }
            }
            else
            {
                return "failed";
            }
        }


        private string DealWithCoupons(PayNotifyModel payNotifyModel)
        {
            string order_code = payNotifyModel.Out_Trade_No;
            var db = new RRLDB();
            try
            {
                var ds = db.ExeQuery($@"SELECT TOP(1) * FROM rrl_coupons WHERE order_code = '{order_code}'");
                bool is_paid = Convert.ToBoolean(ds.Tables[0].Rows[0]["is_paid"]);
                ds = db.ExeQuery($@"SELECT DISTINCT is_paid,uid FROM rrl_coupons WHERE order_code = '{order_code}'");
                var count = ds.Tables[0].Rows.Count;
                if (count == 1 && !is_paid)
                {
                    // lcl 2018-07-27 Modify
                    ds = db.ExeQuery($@"SELECT SUM(goldbean) as h_money,MAX(uid) as uid,SUM(moeny) as money, SUM (goldcoin) AS coin,sum(isnull(redpacket,0)) as redpacket ,max(type) as type ,max(relation_trans_id) as relation_trans_id FROM rrl_coupons where order_code = '{order_code}'");
                    var hMoney = Convert.ToDouble(ds.Tables[0].Rows[0]["h_money"]);
                    var uid = Convert.ToInt32(ds.Tables[0].Rows[0]["uid"]);
                    var money = Convert.ToDouble(ds.Tables[0].Rows[0]["money"]);
                    var coin = Convert.ToDouble(ds.Tables[0].Rows[0]["coin"]);
                    string strType = Convert.ToString(ds.Tables[0].Rows[0]["type"]);

                    if (strType == "2")
                    {
                        // 充值送红包 (注：从调用充值到支付完成回调可能存在跨天的问题，所以不加receive_date的日期条件)
                        var dsData = db.ExeQuery($@"select top 1 isnull(h_money_free,0) as h_money_free ,receive_date ,receive_times from game_auto_redpackage_daily(nolock) where (uid = {uid}) and (is_receive = 0) order by addtime desc");
                        if (dsData != null && dsData.Tables.Count > 0 && dsData.Tables[0].Rows.Count > 0)
                        {
                            decimal mH_Money_Free = Convert.ToDecimal(dsData.Tables[0].Rows[0]["h_money_free"]);
                            DateTime dtReceiveDate = Convert.ToDateTime(dsData.Tables[0].Rows[0]["receive_date"]);
                            int intReceiveTimes = Convert.ToInt32(dsData.Tables[0].Rows[0]["receive_times"]);
                            int intResult = db.ExeCMD($@"update game_auto_redpackage_daily set is_receive = 1 ,pay_callback_time = getdate() where (uid = {uid}) and (is_receive = 0) and (receive_date = '{dtReceiveDate}') and (receive_times = {intReceiveTimes})");
                            if (intResult > 0)
                            {
                                // 红包领取状态更新成功才执行用户账户操作
                                // 更新用户账户
                                db.ExeCMD($@"update rrl_user set h_money = h_money + {hMoney} ,h_money_free = isnull(h_money_free,0) + {mH_Money_Free} ,pay_coupons_total_money = isnull(pay_coupons_total_money,0) + {money} where id = {uid}");
                                // 记录资金流水
                                db.ExeCMD($@"insert into rrl_user_money_record ([uid] ,[money] ,[type] ,[remark]) values ({uid} ,-{money} ,304 ,N'送红包的充值购券消费')");
                                db.ExeCMD($@"insert into rrl_user_money_record ([uid] ,[money] ,[type] ,[remark]) values ({uid} ,{mH_Money_Free} ,305 ,N'充值购券{money}送红包')");
                                if (hMoney > 0.1d)
                                {
                                    db.ExeCMD($@"insert into rrl_user_money_record ([uid] ,[money] ,[type] ,[remark]) values ({uid} ,{hMoney} ,306 ,N'充值购券{money}送金豆')");
                                }
                                // 更新购券数据
                                db.ExeCMD($@"update rrl_coupons SET is_paid = 1,three_trans_id='{payNotifyModel.Transaction_id}',three_notify_id='{payNotifyModel.Notify_id}',three_pay_type='{payNotifyModel.Three_Pay_Type}',three_completed_trans_time='{payNotifyModel.Completed_Trans_Time.ToString("yyyy-MM-dd HH:mm:ss")}' where order_code = '{order_code}'");
                            }
                        }
                    }
                    else if (strType == "4")
                    {
                        // 分时段充值送红包
                        string strTransId = Convert.ToString(ds.Tables[0].Rows[0]["relation_trans_id"]);
                        int intResult = db.ExeCMD($@"update time_interval_redpaket_receive set is_paid = 1 where (receive_id = '{strTransId}') and (is_paid = 0)");
                        if (intResult > 0)
                        {
                            decimal mH_Money_Free = 0;
                            if (decimal.TryParse(ds.Tables[0].Rows[0]["redpacket"].ToString(), out decimal redpacket_try))
                            {
                                mH_Money_Free = redpacket_try;
                            }
                            // 红包领取状态更新成功才执行用户账户操作
                            // 更新用户账户
                            // lcl 2018-10-19 Modify 将送免费红包修改为赠送小红包，并且数额不累加
                            //db.ExeCMD($@"update rrl_user set h_money = h_money + {hMoney} ,h_money_free = isnull(h_money_free,0) + {mH_Money_Free} ,pay_coupons_total_money = isnull(pay_coupons_total_money,0) + {money} where id = {uid}");
                            double h_money_pay_old = 0;
                            double rateof_vmoney_to_needplay = 1.5;
                            ds = db.ExeQuery($@"select [Value] from rrl_config where Item='rateof_vmoney_to_needplay'");
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                rateof_vmoney_to_needplay = double.Parse(ds.Tables[0].Rows[0][0].ToString());
                            }
                            int new_need_play_conut = Convert.ToInt32((Convert.ToDouble(mH_Money_Free)-money) / rateof_vmoney_to_needplay);
                            if (new_need_play_conut == 0) new_need_play_conut = 1;

                            bool bComplete = GameManager.IsCompletePlayCountToday(uid,out h_money_pay_old);
                            int res = db.ExeCMD($@"update rrl_user set h_money = h_money + {hMoney} ,h_money_pay = {mH_Money_Free},need_play_conut={new_need_play_conut} ,pay_coupons_total_money = isnull(pay_coupons_total_money,0) + {money} where id = {uid}");
                            if (res>0)//更新局数
                            {
                                if(bComplete)//转金豆
                                {
                                    db.ExeCMD($@"update rrl_user set h_money=h_money+{h_money_pay_old} where id={uid}");
                                }
                                string today = DateTime.Now.ToString("yyyy-MM-dd");
                                db.ExeCMD($@"update game_user_daily_count set count = 0 where uid = {uid} and date='{today}'");

                            }
                            // 记录资金流水
                            db.ExeCMD($@"insert into rrl_user_money_record ([uid] ,[money] ,[type] ,[remark]) values ({uid} ,-{money} ,304 ,N'分时段充值送红包的充值购券消费')");
                            db.ExeCMD($@"insert into rrl_user_money_record ([uid] ,[money] ,[type] ,[remark]) values ({uid} ,{mH_Money_Free} ,305 ,N'分时段充值购券{money}送红包')");
                            if (hMoney > 0.1d)
                            {
                                db.ExeCMD($@"insert into rrl_user_money_record ([uid] ,[money] ,[type] ,[remark]) values ({uid} ,{hMoney} ,306 ,N'分时段充值购券{money}送金豆')");
                            }
                            // 更新购券数据
                            db.ExeCMD($@"update rrl_coupons SET is_paid = 1,three_trans_id='{payNotifyModel.Transaction_id}',three_notify_id='{payNotifyModel.Notify_id}',three_pay_type='{payNotifyModel.Three_Pay_Type}',three_completed_trans_time='{payNotifyModel.Completed_Trans_Time.ToString("yyyy-MM-dd HH:mm:ss")}' where order_code = '{order_code}'");
                        }
                    }
                    else
                    {
                        // 其他场景的购券
                        decimal redpacket = 0;
                        if (decimal.TryParse(ds.Tables[0].Rows[0]["redpacket"].ToString(), out decimal redpacket_try))
                        {
                            redpacket = redpacket_try;
                        }
                        db.ExeCMD($@"UPDATE rrl_user SET h_money_pay=isnull(h_money_pay,0)+{redpacket}, h_money = h_money + {hMoney},plate_to_return_money = plate_to_return_money + {coin},pay_coupons_total_money=isnull(pay_coupons_total_money,0)+{money} WHERE id = {uid}");
                        db.ExeCMD($@"INSERT INTO [dbo].[rrl_user_money_record] ([uid], [money],[type], [remark]) VALUES ({uid}, -{money},{1} ,N'购入金豆消费')");
                        db.ExeCMD($@"INSERT INTO [dbo].[rrl_user_money_record] ([uid], [money],[type], [remark]) VALUES ({uid}, {hMoney},{14} ,N'购入金豆')");
                        db.ExeCMD($@"UPDATE rrl_coupons SET is_paid = 1,three_trans_id='{payNotifyModel.Transaction_id}',three_notify_id='{payNotifyModel.Notify_id}',three_pay_type='{payNotifyModel.Three_Pay_Type}',three_completed_trans_time='{payNotifyModel.Completed_Trans_Time.ToString("yyyy-MM-dd HH:mm:ss")}' WHERE order_code = '{order_code}'");
                        if (coin > 0)
                        {
                            db.ExeCMD($@"INSERT INTO rrl_user_plate_to_return_record (uid,order_id,money,type,remark) VALUES({uid},0,{coin},5,N'购买金币购物卡');");
                        }
                        if (redpacket > 0)
                        {
                            db.ExeCMD($@"UPDATE rrl_user SET  rnd_pay_redpacket=0 WHERE id = {uid}");
                            db.ExeCMD($@"INSERT INTO [dbo].[rrl_user_money_record] ([uid], [money],[type], [remark]) VALUES ({uid}, {money},100 ,N'购入小红包')");
                        }
                    }
                }
                return "success";
            }
            //catch (Exception ex)
            //{
            //    System.IO.File.AppendAllText(@"C:\SDOUDOU\logs\paylog.txt", $@"{ex.Message}|||{ex.StackTrace}");
            //    return "success";
            //}
            finally
            {
                db.Close();
            }

        }

        // lcl 2018-10-21 Insert
        /// <summary>
        /// 调用卡券平台的支付接口
        /// </summary>
        /// <param name="orderId">3.0中的订单编号</param>
        //private void CallPayFromCardPlatform(string orderId)
        public void CallPayFromCardPlatform(int uid, string orderId)
        {
            SqlDataBase sqlDB = new SqlDataBase();
            string strSql = string.Empty;

            // 根据订单ID查找3.0订单数据
            strSql = @"select ord.ordercode ,g.card_goods_id
                         from rrl_order ord
                        inner join rrl_order_info_goods g
                           on ord.id = g.order_id
                        where (ord.id = @id)";
            var orderInfo = sqlDB.Single<dynamic>(strSql, new { id = orderId });
            if (orderInfo == null)
            {
                // 写日志
                tradeBiz.LogOrderPay(uid, $@"调用卡券支付接口之前，查找订单数据 -- 订单ID【{orderId}】未找到");
                return;
            }

            if (orderInfo.card_goods_id == null)
            {
                // 不是卡券商城的商品
                return;
            }

            // 构造支付接口
            string strPayUrl = $@"{RRLConfig.Card_Platform_Pay_Url}?cardNum={orderInfo.card_goods_id}&num=1&dingdang={orderInfo.ordercode}";
            // 调用接口并解析返回结果
            string strResponseJson = MyHttpHelper.HttpGet(strPayUrl);

            // TODO 写卡券接口调用日志

            var jsonData = JsonConvert.DeserializeObject<dynamic>(strResponseJson);
            if (jsonData.code == true)
            {
                // 将卡号、卡密码更新到3.0订单表
                var cardInfo = jsonData.card;
                sqlDB.Execute("update rrl_order set receive_name = @receive_name ,receive_address = @receive_address where (id = @id)",
                    new { receive_name = cardInfo.card.ToString(), receive_address = cardInfo.pwd.ToString(), id = orderId });
            }
        }
    }
}