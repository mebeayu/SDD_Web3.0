using RRL.Core.Models;
using RRL.Core.Utility;
using RRL.WEB.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using RRL.WEB.Models.Request;
using RRL.Config;
using System.Dynamic;
using RRL.DB;
using System.Text;
using RRL.WEB.Ulity;
using Newtonsoft.Json;

namespace RRL.WEB.Areas.WebApi.Controllers
{
    public partial class TradeManagerController  
    {


        /// <summary>
        /// 通过商品创建订单
        /// </summary>
        /// <param name="goods_id">商品id</param>
        /// <param name="goods_count">商品数量</param>
        /// <param name="token">短效token</param>
        /// <returns>结果对象</returns>
        [HttpGet]
        [ActionName("CreateOrderFromGoodsV3")]
        public DataResult CreateOrderFromGoodsV3(int goods_id, int goods_count, string token)
        {
            List<int> order_list = new List<int>();
            int res;
            TokenObject Token = TokenObject.InitTokenObjFromString(token);
            if (!string.Equals(TokenObject.ShortTimeToken, Token.Prefix))
            {
                res = MessageCode.ERROR_TOKEN_VALIDATE;
            }
            else
            {
                UserAuth User = new UserAuth();
                res = User.Load(Token.id);
                if (res == MessageCode.SUCCESS)
                {
                    int? order_id = 0;
                    var s=  tm.CreateOrderFromGoodsV3(User.id, goods_id, goods_count,"","","","", out order_id);
                    var result = new DataResult() { status = s.status, message = s.message };// AutoMapper.Mapper.Map< BussResult, DataResult >(s);
                    if(order_id!=null)
                        order_list.Add(order_id.Value);
                    result.data = order_list;
                    return result;
                }
            }
            DataResult Resault = DataResult.InitFromMessageCode(res);
            Resault.data = order_list;
            return Resault;
        }
        /// <summary>
        /// 创建带推荐人的订单
        /// </summary>
        /// <param name="goods_id">商品id</param>
        /// <param name="goods_count">商品数量</param>
        /// <param name="token"></param>
        /// <param name="spreader_uid">推荐人id</param>
        /// <returns></returns>
        [HttpGet]
        [ActionName("CreateSpreaderOrderFromGoodsV3")]
        public DataResult CreateSpreaderOrderFromGoodsV3_(int goods_id, int goods_count, string token, int spreader_uid = 0)
        {
            List<int> order_list = new List<int>();
            int res;
            TokenObject Token = TokenObject.InitTokenObjFromString(token);
            if (!string.Equals(TokenObject.ShortTimeToken, Token.Prefix))
            {
                res = MessageCode.ERROR_TOKEN_VALIDATE;
            }
            else
            {
                UserAuth User = new UserAuth();
                res = User.Load(Token.id);
                if (res == MessageCode.SUCCESS)
                {
                    int? order_id = 0;
                    var s = tm.CreateOrderFromGoodsV3(User.id, goods_id, goods_count, "", "", "", "", out order_id,970, spreader_uid);
                    if (order_id != null)
                    {

                    }
                    var result = new DataResult() { status = s.status, message = s.message };// AutoMapper.Mapper.Map< BussResult, DataResult >(s);
                    if (order_id != null)
                        order_list.Add(order_id.Value);
                    result.data = order_list;
                    
                    return result;
                }
            }
            DataResult Resault = DataResult.InitFromMessageCode(res);
            Resault.data = order_list;
            return Resault;
        }
        /// <summary>
        /// 给当前用户添加推荐人
        /// </summary>
        /// <param name="token"></param>
        /// <param name="spreader_uid">推荐人id</param>
        /// <returns></returns>
        [HttpGet]
        [ActionName("AddToSpreader")]
        public DataResult AddToSpreader(string token, int spreader_uid)
        {
            int res;
            TokenObject Token = TokenObject.InitTokenObjFromString(token);
            if (!string.Equals(TokenObject.ShortTimeToken, Token.Prefix))
            {
                return DataResult.InitFromMessageCode( MessageCode.ERROR_TOKEN_VALIDATE);
            }
            else
            {
                int new_spreader_uid;
                res = tm.AddToSpreader(Token.id, spreader_uid,out new_spreader_uid);
                return DataResult.InitFromMessageCode(res);
            }
        }
        /// <summary>
        /// 通过订单数组获取预处理订单
        /// (预处理订单指尚未支付的订单，也就是说此接口无法获取状态1以外的订单)
        /// 该接口用来处理订单拼接支付前的预渲染
        /// </summary>
        /// <param name="orderlist">订单标识数组，格式：1，2，3</param>
        /// <param name="token">短效token</param>
        /// <returns>结果对象,additional_data:当前用户退款账户金额</returns>
        [HttpGet]
        [ActionName("PreOrderListV3")]
        public DataResult GetPreOrderListV3(string orderlist, string token)
        {
            int res;
            if (string.IsNullOrWhiteSpace(orderlist))
            {
                var dr = DataResult.InitFromMessageCode(99);
                dr.message = "订单列表为空！";
                return dr;
            }
            var Token = TokenObject.InitTokenObjFromString(token);
            if (!string.Equals(TokenObject.ShortTimeToken, Token.Prefix))
            {
                res = MessageCode.ERROR_TOKEN_VALIDATE;
            }
            else
            {
                var user = new UserAuth();
                res = user.Load(Token.id);
                if (res == MessageCode.SUCCESS)
                {
                    var ls = tm.GetPreOrderListV3(user.id, orderlist);
                    var resault = DataResult.InitFromMessageCode(res);
                    resault.data = ls;
                    resault.additional_data = user.x_money;
                    resault.add_data = new
                    {
                        h_money = user.h_money,
                        h_money_rate = "RMB1元 = 100金豆",
                        x_money= user.x_money
                    };
                    return resault;
                }
            }
            return DataResult.InitFromMessageCode(res);
        }


