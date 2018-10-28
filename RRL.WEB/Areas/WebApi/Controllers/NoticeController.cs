using RRL.Core.Manager;
using RRL.Core.Models;
using RRL.Core.Utility;
using RRL.DB;
using RRL.WEB.Models.Response;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Http;
using WebApi.OutputCache.V2;

namespace RRL.WEB.Areas.WebApi.Controllers
{
    /// <summary>
    /// 公告接口
    /// </summary>
    public class NoticeController: ApiController
    {
        ShopManager shop = new ShopManager();
        // GET: WebApi/Notice
        [HttpGet]
        [ActionName("GetNoticeList")]
      
        public DataResult GetNoticeList()
        {
            DataSet ds =  shop.GetNoticeList();
            return DataResult.InitFromDataSet(ds);
        }
        [HttpGet]
        [Route("SWebApi/Notice/GetNoticeList.json")]
        [CacheOutput(ServerTimeSpan = 7200, ClientTimeSpan = 7200, ExcludeQueryStringFromCacheKey = true)]
        public DataResult GetNoticeList_json()
        {
            return GetNoticeList();
        }
        /// <summary>
        ///根据id获取公告内容
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [ActionName("NoticeDetail")]
        public DataResult NoticeDetail(string id)
        {
            HelpManager.MarkPageView(string.Format("/#/NoticeDetail/{0}", id));
            DataResult Resault;
            DataSet ds =  shop.GetNoticeDetail(id);
            Resault = DataResult.InitFromDataSet(ds);
            return Resault;
        }


        /// <summary>
        /// 获取浮窗信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ActionName("Float_Window_info")]
        public DataResult GetFloatInfo()
        {
            var db =new  SqlDataBase();
            DateTime nowTime = DateTime.Now;
          var ds= db.Select<dynamic>($@"select top 1 id ,float_url,float_picUrl,float_start,float_end from rrl_float_window where float_status=1 and '{nowTime}' between float_start and float_end");
            if (ds != null)
            {
                return new DataResult { status = 0, message = "成功", data = ds };
            }
            else
            {
                return new DataResult { status = 99, message = "未查到相关信息", data = ds };
            }
        }

        /// <summary>
        /// 新弹框
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpGet]
        [ActionName("New_Float_Window_Info")]
        public DataResult GetNewFloatInfo(string token)
        {
            TokenObject Token = TokenObject.InitTokenObjFromString(token);
            if (!string.Equals(TokenObject.ShortTimeToken, Token.Prefix))
            {
                return new DataResult { status = MessageCode.ERROR_TOKEN_VALIDATE, message = "Token验证失败" };
            }

            else
            {
                    var db = new SqlDataBase();
                    var model = db.Select<dynamic>($@"select id,is_new from rrl_user where id='{Token.id}'");
                    if (model != null && model.Count > 0)
                    {
                        var is_new = model[0].is_new==null||false?false:true;
                        DateTime nowTime = DateTime.Now;
                        var list = db.Select<dynamic>($@"select top 1 id ,float_url,float_picUrl,float_start,float_end from rrl_float_window where float_status=1 and '{nowTime}' between float_start and float_end");
                        if (!is_new)
                        {
                           int effct=db.Execute($@"update rrl_user set is_new=1 where id='{Token.id}'");
                                {
                                    return new DataResult { status = 0, message = "成功", data = list };
                                }
                        }
                        else
                        {
                            return new DataResult { status = 99, message = "是老用户", data = null };
                        }
                    }
                    else
                    {
                        return new DataResult { status = 99, message = "用户不存在", data = null };
                    }
                     
                }
               
            
        }
        /// <summary>
        /// 兑奖码兑换
        /// </summary>
        /// <param name="code">兑奖码</param>
        /// <param name="token">token</param>
        /// <returns></returns>
        [HttpGet]
        [ActionName("Verif_Activity_Code")]
        public DataResult VerifActivityCode(string code,string token)
        {
            
             TokenObject Token = TokenObject.InitTokenObjFromString(token);
            if (!string.Equals(TokenObject.ShortTimeToken, Token.Prefix))
            {
                return new DataResult { status = MessageCode.ERROR_TOKEN_VALIDATE, message= "Token验证失败" };
            }
            
            else
            {
                UserAuth User = new UserAuth();
                int res = User.Load(Token.id);
                if (res == MessageCode.SUCCESS)
                {
                    var db = new SqlDataBase();
                    var ds = db.Select<dynamic>(@"select id,code,cash_money,goods_id,award_type from rrl_activity   where is_receive=0 and code=@code", new { code = code });
                    if (ds.Count>0)
                    {

                        if (ds[0].award_type == 3)
                        {
                            var count = db.Execute(@"update  rrl_activity set is_receive=1,receive_time=getdate() where code=@code  and is_receive=0", new { code = code });
                            if (count > 0)
                            {
                                UpdateUserH_money(Token.id, ds[0].cash_money, ds[0].award_type);
                            }
                            return new DataResult { status = 0, message = "兑奖成功", data = new { cash_money = ds[0].cash_money, award_type = ds[0].award_type } };

                        }
                        else if (ds[0].award_type == 1)
                        {

                            var count = db.Execute(@"update  rrl_activity set is_receive=1,receive_time=getdate() where code=@code and is_receive=0", new { code = code });
                            if (count > 0)
                            {
                                UpdateUserH_money_free(Token.id, ds[0].cash_money, ds[0].award_type);
                            }
                            return new DataResult { status = 0, message = "兑奖成功", data = new { cash_money = ds[0].cash_money, award_type = ds[0].award_type } };


                        }
                        else
                        {

                            return new DataResult { status = 0, message = "看你信息地址是否完善", data = new { award_type = ds[0].award_type } };

                        }
                        
                        

                       // return new DataResult { status = 0, message = "成功", data = ds };
                      
                    }
                    else
                    {
                        var dt = db.Select<dynamic>(@"select id,code,cash_money,goods_id,award_type from rrl_activity where is_receive=1 and  code=@code", new { code = code });
                        if (dt.Count>0)
                        {
                            return new DataResult { status = 99, message = "已兑过奖" };
                        }
                        else
                        {
                             
                            return new DataResult { status = 99, message = "兑奖码不存在,请重新输入" };
                            
                        }


                    }
                }
                else
                {
                    var Islogin = 1;
                    return new DataResult { status = 99, message = "你还不是我们的用户请先注册",data=Islogin};
                }
            }
            
        }

