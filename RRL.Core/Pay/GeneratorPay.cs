using Aop.Api.Util;
using RRL.Core.Pay.AliPay;
using RRL.Core.Pay.WxPay;
using RRL.Core.Utility;
using System;
using System.Collections.Generic;
using System.Web;

namespace RRL.Core.Pay
{
    public class GeneratorPay
    {
        /// <summary>
        /// 创建支付宝支付请求
        /// </summary>
        /// <param name="body">支付描述</param>
        /// <param name="subject">支付标题</param>
        /// <param name="price">支付金额</param>
        /// <param name="ordercode">订单号</param>
        /// <returns>请求结果字符串</returns>
        public static string BuildAliPay(string body, string subject, string price, string ordercode)
        {
            Dictionary<string, string> payinfo = new Dictionary<string, string>();
            payinfo.Add("_input_charset", "\"" + AliPayConfig.input_charset + "\"");
            payinfo.Add("body", "\"" + body + "\"");
            payinfo.Add("timeout_express", "\"" + AliPayConfig.timeout_express + "\"");
            payinfo.Add("notify_url", "\"" + AliPayConfig.notify_url + "\"");
            payinfo.Add("out_trade_no", "\"" + ordercode + "\"");
            payinfo.Add("partner", "\"" + AliPayConfig.partner + "\"");
            payinfo.Add("seller_id", "\"" + AliPayConfig.seller_id + "\"");
            payinfo.Add("service", "\"" + AliPayConfig.service + "\"");
            payinfo.Add("subject", "\"" + subject + "\"");
            payinfo.Add("total_fee", "\"" + price + "\"");

            string orderInfo = AliPayCore.CreateLinkString(payinfo);
            string sign = AlipaySignature.RSASign(orderInfo, AliPayConfig.private_key_path, AliPayConfig.input_charset, AliPayConfig.sign_type);
            sign = HttpUtility.UrlEncode(sign, System.Text.Encoding.UTF8);
            string payInfo = orderInfo + "&sign=\"" + sign + "\"&"
                + "sign_type=\"RSA\"";
            //HelpManager.Mark(payInfo);
            return payInfo;
        }

        /// <summary>
        /// 创建微信支付请求
        /// </summary>
        /// <param name="add_att">支付描述</param>
        /// <param name="subject">支付标题</param>
        /// <param name="price">支付价格</param>
        /// <param name="ordercode">订单号</param>
        /// <param name="IP">IP</param>
        /// <returns>请求字符串</returns>
        public static string BuildWxPay(string add_att, string subject, string price, string ordercode, string IP)
        {
            int fee = Convert.ToInt32(Convert.ToDouble(price) * 100);
            string url = "https://api.mch.weixin.qq.com/pay/unifiedorder";
            string attach = add_att;//附加数据
            string body = subject;//商品描述
            string out_trade_no = ordercode;//订单号
            string spbill_create_ip = IP;
            string total_fee = fee.ToString();//交易金额 分
            string trade_type = "APP";//交易类型
            string nonce_str = WxPayApi.GenerateNonceStr();

            string stringA = string.Format(@"appid={0}&attach={1}&body={2}&mch_id={3}&nonce_str={4}&notify_url={5}&out_trade_no={6}&spbill_create_ip={7}&total_fee={8}&trade_type={9}",
                WxPayConfig.APPID,
                attach,
                body,
                WxPayConfig.MCHID,
                nonce_str,
                WxPayConfig.NOTIFY_URL,
                out_trade_no,
                spbill_create_ip,
                total_fee,
                trade_type);

            string stringSignTemp = stringA + "&key=" + WxPayConfig.KEY;

            WxPayData data_prepayid = null;

            try
            {
                string sign = PublicAPI.GetMD5Hash(stringSignTemp).ToUpper();

                data_prepayid = new WxPayData();
                data_prepayid.SetValue("appid", WxPayConfig.APPID);
                data_prepayid.SetValue("attach", attach);
                data_prepayid.SetValue("body", body);
                data_prepayid.SetValue("mch_id", WxPayConfig.MCHID);
                data_prepayid.SetValue("nonce_str", nonce_str);
                data_prepayid.SetValue("notify_url", WxPayConfig.NOTIFY_URL);
                //data_prepayid.SetValue("openid", openid);
                data_prepayid.SetValue("out_trade_no", out_trade_no);
                data_prepayid.SetValue("spbill_create_ip", spbill_create_ip);
                data_prepayid.SetValue("total_fee", total_fee);
                data_prepayid.SetValue("trade_type", trade_type);
                data_prepayid.SetValue("sign", sign);
            }
            catch (Exception ex)
            {
            }

            string xml = data_prepayid.ToXml();
            Log.Debug("WxPayApi", "UnfiedOrder request : " + xml);
            string response = "";
            string code = "FAILED";
            WxPayData result = new WxPayData();

            try
            {
                byte[] data = System.Text.Encoding.UTF8.GetBytes(xml);
                response = PublicAPI.HttpPost(url, data);
                result.FromXml(response);
                code = result.GetValue("result_code").ToString();
            }
            catch (Exception ex)
            {
                code = "FAILED";
            }

            if (code != "SUCCESS")
            {
                return "";
            }

            WxPayData data_json = new WxPayData();
            data_json.SetValue("appid", WxPayConfig.APPID);
            data_json.SetValue("partnerid", WxPayConfig.MCHID);
            data_json.SetValue("package", "Sign=WXPay");
            data_json.SetValue("noncestr", nonce_str);
            data_json.SetValue("timestamp", WxPayApi.GenerateTimeStamp());
            data_json.SetValue("prepayid", result.GetValue("prepay_id"));
            data_json.SetValue("sign", data_json.MakeSign());
            string res = data_json.ToJson();
            //todo
            return res;
        }