        /// <summary>
        /// 通过购物车创建订单
        /// </summary>
        /// <param name="cartlist">购物车列表，格式为：1，2，3</param>
        /// <param name="token">短效token</param>
        /// <returns>结果对象</returns>
        [HttpGet]
        [ActionName("CreateOrderFromCartV3")]
        public DataResult CreateOrderFromCartV3(string cartlist,  string token)
        {
            List<int> order_list = new List<int>();
            List<int> list = PublicAPI.StrToIntList(cartlist);
            int res;
            TokenObject Token = TokenObject.InitTokenObjFromString(token);
            if (!string.Equals(TokenObject.ShortTimeToken, Token.Prefix))
            {
                res = MessageCode.ERROR_TOKEN_VALIDATE;
            }
            else
            {
                UserAuth User = new UserAuth();
                res = User.Load(Token.id);
                if (res == MessageCode.SUCCESS)
                {
                    var bussResult = tm.CreateOrderFromCartV3(User.id, list, out order_list);
                    var result = new DataResult() { status = bussResult.status, message = bussResult.message };
                    result.data = order_list;
                    return result;
                }
            }
            DataResult Resault = DataResult.InitFromMessageCode(res);
            Resault.data = order_list;
            return Resault;
        }


        /// <summary>
        /// 通过购物车创建订单
        /// </summary>
        /// <param name="cartlist">购物车列表，格式为：1，2，3</param>
        /// <param name="token">短效token</param>
        /// <param name="discount_type">discount_type 0=正常比例,1=优惠比例(30%,70%)</param>
        /// <returns>结果对象</returns>
        [HttpGet]
        [ActionName("UpdateOrderIsBeansPayV3")]
        public DataResult UpdateOrderIsBeansPayV3(string orderlist, string is_beans_pay,string token,string discount_type="0")
        {
            if(string.IsNullOrWhiteSpace(orderlist))
            {
               return new DataResult() { status = 0, message = "orderlist参数为空" };
            }
            List<int> order_list = new List<int>();
            List<int> list = PublicAPI.StrToIntList(orderlist);
            int res;
            TokenObject Token = TokenObject.InitTokenObjFromString(token);
            if (!string.Equals(TokenObject.ShortTimeToken, Token.Prefix))
            {
                res = MessageCode.ERROR_TOKEN_VALIDATE;
            }
            else
            {
                UserAuth User = new UserAuth();
                res = User.Load(Token.id);
                if (res == MessageCode.SUCCESS)
                {
                    int status = tm.UpdateOrderIsBeansPayV3(User.id, orderlist, is_beans_pay, discount_type);
                    var result = new DataResult() { status = status, message = "ok" };
                    return result;
                }
            }
            DataResult Resault = DataResult.InitFromMessageCode(res);
            Resault.data = order_list;
            return Resault;
        }