        /// <summary>
        /// 创建订单
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="cash_money"></param>
        /// <param name="db"></param>
        public int CreateOrderInfo(int uid,decimal cash_money,SqlDataBase db,int goods_id)
        {
            string ordercode = Guid.NewGuid().ToString();
            string receive_name = "";
            string receive_address = "";
            string receive_mobile = "";
            string sql = $@"insert into rrl_order
            (ordercode, shop_id, buyer_id, trans_type, money, status, shop_name, shop_address, area_code,
            lng, lat, receive_name, receive_address, receive_mobile, combine_code, is_paid, is_settled, check_code, message,is_paid_beans) values
                     ('{ordercode}', 1,{ uid},3,{ cash_money},2,'省兜兜自营商城',
            ' ',' ',0,0,
            '{receive_name}','{receive_address}',
            '{receive_mobile}',' ',1,0,' ',' ',1) select @@identity";

         int order_id=Convert.ToInt32(db.ExecuteScalar<dynamic>(sql));
            CreateOrderDetatiInfo(order_id, goods_id, cash_money, db);
            return order_id;
        }

        /// <summary>
        /// 订单详细记录
        /// </summary>
        /// <param name="order_id"></param>
        /// <param name="goods_id"></param>
        /// <param name="cash_money"></param>
        /// <param name="db"></param>
        public void CreateOrderDetatiInfo(int order_id,int goods_id,decimal cash_money,SqlDataBase db)
        {
               string sql = "";
                var ds = db.Select($@"select name,goods_type,pic_id from  rrl_goods where id='{goods_id}'");
                if (ds.Count>0)
                {
                    sql = $@"insert into rrl_order_info_goods
                    (order_id, goods_id, goods_count, goods_type, goods_price, goods_name, goods_pic_id, cash_pay_rate,
                    beans_pay_rate, msg_leave_word) values({ order_id},{ goods_id},1,{0},
                    { cash_money},'{ds[0].name}',
                    {ds[0].pic_id},{0},
                    { 0},'xxxxx')";
                  db.Execute(sql);
                }
             
                ;
           
        }