        /// <summary>
        /// 创建微信JS支付请求,获取预支付订单号
        /// </summary>
        /// <param name="add_att">支付描述</param>
        /// <param name="subject">支付标题</param>
        /// <param name="price">支付价格</param>
        /// <param name="ordercode">订单号</param>
        /// <param name="IP">IP</param>
        /// <returns>预支付订单号</returns>
        public static string BuildWxJsPrepayID(string add_att, string subject, string price, string ordercode, string IP, string openid)
        {
            int fee = Convert.ToInt32(Convert.ToDouble(price) * 100);
            string url = "https://api.mch.weixin.qq.com/pay/unifiedorder";
            string attach = add_att;//附加数据
            string body = ordercode;//商品描述
            string out_trade_no = ordercode;//订单号
            string spbill_create_ip = IP;
            string total_fee = fee.ToString();//交易金额 分
            string trade_type = "JSAPI";//交易类型
            string nonce_str = WxPayApi.GenerateNonceStr();

            string stringA = string.Format(@"appid={0}&attach={1}&body={2}&mch_id={3}&nonce_str={4}&notify_url={5}&openid={6}&out_trade_no={7}&spbill_create_ip={8}&total_fee={9}&trade_type={10}",
                WxPayConfig.WxMPAPPID,
                attach,
                body,
                WxPayConfig.WxMPMCHID,
                nonce_str,
                WxPayConfig.NOTIFY_URL,
                openid,
                out_trade_no,
                spbill_create_ip,
                total_fee,
                trade_type);

            string stringSignTemp = stringA + "&key=" + WxPayConfig.WxMPKEY;

            WxPayData data_prepayid = null;

            try
            {
                string sign = PublicAPI.GetMD5Hash(stringSignTemp).ToUpper();

                data_prepayid = new WxPayData();
                data_prepayid.SetValue("appid", WxPayConfig.WxMPAPPID);
                data_prepayid.SetValue("attach", attach);
                data_prepayid.SetValue("body", body);
                data_prepayid.SetValue("mch_id", WxPayConfig.WxMPMCHID);
                data_prepayid.SetValue("nonce_str", nonce_str);
                data_prepayid.SetValue("notify_url", WxPayConfig.NOTIFY_URL);
                data_prepayid.SetValue("openid", openid);
                data_prepayid.SetValue("out_trade_no", out_trade_no);
                data_prepayid.SetValue("spbill_create_ip", spbill_create_ip);
                data_prepayid.SetValue("total_fee", total_fee);
                data_prepayid.SetValue("trade_type", trade_type);
                data_prepayid.SetValue("sign", sign);
            }
            catch (Exception ex)
            {
            }

            string xml = data_prepayid.ToXml();
            //Log.Debug("WxPayApi", "UnfiedOrder request : " + xml);
            string response = "";
            string code = "FAILED";
            WxPayData result = new WxPayData();
            string prepay_id = "";

            try
            {
                byte[] data = System.Text.Encoding.UTF8.GetBytes(xml);
                response = PublicAPI.HttpPost(url, data);
                result.FromXml(response);
                code = result.GetValue("result_code").ToString();
                prepay_id = result.GetValue("prepay_id").ToString();
            }
            catch (Exception ex)
            {
                code = "FAILED";
            }

            if (code != "SUCCESS")
            {
                return "";
            }

            return prepay_id;
        }

        /// <summary>
        /// 创建H5支付
        /// </summary>
        /// <param name="add_att"></param>
        /// <param name="subject"></param>
        /// <param name="price"></param>
        /// <param name="ordercode"></param>
        /// <param name="IP"></param>
        /// <returns></returns>
        public static string BuildWxNativePayUrl(string add_att, string subject, string price, string ordercode, string IP)
        {
            var nonceStr = WxPayApi.GenerateNonceStr();
            int fee = Convert.ToInt32(Convert.ToDouble(price) * 100);
            var data = new WxPayData();
            data.SetValue("appid", WxPayConfig.WxMPAPPID);
            data.SetValue("attach", add_att);
            data.SetValue("body", ordercode);
            data.SetValue("mch_id", WxPayConfig.WxMPMCHID);
            data.SetValue("nonce_str", nonceStr);
            data.SetValue("notify_url", WxPayConfig.NOTIFY_URL);
            data.SetValue("out_trade_no", ordercode);
            data.SetValue("spbill_create_ip", IP);
            data.SetValue("total_fee", fee.ToString());
            data.SetValue("trade_type", "NATIVE");
            data.SetValue("sign", data.MakeSign(WxPayConfig.WxMPKEY));

            string xml = data.ToXml();
            //Log.Debug("WxPayApi", "UnfiedOrder request : " + xml);
            string response = "";
            string code = "FAILED";
            WxPayData result = new WxPayData();
            string url = "https://api.mch.weixin.qq.com/pay/unifiedorder";
            try
            {
                byte[] bdata = System.Text.Encoding.UTF8.GetBytes(xml);
                response = PublicAPI.HttpPost(url, bdata);
                result.FromXml(response);
                code = result.GetValue("result_code").ToString();
            }
            catch (Exception ex)
            {
                code = "FAILED";
            }

            if (code != "SUCCESS")
            {
                return "";
            }
            url = result.GetValue("code_url").ToString();
            return url;
        }
    }
}