        /// <summary>
        /// 更新商品数量或者商品备注信息
        /// </summary>
        /// <param name="order_id"></param>
        /// <param name="goods_id"></param>
        /// <param name="token"></param>
        /// <param name="goods_count"></param>
        /// <param name="msg_leave_word"></param>
        /// <param name="msg_phone"></param>
        /// <param name="msg_realname"></param>
        /// <param name="msg_idcardno"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("UpdateOrderGoodsAttrV3")]
        public DataResult UpdateOrderGoodsAttrV3(int order_id,int goods_id, string token,int? user_coupons_id=null, int? goods_count=null, string msg_leave_word=null, string msg_phone=null, string msg_realname=null, string msg_idcardno=null   )
        {
           
            int res;
            TokenObject Token = TokenObject.InitTokenObjFromString(token);
            if (!string.Equals(TokenObject.ShortTimeToken, Token.Prefix))
            {
                res = MessageCode.ERROR_TOKEN_VALIDATE;
            }
            else
            {
                UserAuth user = new UserAuth();
                res = user.Load(Token.id);
                if (res == MessageCode.SUCCESS)
                {
                    int count = tm.OrderInfoGoods_Update(user.id,   order_id,   goods_id,   goods_count,   msg_leave_word,   msg_phone,   msg_realname,   msg_idcardno, user_coupons_id);
                    int dcount = tm.UserCouponsApplyOrder(user.id, order_id, user_coupons_id);
                    
                    var resault = DataResult.InitFromMessageCode(res);
                    if (count==0)
                    {
                        resault.status = 99;
                        resault.message = "更新商品数据失败!";
                    }
                    var ls = tm.GetPreOrderListV3(user.id, order_id+"");
                    resault.data = ls;
                    resault.additional_data = user.x_money;
                    resault.add_data = new
                    {
                        h_money = user.h_money,
                        h_money_rate = "RMB1元 = 100金豆",
                        x_money = user.x_money
                    };
                    return resault;
                }
            }
            return DataResult.InitFromMessageCode(res);
 
        }

