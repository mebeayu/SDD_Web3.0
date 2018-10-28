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

namespace RRL.WEB.Areas.WebApi.Controllers
{
    /// <summary>
    /// 用户管理接口
    /// </summary>
    public partial class UserManagerController 
    {
        
        /// <summary>
        /// 获取推荐人列表
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="token"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet]
        [ActionName("getSperaderListByUid")]
        public DataResult getSperaderListByUid(int uid,string token, int pageIndex = 1, int pageSize = 40)
        {
            int res;
            string errorMessage = "";
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
                   
                    dynamic d= um.getSperaderListByUid(uid,out res,out errorMessage,pageIndex,pageSize);
                    return new DataResult() { status = 0, message = "", data = d };
                }
            }
            return  new DataResult() { status = 99, message = "认证失败!" };
        }

        /// <summary>
        /// 用户门槛配置
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="token"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet]
        [ActionName("GetUserHoldMoneyConfig")]
        public DataResult GetUserHoldMoneyConfig(string token=null)
        {
            int res;
            List<dynamic> data = null;
            dynamic hold_money = 0;
            UserAuth User = new UserAuth();
            if (!string.IsNullOrWhiteSpace(token))
            {
                TokenObject Token = TokenObject.InitTokenObjFromString(token);
                if (!string.Equals(TokenObject.ShortTimeToken, Token.Prefix))
                {
                    res = MessageCode.ERROR_TOKEN_VALIDATE;
                }
                else
                {
                    res = User.Load(Token.id);
                    if (res == MessageCode.SUCCESS)
                    {
                        hold_money = um.GetUserHoldMoney(User);
                    }
                }
            }

            if (!string.IsNullOrWhiteSpace(User.level_category))
            {
                // 用户等级类型有效，则获取对应的等级类型的门槛配置数据
                data = um.GetUserHoldMoneyConfig(User.level_category);
            }
            else
            {
                // 用户等级类型无效，则获取默认的门槛配置数据
                data = um.GetUserHoldMoneyConfig();
            }

            return new DataResult() { status = 0, data = data, add_data = new { pay_coupons_total_money = User.pay_coupons_total_money, hold_money = hold_money }, message = "ok" };
        }

        [HttpGet]
        [ActionName("GetUserHoldMoney")]
        public DataResult GetUserHoldMoney(string token)
        {
            int res;
            UserAuth User = new UserAuth();
            dynamic hold_money = 0;
            if (!string.IsNullOrWhiteSpace(token))
            {
                TokenObject Token = TokenObject.InitTokenObjFromString(token);
                if (!string.Equals(TokenObject.ShortTimeToken, Token.Prefix))
                {
                    res = MessageCode.ERROR_TOKEN_VALIDATE;
                }
                else
                {
                    res = User.Load(Token.id);
                    if (res == MessageCode.SUCCESS)
                    {
                        hold_money = um.GetUserHoldMoney(User);
                    }
                }
            }
            return new DataResult() { status = 0, data   = new { pay_coupons_total_money = User.pay_coupons_total_money, hold_money = hold_money }, message = "ok" };
        }

        // lcl 2018-08-24 Insert
        /// <summary>
        /// APP上使用短信验证码注册用户
        /// </summary>
        /// <param name="userdata">加密后的用户注册数据。（加密前的格式：手机号码,短信验证码）</param>
        /// <param name="spreader">推荐人</param>
        /// <param name="channel_open_id">开放式平台上的用户身份唯一ID</param>
        /// <param name="fromcode">来源编码</param>
        /// <returns>结果对象</returns>
        [HttpGet]
        public DataResult ResgistWithSMSInApp(string userdata, string spreader = null, string channel_open_id = null, string fromcode = null)
        {
            if (string.IsNullOrWhiteSpace(userdata))
            {
                return new DataResult() { status = 99, message = "用户登录数据不完整!" };
            }

            string userDataString = DES.DecryptDES(userdata);
            string[] arrUserData = userDataString.Split(',');
            if (arrUserData.Length < 2 || arrUserData[0].Length != 11)
            {
                return new DataResult() { status = 99, message = "输入的数据格式不正确!" };
            }

            string username = arrUserData[0];
            string smscode = arrUserData[1];

            //用户不存在就创建用户
            var res = um.RegistWithSMSCode(smscode, username, spreader, string.Empty, RRLConfig.PlateSpreaderCode, fromcode);
            if (MessageCode.ERROR_EXIST_USER == res)//用户存在直接登录
            {
                string shortTimeToken;
                string longTimeToken;
                var res_login = um.LoginWithSMSCode(username, smscode, string.Empty, out longTimeToken, out shortTimeToken);
                if (!string.IsNullOrWhiteSpace(channel_open_id))
                {
                    um.UpdateOpenIdByUserName(username, channel_open_id);//更新微信open_id
                }
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

                rdSession["token"] = shortTimeToken;
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
                {
                    um.UpdateOpenIdByUserName(username, channel_open_id);//更新微信open_id
                }
                var token = user.EncryptShortTimeToken();
                result.data = token;
                result.additional_data = user.EncryptLongTimeToken();
                rdSession["token"] = token;
                result.add_data = new { isNewUser = 1 };
            }
            return result;
        }

        // lcl 2018-08-27 Insert
        /// <summary>
        /// 记录Android设备信息
        /// </summary>
        /// <param name="request">基础token请求</param>
        /// <returns></returns>
        [HttpGet]
        public DataResult LogAndroidInfo([FromUri]AndroidInfoRequest request)
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

            string strSql = $@"insert into rrl_user_device_info_android
                                      (uid ,imei ,serial ,mac_wifi ,providersName ,networkKoperatorName ,phoneNumber ,addtime)
                               values (@uid ,@imei ,@serial ,@mac_wifi ,@providersName ,@networkKoperatorName ,@phoneNumber ,getdate())";
            new SqlDataBase().Execute(strSql, new
            {
                uid = user.id,
                imei = request.imei,
                serial = request.serial,
                mac_wifi = request.mac_wifi,
                providersName = request.providersName,
                networkKoperatorName = request.networkKoperatorName,
                phoneNumber = request.phoneNumber
            });

            var result = DataResult.InitFromMessageCode(MessageCode.SUCCESS);
            return result;
        }

        // lcl 2018-08-27 Insert
        /// <summary>
        /// 记录IOS设备信息
        /// </summary>
        /// <param name="request">基础token请求</param>
        /// <returns></returns>
        [HttpGet]
        public DataResult LogIosInfo([FromUri]IOSInfoRequest request)
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

            string strSql = $@"insert into rrl_user_device_info_ios
                                      (uid ,deviceName, deviceIPAdress ,uuid ,addtime)
                               values (@uid ,@deviceName, @deviceIPAdress ,@uuid ,getdate())";
            new SqlDataBase().Execute(strSql, new
            {
                uid = user.id,
                deviceName = request.deviceName,
                deviceIPAdress = request.deviceIPAdress,
                uuid = request.uuid
            });

            var result = DataResult.InitFromMessageCode(MessageCode.SUCCESS);
            return result;
        }
    }
}