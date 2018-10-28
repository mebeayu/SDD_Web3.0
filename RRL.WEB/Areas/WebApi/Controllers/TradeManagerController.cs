 using RRL.Core.Manager;
using RRL.Core.Models;
using RRL.Core.Utility;
using RRL.WEB.Models.Response;
using RRL.WEB.Ulity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Http;
using System.Linq;
using RRL.DB;

namespace RRL.WEB.Areas.WebApi.Controllers
{
    /// <summary>
    /// 交易管理接口
    /// </summary>
    public partial class TradeManagerController : ApiController
    {
        private TradeManager tm = new TradeManager();

        /// <summary>
        /// 获取购物车列表
        /// </summary>
        /// <param name="token">短效token</param>
        /// <returns>结果对象</returns>
        [HttpGet]
        [ActionName("CartList")]
        public DataResult GetShopCartList(string token)
        {
            int res;
            TokenObject Token = TokenObject.InitTokenObjFromString(token);
            if (!string.Equals(TokenObject.ShortTimeToken, Token.Prefix))
            {
                res = MessageCode.ERROR_TOKEN_VALIDATE;
                return DataResult.InitFromMessageCode(res);
            }
            else
            {
                UserAuth User = new UserAuth();
                res = User.Load(Token.id);
                if (res == MessageCode.SUCCESS)
                {
                    DataSet ds = tm.GetShopCartList(User.id);
                    return DataResult.InitFromDataSet(ds);
                }
                return DataResult.InitFromMessageCode(res);
            }
        }