        /// <summary>
        /// 申请支付
        /// </summary>
        /// <param name="orderlist">订单数组,例如:1,2,3</param>
        /// <param name="discount">申请抵扣金币??没用</param>
        /// <param name="trans_type">支付类型:1,微信支付，2,支付宝支付，3,退款账户余额支付</param>
        /// <param name="IP">请求ip地址</param>
        /// <param name="token">短效token</param>
        /// <param name="is_beans_pay">"1"=用豆支付,"0"=纯现金支付</param>
        /// <returns>结果对象</returns>
        [HttpGet]
        [ActionName("ApplyPayVer3")]
        public DataResult ApplyPayForOrderListVer3(string orderlist, int trans_type, string token, string IP = null)
        {
            try
            {

                if (IP == null)
                {
                    IP = "0.0.0.0";
                }
                int res;
                DataResult result;
                string data = "";
                TokenObject Token = TokenObject.InitTokenObjFromString(token);
                if (!string.Equals(TokenObject.ShortTimeToken, Token.Prefix))
                {
                    res = MessageCode.ERROR_TOKEN_VALIDATE;
                }
                else
                {
                    UserAuth user = new UserAuth();
                    res = user.Load(Token.id);


                    if (res == MessageCode.SUCCESS)
                    {
                        if ("1".Equals(user.is_locked_login) || "1".Equals(user.is_locked_trade))
                        {
                            return new DataResult() { status = 99, message = "您的帐号被锁定,请用注册手机号码联系客服!" };
                        }
                        //总金币
                        //var goldBeans_count =(decimal) User.plate_to_return_money;// + User.ex_plate_to_return_money;
                        data = tm.ApplyPayV3(user, orderlist, trans_type, IP, out res);
                    }
                }
                result = DataResult.InitFromMessageCode(res);
                result.message = data;
                result.data = data;
                return result;
            }catch(Exception exs)
            {
                return new DataResult() { status = 99, message = "支付异常请联系客服" };
            }
        }

        // lcl 2018-07-28 Insert
        /// <summary>
        /// 二手一键转卖，计算用户可以兑换的卡面额和卡数量
        /// </summary>
        /// <param name="request">基础token请求</param>
        /// <returns></returns>
        [HttpGet]
        public DataResult SecondHandBuyCardData([FromUri]BaseTokenRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.token))
            {
                // token字符串无效，不做处理
                return DataResult.InitFromMessageCode(MessageCode.ERROR_TOKEN_LENGTH);
            }

            // 获取token对象
            TokenObject tokenObj = TokenObject.InitTokenObjFromString(request.token);
            UserAuth user = new UserAuth();
            int res = user.Load(tokenObj.id);
            if (res != MessageCode.SUCCESS)
            {
                // 加载用户数据失败，返回错误信息
                return DataResult.InitFromMessageCode(res);
            }

            decimal dGoldBean_To_RMB_Rate = RRLConfig.GoldBean_To_RMB_Rate; // 金豆兑换人民币的比率
            decimal dPlatform_Fee_Rate = RRLConfig.Platform_Fee_Rate; // 平台现金手续费比率

            // 用户现有金豆数
            decimal dH_Money = 0m;
            decimal.TryParse(user.h_money.ToString(), out dH_Money);
            var buyCardData = tm.SecondHandBuyCardCalculate(dH_Money);
            // 用户金豆总额
            buyCardData.goldBeanTotal = dH_Money;
            // 实际能够使用的金豆数量
            buyCardData.canUseGoldBeans = dH_Money - buyCardData.holdMoney;
            if (buyCardData.canUseGoldBeans < 0)
            {
                // 可用于购卡的金豆不能是负数（注：用户现有金豆数减去门槛数额，可能为负数）
                buyCardData.canUseGoldBeans = 0;
            }
            // 购卡后剩余的金豆数
            buyCardData.remainingGoldBeans = dH_Money - buyCardData.actualPayGoldBeans;
            // 平台手续费前的现金数额
            decimal dCardCashTotal = buyCardData.actualPayGoldBeans * dGoldBean_To_RMB_Rate;
            buyCardData.cardCashTotal = dCardCashTotal;
            // 平台手续费数额
            buyCardData.platformFeeTotal = dCardCashTotal * dPlatform_Fee_Rate;

            var result = DataResult.InitFromMessageCode(MessageCode.SUCCESS);
            result.data = buyCardData;