        [HttpGet]
        public DataResult GetDefalutAddress(string token)
        {
            TokenObject Token = TokenObject.InitTokenObjFromString(token);
            if (!string.Equals(TokenObject.ShortTimeToken, Token.Prefix))
            {
                return new DataResult { status = MessageCode.ERROR_TOKEN_VALIDATE, message = "Token验证失败" };
            }

            else
            {
                var db = new SqlDataBase();
                string addInfo = "";
                string sqlStr = $@"select top 1 uid,area_code,receive_name,receive_address,receive_mobile from rrl_user_receive_info where deletemark is null and uid='{Token.id}'";
                var model = db.Select<dynamic>(sqlStr);
                if (model.Count>0)
                {
                    addInfo = (model[0].area_code.ToString());
                    var province = db.Select<dynamic>($@"select * from area_code where code= '{addInfo.Substring(0, 2)}0000'");
                    var city = db.Select<dynamic>($@"select * from area_code where code='{addInfo.Substring(0, 4)}00'");
                    var county = db.Select<dynamic>($@"select * from area_code where code='{addInfo}'");
                    //addInfo = province[0].Name + city[0].Name + county[0].Name;
                    var dataInfo = new { province = province[0].Name, key1 = addInfo.Substring(0, 2) + "0000", city = city[0].Name, key2 = addInfo.Substring(0, 4) + "00", county = county[0].Name, key3 = addInfo };
                    var data = new
                    {
                        addDefalutInfo = model,
                        provinceCity = dataInfo
                    };
                    return new DataResult { status = 0, message = "成功", data = data};
                }
                else
                {
                    return new DataResult { status = 99, message = "未查到地址信息" };
                }
            }
        }
        /// <summary>
        /// 根据订单id更新地址信息
        /// </summary>
        /// <param name="order_id"></param>
        /// <returns></returns>
        [HttpGet]
        public DataResult UpdateAddressInfo(string code,string token, string receive_name = "", string receive_address = "", string receive_mobile = "", string message = "")
        {
            TokenObject Token = TokenObject.InitTokenObjFromString(token);
            if (!string.Equals(TokenObject.ShortTimeToken, Token.Prefix))
            {
                return new DataResult { status = MessageCode.ERROR_TOKEN_VALIDATE, message = "Token验证失败" };
            }
            else
            {
                var db = new SqlDataBase();
                var model = db.Select(@"select goods_id,award_type from rrl_activity where code=@code and award_type=2 and is_receive=0", new { code=code});
                if (model!=null&&model.Count>0)
                {
                    int effect=db.Execute(@"update  rrl_activity set is_receive=1,receive_time=getdate() where code=@code and is_receive=0", new { code = code });
                    if(effect > 0)
                    {
                        int order_id = CreateOrderInfo(Token.id, 0, db, model[0].goods_id);
                        string sql = $@"update rrl_order set receive_name=@receive_name,receive_address=@receive_address,receive_mobile=@receive_mobile,shop_address=@shop_address,message=@message where id=@order_id";
                        int count = db.Execute(sql,new { receive_name= receive_name, receive_address= receive_address, receive_mobile= receive_mobile ,shop_address=message, message= code, order_id = order_id });
                        //if (1 == 1)
                        {
                            return new DataResult { status = 0, message = "地址信息已更新成功" };
                        }
                    } else
                    {
                        return new DataResult { status = 99, message = "已兑过奖" };
                    }
                }
                else
                {
                    var dt = db.Select<dynamic>(@"select id,code,cash_money,goods_id,award_type from rrl_activity where is_receive=1 and award_type =2 and  code=@code", new { code = code });
                    if (dt.Count > 0)
                    {
                        return new DataResult { status = 99, message = "已兑过奖" };
                    }
                    else
                    {
                        return new DataResult { status = 99, message = "兑奖码不存在,请重新输入" };
                    }
                    
                }
            }
        }

        /// <summary>
        /// 更新金豆账户并记录
        /// </summary>
        /// <param name="id"></param>
        /// <param name="h_money"></param>
        /// <param name="type"></param>
        public void UpdateUserH_money(int id,decimal h_money,int type)
        {
            var db = new SqlDataBase();
            string sql = @"update rrl_user set h_money=h_money+@h_money where id=@id";
            db.Execute(sql,new { h_money = h_money,id=id});
              string sqlStr = @"insert into rrl_user_money_record
            (uid, order_id, money, type, remark, record_type, rec_class)
            values
            (@uid,@order_id,@money,@type,@remark,@record_type,@rec_class)";
            db.Execute(sqlStr,new { uid=id,order_id=0,money=h_money,type=300, remark="活动赠送金豆", record_type=1, rec_class=1});
        }

        /// <summary>
        /// 更新红包账户并记录
        /// </summary>
        /// <param name="id"></param>
        /// <param name="h_money"></param>
        /// <param name="type"></param>
        public void UpdateUserH_money_free(int id, decimal h_money_free, int type)
        {
            var db = new SqlDataBase();
            string sql = @"update rrl_user set h_money_pay=h_money_pay+@h_money_free where id=@id";
           int count= db.Execute(sql, new { h_money_free = h_money_free, id = id });
            string sqlStr = @"insert into rrl_user_money_record
            (uid, order_id, money, type, remark, record_type, rec_class)
            values
            (@uid,@order_id,@money,@type,@remark,@record_type,@rec_class)";
            db.Execute(sqlStr, new { uid = id, order_id = 0, money = h_money_free, type = 301, remark = "活动赠送小红包", record_type = 1, rec_class = 1 });
        }  
    }
}