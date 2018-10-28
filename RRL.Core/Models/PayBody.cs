using Newtonsoft.Json;
using RRL.Core.External;
using RRL.Core.Utility;
using RRL.DB;
using System;
using System.Data;
using System.Data.SqlClient;
using RRL.Core.Manager;

namespace RRL.Core.Models
{
    public class PayBody
    {
        public int uid = 0;
        public string order_list = "";
        public double money = 0;
        public string order_code = "";
        public double discount = 0;
        public double postage = 0;
        public string spreador = "";
        public bool isticket = false;
        public int ticketHmoney = 0;

        /// <summary>
        /// 加密PayBody
        /// </summary>
        /// <param name="body">PayBody</param>
        /// <returns>密文记录id</returns>
        public static string EncryptBody(PayBody body)
        {
            int record_id;
            string json = JsonConvert.SerializeObject(body, Formatting.Indented);
            string encrypt_body = DES.EncryptDES(json);
            RRLDB db = new RRLDB();
            int res = db.ExecSP(@"sp_TRANS_BODY_ADD",
                new SqlParameter("@order_code", body.order_code),
                new SqlParameter("@encrypt_body", encrypt_body));
            record_id = Convert.ToInt32(db.sm.Parameters["@return"].Value);
            if (res <= 0)
            {
                record_id = 0;
            }
            db.Close();
            return record_id.ToString();
        }

        /// <summary>
        /// 解密PayBody
        /// </summary>
        /// <param name="record_id">密文记录id</param>
        /// <param name="order_code">订单号</param>
        /// <returns>PayBody</returns>
        public static PayBody DecryptBody(string record_id, string order_code)
        {
            RRLDB db = new RRLDB();
            DataSet ds = db.ExeQuery(string.Format(@"SELECT encrypt_body FROM rrl_trans_body_record WHERE id = {0} AND order_code = {1}", record_id, order_code));
            string decrypt_body = DES.DecryptDES(ds.Tables[0].Rows[0]["encrypt_body"].ToString());
            db.Close();
            if (ds.Tables[0].Rows.Count > 0)
            {
                return JsonConvert.DeserializeObject<PayBody>(decrypt_body);
            }
            return new PayBody();
        }

        /// <summary>
        /// 标记异常订单
        /// </summary>
        /// <param name="order_list"></param>
        /// <returns></returns>
        public static int MarkOrderExce(string order_list)
        {
            RRLDB db = new RRLDB();
            string presql = @"UPDATE rrl_order SET is_paid=1,status = -2 WHERE id IN ({0})";
            string sql = string.Format(presql, order_list);
            int res = db.ExeCMD(sql);
            db.Close();
            if (res <= 0)
            {
                return MessageCode.ERROR_EXECUTE_SQL;
            }
            return MessageCode.SUCCESS;
        }

        /// <summary>
        /// 标记订单已付款
        /// </summary>
        /// <param name="body">支付数据</param>
        /// <returns>标记结果</returns>
        public static int MarkOrderConfirm(PayBody body)
        {
            //HelpManager.Mark($@"Confirm-First-Step || body:{JsonConvert.SerializeObject(body)}");
            //HelpManager.Mark(JsonConvert.SerializeObject(body, Formatting.Indented));
            int res;
            var db = new RRLDB();

            var ds = db.ExeQuery($@"SELECT TOP(1) is_paid FROM rrl_order WHERE [ordercode] = '{body.order_code}'");
            var isdealed = Convert.ToBoolean(ds.Tables[0].Rows[0][0]);
            ds = db.ExeQuery($@"SELECT COUNT(*) FROM rrl_user_money_record WHERE order_id = (SELECT id FROM rrl_order WHERE ordercode = '{body.order_code}')");
            var count = Convert.ToInt32(ds.Tables[0].Rows[0][0]);

            //HelpManager.Mark($@"Confirm-Mark || isdeal:{isdealed},count:{count}");

            if (!isdealed)
            {
                //HelpManager.Mark($@"Confirm-Third-Step || init");
                //改变订单状态
                res = db.ExeCMD($@"UPDATE rrl_order SET is_paid=1,status=2 WHERE id IN ({body.order_list}) AND status = 1");
                //改变订单状态,更新用户账户，添加各种记录
                //这个时候的订单combine_code = order_code,is_paid=1,status=2,通过以上三个状态进行订单过滤
                res = db.ExecSP(@"sp_ORDER_CONFIRM_PAYMENT",
                    new SqlParameter("@uid", SqlDbType.Int, 4) { Value = body.uid },
                    new SqlParameter("@order_code", body.order_code),
                    new SqlParameter("@money", SqlDbType.Decimal, 8) { Value = body.money, Precision = 12, Scale = 2 },
                    new SqlParameter("@discount", SqlDbType.Decimal, 8) { Value = body.discount, Precision = 12, Scale = 2 },
                    new SqlParameter("@postage", SqlDbType.Decimal, 8) { Value = body.postage, Precision = 12, Scale = 2 });
                int uid = Convert.ToInt32(db.sm.Parameters["@return"].Value);

                if (body.isticket)
                {
                    db.ExeCMD($@"UPDATE rrl_user SET h_money = h_money - {body.ticketHmoney} WHERE id = {body.uid}");
                    db.ExeCMD(
                        $@"INSERT INTO rrl_user_money_record (uid,order_id,money,type,remark) VALUES({body.uid},{
                                body.order_list.Split(',')[0]
                            },-{body.ticketHmoney},13,'金豆购物消费')");
                }

                if (uid != 0)
                {
                    try
                    {
                        ds = db.ExeQuery($"SELECT username FROM rrl_user WHERE id = {uid}");

                        var mobile = Convert.ToString(ds.Tables[0].Rows[0][0]);
                        var sms = new AliSMS();
                        sms.SendSMS(mobile, SMSTemplate.ORDER_PAYMENT_SUCCESS_SMS, new OrderPaymentSuccessSMS());

                        try
                        {
                            var user = new UserAuth(body.uid);
                            if (!string.IsNullOrWhiteSpace(user.wx_mp_open_id))
                            {
                                ds = db.ExeQuery($@"SELECT rrl_goods.name,rrl_order_info_goods.goods_price,rrl_order_info_goods.goods_count FROM rrl_order_info_goods LEFT JOIN rrl_goods ON rrl_order_info_goods.goods_id = rrl_goods.id WHERE rrl_order_info_goods.order_id IN ({body.order_list})");
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
                            // ignored
                        }

                        //ds = db.ExeQuery(string.Format(@"SELECT COUNT(1) FROM rrl_order WHERE buyer_id = {0} AND is_paid = 1", uid));

                        //int count = Convert.ToInt32(ds.Tables[0].Rows[0][0]);

                        //if (count!=0 && body.spreador.Length == 11)
                        //{
                        //    //ds = db.ExeQuery(string.Format(@"SELECT spreader_uid FROM rrl_user WHERE id = {0}", uid));
                        //    //int sperader_uid = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
                        //    //if (sperader_uid == 0)
                        //    //{
                        //    //    db.ExeCMD(string.Format(@"UPDATE rrl_user SET spreader_uid = (SELECT id FROM rrl_user WHERE username = {0}) WHERE id = {1}", body.spreador, uid));
                        //    //}
                        //}
                    }
                    catch
                    {
                        //HelpManager.Mark($@"Confirm-Mark || Exception");
                        // ignored
                    }
                    
                }
                db.Close();
                if (res <= 0)
                {
                    return MessageCode.ERROR_EXECUTE_SQL;
                }
                return MessageCode.SUCCESS;

            }

            return MessageCode.ERROR_EXECUTE_SQL;

        }
    }
}