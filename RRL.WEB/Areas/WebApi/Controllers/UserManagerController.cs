using System;
using RRL.Config;
using RRL.Core.Manager;
using RRL.Core.Models;
using RRL.Core.Utility;
using RRL.WEB.Models.Request;
using RRL.WEB.Models.Response;
using RRL.WEB.Models.ViewModel;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.Http;
using RRL.WEB.Ulity;
using RRL.DB;
using RRL.WEB.Controllers;
using System.Linq;
using EasyHttp.Http;
using System.Text;

namespace RRL.WEB.Areas.WebApi.Controllers
{
    /// <summary>
    /// 用户管理接口
    /// </summary>
    public partial class UserManagerController : ApiController
    {
        private UserManager um = new UserManager();
        private RdSession rdSession = new RdSession(HttpContext.Current);

        /// <summary>
        /// 添加商品收藏(已脱离过程,即将过期)
        /// </summary>
        /// <param name="goods_id">商品id</param>
        /// <param name="token">短效token</param>
        /// <returns>结果对象</returns>
        [HttpGet]
        [ActionName("AddFavouriteGoods")]
        public DataResult AddFavouriteGoodsByGet(int goods_id, string token)
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
                    res = um.AddFavouriteGoods(goods_id, User.id);
                }
            }
            return DataResult.InitFromMessageCode(res);
        }

        /// <summary>
        /// 添加商品收藏(已脱离过程,推荐使用)
        /// </summary>
        /// <param name="Request">商品基础请求对象</param>
        /// <returns>结果对象</returns>
        [HttpPost]
        [ActionName("AddFavouriteGoods")]
        public DataResult AddFavouriteGoods([FromBody]BaseGoodsRequest Request)
        {
            int res;
            TokenObject Token = TokenObject.InitTokenObjFromString(Request.token);
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
                    res = um.AddFavouriteGoods(Request.goods_id, User.id);
                }
            }
            return DataResult.InitFromMessageCode(res);
        }

        /// <summary>
        /// 移除商品收藏
        /// </summary>
        /// <param name="goods_id">商品id</param>
        /// <param name="token">短效token</param>
        /// <returns>结果对象</returns>
        [HttpGet]
        [ActionName("RemoveFavouriteGoods")]
        public DataResult RemoveFavouriteGoods(int goods_id, string token)
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
                    res = um.RemoveFavouriteGoods(goods_id, User.id);
                }
            }
            return DataResult.InitFromMessageCode(res);
        }

        /// <summary>
        /// 获取用户商品收藏列表
        /// </summary>
        /// <param name="Page">页码</param>
        /// <param name="PageSize">页面大小</param>
        /// <param name="token">短效token</param>
        /// <returns>结果对象</returns>
        [HttpGet]
        [ActionName("FavouriteGoodsList")]
        public DataResult GetUserFavouriteGoodsList(int Page, int PageSize, string token)
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
                    DataSet ds = um.GetFavouriteGoodsList(User.id, Page, PageSize);
                    return DataResult.InitFromDataSet(ds);
                }
                return DataResult.InitFromMessageCode(res);
            }
        }

        /// <summary>
        /// 身份信息补充
        /// </summary>
        /// <param name="request">请求</param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("Identity")]
        public DataResult FillUserIdentity([FromBody]IdentityRequest request)
        {
            int res;
            TokenObject token = TokenObject.InitTokenObjFromString(request.token);
            if (!string.Equals(TokenObject.ShortTimeToken, token.Prefix))
            {
                res = MessageCode.ERROR_TOKEN_VALIDATE;
                return DataResult.InitFromMessageCode(res);
            }
            else
            {
                UserAuth user = new UserAuth();
                res = user.Load(token.id);
                if (res == MessageCode.SUCCESS)
                {
                    um.FillIdentity(request.realname, request.identity, user.id);
                }
                return DataResult.InitFromMessageCode(res);
            }
        }

        /// <summary>
        /// 添加商店收藏(已脱离过程,即将过期)
        /// </summary>
        /// <param name="shop_id">商店id</param>
        /// <param name="token">短效token</param>
        /// <returns>结果对象</returns>
        [HttpGet]
        [ActionName("AddFavouriteShop")]
        public DataResult AddFavouriteShopByGet(int shop_id, string token)
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
                    res = um.AddFavouriteShop(shop_id, User.id);
                }
            }
            return DataResult.InitFromMessageCode(res);
        }

        /// <summary>
        /// 添加商店收藏(已脱离过程,推荐使用)
        /// </summary>
        /// <param name="Request">商店基础请求对象</param>
        /// <returns>结果对象</returns>
        [HttpPost]
        [ActionName("AddFavouriteShop")]
        public DataResult AddFavouriteShop([FromBody]BaseShopRequest Request)
        {
            int res;
            TokenObject Token = TokenObject.InitTokenObjFromString(Request.token);
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
                    res = um.AddFavouriteShop(Request.shop_id, User.id);
                }
            }
            return DataResult.InitFromMessageCode(res);
        }

        /// <summary>
        /// 移除商店收藏
        /// </summary>
        /// <param name="shop_id">商店id</param>
        /// <param name="token">短效token</param>
        /// <returns>结果对象</returns>
        [HttpGet]
        [ActionName("RemoveFavouriteShop")]
        public DataResult RemoveFavouriteShop(int shop_id, string token)
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
                    res = um.RemoveFavouriteShop(shop_id, User.id);
                }
            }
            return DataResult.InitFromMessageCode(res);
        }

        /// <summary>
        /// 获取用户商店收藏列表
        /// </summary>
        /// <param name="Page">页码</param>
        /// <param name="PageSize">页面大小</param>
        /// <param name="token">短效token</param>
        /// <returns>结果对象</returns>
        [HttpGet]
        [ActionName("FavouriteShopList")]
        public DataResult GetUserFavouriteShopList(int Page, int PageSize, string token)
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
                    DataSet ds = um.GetFavouriteShopList(User.id, Page, PageSize);
                    return DataResult.InitFromDataSet(ds);
                }
                return DataResult.InitFromMessageCode(res);
            }
        }

        /// <summary>
        /// 设置用户基本信息
        /// </summary>
        /// <param name="nick_name">昵称</param>
        /// <param name="sex">性别:1:男，2:女</param>
        /// <param name="area_code">区域编码</param>
        /// <param name="token">短效token</param>
        /// <returns>结果对象</returns>
        [HttpGet]
        [ActionName("SetUserInfo")]
        public DataResult SetUserInfo(string nick_name, int sex, string token, string area_code = null)
        {
            if (area_code == null || string.Equals(area_code, ""))
            {
                area_code = "530111";
            }
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
                    res = um.SetBaseInfo(User.id, nick_name, sex, area_code);
                }
            }
            return DataResult.InitFromMessageCode(res);
        }

        /// <summary>
        /// 设置用户头像
        /// </summary>
        /// <param name="obj">用户头像设置对象</param>
        /// <returns>结果对象</returns>
        [HttpPost]
        [ActionName("SetUserHeadPic")]
        public DataResult SetUserHeadPic([FromBody]HeadPicRequest obj)
        {
            if (string.IsNullOrWhiteSpace(obj.base64))
            {
                return new DataResult() { status = 99, message = "图片上传失败!" };
            }
            int res;
            TokenObject Token = TokenObject.InitTokenObjFromString(obj.token);
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
                    long picId = 0;
                    int resultstate = um.SetUserHeadPic(User.id, obj.base64, out picId);
                    return new DataResult() { status = 0, data = picId };
                }
            }
            return new DataResult() { status = 99, data = 0, message = "上传图片失败!" };
        }

        /// <summary>
        /// 获取用户基本信息
        /// </summary>
        /// <param name="token">短效token</param>
        /// <returns>结果对象</returns>
        [HttpGet]
        [ActionName("UserInfo")]
        public DataResult GetUserBaseInfo(string token)
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
                    DataSet ds = um.GetUserBaseInfo(User.id);
                    return DataResult.InitFromDataSet(ds);
                }
                return DataResult.InitFromMessageCode(res);
            }
        }

        /// <summary>
        /// 添加收件人信息
        /// </summary>
        /// <param name="name">收件人姓名</param>
        /// <param name="address">收件人地址</param>
        /// <param name="mobile">收件人联系电话</param>
        /// <param name="area_code">收件人区域编码</param>
        /// <param name="zip_code">收件人邮政编码</param>
        /// <param name="token">短效token</param>
        /// <returns>结果对象</returns>
        [HttpGet]
        [ActionName("AddReceiveInfo")]
        public DataResult AddReceiveInfo(string name, string address, string mobile, string area_code, string token, string zip_code = null)
        {
            if (zip_code == null)
            {
                zip_code = "";
            }
            int res, receive_id = 0;
            DataResult Resault;
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
                    res = um.AddReceiveInfo(User.id, name, address, mobile, area_code, zip_code, out receive_id);
                }
            }
            Resault = DataResult.InitFromMessageCode(res);
            Resault.data = receive_id;
            return Resault;
        }

        /// <summary>
        /// 设置默认收件人信息
        /// </summary>
        /// <param name="receive_id">收获信息id</param>
        /// <param name="token">短效token</param>
        /// <returns>结果对象</returns>
        [HttpGet]
        [ActionName("SetDefaultReceiveInfo")]
        public DataResult SetDefaultReceiveInfo(int receive_id, string token)
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
                    res = um.SetDefaultReceiveInfo(User.id, receive_id);
                }
            }
            return DataResult.InitFromMessageCode(res);
        }

        /// <summary>
        /// 添加收件人信息并设为默认值
        /// </summary>
        /// <param name="name">收件人姓名</param>
        /// <param name="address">收件人地址</param>
        /// <param name="mobile">收件人联系电话</param>
        /// <param name="area_code">收件人区域编码</param>
        /// <param name="zip_code">收件人邮政编码</param>
        /// <param name="token">短效token</param>
        /// <returns>结果对象</returns>
        [HttpGet]
        [ActionName("AddReceiveInfoAsDefault")]
        public DataResult AddReceiveInfoAsDefault(string name, string address, string mobile, string area_code, string token, string zip_code = null)
        {
            if (zip_code == null)
            {
                zip_code = "";
            }
            int res, receive_id = 0;
            DataResult Resault;
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
                    res = um.AddReceiveInfoAsDefault(User.id, name, address, mobile, area_code, zip_code, out receive_id);
                }
            }
            Resault = DataResult.InitFromMessageCode(res);
            Resault.data = receive_id;
            return Resault;
        }

        /// <summary>
        /// 移除收件人信息
        /// </summary>
        /// <param name="receive_id">收件人信息列表id</param>
        /// <param name="token">短效token</param>
        /// <returns>结果对象</returns>
        [HttpGet]
        [ActionName("RemoveReceiveInfo")]
        public DataResult RemoveReceiveInfo(int receive_id, string token)
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
                    res = um.RemoveReceiveInfo(User.id, receive_id);
                }
            }
            return DataResult.InitFromMessageCode(res);
        }

        /// <summary>
        /// 获取用户收件人信息列表
        /// </summary>
        /// <param name="token">短效token</param>
        /// <returns>结果对象</returns>
        /// [HttpGet]
        [HttpGet]
        [ActionName("ReceiveList")]
        public DataResult GetUserReceiveInfoList(string token)
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
                    DataSet ds = um.GetUserReceiveInfoList(User.id);
                    return DataResult.InitFromDataSet(ds);
                }
                return DataResult.InitFromMessageCode(res);
            }
        }

        /// <summary>
        /// 获取默认收货信息
        /// </summary>
        /// <param name="token">短效token</param>
        /// <returns>结果对象</returns>
        [HttpGet]
        [ActionName("DefaultReceive")]
        public DataResult GetDefaultReceiveInfo(string token)
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
                    DataSet ds = um.GetDefaultReceiveInfo(User.id);
                    return DataResult.InitFromDataSet(ds);
                }
                return DataResult.InitFromMessageCode(res);
            }
        }

        /// <summary>
        /// 添加银行卡
        /// </summary>
        /// <param name="bank_en">银行英文缩写</param>
        /// <param name="card_no">卡号</param>
        /// <param name="card_holder">持卡人姓名</param>
        /// <param name="sms_code">短信验证码</param>
        /// <param name="token">短效token</param>
        /// <returns>结果对象</returns>
        [HttpGet]
        [ActionName("AddBankCard")]
        public DataResult AddUserBankCard(string bank_en, string card_no, string card_holder, string sms_code, string token)
        {
            int res;
            TokenObject Token = TokenObject.InitTokenObjFromString(token);
            if (!string.Equals(TokenObject.ShortTimeToken, Token.Prefix))
            {
                res = MessageCode.ERROR_TOKEN_VALIDATE;
            }
            else if (!UserAuth.CheckSMSForExistsUser(Token.Username, sms_code))
            {
                res = MessageCode.ERROR_BAD_SMS;
            }
            else
            {
                UserAuth User = new UserAuth();
                res = User.Load(Token.id);
                if (res == MessageCode.SUCCESS)
                {
                    res = um.AddBankCard(User.id, bank_en, card_no, card_holder);
                }
            }
            return DataResult.InitFromMessageCode(res);
        }

        /// <summary>
        /// 移除银行卡
        /// </summary>
        /// <param name="card_id">银行卡id</param>
        /// <param name="sms_code">短信验证码</param>
        /// <param name="token">短效token</param>
        /// <returns>结果对象</returns>
        [HttpGet]
        [ActionName("RemoveBankCard")]
        public DataResult RemoveUserBankCard(int card_id, string sms_code, string token)
        {
            int res;
            TokenObject Token = TokenObject.InitTokenObjFromString(token);
            if (!string.Equals(TokenObject.ShortTimeToken, Token.Prefix))
            {
                res = MessageCode.ERROR_TOKEN_VALIDATE;
            }
            else if (!UserAuth.CheckSMSForExistsUser(Token.Username, sms_code))
            {
                res = MessageCode.ERROR_BAD_SMS;
            }
            else
            {
                UserAuth User = new UserAuth();
                res = User.Load(Token.id);
                if (res == MessageCode.SUCCESS)
                {
                    res = um.RemoveBankCard(User.id, card_id);
                }
            }
            return DataResult.InitFromMessageCode(res);
        }

        /// <summary>
        /// 获取用户银行卡列表
        /// </summary>
        /// <param name="token">短效token</param>
        /// <returns>结果对象</returns>
        [HttpGet]
        [ActionName("BankCardList")]
        public DataResult GetUserBankCardList(string token)
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
                    DataSet ds = um.GetUserBankCardList(User.id);
                    return DataResult.InitFromDataSet(ds);
                }
                return DataResult.InitFromMessageCode(res);
            }
        }

        /// <summary>
        /// 获取用户账户基本信息
        /// </summary>
        /// <param name="token">短效token</param>
        /// <returns>
        /// 结果对象<br/>
        /// uid:用户id<br/>
        /// r_money:奖励金账户<br/>
        /// x_money:退款账户<br/>
        /// y_money:推广奖励金账户<br/>
        /// h_money：金豆账户<br/>
        /// plate_to_return_money:待反金额<br/>
        /// plate_to_return_today:今日购物奖励<br/>
        /// total_plate_to_return_money:累计购物奖励<br/>
        /// recommend_today:今日推荐奖励<br/>
        /// total_recomend_money:累计推荐奖励<br/>
        /// </returns>
        [HttpGet]
        [ActionName("UserAccountInfo")]
        public DataResult GetUserAccountInfo(string token)
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
                    DataSet ds = um.GetUserAccountInfo(User.id);
                    return DataResult.InitFromDataSet(ds);
                }
                return DataResult.InitFromMessageCode(res);
            }
        }

        /// <summary>
        /// 获取用户账户信息
        /// </summary>
        /// <param name="Request">基础token请求</param>
        /// <returns>结果对象</returns>
        [HttpPost]
        [ActionName("UserAccountInfo")]
        public ResultView<UserAccount> GetGetUserAccountInfo2([FromBody]BaseTokenRequest Request)
        {
            int res;
            TokenObject token = TokenObject.InitTokenObjFromString(Request.token);
            if (!string.Equals(TokenObject.ShortTimeToken, token.Prefix))
            {
                res = MessageCode.ERROR_TOKEN_VALIDATE;
                return ResultView<UserAccount>.InitFromMessageCode(res);
            }
            else
            {
                var user = new UserAuth();
                res = user.Load(token.id);
                if (res == MessageCode.SUCCESS)
                {
                    DataSet ds = um.GetUserAccountInfoNew(user.id);
                    var result = ResultView<UserAccount>.InitFromMessageCode(res);
                    result.data = new UserAccount(ds.Tables[0].Rows[0]);
                    return result;
                }
                return ResultView<UserAccount>.InitFromMessageCode(res);
            }
        }

        /// <summary>
        /// 修改信息阅读状态
        /// </summary>
        /// <param name="Request">基础令牌请求</param>
        /// <returns></returns>
        public DataResult MarkInfoRead([FromBody]BaseTokenRequest Request)
        {
            int res;
            TokenObject Token = TokenObject.InitTokenObjFromString(Request.token);
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
                    res = um.MarkUserInfoHasRead(User.id);
                }
                return DataResult.InitFromMessageCode(res);
            }
        }

        /// <summary>
        /// 获取用户帐单列表
        /// </summary>
        /// <param name="Page">页码</param>
        /// <param name="PageSize">页面大小</param>
        /// <param name="token">短效token</param>
        /// <returns>结果对象:1:普通消费,2:每日分配,3:推荐奖励,4:消费抵扣,5:提现,6:消费退款,7:历史积分转换</returns>
        [HttpGet]
        [ActionName("UserMoneyRecord")]
        public DataResult GetUserMoneyRecordList(int Page, int PageSize, string token)
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
                    DataSet ds = um.GetUserMoneyRecord(User.id, Page, PageSize);
                    return DataResult.InitFromDataSet(ds);
                }
                return DataResult.InitFromMessageCode(res);
            }
        }

        /// <summary>
        /// 获取用户收益列表
        /// </summary>
        /// <param name="Request">基础分页请求</param>
        /// <returns>
        /// 结果对象 record_type(1:现金记录):1:普通消费,2:每日分配,3:推荐奖励,4:消费抵扣,5:提现,6:消费退款,7:历史积分转换
        /// record_type(1:应反记录):1:普通消费,2:历史积累,3:系统赠送
        /// </returns>
        [HttpPost]
        [ActionName("UserIncomeRecord")]
        public ResultView<List<UserIncomeRecord>> GetUserIncomeRecordList([FromBody]PaginateTokenRequest Request)
        {
            int res;
            TokenObject Token = TokenObject.InitTokenObjFromString(Request.token);
            if (!string.Equals(TokenObject.ShortTimeToken, Token.Prefix))
            {
                res = MessageCode.ERROR_TOKEN_VALIDATE;
                return ResultView<List<UserIncomeRecord>>.InitFromMessageCode(res);
            }
            else
            {
                UserAuth User = new UserAuth();
                res = User.Load(Token.id);
                if (res == MessageCode.SUCCESS)
                {
                    DataSet ds = um.GetUserIncomeRecord(User.id, Request.Page, Request.PageSize);
                    return ResultView<UserIncomeRecord>.InitFromDataSet(ds);
                }
                return ResultView<List<UserIncomeRecord>>.InitFromMessageCode(res);
            }
        }

        /// <summary>
        /// 申请用户提现
        /// </summary>
        /// <param name="apply_money">申请金额</param>
        /// <param name="type">提现类型:1:r_money(奖励金账户);2:x_money(退款账户);3:y_money(佣金账户)</param>
        /// <param name="card_id">银行卡id</param>
        /// <param name="sms_code">短信验证码</param>
        /// <param name="token">短效token</param>
        /// <returns>结果对象</returns>
        [HttpGet]
        [ActionName("MakeCashApply")]
        public DataResult UserApplyCash(double apply_money, int type, int card_id, string sms_code, string token)
        {
            int res;
            if (type == 3)
            {
                //return new DataResult() { status = 99, message = "推荐奖励出现异常,正在处理中...!" };
            }

            if (apply_money < 9.999999d)
            {
                return new DataResult() { status = 99, message = "提现金额必须大于或等于10元" };
            }
            TokenObject Token = TokenObject.InitTokenObjFromString(token);
            if (!string.Equals(TokenObject.ShortTimeToken, Token.Prefix))
            {
                res = MessageCode.ERROR_TOKEN_VALIDATE;
            }
            else if (!UserAuth.CheckSMSForExistsUser(Token.Username, sms_code))
            {
                res = MessageCode.ERROR_BAD_SMS;
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
                    res = um.ApplyCash(User.id, type, apply_money, card_id);
                }
            }
            var result = DataResult.InitFromMessageCode(res);
            if (res == MessageCode.ERROR_EXECUTE_SQL)
                result.message = "提现超出账户金额数量!";
            return result;
            //DataResult Result = new DataResult
            //{
            //    status = MessageCode.ERROR_UNKONWN,
            //    message = "提现功能异常，现正修复中，请耐心等待",
            //    data = null,
            //    additional_data = null
            //};
            //return Result;
        }

        /// <summary>
        /// 获取用户提现申请列表
        /// </summary>
        /// <param name="Page">页码</param>
        /// <param name="PageSize">页面尺寸</param>
        /// <param name="token">短效token</param>
        /// <returns>结果对象</returns>
        [HttpGet]
        [ActionName("CashApplyList")]
        public DataResult GetUserCashApplyList(int Page, int PageSize, string token)
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
                    DataSet ds = um.GetCashApplyList(User.id, Page, PageSize);
                    return DataResult.InitFromDataSet(ds);
                }
                return DataResult.InitFromMessageCode(res);
            }
        }

        /// <summary>
        /// 获取用户平台应反记录(那啥金币)
        /// </summary>
        /// <param name="Request">基础分页请求</param>
        /// <returns>结果对象</returns>
        [HttpPost]
        [ActionName("PlatToReturnRecord")]
        public ResultView<List<UserIncomeRecord>> GetUserPlatToReturnRecord([FromBody]PaginateTokenRequest Request)
        {
            int res;
            TokenObject Token = TokenObject.InitTokenObjFromString(Request.token);
            if (!string.Equals(TokenObject.ShortTimeToken, Token.Prefix))
            {
                res = MessageCode.ERROR_TOKEN_VALIDATE;
                return ResultView<List<UserIncomeRecord>>.InitFromMessageCode(res);
            }
            else
            {
                UserAuth User = new UserAuth();
                res = User.Load(Token.id);
                if (res == MessageCode.SUCCESS)
                {
                    DataSet ds = um.GetUserPlateToReturnRecord(User.id, Request.Page, Request.PageSize);
                    return ResultView<UserIncomeRecord>.InitFromDataSet(ds);
                }
                return ResultView<List<UserIncomeRecord>>.InitFromMessageCode(res);
            }
        }

        /// <summary>
        /// 获取用户现金账户记录(x_money)
        /// </summary>
        /// <param name="Request">基础分页请求</param>
        /// <returns>结果对象</returns>
        [HttpPost]
        [ActionName("UserXMoneyRecord")]
        public ResultView<List<UserIncomeRecord>> GetUserXMoneyRecord([FromBody]PaginateTokenRequest Request)
        {
            int res;
            TokenObject Token = TokenObject.InitTokenObjFromString(Request.token);
            if (!string.Equals(TokenObject.ShortTimeToken, Token.Prefix))
            {
                res = MessageCode.ERROR_TOKEN_VALIDATE;
                return ResultView<List<UserIncomeRecord>>.InitFromMessageCode(res);
            }
            else
            {
                UserAuth User = new UserAuth();
                res = User.Load(Token.id);
                if (res == MessageCode.SUCCESS)
                {
                    DataSet ds = um.GetUserXMoneyRecord(User.id, Request.Page, Request.PageSize);
                    return ResultView<UserIncomeRecord>.InitFromDataSet(ds);
                }
                return ResultView<List<UserIncomeRecord>>.InitFromMessageCode(res);
            }
        }

        /// <summary>
        /// 获取用户消费账户记录(r_money)
        /// </summary>
        /// <param name="Request">基础分页请求</param>
        /// <returns>结果对象</returns>
        [HttpPost]
        [ActionName("UserRMoneyRecord")]
        public ResultView<List<UserIncomeRecord>> GetUserRMoneyRecord([FromBody]PaginateTokenRequest Request)
        {
            int res;
            TokenObject Token = TokenObject.InitTokenObjFromString(Request.token);
            if (!string.Equals(TokenObject.ShortTimeToken, Token.Prefix))
            {
                res = MessageCode.ERROR_TOKEN_VALIDATE;
                return ResultView<List<UserIncomeRecord>>.InitFromMessageCode(res);
            }
            else
            {
                UserAuth User = new UserAuth();
                res = User.Load(Token.id);
                if (res == MessageCode.SUCCESS)
                {
                    DataSet ds = um.GetUserRMoneyRecord(User.id, Request.Page, Request.PageSize);
                    return ResultView<UserIncomeRecord>.InitFromDataSet(ds);
                }
                return ResultView<List<UserIncomeRecord>>.InitFromMessageCode(res);
            }
        }

        /// <summary>
        /// 获取用户推荐账户记录(y_money)
        /// </summary>
        /// <param name="Request">基础分页请求</param>
        /// <returns>结果对象</returns>
        [HttpPost]
        [ActionName("UserYMoneyRecord")]
        public ResultView<List<UserIncomeRecord>> GetUserYMoneyRecord([FromBody]PaginateTokenRequest Request)
        {
            int res;
            TokenObject Token = TokenObject.InitTokenObjFromString(Request.token);
            if (!string.Equals(TokenObject.ShortTimeToken, Token.Prefix))
            {
                res = MessageCode.ERROR_TOKEN_VALIDATE;
                return ResultView<List<UserIncomeRecord>>.InitFromMessageCode(res);
            }
            else
            {
                UserAuth User = new UserAuth();
                res = User.Load(Token.id);
                if (res == MessageCode.SUCCESS)
                {
                    DataSet ds = um.GetUserYMoneyRecord(User.id, Request.Page, Request.PageSize);
                    return ResultView<UserIncomeRecord>.InitFromDataSet(ds);
                }
                return ResultView<List<UserIncomeRecord>>.InitFromMessageCode(res);
            }
        }

        /// <summary>
        /// 获取用户上线列表
        /// </summary>
        /// <param name="Request">基础分页请求</param>
        /// <returns>结果对象</returns>
        [HttpPost]
        [ActionName("GetUserSpreadorList")]
        public ResultView<List<UserReferral>> GetUserSpreadorList([FromBody]PaginateTokenRequest Request)
        {
            int res;
            TokenObject Token = TokenObject.InitTokenObjFromString(Request.token);
            if (!string.Equals(TokenObject.ShortTimeToken, Token.Prefix))
            {
                res = MessageCode.ERROR_TOKEN_VALIDATE;
                return ResultView<List<UserReferral>>.InitFromMessageCode(res);
            }
            else
            {
                UserAuth User = new UserAuth();
                res = User.Load(Token.id);
                if (res == MessageCode.SUCCESS)
                {
                    DataSet ds = um.GetUserSpreadorList(User.id, Request.Page, Request.PageSize);
                    return ResultView<UserReferral>.InitFromDataSet(ds);
                }
                return ResultView<List<UserReferral>>.InitFromMessageCode(res);
            }
        }

        /// <summary>
        /// 获取用户下线列表
        /// </summary>
        /// <param name="Request">基础分页请求</param>
        /// <returns>结果对象</returns>
        [HttpPost]
        [ActionName("GetUserReferralsList")]
        public ResultView<List<UserReferral>> GetUserReferralsList([FromBody]PaginateTokenRequest Request)
        {
            int res;
            TokenObject Token = TokenObject.InitTokenObjFromString(Request.token);
            if (!string.Equals(TokenObject.ShortTimeToken, Token.Prefix))
            {
                res = MessageCode.ERROR_TOKEN_VALIDATE;
                return ResultView<List<UserReferral>>.InitFromMessageCode(res);
            }
            else
            {
                UserAuth User = new UserAuth();
                res = User.Load(Token.id);
                if (res == MessageCode.SUCCESS)
                {
                    DataSet ds = um.GetUserReferralsList(User.id, Request.Page, Request.PageSize);
                    return ResultView<UserReferral>.InitFromDataSet(ds);
                }
                return ResultView<List<UserReferral>>.InitFromMessageCode(res);
            }
        }

        /// <summary>
        /// 获取用户推荐统计信息
        /// </summary>
        /// <param name="Request">基础token请求</param>
        /// <returns>结果对象</returns>
        [HttpPost]
        [ActionName("UserRecommandCount")]
        public ResultView<UserRecommandCount> GetUserRecommandCount([FromBody]BaseTokenRequest Request)
        {
            int res;
            TokenObject Token = TokenObject.InitTokenObjFromString(Request.token);
            if (!string.Equals(TokenObject.ShortTimeToken, Token.Prefix))
            {
                res = MessageCode.ERROR_TOKEN_VALIDATE;
                return ResultView<UserRecommandCount>.InitFromMessageCode(res);
            }
            else
            {
                UserAuth User = new UserAuth();
                res = User.Load(Token.id);
                if (res == MessageCode.SUCCESS)
                {
                    DataSet ds = um.GetUserRecommandCount(User.id);
                    var result = ResultView<UserRecommandCount>.InitFromMessageCode(res);
                    result.data = new UserRecommandCount(ds.Tables[0].Rows[0]);
                    return result;
                }
                return ResultView<UserRecommandCount>.InitFromMessageCode(res);
            }
        }

        // lcl 20180426 Insert
        /// <summary>
        /// 获取用户推荐统计信息
        /// </summary>
        /// <param name="Request">基础token请求</param>
        /// <returns>结果对象</returns>
        [HttpPost]
        [ActionName("UserRecommandCountV2")]
        public ResultView<UserRecommandCount> GetUserRecommandCountV2([FromBody]BaseTokenRequest Request)
        {
            int res;
            TokenObject Token = TokenObject.InitTokenObjFromString(Request.token);
            if (!string.Equals(TokenObject.ShortTimeToken, Token.Prefix))
            {
                res = MessageCode.ERROR_TOKEN_VALIDATE;
                return ResultView<UserRecommandCount>.InitFromMessageCode(res);
            }
            else
            {
                UserAuth User = new UserAuth();
                res = User.Load(Token.id);
                if (res == MessageCode.SUCCESS)
                {
                    DataSet ds = um.GetUserRecommandCount(User.id);
                    var result = ResultView<UserRecommandCount>.InitFromMessageCode(res);

                    var userRecommand = new UserRecommandCount(ds.Tables[0].Rows[0]);
                    // 本月推荐用户数合计
                    int intMonthRecommandUserCount = 0;
                    // 分级推荐人数统计数据
                    dynamic[] eachLevelRecommandUserCount = new dynamic[] { 0, 0, 0, 0 };
                    // 本月推荐人数排名
                    int intMonthRecommandUserCountRanking = 0;
                    // 本月推荐奖励金额
                    decimal mMonthRecommandRewardsMoney = 0;
                    // 本月推荐奖励排名
                    int intMonthRecommandRewardsRanking = 0;
                    DateTime dtNow = DateTime.Now.Date;
                    // 当月第一天，即1日零点零分零秒
                    DateTime dtStartTime = dtNow.AddDays(-(dtNow.Day) + 1);
                    // 当月最后一天的23点59分59秒
                    DateTime dtEndTime = dtStartTime.AddMonths(1).AddSeconds(-1);

                    // 获取与推荐的用户人数相关的当月的统计数据
                    um.GetPersonStatForRecommand(User.id, User.is_partner, dtStartTime, dtEndTime,
                                                 out intMonthRecommandUserCount, out eachLevelRecommandUserCount, out intMonthRecommandUserCountRanking);
                    userRecommand.month_recommand_user_count = intMonthRecommandUserCount;
                    userRecommand.each_level_recommand_user_count = eachLevelRecommandUserCount;
                    userRecommand.month_recommand_user_count_ranking = intMonthRecommandUserCountRanking;
                    // 获取与推荐奖励相关的当月的统计数据
                    um.GetRewardsStatForRecommand(User.id, dtStartTime, dtEndTime, out mMonthRecommandRewardsMoney, out intMonthRecommandRewardsRanking);
                    userRecommand.month_recommand_rewards_ranking = intMonthRecommandRewardsRanking;
                    // 设置待确认奖励金额
                    userRecommand.month_unconfirm_order_sum_reward = um.getUnConfirmOrderSum_Reward(User.id, dtStartTime.ToString("yyyy-MM"));

                    result.data = userRecommand;
                    return result;
                }
                return ResultView<UserRecommandCount>.InitFromMessageCode(res);
            }
        }

        /// <summary>
        /// 获取用户月度推荐用户统计数据
        /// </summary>
        /// <param name="Request">基础月份请求</param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("MonthRecommandUserData")]
        public ResultView<MonthUserRecommandData> GetMonthRecommandUserData([FromBody]MonthPaginateRequest Request)
        {
            int res;
            TokenObject Token = TokenObject.InitTokenObjFromString(Request.token);
            if (!string.Equals(TokenObject.ShortTimeToken, Token.Prefix))
            {
                res = MessageCode.ERROR_TOKEN_VALIDATE;
                return ResultView<MonthUserRecommandData>.InitFromMessageCode(res);
            }
            else
            {
                UserAuth User = new UserAuth();
                res = User.Load(Token.id);
                if (res == MessageCode.SUCCESS)
                {
                    DataSet ds = um.GetUserMonthRecommandUserCount(User.id, Request.month);
                    DataSet list = um.GetUserMonthRecommandUserList(User.id, Request.month, Request.Page, Request.PageSize);
                    var result = ResultView<MonthUserRecommandData>.InitFromMessageCode(res);
                    result.data = new MonthUserRecommandData(ds.Tables[0].Rows[0], list.Tables[0]);
                    return result;
                }
                return ResultView<MonthUserRecommandData>.InitFromMessageCode(res);
            }
        }

        /// <summary>
        /// 获取用户月度推荐订单统计数据
        /// </summary>
        /// <param name="Request">基础月份请求</param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("MonthRecommandOrderData")]
        public ResultView<MonthOrderRecommandData> GetMonthRecommandOrderData([FromBody]MonthPaginateRequest Request)
        {
            int res;
            TokenObject Token = TokenObject.InitTokenObjFromString(Request.token);
            if (!string.Equals(TokenObject.ShortTimeToken, Token.Prefix))
            {
                res = MessageCode.ERROR_TOKEN_VALIDATE;
                return ResultView<MonthOrderRecommandData>.InitFromMessageCode(res);
            }
            else
            {
                UserAuth User = new UserAuth();
                res = User.Load(Token.id);
                if (res == MessageCode.SUCCESS)
                {
                    DataSet ds = um.GetUserMonthRecommandOrderCount(User.id, Request.month);
                    DataSet list = um.GetUserMonthRecommandOrderList(User.id, Request.month, Request.Page, Request.PageSize);
                    var result = ResultView<MonthOrderRecommandData>.InitFromMessageCode(res);
                    result.data = new MonthOrderRecommandData(ds.Tables[0].Rows[0], list.Tables[0]);
                    return result;
                }
                return ResultView<MonthOrderRecommandData>.InitFromMessageCode(res);
            }
        }

        /// <summary>
        /// 获取用户月度推荐收益统计数据
        /// </summary>
        /// <param name="Request">基础月份请求</param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("MonthRecommandMoneyData")]
        public ResultView<MonthRecommandMoneyData> GetMonthRecommandMoneyData([FromBody]MonthPaginateRequest Request)
        {
            int res;
            TokenObject Token = TokenObject.InitTokenObjFromString(Request.token);
            if (!string.Equals(TokenObject.ShortTimeToken, Token.Prefix))
            {
                res = MessageCode.ERROR_TOKEN_VALIDATE;
                return ResultView<MonthRecommandMoneyData>.InitFromMessageCode(res);
            }
            else
            {
                UserAuth User = new UserAuth();
                res = User.Load(Token.id);
                if (res == MessageCode.SUCCESS)
                {
                    DataSet ds = um.GetUserMonthRecommandMoneyCount(User.id, Request.month);
                    DataSet list = um.GetUserMonthRecommandMoneyList(User.id, Request.month, Request.Page, Request.PageSize);
                    var result = ResultView<MonthRecommandMoneyData>.InitFromMessageCode(res);
                    result.data = new MonthRecommandMoneyData(ds.Tables[0].Rows[0], list.Tables[0]);
                    return result;
                }
                return ResultView<MonthRecommandMoneyData>.InitFromMessageCode(res);
            }
        }

        /// <summary>
        /// 使用短信验证码注册用户(已更新)
        /// </summary>
        /// <param name="smscode">验证码</param>
        /// <param name="username">用户名</param>
        /// <param name="spreader">推荐码</param>
        /// <param name="devicecode"></param>
        /// <param name="channel"></param>
        /// <param name="fromcode">来源编码</param>
        /// <returns>结果对象</returns>
        [HttpGet]
        [ActionName("ResgistWithSMS")]
        public DataResult UserRegistWithSMSCode(string smscode, string username, string spreader = null, string devicecode = null, string channel = null, string channel_open_id = null, string picture_code = null, string fromcode = null)
        {
            // lcl 2018-08-21 Insert
            var httpHeads = HttpContext.Current.Request.Headers;
            var arrValue = httpHeads.GetValues("User-Agent");
            if (httpHeads != null && arrValue.Length > 0 && arrValue[0].ToLower().IndexOf("windows") > -1)
            {
                return new DataResult() { status = 99, message = "请在移动端进行操作!" };
            }

            //todo
            if (string.IsNullOrWhiteSpace(channel_open_id))
            {
                if (string.IsNullOrWhiteSpace(picture_code))
                {
                    return new DataResult() { status = 99, message = "未输入图片验证码!" };
                }
                var redisPictureCode = rdSession.Redis.StringGet(rdSession.SessionId + VerCodeController.IMAGEKEY_SUBFIX);
                if (!picture_code.Equals(redisPictureCode))
                {
                    return new DataResult() { status = 99, message = "图片验证码错误!" };
                }
            }
            //用户不存在就创建用户
            var res = um.RegistWithSMSCode(smscode, username, spreader, devicecode, RRLConfig.PlateSpreaderCode, fromcode);
            if (MessageCode.ERROR_EXIST_USER == res)//用户存在直接登录
            {
                /*var  rts=DataResult.InitFromMessageCode(res);
                rts.message = "用户已经注册过,请直接登录";
                return rts;*/
                string shortTimeToken;
                string longTimeToken;
                var res_login = um.LoginWithSMSCode(username, smscode, devicecode, out longTimeToken, out shortTimeToken);
                if (!string.IsNullOrWhiteSpace(channel_open_id))
                    um.UpdateOpenIdByUserName(username, channel_open_id);//更新微信open_id
                UserAuth user = new UserAuth();
                user.Load(username);
                double spreaderMoney;
                um.ReceiveHongMoney(user.id, user.has_received_spreader_h_money, spreader, out spreaderMoney, "用户注册补送红包");

                var resault = new DataResult
                {
                    status = res_login,
                    data = shortTimeToken,
                    additional_data = longTimeToken,
                    message = MessageCode.TranslateMessageCode(res_login)
                };
                //System.Web.HttpContext.Current.Session["token"] = shortTimeToken;
                rdSession["token"] = shortTimeToken;
                // lcl 2018-07-07 Insert
                resault.add_data = new { isNewUser = 0 };
                return resault;
            }
            //用户不存在
            var result = DataResult.InitFromMessageCode(res);
            if (res == MessageCode.SUCCESS)
            {
                var user = new UserAuth(username);
                double spreaderMoney;
                um.ReceiveHongMoney(user.id, user.has_received_spreader_h_money, spreader, out spreaderMoney, "新用户注册送红包");
                if (!string.IsNullOrWhiteSpace(channel_open_id))
                    um.UpdateOpenIdByUserName(username, channel_open_id);//更新微信open_id
                var token = user.EncryptShortTimeToken();
                result.data = token;
                result.additional_data = user.EncryptLongTimeToken();
                rdSession["token"] = token;
                //System.Web.HttpContext.Current.Session["token"] = result.data;
                //user.Save(); meiyong  zetee

                // lcl 2018-07-07 Insert
                result.add_data = new { isNewUser = 1 };
            }
            return result;
        }

        /// <summary>
        /// 代理商推广用户注册接口(已更新)
        /// </summary>
        /// <param name="smscode">验证码</param>
        /// <param name="username">用户名</param>
        /// <param name="spreader_code">代理商码</param>
        /// <param name="spreader">推荐码</param>
        /// <param name="devicecode"></param>
        /// <param name="channel"></param>
        /// <returns>结果对象</returns>
        [HttpGet]
        [ActionName("ResgistFromPush")]
        private DataResult UserRegistFromPush(string smscode, string username, string spreader_code, string spreader = null, string devicecode = null, string channel = null)
        {
            //todo
            var res = um.RegistWithSMSCode(smscode, username, spreader, devicecode, spreader_code);
            var result = DataResult.InitFromMessageCode(res);
            if (res == MessageCode.SUCCESS)
            {
                var user = new UserAuth(username);
                var token = user.EncryptShortTimeToken();
                result.data = token;
                result.additional_data = user.EncryptLongTimeToken();
                //System.Web.HttpContext.Current.Session["token"] = result.data;
                rdSession["token"] = token;
                user.Save();
            }
            return result;
        }

        /// <summary>
        /// 使用短信验证码登录
        /// </summary>
        /// <param name="UserName">用户名</param>
        /// <param name="SMSCode">短信码</param>
        /// <param name="devicecode"></param>
        /// <param name="channel"></param>
        /// <returns>结果对象</returns>
        [HttpGet]
        [ActionName("LoginWithSMS")]
        public DataResult UserLoginWithSMSCode(string UserName, string SMSCode, string devicecode = null, string channel = null, string picture_code = null)
        {
            //todo
            string shortTimeToken;
            string longTimeToken;
            var res = um.LoginWithSMSCode(UserName, SMSCode, devicecode, out longTimeToken, out shortTimeToken);
            var resault = new DataResult
            {
                status = res,
                data = shortTimeToken,
                additional_data = longTimeToken,
                message = MessageCode.TranslateMessageCode(res)
            };
            //System.Web.HttpContext.Current.Session["token"] = shortTimeToken;
            rdSession["token"] = shortTimeToken;
            return resault;
        }

        /// <summary>
        /// 使用短信码登录或注册
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="smscode">短信验证码</param>
        /// <param name="spreader">推荐人手机号(注册用)</param>
        /// <param name="devicecode"></param>
        /// <param name="channel"></param>
        /// <returns>结果对象</returns>
        [HttpGet]
        [ActionName("LoginOrRegistWithSMS")]
        public DataResult UserLoginOrRegistWithSMSCode(string username, string smscode, string spreader = null, string devicecode = null, string channel = null, string channel_open_id = null)
        {
            //todo
            DataResult result;
            int res;
            if (UserAuth.Exists(username))
            {
                string shortTimeToken;
                string longTimeToken;
                res = um.LoginWithSMSCode(username, smscode, devicecode, out longTimeToken, out shortTimeToken);
                result = new DataResult
                {
                    status = res,
                    data = shortTimeToken,
                    additional_data = longTimeToken,
                    message = MessageCode.TranslateMessageCode(res)
                };
                //System.Web.HttpContext.Current.Session["token"] = shortTimeToken;
                rdSession["token"] = shortTimeToken;
                return result;
            }
            res = um.RegistWithSMSCode(smscode, username, spreader, devicecode, RRLConfig.PlateSpreaderCode);
            result = DataResult.InitFromMessageCode(res);
            if (res != MessageCode.SUCCESS) return result;
            var user = new UserAuth(username);
            var token = user.EncryptShortTimeToken();
            result.data = token;
            result.additional_data = user.EncryptLongTimeToken();
            //System.Web.HttpContext.Current.Session["token"] = result.data;
            rdSession["token"] = token;
            user.Save();
            return result;
        }

        /// <summary>
        /// 领取分享红包（注册登录一体化）
        /// </summary>
        /// <param name="request">红包分享注册请求</param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("ReceiveFreeHongMoney")]
        public DataResult ReceiveFreeHongMoney([FromBody]HongRequest request)
        {
            // lcl 2018-08-21 Insert
            var httpHeads = HttpContext.Current.Request.Headers;
            var arrValue = httpHeads.GetValues("User-Agent");
            if (httpHeads != null && arrValue.Length > 0 && arrValue[0].ToLower().IndexOf("windows") > -1)
            {
                return new DataResult() { status = 99, message = "请在移动端进行操作!" };
            }

            if (string.IsNullOrWhiteSpace(request.picture_code))
            {
                return new DataResult() { status = 99, message = "未输入图片验证码!" };
            }
            var redisPictureCode = rdSession.Redis.StringGet(rdSession.SessionId + VerCodeController.IMAGEKEY_SUBFIX);
            if (!request.picture_code.Equals(redisPictureCode))
            {
                return new DataResult() { status = 99, message = "图片验证码错误!" };
            }

            int res;
            string spreader, spreaderStr = string.Empty;
            double spreaderMoney;
            DataResult result;
            TokenObject Token = TokenObject.InitTokenObjFromString(request.token);
            if (!string.Equals(TokenObject.ShortTimeToken, Token.Prefix))
            {
                spreader = "";
            }
            else
            {
                SqlDataBase db = new SqlDataBase();
                var count = db.ExecuteScalar<int>(@"select count(1) from [rrl_user] where username = @username and isnull(is_locked_login,0)='0'", new { username = Token.Username });
                if (count == 1)
                    spreader = Token.Username;
                else
                    spreader = "";
            }
            var user = new UserAuth();
            if (UserAuth.Exists(request.username))
            {
                string shortTimeToken;
                string longTimeToken;
                res = um.LoginWithSMSCode(request.username, request.smscode, request.devicecode, out longTimeToken, out shortTimeToken);
                user.Load(request.username);

                // lcl 2018-07-12 启用新的自动发红包机制，取消新注册用户的红包规则
                //um.ReceiveHongMoney(user.id, user.has_received_spreader_h_money, spreader, out spreaderMoney);
                //if (user.has_received_spreader_h_money)
                //{
                //    spreaderStr = "您已领取过红包，请勿重复领取。";
                //}
                //else
                //{
                //    spreaderStr = $@"您已成功领取{spreaderMoney}红包。";
                //}


                result = new DataResult
                {
                    status = res,
                    data = shortTimeToken,
                    additional_data = longTimeToken,
                    add_data = spreaderStr,
                    message = MessageCode.TranslateMessageCode(res)
                };
                //System.Web.HttpContext.Current.Session["token"] = shortTimeToken;
                rdSession["token"] = shortTimeToken;
                return result;
            }
           
            res = um.RegistWithSMSCode(request.smscode, request.username, spreader, request.devicecode, RRLConfig.PlateSpreaderCode);
            #region 王龙2018年8月10日20:41:08 分享购物优惠
            if (res == 0)
            {
                um.insert_rrl_order_shareddiscount(request.orderid);
            } 
            #endregion
            result = DataResult.InitFromMessageCode(res);
            if (res != MessageCode.SUCCESS) return result;
            user.Load(request.username);

            // lcl 2018-08-13 新增红包裂变的帮拆操作
            if (!string.IsNullOrWhiteSpace(request.tid) && Token.id > 0)
            {
                // 红包裂变的任务ID有效，并且推荐人有效，则执行帮拆操作
                new GameManager().OpenFissionRedpackage(request.tid, Token.id, user.id);
            }

            // lcl 2018-07-12 启用新的自动发红包机制，取消新注册用户的红包规则
            //um.ReceiveHongMoney(user.id, user.has_received_spreader_h_money, spreader, out spreaderMoney);
            //if (user.has_received_spreader_h_money)
            //{
            //    spreaderStr = "您已领取过红包，请勿重复领取。";
            //}
            //else
            //{
            //    spreaderStr = $@"您已成功领取{spreaderMoney}红包。";
            //}


            var token = user.EncryptShortTimeToken();
            result.data = token;
            result.additional_data = user.EncryptLongTimeToken();
            result.add_data = spreaderStr;
            //System.Web.HttpContext.Current.Session["token"] = result.data;
            rdSession["token"] = token;
            user.Save();
            return result;
        }

        /// <summary>
        /// 绑定用户Openid
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="smscode">验证码</param>
        /// <param name="openid">openid</param>
        /// <param name="type">openid类型:WxOpen(开放平台),WxMP(公众平台)</param>
        /// <param name="spreader">推荐人用户名</param>
        /// <returns>结果对象</returns>
        [HttpGet]
        [ActionName("BindOpenId")]
        public DataResult UserBindOpenIdWithUserName(string username, string smscode, string openid, string type, string spreader = null)
        {
            string shortTimeToken;
            string longTimeToken;
            var res = um.BindOpenId(username, smscode, spreader, openid, type, out longTimeToken, out shortTimeToken);
            var resault = new DataResult
            {
                status = res,
                data = shortTimeToken,
                additional_data = longTimeToken,
                message = MessageCode.TranslateMessageCode(res)
            };
            //System.Web.HttpContext.Current.Session["token"] = shortTimeToken;
            rdSession["token"] = shortTimeToken;
            return resault;
        }

        /// <summary>
        /// 使用Openid申请token
        /// </summary>
        /// <param name="openid">openid</param>
        /// <param name="type">openid类型:WxOpen(开放平台),WxMP(公众平台)</param>
        /// <param name="devicecode"></param>
        /// <param name="channel"></param>
        /// <returns>结果对象</returns>
        [HttpGet]
        [ActionName("ApplyTokenViaOpenId")]
        public DataResult ApplyTokenViaOpenId(string openid, string type, string devicecode = null, string channel = null)
        {
            //todo
            string shortTimeToken;
            string longTimeToken;
            var res = um.ApplyTokenViaOpenid(openid, type, out shortTimeToken, out longTimeToken);
            var resault = new DataResult
            {
                status = res,
                data = shortTimeToken,
                additional_data = longTimeToken,
                message = MessageCode.TranslateMessageCode(res)
            };
            //System.Web.HttpContext.Current.Session["token"] = shortTimeToken;
            rdSession["token"] = shortTimeToken;
            return resault;
        }

        /// <summary>
        /// 请求短效Token
        /// </summary>
        /// <param name="token">长效Token</param>
        /// <param name="devicecode"></param>
        /// <param name="channel"></param>
        /// <returns>结果对象</returns>
        [HttpGet]
        [ActionName("ApplyToken")]
        public DataResult ReApplyShortTimeToken(string token, string devicecode = null, string channel = null)
        {
            //todo
            int res;
            string ShortTimeToken = "";
            TokenObject Token = TokenObject.InitTokenObjFromString(token);
            if (!string.Equals(TokenObject.LongTimeToken, Token.Prefix))
            {
                res = MessageCode.ERROR_TOKEN_VALIDATE;
            }
            else
            {
                res = um.ApplyShortTimeToken(Token.id, out ShortTimeToken);
            }

            DataResult resault = new DataResult();
            resault.status = res;
            resault.data = ShortTimeToken;
            resault.message = MessageCode.TranslateMessageCode(res);
            //System.Web.HttpContext.Current.Session["token"] = ShortTimeToken;
            rdSession["token"] = ShortTimeToken;
            return resault;
        }

        /// <summary>
        /// 用户登出
        /// </summary>
        /// <param name="token">任意有效token</param>
        [HttpGet]
        [ActionName("Logout")]
        public void UserLogout(string token)
        {
            rdSession["token"] = "";
            um.Logout(token);
        }
        #region 获取我拥有的卡券
        /// <summary>
        /// 获取我拥有的卡券
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpGet]
        [ActionName("GetMyCoupons")]
        public DataResult GetMyCoupons(string token, int page_index, int page_size)
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
                    List<TradeManager.MyCoupons> list = um.GetMyCoupons(User.id, page_index, page_size, out int errorCode, out string errMsg);
                    DataResult dataResult = new DataResult();
                    dataResult.data = list;
                    dataResult.status = 0;
                    dataResult.message = "ok";
                    return dataResult;
                }
                return DataResult.InitFromMessageCode(MessageCode.ERROR_TOKEN_VALIDATE);
            }

        }
        #endregion

        // lcl 20180427 Insert
        /// <summary>
        /// 获取用户月度推荐人数排名
        /// </summary>
        /// <param name="Request">基础token请求</param>
        /// <returns>结果对象</returns>
        [HttpPost]
        //[HttpGet]
        [ActionName("MonthRecommandUserCountRanking")]
        public ResultView<dynamic> GetMonthRecommandUserRanking([FromBody]RecommandStatisticsRequest Request)
        {
            int res;
            TokenObject Token = TokenObject.InitTokenObjFromString(Request.token);
            if (!string.Equals(TokenObject.ShortTimeToken, Token.Prefix))
            {
                res = MessageCode.ERROR_TOKEN_VALIDATE;
                return ResultView<dynamic>.InitFromMessageCode(res);
            }
            else
            {
                UserAuth User = new UserAuth();
                res = User.Load(Token.id);
                if (res == MessageCode.SUCCESS)
                {
                    DateTime dtNow = DateTime.Now.Date;
                    // 月份第一天，即1日零点零分零秒
                    DateTime dtStartTime;
                    // 月份最后一天的23点59分59秒
                    DateTime dtEndTime;

                    if (string.IsNullOrWhiteSpace(Request.month))
                    {
                        // 如果年月参数为空，则默认查询全部数据
                        dtStartTime = dtNow.AddYears(-50);
                        dtEndTime = dtNow.AddYears(50);
                    }
                    else
                    {
                        // 解析年月字符串
                        if (!DateTime.TryParse($@"{Request.month}-01", out dtStartTime))
                        {
                            // 如果时间转换失败，退出
                            return new ResultView<dynamic>()
                            {
                                status = -1,
                                message = "日期格式错误"
                            };
                        }
                        // 设置截止时间
                        dtEndTime = dtStartTime.AddMonths(1).AddSeconds(-1);
                    }

                    long lgRecommandUserCountRanking = 0;
                    // 获取推荐人数排名名次数据
                    var monthRanking = um.GetRecommandUserRanking(User.id, dtStartTime, dtEndTime);
                    if (monthRanking != null && monthRanking.Count > 0)
                    {
                        lgRecommandUserCountRanking = monthRanking.First().rows;
                    }

                    // 获取推荐人数排名列表数据
                    var monthRankingList = um.GetRecommandUserRankingList(dtStartTime, dtEndTime);
                    if (monthRankingList != null && monthRankingList.Count > 0)
                    {
                        // 隐藏用户手机号中间四位数字
                        monthRankingList.ForEach(p => p.username = p.username.Remove(3, 4).Insert(3, "****"));
                    }

                    // 获取用户当月的奖励金额总数
                    decimal dRewardsMoney = um.GetUserRewardsCount(User.id, dtStartTime, dtEndTime);

                    var result = ResultView<dynamic>.InitFromMessageCode(res);
                    result.data = new
                    {
                        month = string.IsNullOrWhiteSpace(Request.month) ? "全部" : dtStartTime.ToString("yyyy.MM"), // 当前年月
                        current_user_ranking = lgRecommandUserCountRanking, // 当前用户本月推荐人数排名
                        current_user_rewards = dRewardsMoney, // 当前用户本月推荐奖励金额
                        ranking_list = monthRankingList
                    };
                    return result;
                }
                return ResultView<dynamic>.InitFromMessageCode(res);
            }
        }

        // lcl 20180427 Insert
        /// <summary>
        /// 获取我的推荐用户的多维度统计数据
        /// </summary>
        /// <param name="Request">基础token请求</param>
        /// <returns>结果对象</returns>
        [HttpPost]
        //[HttpGet]
        [ActionName("MyRecommandUserStat")]
        public ResultView<dynamic> GetMyRecommandUserStat([FromBody]BaseTokenRequest Request)
        {
            int res;
            TokenObject Token = TokenObject.InitTokenObjFromString(Request.token);
            if (!string.Equals(TokenObject.ShortTimeToken, Token.Prefix))
            {
                res = MessageCode.ERROR_TOKEN_VALIDATE;
                return ResultView<dynamic>.InitFromMessageCode(res);
            }
            else
            {
                UserAuth User = new UserAuth();
                res = User.Load(Token.id);
                if (res == MessageCode.SUCCESS)
                {
                    var result = ResultView<dynamic>.InitFromMessageCode(res);

                    // 本月、上月、总推荐的推荐用户数合计
                    int intCurrentMonthRecommandUserCount = 0, intLastMonthRecommandUserCount = 0, intAllRecommandUserCount = 0;
                    // 本月、上月、总推荐的分级推荐人数统计
                    dynamic[] currentMonthEachLevelRecommandUser = new dynamic[0], lastMonthEachLevelRecommandUser = new dynamic[0], allEachLevelRecommandUser = new dynamic[0];
                    // 本月、上月、总推荐的推荐人数排名
                    int intCurrentMonthRecommandUserRanking = 0, intLastMonthRecommandUserRanking = 0, intAllRecommandUserRanking = 0;
                    DateTime dtNow = DateTime.Now.Date; // 当前日期时间
                    DateTime dtStartTime; // 开始时间
                    DateTime dtEndTime; // 截止时间

                    // 获取本月数据
                    // 当月第一天，即1日零点零分零秒
                    dtStartTime = dtNow.AddDays(-(dtNow.Day) + 1);
                    // 当月最后一天的23点59分59秒
                    dtEndTime = dtStartTime.AddMonths(1).AddSeconds(-1);
                    um.GetPersonStatForRecommand(User.id, User.is_partner, dtStartTime, dtEndTime,
                                                 out intCurrentMonthRecommandUserCount, out currentMonthEachLevelRecommandUser, out intCurrentMonthRecommandUserRanking);

                    // 获取上月数据
                    // 上月第一天，即1日零点零分零秒
                    dtStartTime = dtNow.AddDays(-(dtNow.Day) + 1).AddMonths(-1);
                    // 上月最后一天的23点59分59秒
                    dtEndTime = dtStartTime.AddMonths(1).AddSeconds(-1);
                    um.GetPersonStatForRecommand(User.id, User.is_partner, dtStartTime, dtEndTime,
                                                 out intLastMonthRecommandUserCount, out lastMonthEachLevelRecommandUser, out intLastMonthRecommandUserRanking);

                    // 获取总推荐数据
                    dtStartTime = dtNow.AddYears(-50);
                    dtEndTime = dtNow.AddYears(50);
                    um.GetPersonStatForRecommand(User.id, User.is_partner, dtStartTime, dtEndTime,
                                                 out intAllRecommandUserCount, out allEachLevelRecommandUser, out intAllRecommandUserRanking);

                    // 构造结果
                    result.data = new
                    {
                        current_month_recommand_user_count = intCurrentMonthRecommandUserCount,
                        current_month_each_level_recommand_user = currentMonthEachLevelRecommandUser,
                        current_month_recommand_user_ranking = intCurrentMonthRecommandUserRanking,
                        last_month_recommand_user_count = intLastMonthRecommandUserCount,
                        last_month_each_level_recommand_user = lastMonthEachLevelRecommandUser,
                        last_month_recommand_user_ranking = intLastMonthRecommandUserRanking,
                        all_recommand_user_count = intAllRecommandUserCount,
                        all_each_level_recommand_user = allEachLevelRecommandUser,
                        all_recommand_user_ranking = intAllRecommandUserRanking
                    };

                    return result;
                }
                return ResultView<dynamic>.InitFromMessageCode(res);
            }
        }

        // lcl 20180427 Insert
        /// <summary>
        /// 获取用户月度推荐奖励排名
        /// </summary>
        /// <param name="Request">基础token请求</param>
        /// <returns>结果对象</returns>
        [HttpPost]
        //[HttpGet]
        [ActionName("MonthRecommandRewardsRanking")]
        public ResultView<dynamic> GetMonthRecommandRewardsRanking([FromBody]RecommandStatisticsRequest Request)
        {
            int res;
            TokenObject Token = TokenObject.InitTokenObjFromString(Request.token);
            if (!string.Equals(TokenObject.ShortTimeToken, Token.Prefix))
            {
                res = MessageCode.ERROR_TOKEN_VALIDATE;
                return ResultView<dynamic>.InitFromMessageCode(res);
            }
            else
            {
                UserAuth User = new UserAuth();
                res = User.Load(Token.id);
                if (res == MessageCode.SUCCESS)
                {
                    DateTime dtNow = DateTime.Now.Date;
                    // 月份第一天，即1日零点零分零秒
                    DateTime dtStartTime;
                    // 月份最后一天的23点59分59秒
                    DateTime dtEndTime;

                    if (string.IsNullOrWhiteSpace(Request.month))
                    {
                        // 如果年月参数为空，则默认查询全部数据
                        dtStartTime = dtNow.AddYears(-50);
                        dtEndTime = dtNow.AddYears(50);
                    }
                    else
                    {
                        // 解析年月字符串
                        if (!DateTime.TryParse($@"{Request.month}-01", out dtStartTime))
                        {
                            // 如果时间转换失败，退出
                            return new ResultView<dynamic>()
                            {
                                status = -1,
                                message = "日期格式错误"
                            };
                        }
                        // 设置截止时间
                        dtEndTime = dtStartTime.AddMonths(1).AddSeconds(-1);
                    }

                    long lgRecommandRewardsRanking = 0;
                    // 获取推荐奖励排名数据
                    var monthRanking = um.GetRecommandRewardsRanking(dtStartTime, dtEndTime);
                    if (monthRanking != null && monthRanking.Count > 0)
                    {
                        // 获取当月排名数据
                        var currentUserRanking = monthRanking.Where(p => p.uid == User.id);
                        if (currentUserRanking.Count() > 0)
                        {
                            // 如果当前用户存在当月数据，则获取名次
                            lgRecommandRewardsRanking = currentUserRanking.First().rows;
                        }
                        // 隐藏用户手机号中间四位数字
                        monthRanking.ForEach(p => p.username = p.username.Remove(3, 4).Insert(3, "****"));
                    }

                    // 获取用户当月的奖励金额总数
                    decimal dRewardsMoney = um.GetUserRewardsCount(User.id, dtStartTime, dtEndTime);

                    var result = ResultView<dynamic>.InitFromMessageCode(res);
                    result.data = new
                    {
                        month = string.IsNullOrWhiteSpace(Request.month) ? "全部" : dtStartTime.ToString("yyyy.MM"), // 当前年月
                        current_user_ranking = lgRecommandRewardsRanking, // 当前用户本月推荐奖励排名
                        current_user_rewards = dRewardsMoney, // 当前用户本月推荐奖励金额
                        ranking_list = monthRanking.Take(50)
                    };
                    return result;
                }
                return ResultView<dynamic>.InitFromMessageCode(res);
            }
        }

        // lcl 20180427 Insert
        /// <summary>
        /// 获取我的推荐奖励的多维度统计数据
        /// </summary>
        /// <param name="Request">基础token请求</param>
        /// <returns>结果对象</returns>
        [HttpPost]
        [HttpGet]
        [ActionName("MyRecommandRewardsStat")]
        public ResultView<dynamic> GetMyRecommandRewardsStat([FromBody]BaseTokenRequest Request)
        {
            int res;
            TokenObject Token = TokenObject.InitTokenObjFromString(Request.token);
            if (!string.Equals(TokenObject.ShortTimeToken, Token.Prefix))
            {
                res = MessageCode.ERROR_TOKEN_VALIDATE;
                return ResultView<dynamic>.InitFromMessageCode(res);
            }
            else
            {
                UserAuth User = new UserAuth();
                res = User.Load(Token.id);
                if (res == MessageCode.SUCCESS)
                {
                    var result = ResultView<dynamic>.InitFromMessageCode(res);

                    // 本月、上月、总推荐的推荐奖励金额
                    decimal mCurrentMonthRecommandRewardsMoney = 0m, mLastMonthRecommandRewardsMoney = 0m, mAllRecommandRewardsMoney = 0m;
                    // 本月、上月、总推荐的推荐奖励排名
                    int intCurrentMonthRecommandRewardsRanking = 0, intLastMonthRecommandRewardsRanking = 0, intAllRecommandRewardsRanking = 0;
                    // 本月、上月待确认奖励金额
                    decimal mCurrentMonthUnConfirmOrderSumReward = 0m, mLastMonthUnConfirmOrderSumReward = 0m;
                    DateTime dtNow = DateTime.Now.Date; // 当前日期时间
                    DateTime dtStartTime; // 开始时间
                    DateTime dtEndTime; // 截止时间

                    // 获取本月数据
                    // 当月第一天，即1日零点零分零秒
                    dtStartTime = dtNow.AddDays(-(dtNow.Day) + 1);
                    // 当月最后一天的23点59分59秒
                    dtEndTime = dtStartTime.AddMonths(1).AddSeconds(-1);
                    um.GetRewardsStatForRecommand(User.id, dtStartTime, dtEndTime,
                                                  out mCurrentMonthRecommandRewardsMoney, out intCurrentMonthRecommandRewardsRanking);
                    // 本月待确认奖励金额
                    mCurrentMonthUnConfirmOrderSumReward = um.getUnConfirmOrderSum_Reward(User.id, dtStartTime.ToString("yyyy-MM"));

                    // 获取上月数据
                    // 上月第一天，即1日零点零分零秒
                    dtStartTime = dtNow.AddDays(-(dtNow.Day) + 1).AddMonths(-1);
                    // 上月最后一天的23点59分59秒
                    dtEndTime = dtStartTime.AddMonths(1).AddSeconds(-1);
                    um.GetRewardsStatForRecommand(User.id, dtStartTime, dtEndTime,
                                                  out mLastMonthRecommandRewardsMoney, out intLastMonthRecommandRewardsRanking);
                    // 上月待确认奖励金额
                    mLastMonthUnConfirmOrderSumReward = um.getUnConfirmOrderSum_Reward(User.id, dtStartTime.ToString("yyyy-MM"));

                    // 获取总推荐数据
                    dtStartTime = dtNow.AddYears(-50);
                    dtEndTime = dtNow.AddYears(50);
                    um.GetRewardsStatForRecommand(User.id, dtStartTime, dtEndTime,
                                                  out mAllRecommandRewardsMoney, out intAllRecommandRewardsRanking);

                    // 构造结果
                    result.data = new
                    {
                        current_month_recommand_rewards_money = mCurrentMonthRecommandRewardsMoney,
                        current_month_recommand_rewards_ranking = intCurrentMonthRecommandRewardsRanking,
                        current_month_unconfirm_order_sum_reward = mCurrentMonthUnConfirmOrderSumReward,
                        last_month_recommand_rewards_money = mLastMonthRecommandRewardsMoney,
                        last_month_recommand_rewards_ranking = intLastMonthRecommandRewardsRanking,
                        last_month_unconfirm_order_sum_reward = mLastMonthUnConfirmOrderSumReward,
                        all_recommand_rewards_money = mAllRecommandRewardsMoney,
                        all_recommand_rewards_ranking = intAllRecommandRewardsRanking
                    };

                    return result;
                }
                return ResultView<dynamic>.InitFromMessageCode(res);
            }
        }


        // 苹果系统。第三方调用的接口
        [HttpPost]
        [HttpGet]
        [ActionName("thrid_part_upload_device_info")]
        /// <summary>
        /// 获取点击广告后设备号信息(针对苹果设备)
        /// </summary>
        /// <param name="osifa">设备号</param>
        /// <param name="mac">ip</param>
        /// <param name="callback_url">回调url</param>
        /// <param name="ymchn">设备来源</param>
        /// <returns></returns>
        public DataResult GetExtension(string osifa, string mac, string callback_url, string ymchn = "")
        {
            //var db = new RRLDB();
            var db = new SqlDataBase();
            DataResult msg = new DataResult();


            if (!string.IsNullOrWhiteSpace(osifa))
            {
                DateTime addtime = DateTime.Now;

                int effectCount = db.ExecuteScalar<int>($@"select count(*) from rrl_three_device_info where osifa=@osifa", new { osifa = osifa });
                if (effectCount == 0)
                {
                    string id = Guid.NewGuid().ToString();
                    string sql = @"INSERT into rrl_three_device_info(Id,osifa,mac,callback_url,add_time,isOpenApp,isLogin,device_source)
                                    VALUES(@id,@osifa,@mac,@callback_url,@addtime,@isOpenApp,@isLogin,@device_source)";
                    var des = db.Execute(sql, new { id = id, osifa = osifa, mac = mac, callback_url = callback_url, addtime = addtime, isOpenApp = 0, isLogin = 0, device_source = ymchn });
                    if (des > 0)
                    {
                        //msg.status = MessageCode.SUCCESS;
                        //msg.message = MessageCode.TranslateMessageCode(MessageCode.SUCCESS);
                        return new DataResult { status = 0, message = "成功" };
                    }
                    else
                    {
                        return new DataResult { status = 99, message = "数据异常" };
                    }

                }
                else
                {
                    UpdateCallback_url(osifa, callback_url, db);//更新callback_url
                    return new DataResult { message = "设备号已存在,更新callback_url成功", status = 99 };
                }
            }
            else
            {
                return new DataResult { message = "设备号为空", status = 99 };

            }



        }

        // 苹果系统。回调第三方接口
        [HttpPost]
        [HttpGet]
        [ActionName("upload_device_for_open_app")]
        /// <summary>
        /// 获取打开app||注册后的设备号
        /// </summary>
        /// <param name="osifa">设备号</param>
        /// <param name="isOpenApp">是否打开app</param>
        /// <param name="is_login">是否注册</param>
        public DataResult GetLoginExtension(string osifa, bool isOpenApp = false, bool is_login = false)

        {
            var db = new SqlDataBase();
            DataResult msg = new DataResult();
            if (!string.IsNullOrWhiteSpace(osifa))
            {
                List<dynamic> ds = db.Select(@"select Id,callback_url,isOpenApp,isLogin from rrl_three_device_info where osifa=@osifa", new { osifa = osifa });
                if (ds.Count > 0)
                {
                    string callback_url = ds[0].callback_url;
                    bool isOpen = ds[0].isOpenApp;
                    bool isLogin = ds[0].isLogin;

                    if (isOpenApp)
                    {
                        ///判断是否打开过app
                        if (!isOpen)
                        {
                            db.Execute(@"update rrl_three_device_info set isOpenApp=1 where osifa=@osifa", new { osifa = osifa });
                            // Callback_url(callback_url);

                        }
                        open_device_info_record(osifa, db);
                    }
                    if (is_login)
                    {
                        //判断是否注册过
                        if (!isLogin)
                        {
                            db.Execute(@"update rrl_three_device_info set isLogin=1 where osifa=@osifa", new { osifa = osifa });
                            Callback_url(callback_url);
                        }
                    }
                    msg.status = MessageCode.SUCCESS;
                    msg.message = MessageCode.TranslateMessageCode(MessageCode.SUCCESS);
                }
                else
                {
                    open_device_info_record(osifa, db);
                }

            }
            else
            {

                return new DataResult() { status = 99, message = "设备号为空" };
            }
            return msg;
        }

        /// <summary>
        /// 记录打开app 的设备信息
        /// </summary>
        /// <param name="osifa"></param>
        /// <param name="db"></param>
        private void open_device_info_record(string osifa, SqlDataBase db)
        {
            string id = Guid.NewGuid().ToString();
            DateTime addtime = DateTime.Now;
            string sql = "insert into rrl_device_record(id,osifa,addtime)VALUES(@id,@osifa,@addtime)";
            db.Execute(sql, new { id = id, osifa = osifa, addtime = addtime });
        }

        /// <summary>
        /// 回调第三方
        /// </summary>
        /// <param name="callback_url"></param>
        private void Callback_url(string callback_url)
        {
            var httpClient = new HttpClient();
            var html = httpClient.Post(callback_url, null, HttpContentTypes.ApplicationJson);
        }

        /// <summary>
        /// 更新callback_url
        /// </summary>
        public void UpdateCallback_url(string osifa, string callback_url, SqlDataBase db)
        {
            string sql = @"update rrl_three_device_info set callback_url=@callback_url where osifa=@osifa";
            db.Execute(sql, new { callback_url = callback_url, osifa = osifa });
        }

        // Android系统。第三方调用的接口
        /// <summary>
        /// 获取第三方安卓设备信息
        /// </summary>
        /// <param name="mac"></param>
        /// <param name="device_id">设备id</param>
        /// <param name="advertising_id">广告id</param>
        /// <param name="callback_url">回调url</param>

        /// <returns></returns>
        [ActionName("three_android_device_info")]
        [Route("SWebApi/Device/three_android_device_info")]
        public DataResult getAndroidInfo(string mac,string device_id,string advertising_id,string callback_url)
        {
            //Console.WriteLine($@"有米调用平台Android接口，mac：{mac}；设备：{device_id}；advertising_id：{advertising_id}；callback_url：{callback_url}。");

            if (!string.IsNullOrWhiteSpace(device_id))
            {
                var db = new SqlDataBase();
               var model= db.Select<dynamic>(@"select device_id,phone from rrl_android_device_info where device_id=@device_id",new { device_id=device_id});
                if (model.Count ==0)
                {
                    string id = Guid.NewGuid().ToString("N");
                    db.Execute(@"insert into rrl_android_device_info(id,mac,device_id,advertising_id,callback_url,addtime)
                    VALUES(@id,@mac,@device_id,@advertising_id,@callback_url,@addtime)",new
                    {
                      id=id,mac=mac,device_id=device_id,advertising_id=advertising_id,callback_url=callback_url,addtime=DateTime.Now
                    }
                    );

                    return new DataResult { status = 0, message = "成功" };
                }
                else
                {
                    return new DataResult { status = 99, message = "设备号已存在" };
                }
            }
            else
            {
                return new DataResult { status = 99, message = "设备号为空" };
            }
        }

        // Android系统。回调第三方接口
        /// <summary>
        /// 获取登录的手机号
        /// </summary>
        /// <returns name="phone">手机号</returns>
        [ActionName("get_register_phone")]
        [Route("SWebApi/Device/get_register_phone")]
        public DataResult getRegisterPhone(string phone, string device_id)
        {
            //Console.WriteLine($@"Android回调有米接口，手机：{phone}；设备：{device_id}。");

            var db = new SqlDataBase();

            try
            {
                db.Execute(@"insert into rrl_android_login_log (device_id ,phone) values (@device_id ,@phone)", new { device_id = device_id, phone = phone });
            }
            catch (Exception ex)
            {
                Console.WriteLine($@"Android回调有米接口，手机：{phone}；设备：{device_id}。异常：{ex.Message} |######| 堆栈：{ex.StackTrace}");
            }

            if (!string.IsNullOrWhiteSpace(phone))
            {
                var model = db.Select<dynamic>("select * from [dbo].[rrl_android_device_info] where phone=@phone and device_id=@device_id", new { phone = phone, device_id = device_id });
                if (model.Count == 0)
                {
                    var effict = db.Select<dynamic>($@"select * from [dbo].[rrl_android_device_info] where device_id='{device_id}' and phone is null");
                    if (effict.Count > 0)
                    {
                        db.Execute(@"update rrl_android_device_info set phone=@phone where device_id=@device_id", new { phone = phone, device_id = device_id });
                        try
                        {
                            db.Execute(@"insert into rrl_android_callback_log (device_id ,phone) values (@device_id ,@phone)", new { device_id = device_id, phone = phone});

                            Callback_url(effict[0].callback_url);
                        }
                        catch { }

                        return new DataResult { status = 0, message = "注册成功" };
                    }
                    else
                    {
                        return new DataResult { status = 99, message = "没有匹配到已知的第三方设备号" };

                    }

                }
                else
                {
                    return new DataResult { status = 99, message = "已注册" };
                }
            }
            else
            {
                return new DataResult { status = 99, message = "手机号为空" };
            }
        }

        // lcl 2018-07-17 Insert
        /// <summary>
        /// 按天统计所有被推荐人在指定月份内的汇总数据
        /// </summary>
        /// <param name="request">基础token请求</param>
        /// <returns></returns>
        [HttpGet]
        public DataResult ProxyStatisticsDaily([FromUri]ProxyStatisticsByMonthlyRequest request)
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

            DateTime dtStart, dtEnd;
            // 构造开始时间，取月份的第一天，即1日
            if (!DateTime.TryParse($@"{request.year}-{request.month}-01", out dtStart))
            {
                // 开始时间转换出错
                return new DataResult() { status = -99, message = "日期格式错误" };
            }
            // 构造截止时间，取月末最后一天的23:59:59
            dtEnd = dtStart.AddMonths(1).AddSeconds(-1);

            var result = DataResult.InitFromMessageCode(MessageCode.SUCCESS);
            // 按天显示的数据
            var DataDaily = um.GetProxyStatisticsByDaily(user.id, dtStart, dtEnd);
            result.data = DataDaily;
            // 月合计
            var montyTotal = um.GetProxyStatistics(user.id, dtStart, dtEnd);
            if (montyTotal.Count > 0)
            {
                result.additional_data = montyTotal.First();
            }

            // 历史盈利
            StringBuilder sbHistory = new StringBuilder();
            // 构造开始时间，取年的第一天，即1月1日
            dtStart = new DateTime(request.year, 1, 1);
            // 构造截止时间，取年末最后一天的23:59:59
            dtEnd = dtStart.AddYears(1).AddSeconds(-1);
            var historyProfit = um.GetHistoryProfit(user.id, dtStart, dtEnd);
            if (historyProfit.Count > 0)
            {
                var total = 0m;
                foreach (var item in historyProfit)
                {
                    sbHistory.Append($@"{item.monthNum}月：{GetFormatNumber(item.profit_total)}  ");
                    total += item.profit_total;
                }
                sbHistory.Append($@"总计：{GetFormatNumber(total)}");
            }
            result.add_data = sbHistory.ToString();

            return result;
        }

        // lcl 2018-07-17 Insert
        /// <summary>
        /// 根据指定的日期，获取所有被推荐人的数据
        /// </summary>
        /// <param name="request">基础token请求</param>
        /// <returns></returns>
        [HttpGet]
        public DataResult ProxyDataByDate([FromUri]ProxyStatisticsByDayRequest request)
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

            DateTime dtDate;
            if (!DateTime.TryParse(request.date, out dtDate))
            {
                // 开始时间转换出错
                return new DataResult() { status = -99, message = "日期格式错误" };
            }

            var result = DataResult.InitFromMessageCode(MessageCode.SUCCESS);
            var dataList = um.GetProxyDataByUserDaily(user.id, dtDate, dtDate);
            result.data = dataList;

            return result;
        }

        // lcl 2018-07-17 Insert
        /// <summary>
        /// 根据指定的年月，获取所有被推荐人的按月统计数据
        /// </summary>
        /// <param name="request">基础token请求</param>
        /// <returns></returns>
        [HttpGet]
        public DataResult ProxyStatisticsByUserMonthly([FromUri]ProxyStatisticsByMonthlyRequest request)
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

            DateTime dtStart, dtEnd;
            // 构造开始时间，取月份的第一天，即1日
            if (!DateTime.TryParse($@"{request.year}-{request.month}-01", out dtStart))
            {
                // 开始时间转换出错
                return new DataResult() { status = -99, message = "日期格式错误" };
            }
            // 构造截止时间，取月末最后一天的23:59:59
            dtEnd = dtStart.AddMonths(1).AddSeconds(-1);

            var result = DataResult.InitFromMessageCode(MessageCode.SUCCESS);
            var dataList = um.GetProxyStatisticsByUser(user.id, dtStart, dtEnd);
            result.data = dataList;

            return result;
        }

        private string GetFormatNumber(dynamic num)
        {
            if (num <= 0)
            {
                return num.ToString();
            }

            return $@"+{num}";
        }
    }
}