        /// <summary>
        /// 向购物车中添加或减少商品(自更新)
        /// </summary>
        /// <param name="goods_id">商品id</param>
        /// <param name="goods_count">商品数量(可为负数)</param>
        /// <param name="token">短效token</param>
        /// <returns>结果对象</returns>
        [HttpGet]
        [ActionName("AddGoodsToCart")]
        public DataResult AddGoodsToCart(int goods_id, int goods_count, string token)
        {
            int res;
            DataResult Resault;
            DataTable data = null;
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
                    res = tm.AddGoodsToCart(User.id, goods_id, goods_count);
                }
                data = tm.GetShopCartList(User.id).Tables[0];
            }
            Resault = DataResult.InitFromMessageCode(res);
            Resault.data = data;
            return Resault;
        }

        /// <summary>
        /// 从购物车中移除条目(自更新)
        /// </summary>
        /// <param name="cart_id">购物车条目id</param>
        /// <param name="token">短效token</param>
        /// <returns>结果对象</returns>
        [HttpGet]
        [ActionName("RemoveGoodsInCart")]
        public DataResult RemoveGoodsInCart(string cart_id, string token)
        {
            int res;
            DataResult Resault;
            DataTable data = null;
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
                    res = tm.RemoveGoodsInCart(User.id, cart_id);
                }
                data = tm.GetShopCartList(User.id).Tables[0];
            }
            Resault = DataResult.InitFromMessageCode(res);
            Resault.data = data;
            return Resault;
        }

        /// <summary>
        /// 通过商品创建订单
        /// </summary>
        /// <param name="goods_id">商品id</param>
        /// <param name="goods_count">商品数量</param>
        /// <param name="token">短效token</param>
        /// <returns>结果对象</returns>
        [HttpGet]
        [ActionName("CreateOrderFromGoods")]
        public DataResult CreateOrderFromGoods(int goods_id, int goods_count,/* int receive_id,*/ string token)
        {
            return new DataResult() { status = 99, message = @"请更新到最新版本的App!" };
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
                    res = tm.CreateOrderFromGoods(User.id, goods_id, goods_count, out order_list);
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
        /// <returns>结果对象</returns>
        [HttpGet]
        [ActionName("CreateOrderFromCart")]
        public DataResult CreateOrderFromCart(string cartlist, /*int receive_id,*/ string token)
        {
            return new DataResult() { status = 99, message = @"请更新到最新版本的App!" };
            
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
                    res = tm.CreateOrderFromCart(User.id, list, out order_list);
                }
            }
            DataResult Resault = DataResult.InitFromMessageCode(res);
            Resault.data = order_list;
            return Resault;
        }

        /// <summary>
        /// 获取订单列表
        /// </summary>
        /// <param name="page">页码</param>
        /// <param name="pagesize">页面尺寸</param>
        /// <param name="token">短效token</param>
        /// <returns>结果对象</returns>
        [HttpGet]
        [ActionName("OrderList")]
        public DataResult GetOrderList(int page, int pagesize, string token)
        {
            int res;
            TokenObject Token = TokenObject.InitTokenObjFromString(token);
            if (!string.Equals(TokenObject.ShortTimeToken, Token.Prefix))
            {
                res = MessageCode.ERROR_TOKEN_VALIDATE;
                return DataResult.InitFromMessageCode(res);
            }
            else
            {
                UserAuth User = new UserAuth();
                res = User.Load(Token.id);
                if (res == MessageCode.SUCCESS)
                {
                    List<ListOrder> ls = tm.GetOrderList(User.id, page, pagesize);
                    DataResult Resault = DataResult.InitFromMessageCode(res);
                    Resault.data = ls;
                    return Resault;
                }
                return DataResult.InitFromMessageCode(res);
            }
        }

        /// <summary>
        /// 获取用户订单分类统计数量
        /// </summary>
        /// <param name="token">短效token</param>
        /// <returns>结果对象</returns>
        [HttpGet]
        [ActionName("OrderStatusCount")]
        public DataResult GetOrderStatusCount(string token)
        {
            int res;
            TokenObject Token = TokenObject.InitTokenObjFromString(token);
            if (!string.Equals(TokenObject.ShortTimeToken, Token.Prefix))
            {
                res = MessageCode.ERROR_TOKEN_VALIDATE;
                return DataResult.InitFromMessageCode(res);
            }
            else
            {
                UserAuth User = new UserAuth();
                res = User.Load(Token.id);
                if (res == MessageCode.SUCCESS)
                {
                    DataSet ds = tm.GetOrderStatusCount(User.id);
                    return DataResult.InitFromDataSet(ds);
                }
                return DataResult.InitFromMessageCode(res);
            }
        }

        /// <summary>
        /// 通过订单状态获取订单列表
        /// </summary>
        /// <param name="status">
        /// -3:订单已关闭(?),
        /// -2:标记异常(后端)，
        /// -1:已取消(后端)，
        /// 0:过程码(后端)，
        /// 1:待付款，
        /// 2：待收货，
        /// 3：待评价，
        /// 4：退货
        /// </param>
        /// <param name="page">页码</param>
        /// <param name="pagesize">页面大小</param>
        /// <param name="token">短效token</param>
        /// <returns>结果对象</returns>
        [HttpGet]
        [ActionName("OrderListByStatus")]
        public DataResult GetOrderListByStatus(int status, int page, int pagesize, string token)
        {
            int res;
            TokenObject Token = TokenObject.InitTokenObjFromString(token);
            if (!string.Equals(TokenObject.ShortTimeToken, Token.Prefix))
            {
                res = MessageCode.ERROR_TOKEN_VALIDATE;
                return DataResult.InitFromMessageCode(res);
            }
            else
            {
                UserAuth User = new UserAuth();
                res = User.Load(Token.id);
                if (res == MessageCode.SUCCESS)
                {
                    List<ListOrder> ls = tm.GetOrderListByStatus(User.id, status, page, pagesize);
                    DataResult Resault = DataResult.InitFromMessageCode(res);
                    Resault.data = ls;
                    return Resault;
                }
                return DataResult.InitFromMessageCode(res);
            }
        }

        /// <summary>
        /// 通过订单标识获取订单详情
        /// </summary>
        /// <param name="order_id">订单id</param>
        /// <param name="token">短效token</param>
        /// <returns>结果对象</returns>
        [HttpGet]
        [ActionName("OrderDetail")]
        public DataResult GetOrderDetail(int order_id, string token)
        {
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
                    ListOrder Order = tm.GetOrderDetail(User.id, order_id);
                    DataResult Resault = DataResult.InitFromMessageCode(res);
                    Resault.data = Order;
                    return Resault;
                }
            }
            return DataResult.InitFromMessageCode(res);
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
        [ActionName("PreOrderList")]
        public DataResult GetPreOrderList(string orderlist, string token)
        {
            return new DataResult() { status = 99, message = @"请更新到最新版本的App!" };
            int res;
            if(string.IsNullOrWhiteSpace(orderlist))
            {
                var dr= DataResult.InitFromMessageCode(99);
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
                    var ls = tm.GetPreOrderList(user.id, orderlist);
                    var resault = DataResult.InitFromMessageCode(res);
                    resault.data = ls;
                    resault.additional_data = user.x_money;
                    resault.add_data = new
                    {
                        h_money = user.h_money,
                        h_money_rate = "RMB1元 = 100金豆"
                    };
                    return resault;
                }
            }
            return DataResult.InitFromMessageCode(res);
        }

        /// <summary>
        /// 补充预处理订单收件地址信息
        /// </summary>
        /// <param name="order_id">订单id</param>
        /// <param name="receive_id">收件信息id</param>
        /// <param name="token">短效token</param>
        /// <returns>结果对象</returns>
        [HttpGet]
        [ActionName("FillReceiveInfo")]
        public DataResult FillOrderReceiveInfo(string order_id, string receive_id, string token)
        {
            if (string.IsNullOrWhiteSpace(receive_id))
            {
                var dr = DataResult.InitFromMessageCode(99);
                dr.message = "请先选择地址！";
                return dr;
            }

            var order_list= order_id.Split(',');
            var order_list_int= order_list.Select(v => Convert.ToInt32(v)).ToList();
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
                    res = tm.FillOrderReceiveInfo(User.id, order_list_int, Convert.ToInt32(receive_id));
                }
            }
            return DataResult.InitFromMessageCode(res);
        }

        /// <summary>
        /// 取消未支付订单
        /// </summary>
        /// <param name="order_id">订单id</param>
        /// <param name="token">短效token</param>
        /// <returns>结果对象</returns>
        [HttpGet]
        [ActionName("CancelOrder")]
        public DataResult CancelOrder(int order_id, string token)
        {
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
                    res = tm.CancelOlder(User.id, order_id);
                }
            }
            return DataResult.InitFromMessageCode(res);
        }

        /// <summary>
        /// 申请订单退货
        /// </summary>
        /// <param name="order_id">订单id</param>
        /// <param name="token">短效token</param>
        /// <returns>结果对象</returns>
        [HttpGet]
        [ActionName("RefundOrder")]
        public DataResult OrderRefundApply(int order_id, string token)
        {
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
                    SqlDataBase db = new SqlDataBase();
                    dynamic oneOrder=db.Single<dynamic>("select id,order_id,isnull(sh_sell_status,'0') as sh_sell_status from rrl_order_info_goods where order_id=@order_id", new { order_id = order_id });
                    if(oneOrder==null)
                    {
                        return new DataResult() { status = 99, message = "订单不存在!" };
                    }
                    if (!"0".Equals(oneOrder.sh_sell_status))
                    {
                        return new DataResult() { status = 99, message = "申请退换失败,请把二手货下架再操作!" };
                    }
                    res = tm.OrderRefundApply(User.id, order_id );
                }
            }
            return DataResult.InitFromMessageCode(res);
        }

        /// <summary>
        /// 结算订单(确认收货)
        /// </summary>
        /// <param name="order_id">订单id</param>
        /// <param name="token">短效token</param>
        /// <returns>结果对象</returns>
        [HttpGet]
        [ActionName("SettleOrder")]
        public DataResult OrderSettlement(int order_id, string token)
        {
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
                    res = tm.OrderSettlement(User.id, order_id);
                }
            }
            return DataResult.InitFromMessageCode(res);
        }

        /// <summary>
        /// 申请支付(已强制过期)
        /// </summary>
        /// <param name="orderlist">订单数组,例如:1,2,3</param>
        /// <param name="discount">申请抵扣金币</param>
        /// <param name="trans_type">支付类型:1,微信支付，2,支付宝支付，3,退款账户余额支付</param>
        /// <param name="IP">请求ip地址</param>
        /// <param name="token">短效token</param>
        /// <returns>结果对象</returns>
        [HttpGet]
        [ActionName("ApplyPay")]
        public DataResult ApplyPayForOrderList(string orderlist, double discount, int trans_type, string token, string IP = null)
        {
            if (IP == null)
            {
                IP = "0.0.0.0";
            }
            int res;
            DataResult Resault = new DataResult();
            //string data = "";
            //TokenObject Token = TokenObject.InitTokenObjFromString(token);
            //if (!string.Equals(TokenObject.ShortTimeToken, Token.Prefix))
            //{
            //    res = MessageCode.ERROR_TOKEN_VALIDATE;
            //}
            //else
            //{
            //    UserAuth User = new UserAuth();
            //    res = User.Load(Token.id);
            //    if (res == MessageCode.SUCCESS)
            //    {
            //        var plate_to_return = User.plate_to_return_money + User.ex_plate_to_return_money;
            //        data = tm.ApplyPay(User.id, orderlist, discount, trans_type, plate_to_return, IP, out res);
            //    }
            //}
            Resault.status = MessageCode.ERROR_CODE;
            Resault.message = "安全起见该版本支付暂停使用，请升级您的APP版本";
            Resault.data = null;
            return Resault;
        }

        /// <summary>
        /// 申请支付
        /// </summary>
        /// <param name="orderlist">订单数组,例如:1,2,3</param>
        /// <param name="discount">申请抵扣金币??没用</param>
        /// <param name="trans_type">支付类型:1,微信支付，2,支付宝支付，3,退款账户余额支付</param>
        /// <param name="IP">请求ip地址</param>
        /// <param name="token">短效token</param>
        /// <returns>结果对象</returns>
        [HttpGet]
        [ActionName("ApplyPayVer2")]
        public DataResult ApplyPayForOrderListVer2(string orderlist, double discount, int trans_type, string token, string IP = null)
        {
            return new DataResult() { status = 99, message = @"请更新到最新版本的App!" };
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
                UserAuth User = new UserAuth();
                res = User.Load(Token.id);
                if ("1".Equals(User.is_locked_login) || "1".Equals(User.is_locked_trade))
                {
                    return new DataResult() { status = 99, message = "您的帐号被锁定,请用注册手机号码联系客服!" };
                }
                if (res == MessageCode.SUCCESS)
                {
                    //总金币
                    var plate_to_return = User.plate_to_return_money + User.ex_plate_to_return_money;
                    data = tm.ApplyPay(User.id, orderlist, discount, trans_type, plate_to_return, IP, out res);
                }
            }
            result = DataResult.InitFromMessageCode(res);
            result.data = data;
            return result;
        }


         
        ///// <summary>
        ///// 请求微信Js支付(弃用)
        ///// </summary>
        ///// <param name="orderlist">订单数组,例如:1,2,3</param>
        ///// <param name="discount">申请抵扣金额</param>
        ///// <param name="IP">请求ip地址</param>
        ///// <param name="token">短效token</param>
        ///// <param name="sperador">推荐人手机号码</param>
        ///// <returns>结果对象</returns>
        //[HttpGet]
        //[ActionName("ApplyWxJsPay")]
        //private DataResult ApplyWxJsPayForOrderList(string orderlist, double discount, string token, string IP = null, string sperador = null)
        //{
        //    if (IP == null)
        //    {
        //        IP = "0.0.0.0";
        //    }
        //    int res;
        //    DataResult Resault;
        //    TokenObject Token = TokenObject.InitTokenObjFromString(token);
        //    if (!string.Equals(TokenObject.ShortTimeToken, Token.Prefix))
        //    {
        //        res = MessageCode.ERROR_TOKEN_VALIDATE;
        //    }
        //    else
        //    {
        //        UserAuth User = new UserAuth();
        //        res = User.Load(Token.id);
        //        //string open_id = User.open_id;
        //        if (res == MessageCode.SUCCESS)
        //        {
        //            string openid = "SessionHelper.Get('openid')";
        //            object data = tm.ApplyWxJsPay(User.id, orderlist, discount, User.r_money, IP, openid, out res, sperador);
        //            Resault = DataResult.InitFromMessageCode(res);
        //            Resault.data = data;
        //            return Resault;
        //        }
        //    }
        //    Resault = DataResult.InitFromMessageCode(res);
        //    return Resault;
        //}

        ///// <summary>
        ///// 请求微信扫码支付
        ///// </summary>
        ///// <param name="orderlist">订单数组,例如:1,2,3</param>
        ///// <param name="discount">申请抵扣金额</param>
        ///// <param name="IP">请求ip地址</param>
        ///// <param name="token">短效token</param>
        ///// <param name="sperador">推荐人手机号码</param>
        ///// <returns>结果对象</returns>
        //[HttpGet]
        //[ActionName("ApplyWxNativePay")]
        //public DataResult ApplyWxH5PayForOrderList(string orderlist, double discount, string token, string IP = null, string sperador = null)
        //{
        //    if (IP == null)
        //    {
        //        IP = "0.0.0.0";
        //    }
        //    int res;
        //    DataResult Resault;
        //    TokenObject Token = TokenObject.InitTokenObjFromString(token);
        //    if (!string.Equals(TokenObject.ShortTimeToken, Token.Prefix))
        //    {
        //        res = MessageCode.ERROR_TOKEN_VALIDATE;
        //    }
        //    else
        //    {
        //        UserAuth User = new UserAuth();
        //        res = User.Load(Token.id);
        //        if (res == MessageCode.SUCCESS)
        //        {
        //            object data = tm.ApplyWxNativePay(User.id, orderlist, discount, User.r_money, IP, out res, sperador);
        //            Resault = DataResult.InitFromMessageCode(res);
        //            Resault.data = data;
        //            return Resault;
        //        }
        //    }
        //    Resault = DataResult.InitFromMessageCode(res);
        //    return Resault;
        //}


        #region 一键转卖接口
        /// <summary>
        /// 一键转卖接口 2018年2月6日 13:29:56
        /// </summary>
        /// <param name="goods_id">商品id</param>
        /// <param name="order_id">订单id</param>
        /// <param name="token">短token</param>
        /// <returns></returns>
        [HttpGet]
        [ActionName("OneKeyReSell")]
        public DataResult OneKeyResell(int goods_id, int order_id,  string token)
        {
     
            int res;
            string url = "";
            TokenObject Token = TokenObject.InitTokenObjFromString(token);
            if (!string.Equals(TokenObject.ShortTimeToken, Token.Prefix))
            {
                res = MessageCode.ERROR_TOKEN_VALIDATE;
                return DataResult.InitFromMessageCode(res);
            }
            else
            {
                UserAuth User = new UserAuth();
                res = User.Load(Token.id);
                if (res == MessageCode.SUCCESS)
                {
                      var resp = tm.OneKeyResell(User.id, User.username, order_id, goods_id);
                    DataResult dataResult = new DataResult();
                    if(resp.IsSuccess)
                    {
                        dataResult.data = resp.Url;
                        dataResult.status = MessageCode.SUCCESS;
                        dataResult.message = "ok";
                    }else
                    {
                        dataResult.data = "";
                        dataResult.status = MessageCode.ERROR_UNKONWN;
                        dataResult.message = resp.errorMessage ;
                    }
                    return dataResult;

                }
                return DataResult.InitFromMessageCode(MessageCode.ERROR_TOKEN_VALIDATE); 
            }
            
        }
        #endregion

        #region  转卖回调接口
        /// <summary>
        /// 转卖回调接口
        /// </summary>
        /// <param name="order_id">订单id</param>
        /// <param name="status">订单状态 0=发布转卖成功,1=商品转卖成功</param>
        /// <param name="sell_token">转卖凭据</param>
        /// <returns></returns>
        [HttpGet]
        [ActionName("ReSellCallBack")]
        public DataResult  ReSellCallBack(string order_id,string status, string sell_token)
        {
             
            string errorMessage;
            int resp = tm.ReSellCallBack(  order_id, status, sell_token,out   errorMessage);
            DataResult dataResult = new DataResult();
            if (resp==0)
            {
                //dataResult.data = resp.Url;
                dataResult.status = MessageCode.SUCCESS;
                dataResult.message = "设置状态成功!";
            }
            else
            {
                //dataResult.data = "";
                dataResult.status = MessageCode.ERROR_UNKONWN;
                dataResult.message = errorMessage;
            }
            return dataResult; 

        }
        #endregion

        #region 转卖详情
        /// <summary>
        /// 转卖详情  2018年2月11日 10:44:57
        /// </summary>
        /// <param name="goods_id">商品id</param>
        /// <param name="order_id">订单id</param>
        /// <param name="token">短token</param>
        /// <returns></returns>
        [HttpGet]
        [ActionName("ReSellDetails")]
        public DataResult ReSellDetails( string token)
        {

            int res;
            string url = "";
            TokenObject Token = TokenObject.InitTokenObjFromString(token);
            if (!string.Equals(TokenObject.ShortTimeToken, Token.Prefix))
            {
                res = MessageCode.ERROR_TOKEN_VALIDATE;
                return DataResult.InitFromMessageCode(res);
            }
            else
            {
                UserAuth User = new UserAuth();
                res = User.Load(Token.id);
                if (res == MessageCode.SUCCESS)
                {
                    var resp = tm .ReSellDetails(User.id, User.username );
                    DataResult dataResult = new DataResult();
                    if (resp.IsSuccess)
                    {
                        dataResult.data = resp.Url;
                        dataResult.status = MessageCode.SUCCESS;
                        dataResult.message = "ok";
                    }
                    else
                    {
                        dataResult.data = "";
                        dataResult.status = MessageCode.ERROR_UNKONWN;
                        dataResult.message = resp.errorMessage;
                    }
                    return dataResult;

                }
                return DataResult.InitFromMessageCode(MessageCode.ERROR_TOKEN_VALIDATE);
            }

        }
        #endregion


        #region 获取我可以的来支付的券包
        /// <summary>
        /// 获取我可以的来支付的券包
        /// </summary>
        /// <param name="order_list"></param>
        /// <param name="token"></param>
        /// <param name="page_index"></param>
        /// <param name="page_size"></param>
        /// <returns></returns>
        [HttpGet]
        [ActionName("GetAvailableCoupons")]
        public DataResult GetAvailableCoupons(string order_list,string token,int page_index,int page_size)
        {

            int res;
            string url = "";
            TokenObject Token = TokenObject.InitTokenObjFromString(token);
            if (!string.Equals(TokenObject.ShortTimeToken, Token.Prefix))
            {
                res = MessageCode.ERROR_TOKEN_VALIDATE;
                return DataResult.InitFromMessageCode(res);
            }
            else
            {
                UserAuth User = new UserAuth();
                res = User.Load(Token.id);
                if (res == MessageCode.SUCCESS)
                {
                    var data= tm.GetAvailableCoupons(User.id, order_list, page_index, page_size,out int errorCode,out string errMsg);
                    DataResult dataResult = new DataResult();
                    dataResult.data = data;
                    dataResult.status = errorCode;
                    dataResult.message = errMsg;
                    return dataResult;
                }
                return DataResult.InitFromMessageCode(MessageCode.ERROR_TOKEN_VALIDATE);
            }

        }
        #endregion


        #region 应用券支付
        /// <summary>
        /// 应用券支付
        /// </summary>
        /// <param name="order_list"></param>
        /// <param name="coupons_id"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpGet]
        [ActionName("ApplyCouponsPay")]
        public DataResult ApplyCouponsPay(string order_id,string coupons_id, string token)
        {
            int res;
            string url = "";
            TokenObject Token = TokenObject.InitTokenObjFromString(token);
            if (!string.Equals(TokenObject.ShortTimeToken, Token.Prefix))
            {
                res = MessageCode.ERROR_TOKEN_VALIDATE;
                return DataResult.InitFromMessageCode(res);
            }
            else
            {
                UserAuth User = new UserAuth();
                res = User.Load(Token.id);
                if (res == MessageCode.SUCCESS)
                {
                    var data = tm.ApplyCouponsPay(User.id, order_id, coupons_id,out int  errorCode, out string  errMsg);
                    DataResult dataResult = new DataResult();
                    dataResult.data = data;
                    dataResult.status = errorCode;
                    dataResult.message = errMsg;
                    return dataResult;
                }
                return DataResult.InitFromMessageCode(MessageCode.ERROR_TOKEN_VALIDATE);
            }
             
        } 
        #endregion
    }
}