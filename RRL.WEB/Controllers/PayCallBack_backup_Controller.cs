using RRL.Core.Models;
using RRL.Core.Pay.AliPay;
using RRL.Core.Pay.WxPay;
using RRL.Core.Utility;
using RRL.DB;
using System;
using System.Text;
using System.Web.Mvc;
using System.Xml.Linq;

namespace RRL.WEB.Controllers
{
    /// <summary>
    /// 支付回调
    /// </summary>
    public class PayCallBack_backup_Controller : Controller
    {
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
            alipayNotifyURL = alipayNotifyURL + "&partner=" + partner + "&notify_id=" + Request.Form["notify_id"];
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
                    if (record_id.Equals("CouponsId"))
                    {
                        var db = new RRLDB();
                        try
                        {
                            var ds = db.ExeQuery($@"SELECT TOP(1) * FROM rrl_coupons WHERE order_code = '{order_code}'");
                            bool is_paid = Convert.ToBoolean(ds.Tables[0].Rows[0]["is_paid"]);
                            ds = db.ExeQuery($@"SELECT DISTINCT is_paid,uid FROM rrl_coupons WHERE order_code = '{order_code}'");
                            var count = ds.Tables[0].Rows.Count;
                            if (count == 1 && !is_paid)
                            {
                                ds = db.ExeQuery($@"SELECT SUM(goldbean) as h_money,MAX(uid) as uid,SUM(moeny) as money, SUM (goldcoin) AS coin,sum(isnull(redpacket,0)) as redpacket FROM rrl_coupons WHERE order_code = '{order_code}'");
                                var hMoney = Convert.ToDouble(ds.Tables[0].Rows[0]["h_money"]);
                                var uid = Convert.ToInt32(ds.Tables[0].Rows[0]["uid"]);
                                var money = Convert.ToDouble(ds.Tables[0].Rows[0]["money"]);
                                var coin = Convert.ToDouble(ds.Tables[0].Rows[0]["coin"]);
                                decimal redpacket = 0;
                                if(decimal.TryParse(ds.Tables[0].Rows[0]["redpacket"].ToString(),out decimal redpacket_try))
                                {
                                    redpacket = redpacket_try;
                                }
                                db.ExeCMD($@"UPDATE rrl_user SET h_money_pay=isnull(h_money_pay,0)+{redpacket}, h_money = h_money + {hMoney},plate_to_return_money = plate_to_return_money + {coin} WHERE id = {uid}");
                                db.ExeCMD($@"INSERT INTO [dbo].[rrl_user_money_record] ([uid], [money],[type], [remark]) VALUES ({uid}, -{money},{1} ,N'购入金豆消费')");
                                db.ExeCMD($@"INSERT INTO [dbo].[rrl_user_money_record] ([uid], [money],[type], [remark]) VALUES ({uid}, {hMoney},{14} ,N'购入金豆')");
                                db.ExeCMD($@"UPDATE rrl_coupons SET is_paid = 1 WHERE order_code = '{order_code}'");
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
                        catch
                        {
                            // ignored
                        }
                        db.Close();
                        return "success";
                    }
                    else
                    {
                        PayBody body = PayBody.DecryptBody(record_id, order_code);
                        //订单校验完成，进行订单处理
                        int res;
                        if (body.uid == 0)
                        {
                            //标记订单异常
                            res = PayBody.MarkOrderExce(body.order_list);
                            if (res != MessageCode.SUCCESS)
                            {
                                return "failed";
                            }
                        }
                        else
                        {
                            //处理订单:改变订单状态，添加现金记录，修改用户账户，添加交易记录
                            res = PayBody.MarkOrderConfirm(body);
                            //if (body.isticket)
                            //{
                            //    var db = new RRLDB();
                            //    db.ExeCMD($@"UPDATE rrl_user SET h_money = h_money - { body.money * 100 } WHERE id = {body.uid}");
                            //    db.ExeCMD($@"INSERT INTO rrl_user_money_record (uid,order_id,money,type,remark) VALUES({body.uid},{body.order_list.Split(',')[0]},-{ body.money * 100 },13,'金豆购物消费')");
                            //    db.Close();
                            //}
                            if (res != MessageCode.SUCCESS)
                            {
                                return "failed";
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
                var order_code = result.Element("xml").Element("out_trade_no").Value;
                if (record_id.Equals("CouponsId"))
                {
                    var db = new RRLDB();
                    try
                    {
                        var ds = db.ExeQuery($@"SELECT TOP(1) * FROM rrl_coupons WHERE order_code = '{order_code}'");
                        var is_paid = Convert.ToBoolean(ds.Tables[0].Rows[0]["is_paid"]);
                        ds = db.ExeQuery($@"SELECT DISTINCT is_paid,uid FROM rrl_coupons WHERE order_code = '{order_code}'");
                        var count = ds.Tables[0].Rows.Count;
                        if (count == 1 && !is_paid)
                        {
                            ds = db.ExeQuery($@"SELECT SUM(goldbean) as h_money,MAX(uid) as uid,SUM(moeny) as money, SUM (goldcoin) AS coin,sum(isnull(redpacket,0)) as redpacket FROM rrl_coupons WHERE order_code = '{order_code}'");
                            var hMoney = Convert.ToDouble(ds.Tables[0].Rows[0]["h_money"]);// 购入金豆
                            var uid = Convert.ToInt32(ds.Tables[0].Rows[0]["uid"]);
                            var money = Convert.ToDouble(ds.Tables[0].Rows[0]["money"]);//花费的前
                            var coin = Convert.ToDouble(ds.Tables[0].Rows[0]["coin"]);
                            decimal redpacket = 0;
                            if (decimal.TryParse(ds.Tables[0].Rows[0]["redpacket"].ToString(), out decimal redpacket_try))
                            {
                                redpacket = redpacket_try;
                            }
                            db.ExeCMD($@"UPDATE rrl_user SET h_money_pay=isnull(h_money_pay,0)+{redpacket}, h_money = h_money + {hMoney},plate_to_return_money = plate_to_return_money + {coin} WHERE id = {uid}");
                            db.ExeCMD($@"INSERT INTO  [rrl_user_money_record] ([uid], [money],[type], [remark]) VALUES ({uid}, -{money},{1} ,N'购入金豆消费')");
                            db.ExeCMD($@"INSERT INTO  [rrl_user_money_record] ([uid], [money],[type], [remark]) VALUES ({uid}, {hMoney},{14} ,N'购入金豆')");
                            db.ExeCMD($@"UPDATE rrl_coupons SET is_paid = 1 WHERE order_code = '{order_code}'");
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
                    catch
                    {
                        // ignored
                    }
                    db.Close();
                    return "success";
                }
                else
                {
                    var req = new WxPayData();
                    WxPayData data = null;
                    string tradeReturnCode;
                    string tradeResultCode;
                    req.SetValue("transaction_id", transaction_id);
                    try
                    {
                        var body = PayBody.DecryptBody(record_id, order_code);
                        //订单校验完成，进行订单处理

                        //if (body.isticket)
                        //{
                        //    var db = new RRLDB();
                        //    db.ExeCMD($@"UPDATE rrl_user SET h_money = h_money - { body.money * 100 } WHERE id = {body.uid}");
                        //    db.ExeCMD($@"INSERT INTO rrl_user_money_record (uid,order_id,money,type,remark) VALUES({body.uid},{body.order_list.Split(',')[0]},-{ body.money * 100 },13,'金豆购物消费')");
                        //    db.Close();
                        //}
                        int res;
                        if (body.uid == 0)
                        {
                            //标记订单异常
                            res = PayBody.MarkOrderExce(body.order_list);
                        }
                        else
                        {
                            //处理订单:改变订单状态，添加现金记录，修改用户账户，添加交易记录
                            res = PayBody.MarkOrderConfirm(body);
                        }
                        data = WxPayApi.OrderQuery(req);
                        tradeReturnCode = data.GetValue("return_code").ToString();
                        tradeResultCode = data.GetValue("result_code").ToString();
                    }
                    catch
                    {
                        tradeReturnCode = "SUCCESS";
                        tradeResultCode = "SUCCESS";
                    }
                    return "success";
                }
            }
            else
            {
                return "failed";
            }
        }
    }
}