            return result;
        }

        // lcl 2018-07-28  Insert
        /// <summary>
        /// 二手一键转卖自动下单
        /// </summary>
        /// <param name="request">基础token请求</param>
        /// <returns></returns>
        [HttpGet]
        public DataResult SecondHandAutoCreateOrder([FromUri]SecondHandAutoOrderRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.token))
            {
                // token字符串无效，不做处理
                return DataResult.InitFromMessageCode(MessageCode.ERROR_TOKEN_LENGTH);
            }

            // 获取token对象
            TokenObject tokenObj = TokenObject.InitTokenObjFromString(request.token);
            UserAuth user = new UserAuth();
            int res = user.Load(tokenObj.id);
            if (res != MessageCode.SUCCESS)
            {
                // 加载用户数据失败，返回错误信息
                return DataResult.InitFromMessageCode(res);
            }

            string strPayAccount = HttpUtility.UrlDecode(request.payAccount);
            string strRealName = HttpUtility.UrlDecode(request.name);

            if (string.IsNullOrWhiteSpace(strPayAccount) || string.IsNullOrWhiteSpace(strRealName))
            {
                return new DataResult() { status = 99, message = "真实姓名和支付宝账号都必须填写" };
            }

            if (!user.level_category.ToUpper().Equals("A"))
            {
                return new DataResult() { status = 99, message = "用户类型不匹配" };
            }

            if (!tm.CheckPayAccount(user.id, strPayAccount))
            {
                return new DataResult() { status = 1001, message = "手机号码与支付宝账号不匹配" };
            }

            // lcl 2018-08-15 Insert
            if (user.is_locked_trade == "1" || user.is_locked_login == "1")
            {
                return new DataResult() { status = 1002, message = "账号已经被锁，不能完成交易" };
            }

            // 自动生成订单，并转入二手平台
            var execResult = SecondHandAutoCreateOrder(user, strPayAccount, strRealName);
            if (execResult.status != 0)
            {
                return new DataResult() { status = execResult.status, message = execResult.message };
            }

            return DataResult.InitFromMessageCode(MessageCode.SUCCESS);
        }

        // lcl 2018-07-31 Insert
        /// <summary>
        /// 二手一键转卖自动生成订单，并转入二手平台
        /// </summary>
        /// <param name="user">用户信息</param>
        /// <param name="payAccount">用户收款账户</param>
        /// <param name="name">用户真实姓名</param>
        /// <returns></returns>
        private dynamic SecondHandAutoCreateOrder(UserAuth user, string payAccount, string name)
        {
            dynamic result = new ExpandoObject();

            // 用户现有金豆数
            decimal dH_Money = 0m;
            decimal.TryParse(user.h_money.ToString(), out dH_Money);
            var buyCardData = tm.SecondHandBuyCardCalculate(dH_Money);
            if (buyCardData.isCanBuy == 0)
            {
                result.status = 99;
                result.message = "您的可用金豆余额不足";
                return result;
            }

            decimal dCard100_GoldBean = RRLConfig.Card100_GoldBean; // 100元卡对应的金豆数
            decimal dCard10_GoldBean = RRLConfig.Card10_GoldBean; // 10元卡对应的金豆数
            decimal dGoldBean_To_RMB_Rate = RRLConfig.GoldBean_To_RMB_Rate; // 金豆兑换人民币的比率
            decimal dPlatform_Fee_Rate = RRLConfig.Platform_Fee_Rate; // 平台现金手续费比率
            decimal dCardMoneyTotal = 0m; // 卡的现金总额(减去手续费后的)
            decimal dPayGoldBeanTotal = 0m; // 需要支付的金豆总数
            decimal dPlatform_Fee_Money = 0m; // 手续费总金额

            // 100元卡在平台的商品ID
            int intCard100GoodsId = 0;
            int.TryParse(RRLConfig.SecondHand_Card100_GoodsId, out intCard100GoodsId);
            // 10元卡在平台的商品ID
            int intCard10GoodsId = 0;
            int.TryParse(RRLConfig.SecondHand_Card10_GoodsId, out intCard10GoodsId);
            // 商品名称
            string strCard100GoodsName = "购物换钱100元卡";
            string strCard10GoodsName = "购物换钱10元卡";

            if (buyCardData.canBuyQty_Card100 > 0)
            {
                // 如果能购买100元的卡，则生成100元卡的订单和转二手
                dPayGoldBeanTotal = dCard100_GoldBean * buyCardData.canBuyQty_Card100;
                dCardMoneyTotal = dPayGoldBeanTotal * dGoldBean_To_RMB_Rate * (1 - dPlatform_Fee_Rate);
                dPlatform_Fee_Money = dPayGoldBeanTotal * dGoldBean_To_RMB_Rate * dPlatform_Fee_Rate;
                var execResult = CreateOrderAndToSecondHand(user, intCard100GoodsId, strCard100GoodsName, buyCardData.canBuyQty_Card100,
                                                            dPayGoldBeanTotal, dCardMoneyTotal, payAccount, name, dPlatform_Fee_Money);
                if (execResult.status != 0)
                {
                    return execResult;
                }
            }

            if (buyCardData.canBuyQty_Card10 > 0)
            {
                // 如果能购买10元的卡，则生成10元卡的订单和转二手
                dPayGoldBeanTotal = dCard10_GoldBean * buyCardData.canBuyQty_Card10;
                dCardMoneyTotal = dPayGoldBeanTotal * dGoldBean_To_RMB_Rate * (1 - dPlatform_Fee_Rate);
                dPlatform_Fee_Money = dPayGoldBeanTotal * dGoldBean_To_RMB_Rate * dPlatform_Fee_Rate;
                var execResult = CreateOrderAndToSecondHand(user, intCard10GoodsId, strCard10GoodsName, buyCardData.canBuyQty_Card10,
                                                            dPayGoldBeanTotal, dCardMoneyTotal, payAccount, name, dPlatform_Fee_Money);
                if (execResult.status != 0)
                {
                    return execResult;
                }
            }

            result.status = 0;
            result.message = "";
            return result;
        }

        private dynamic CreateOrderAndToSecondHand(UserAuth user, int goodsId, string goodsName,int goodsCount,
                                                   decimal payGoldBeanTotal, decimal cardMoneyTotal, string payAccount, string name,
                                                   decimal platformFeeMoney)
        {
            dynamic result = new ExpandoObject();
            SqlDataBase db = new SqlDataBase();
            // 传给二手平台接口的token，该token为用户手机号码的密文
            string strPhoneToken = DES.EncryptDES(user.username, RRLConfig.SecondHand_DES_Key);
            string strSql = string.Empty;

            // 本次交易的ID(在SDD平台发起的与二手平台交易的ID)
            string strCurrentTranId = Guid.NewGuid().ToString();
            // 创建于二手的交易订单
            tm.CreateSecondHandOrder(strCurrentTranId, user.id, goodsId, cardMoneyTotal, payAccount, name);
            // 与二手平台接口通讯的session id
            string strSessionId = string.Empty;

            // 二手平台第1次接口调用：生成二手平台商品
            strSessionId = Guid.NewGuid().ToString(); // 生成会话ID
            StringBuilder sbGenerateGoodsUrl = new StringBuilder();
            sbGenerateGoodsUrl.Append($@"{RRLConfig.SecondHand_PreProduct_Url}?token={strPhoneToken}");
            sbGenerateGoodsUrl.Append($@"&title={HttpUtility.UrlEncode(goodsName)}");
            sbGenerateGoodsUrl.Append($@"&orgprice={cardMoneyTotal}");
            sbGenerateGoodsUrl.Append($@"&order_id={strCurrentTranId}");
            sbGenerateGoodsUrl.Append($@"&sell_token={strSessionId}");
            sbGenerateGoodsUrl.Append($@"&pay_account={HttpUtility.UrlEncode(payAccount)}");
            sbGenerateGoodsUrl.Append($@"&name={HttpUtility.UrlEncode(name)}");
            sbGenerateGoodsUrl.Append($@"&picurl=&postage=0&sdd_order_status=3&redirect=");
            // 调用二手平台生成商品的接口
            string strResponseJson = MyHttpHelper.HttpGet(sbGenerateGoodsUrl.ToString());
            var jsonData = JsonConvert.DeserializeObject<dynamic>(strResponseJson);
            if (jsonData.status != 0)
            {
                // 如果接口返回不成功，则写日志
                tm.WriteSecondHandOrderLog(strCurrentTranId, jsonData.message.ToString());
                result.status = jsonData.status;
                result.message = jsonData.message;
                return result;
            }
            // 解密返回的tran_id
            string strResponseTranId = DES.DecryptDES(jsonData.tran_id.ToString(), RRLConfig.SecondHand_DES_Key);
            string[] arrTranId = strResponseTranId.Split(',');
            if (arrTranId.Length != 2 || arrTranId[0] != strSessionId)
            {
                result.status = 99;
                result.message = "生成商品时，返回值验证失败";
                // 写日志
                tm.WriteSecondHandOrderLog(strCurrentTranId, result.message);
                return result;
            }
            // 获取二手交易ID
            string strSecondHandTranId = arrTranId[1];
            // 模拟生成SDD平台上的订单
            int? intOrderId = 0;
            BussResult bussResult = tm.CreateOrderFromGoodsV3(user.id, goodsId, goodsCount, "", "", "", "", out intOrderId);
            if (intOrderId == null || intOrderId.Value < 1)
            {
                result.status = bussResult.status;
                result.message = bussResult.message;
                // 写日志
                tm.WriteSecondHandOrderLog(strCurrentTranId, result.message);
                return result;
            }
            // 支付金豆
            var payResult = tm.SecondHandPay(strCurrentTranId, user.id, intOrderId.Value, strSecondHandTranId, strSessionId, payGoldBeanTotal, platformFeeMoney);
            if (payResult.status != 0)
            {
                // 如果支付失败，则写日志
                tm.WriteSecondHandOrderLog(strCurrentTranId, payResult.message.ToString());
                result.status = payResult.status;
                result.message = payResult.message;
                return result;
            }

            // 二手平台第2次接口调用：确认成功
            strSessionId = Guid.NewGuid().ToString(); // 生成会话ID
            string strOrderCode = tm.GetOrderCode(intOrderId.Value);
            string strParamTranId = DES.EncryptDES($@"{strSessionId},{strSecondHandTranId},{strOrderCode}", RRLConfig.SecondHand_DES_Key);
            string strGoodsConfirmUrl = $@"{RRLConfig.SecondHand_ComProduct_Url}?token={strPhoneToken}&tran_id={strParamTranId}";
            // 调用二手平台商品确认的接口
            strResponseJson = MyHttpHelper.HttpGet(strGoodsConfirmUrl);
            jsonData = JsonConvert.DeserializeObject<dynamic>(strResponseJson);
            if (jsonData.status != 0)
            {
                // 如果接口返回不成功，则写日志
                tm.WriteSecondHandOrderLog(strCurrentTranId, jsonData.message.ToString());
                result.status = jsonData.status;
                result.message = jsonData.message;
                return result;
            }
            // 解密返回的tran_id
            strResponseTranId = DES.DecryptDES(jsonData.tran_id.ToString(), RRLConfig.SecondHand_DES_Key);
            arrTranId = strResponseTranId.Split(',');
            if (arrTranId.Length != 2 || arrTranId[0] != strSessionId)
            {
                result.status = 99;
                result.message = "确认商品时，返回值验证失败";
                // 写日志
                tm.WriteSecondHandOrderLog(strCurrentTranId, result.message);
                return result;
            }
            // 更新SDD中的二手订单表
            tm.SecondHandConfirm(strCurrentTranId);

            result.status = 0;
            result.message = "";
            return result;
        }

    }
}