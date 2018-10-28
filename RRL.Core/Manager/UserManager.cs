using RRL.Core.Models;
using RRL.Core.Utility;
using RRL.DB;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;

namespace RRL.Core.Manager
{
    public partial class UserManager
    {
        /// <summary>
        /// 添加商品收藏
        /// </summary>
        /// <param name="gid">商品id</param>
        /// <param name="uid">用户id</param>
        /// <returns></returns>
        public int AddFavouriteGoods(int gid, int uid)
        {
            RRLDB db = new RRLDB();
            int res = db.ExeCMD(SqlExeUtility.ADD_FAVOURITE_EXEC_STR,
                new SqlParameter("@uid", SqlDbType.Int, 4) { Value = uid },
                new SqlParameter("@favourite_id", SqlDbType.Int, 4) { Value = gid },
                new SqlParameter("@favourite_type", SqlDbType.Int, 4) { Value = 2 });
            db.Close();
            if (res == -1)
                return MessageCode.ERROR_EXECUTE_SQL;
            if (res == 0)
                return MessageCode.HAS_RECORD;
            return MessageCode.SUCCESS;
        }

        

        /// <summary>
        /// 移除商品收藏
        /// </summary>
        /// <param name="gid">商品id</param>
        /// <param name="uid">用户id</param>
        /// <returns></returns>
        public int RemoveFavouriteGoods(int gid, int uid)
        {
            RRLDB db = new RRLDB();
            //todo ？查询商品是否存在（理论上不需要）
            //商品收藏favourite_type=2
            int res = db.ExecSP(@"sp_FAOURITE_REMOVE",
                new SqlParameter("@uid", SqlDbType.Int, 4) { Value = uid },
                new SqlParameter("@favourite_id", SqlDbType.Int, 4) { Value = gid },
                new SqlParameter("@favourite_type", SqlDbType.Int, 4) { Value = 2 });
            db.Close();
            if (res == -1)
                return MessageCode.ERROR_EXECUTE_SQL;
            if (res == 0)
                return MessageCode.HAS_REMOVE;
            return MessageCode.SUCCESS;
        }

       

        /// <summary>
        /// 获取商品收藏列表
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns>数据集</returns>
        public DataSet GetFavouriteGoodsList(int uid, int page, int pageSize)
        {
            var pageId = Math.Max(page, 1);
            var offSet = (pageId - 1) * pageSize;
            //FAVOURITE_GOODS//
            var db = new RRLDB();
            var ds = db.ExeQuery(
                $@"SELECT TOP {
                    pageSize
                } * FROM(SELECT ROW_NUMBER()OVER(ORDER BY id DESC)AS RowNum ,* FROM FAVOURITE_GOODS WHERE uid = {
                    uid
                })AS TEMP WHERE TEMP.RowNum > {offSet}");
            db.Close();
            return ds;
        }

        /// <summary>
        /// 添加商店收藏
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="sid">商店id</param>
        /// <returns>添加结果</returns>
        public int AddFavouriteShop(int sid, int uid)
        {
            RRLDB db = new RRLDB();
            int res = db.ExeCMD(SqlExeUtility.ADD_FAVOURITE_EXEC_STR,
                new SqlParameter("@uid", SqlDbType.Int, 4) { Value = uid },
                new SqlParameter("@favourite_id", SqlDbType.Int, 4) { Value = sid },
                new SqlParameter("@favourite_type", SqlDbType.Int, 4) { Value = 1 });
            db.Close();
            if (res == -1)
                return MessageCode.ERROR_EXECUTE_SQL;
            if (res == 0)
                return MessageCode.HAS_RECORD;
            return MessageCode.SUCCESS;
        }

        /// <summary>
        /// 移除商店收藏
        /// </summary>
        /// <param name="sid">商店id</param>
        /// <param name="uid">用户id</param>
        /// <returns></returns>
        public int RemoveFavouriteShop(int sid, int uid)
        {
            RRLDB db = new RRLDB();
            //todo ？查询商户是否存在（理论上不需要）
            //商品收藏favourite_type=1
            int res = db.ExecSP(@"sp_FAOURITE_REMOVE",
                new SqlParameter("@uid", SqlDbType.Int, 4) { Value = uid },
                new SqlParameter("@favourite_id", SqlDbType.Int, 4) { Value = sid },
                new SqlParameter("@favourite_type", SqlDbType.Int, 4) { Value = 1 });
            db.Close();
            if (res == -1)
                return MessageCode.ERROR_EXECUTE_SQL;
            if (res == 0)
                return MessageCode.HAS_REMOVE;
            return MessageCode.SUCCESS;
        }

        /// <summary>
        /// 获取商家收藏列表
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public DataSet GetFavouriteShopList(int uid, int page, int pageSize)
        {
            var pageId = Math.Max(page, 1);
            var offSet = (pageId - 1) * pageSize;
            //FAVOURITE_SHOP
            var db = new RRLDB();
            var ds = db.ExeQuery(
                $@"SELECT TOP {
                    pageSize
                } * FROM(SELECT ROW_NUMBER()OVER(ORDER BY id DESC)AS RowNum ,* FROM FAVOURITE_SHOP WHERE uid = {
                    uid
                })AS TEMP WHERE TEMP.RowNum > {offSet}");
            db.Close();
            return ds;
        }

        /// <summary>
        /// 获取用户基本信息
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <returns>数据集</returns>
        public DataSet GetUserBaseInfo(int uid)
        {
            //USER_BASE_INFO_VIEW
            var db = new RRLDB();
            var ds = db.ExeQuery(@"SELECT * FROM USER_BASE_INFO_VIEW WHERE uid = @uid",
                new SqlParameter("@uid", SqlDbType.Int, 4) { Value = uid });
            db.Close();
            return ds;
        }

        /// <summary>
        /// 完善认证信息
        /// </summary>
        /// <param name="name">姓名</param>
        /// <param name="code">身份证号</param>
        /// <param name="uid">用户id</param>
        /// <returns></returns>
        public int FillIdentity(string name, string code, int uid)
        {
            var db = new RRLDB();
            var res = db.ExeCMD($@"UPDATE rrl_user SET the_name = '{name}',id_code = '{code}' WHERE id = {uid}");
            db.Close();
            switch (res)
            {
                case -1:
                    return MessageCode.ERROR_EXECUTE_SQL;
                case 0:
                    return MessageCode.HAS_RECORD;
            }
            return MessageCode.SUCCESS;
        }

        /// <summary>
        /// 设置用户个人信息
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="nickName">用户昵称</param>
        /// <param name="sex">性别</param>
        /// <param name="areaCode">区域编码</param>
        /// <returns>设置结果</returns>
        public int SetBaseInfo(int uid, string nickName, int sex, string areaCode)
        {
            var db = new RRLDB();
            var res = db.ExecSP(@"sp_USER_SET_BASE_INFO",
                new SqlParameter("@uid", SqlDbType.Int, 4) { Value = uid },
                new SqlParameter("@nick_name", nickName),
                new SqlParameter("@sex", SqlDbType.Int, 4) { Value = sex },
                new SqlParameter("@area_code", areaCode));
            db.Close();
            switch (res)
            {
                case -1:
                    return MessageCode.ERROR_EXECUTE_SQL;
                case 0:
                    return MessageCode.HAS_RECORD;
            }
            return MessageCode.SUCCESS;
        }

        /// <summary>
        /// 设置用户头像
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="base64">图片base64编码</param>
        /// <returns>结果对象</returns>
        public int SetUserHeadPic(int uid, string base64,out long picIds)
        {
            picIds = 0;
            var picdb = new RRLPICDB();
            var picId = picdb.AddPic("image/jpeg", base64);
            if (picId == 0)
                return MessageCode.ERROR_SAVE_PIC;
            picdb.Close();
            picIds = picId;
            var db = new RRLDB();
            var res = db.ExecSP(@"sp_USER_SET_HEAD_PIC",
                new SqlParameter("@uid", SqlDbType.Int, 4) { Value = uid },
                new SqlParameter("@pic_id", SqlDbType.Int, 8) { Value = picId });
            db.Close();
            switch (res)
            {
                case -1:
                    return MessageCode.ERROR_EXECUTE_SQL;
                case 0:
                    return MessageCode.HAS_RECORD;
            }
            return MessageCode.SUCCESS;
        }

        /// <summary>
        /// 添加收货信息
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="name">用户名</param>
        /// <param name="address">用户地址</param>
        /// <param name="mobile">用户联系电话</param>
        /// <param name="areaCode">用户区域编码</param>
        /// <param name="zipCode">用户邮政编码</param>
        /// <param name="receiveid"></param>
        /// <returns>添加结果</returns>
        public int AddReceiveInfo(int uid, string name, string address, string mobile, string areaCode, string zipCode, out int receiveid)
        {
            var db = new RRLDB();
            var res = db.ExecSP(@"sp_RECEIVE_INFO_ADD",
                new SqlParameter("@uid", SqlDbType.Int, 4) { Value = uid },
                new SqlParameter("@name", name),
                new SqlParameter("@address", address),
                new SqlParameter("@mobile", mobile),
                new SqlParameter("@area_code", areaCode),
                new SqlParameter("@zip_code", zipCode));
            receiveid = Convert.ToInt32(db.sm.Parameters["@return"].Value);
            db.Close();
            switch (res)
            {
                case -1:
                    return MessageCode.ERROR_EXECUTE_SQL;
                case 0:
                    return MessageCode.HAS_RECORD;
            }
            return MessageCode.SUCCESS;
        }

        /// <summary>
        /// 设置默认收货信息
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="receiveId">收件人id</param>
        /// <returns>设置结果</returns>
        public int SetDefaultReceiveInfo(int uid, int receiveId)
        {
            var db = new RRLDB();
            var res = db.ExecSP(@"sp_RECEIVE_INFO_SET_AS_DEFAULT",
                new SqlParameter("@receive_id", SqlDbType.Int, 4) { Value = receiveId },
                new SqlParameter("@uid", SqlDbType.Int, 4) { Value = uid });
            db.Close();
            switch (res)
            {
                case -1:
                    return MessageCode.ERROR_EXECUTE_SQL;
                case 0:
                    return MessageCode.HAS_RECORD;
            }
            return MessageCode.SUCCESS;
        }

        /// <summary>
        /// 以默认收货地址方式添加收货地址
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="name">用户名</param>
        /// <param name="address">用户地址</param>
        /// <param name="mobile">用户联系电话</param>
        /// <param name="areaCode">用户区域编码</param>
        /// <param name="zipCode">用户邮政编码</param>
        /// <param name="receiveid"></param>
        /// <returns>添加结果</returns>
        public int AddReceiveInfoAsDefault(int uid, string name, string address, string mobile, string areaCode, string zipCode, out int receiveid)
        {
            var db = new RRLDB();
            var res = db.ExecSP(@"sp_RECEIVE_INFO_ADD_AS_DEFAULT",
                new SqlParameter("@uid", SqlDbType.Int, 4) { Value = uid },
                new SqlParameter("@name", name),
                new SqlParameter("@address", address),
                new SqlParameter("@mobile", mobile),
                new SqlParameter("@area_code", areaCode),
                new SqlParameter("@zip_code", zipCode));
            receiveid = Convert.ToInt32(db.sm.Parameters["@return"].Value);
            db.Close();
            switch (res)
            {
                case -1:
                    return MessageCode.ERROR_EXECUTE_SQL;
                case 0:
                    return MessageCode.HAS_RECORD;
            }
            return MessageCode.SUCCESS;
        }

        /// <summary>
        /// 移除收货信息
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="receiveId">收货信息标识</param>
        /// <returns>移除结果</returns>
        public int RemoveReceiveInfo(int uid, int receiveId)
        {
            var db = new RRLDB();
            var res = db.ExecSP(@"sp_RECEIVE_INFO_REMOVE",
                new SqlParameter("@uid", SqlDbType.Int, 4) { Value = uid },
                new SqlParameter("@receive_id", SqlDbType.Int, 4) { Value = receiveId });
            db.Close();
            switch (res)
            {
                case -1:
                    return MessageCode.ERROR_EXECUTE_SQL;
                case 0:
                    return MessageCode.HAS_RECORD;
            }
            return MessageCode.SUCCESS;
        }

        /// <summary>
        /// 查询用户收货信息列表
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <returns>结果数据集</returns>
        public DataSet GetUserReceiveInfoList(int uid)
        {
            //USER_RECEIVE_LIST_VIEW
            var db = new RRLDB();
            var ds = db.ExeQuery(@"SELECT * FROM USER_RECEIVE_LIST_VIEW WHERE uid = @uid",
                new SqlParameter("@uid", SqlDbType.Int, 4) { Value = uid });
            db.Close();
            return ds;
        }

        /// <summary>
        /// 获取默认收货地址
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <returns>数据集</returns>
        public DataSet GetDefaultReceiveInfo(int uid)
        {
            var db = new RRLDB();
            var ds = db.ExeQuery(@"SELECT * FROM USER_RECEIVE_LIST_VIEW WHERE id = dbo.fn_GET_DEFAULT_RECEIVE_ID(@uid)",
                new SqlParameter("@uid", SqlDbType.Int, 4) { Value = uid });
            db.Close();
            return ds;
        }

        /// <summary>
        /// 添加银行卡
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="bankEn">银行卡英文缩写</param>
        /// <param name="cardNo">银行卡号</param>
        /// <param name="cardHolder">持卡人姓名</param>
        /// <returns>添加结果</returns>
        public int AddBankCard(int uid, string bankEn, string cardNo, string cardHolder)
        {
            var db = new RRLDB();
            var res = db.ExecSP(@"sp_USER_BANK_CARD_ADD",
                new SqlParameter("@uid", SqlDbType.Int, 4) { Value = uid },
                new SqlParameter("@bank_en", bankEn),
                new SqlParameter("@card_no", cardNo),
                new SqlParameter("@card_holder", cardHolder));
            db.Close();
            switch (res)
            {
                case -1:
                    return MessageCode.ERROR_EXECUTE_SQL;
                case 0:
                    return MessageCode.HAS_RECORD;
            }
            return MessageCode.SUCCESS;
        }

        /// <summary>
        /// 移除银行卡
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="cardId">银行卡id</param>
        /// <returns>移除结果</returns>
        public int RemoveBankCard(int uid, int cardId)
        {
            var db = new RRLDB();
            var res = db.ExecSP(@"sp_USER_BANKCARD_REMOVE",
                new SqlParameter("@uid", SqlDbType.Int, 4) { Value = uid },
                new SqlParameter("@card_id", SqlDbType.Int, 4) { Value = cardId });
            db.Close();
            switch (res)
            {
                case -1:
                    return MessageCode.ERROR_EXECUTE_SQL;
                case 0:
                    return MessageCode.HAS_RECORD;
            }
            return MessageCode.SUCCESS;
        }

        /// <summary>
        /// 查询用户银行卡列表
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <returns>数据集</returns>
        public DataSet GetUserBankCardList(int uid)
        {
            //USER_BANK_CARD_VIEW
            RRLDB db = new RRLDB();
            DataSet ds = db.ExeQuery(@"SELECT * FROM USER_BANK_CARD_VIEW WHERE uid = @uid",
                new SqlParameter("@uid", SqlDbType.Int, 4) { Value = uid });
            db.Close();
            return ds;
        }

        /// <summary>
        /// 查询用户账户信息
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <returns>数据集</returns>
        public DataSet GetUserAccountInfo(int uid)
        {
            //USER_ACCOUNT_VIEW
            RRLDB db = new RRLDB();
            DataSet ds = db.ExeQuery(@"SELECT * FROM USER_ACCOUNT_VIEW WHERE uid = @uid",
                new SqlParameter("@uid", SqlDbType.Int, 4) { Value = uid });
            db.Close();
            return ds;
        }

        /// <summary>
        /// 查询用户账户信息
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <returns>数据集</returns>
        public DataSet GetUserAccountInfoNew(int uid)
        {
            RRLDB db = new RRLDB();
            DataSet ds = db.ExeQuery($@"SELECT a.* ,isnull((SELECT   COUNT(*) AS Expr1
                     FROM      dbo.rrl_coupons AS c
                     WHERE   (c.uid = a.id) AND (is_used = 0) AND (is_paid = 1)  ),0) AS coupons_count
FROM rrl_user a WHERE a.id = {uid}");
            db.Close();
            return ds;
        }

        /// <summary>
        /// 修改消息阅读状态
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <returns></returns>
        public int MarkUserInfoHasRead(int uid)
        {
            RRLDB db = new RRLDB();
            int res = db.ExeCMD($@"UPDATE rrl_user SET if_info_has_read = 1 WHERE id = {uid}");
            db.Close();
            return res;
        }

        /// <summary>
        /// 获取用户帐单列表
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="page">页码</param>
        /// <param name="pageSize">页面大小</param>
        /// <returns>数据集</returns>
        public DataSet GetUserMoneyRecord(int uid, int page, int pageSize)
        {
            var pageId = Math.Max(page, 1);
            var offSet = (pageId - 1) * pageSize;
            //FAVOURITE_SHOP
            var db = new RRLDB();
            var ds = db.ExeQuery(
                $@"SELECT TOP {
                    pageSize
                } * FROM(SELECT ROW_NUMBER()OVER(ORDER BY id DESC)AS RowNum ,* FROM rrl_user_money_record WHERE uid = {
                    uid
                })AS TEMP WHERE TEMP.RowNum > {offSet}");
            db.Close();
            return ds;
        }

        /// <summary>
        /// 获取用收益列表
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="page">页码</param>
        /// <param name="pageSize">页面大小</param>
        /// <returns>数据集</returns>
        public DataSet GetUserIncomeRecord(int uid, int page, int pageSize)
        {
            var pageId = Math.Max(page, 1);
            var offSet = (pageId - 1) * pageSize;
            //FAVOURITE_SHOP
            var db = new RRLDB();
            var ds = db.ExeQuery($@"SELECT TOP {pageSize} * FROM ( SELECT ROW_NUMBER () OVER (ORDER BY addtime DESC) AS RowNum ,* FROM ( SELECT * FROM rrl_user_money_record WHERE uid = {uid}   UNION SELECT * FROM rrl_user_plate_to_return_record WHERE uid = {uid} AND money > 0 ) AS record ) AS TEMP WHERE TEMP.RowNum > {offSet}");
            db.Close();
            return ds;
        }

        /// <summary>
        /// 申请提现
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="type">提现类型:1:r_money(奖励金账户);2:x_money(退款账户);3:y_money(佣金账户)</param>
        /// <param name="applyMoney">申请提现金额</param>
        /// <param name="cardId">银行卡id</param>
        /// <returns>添加结果</returns>
        public int ApplyCash(int uid, int type, double applyMoney, int cardId)
        {
            var db = new RRLDB();
            var res = db.ExecSP(@"sp_CASH_APPLY_ADD",
                new SqlParameter("@uid", SqlDbType.Int, 4) { Value = uid },
                new SqlParameter("@card_id", SqlDbType.Int, 4) { Value = cardId },
                new SqlParameter("@type", SqlDbType.Int, 4) { Value = type },
                new SqlParameter("@apply_money", SqlDbType.Decimal, 12) { Value = applyMoney, Precision = 12, Scale = 2 });
            db.Close();
            switch (res)
            {
                case -1:
                    return MessageCode.ERROR_EXECUTE_SQL;
                case 0:
                    return MessageCode.HAS_RECORD;
            }
            return MessageCode.SUCCESS;
        }

        /// <summary>
        /// 获取提现申请记录列表
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="page">页码</param>
        /// <param name="pageSize">页面尺寸</param>
        /// <returns>数据集</returns>
        public DataSet GetCashApplyList(int uid, int page, int pageSize)
        {
            //CASH_APPLY_LIST_VIEW
            var pageId = Math.Max(page, 1);
            var offSet = (pageId - 1) * pageSize;
            var db = new RRLDB();
            var ds = db.ExeQuery(
                $@"SELECT TOP {
                    pageSize
                } * FROM(SELECT ROW_NUMBER()OVER(ORDER BY id DESC)AS RowNum ,* FROM CASH_APPLY_LIST_VIEW WHERE uid = {
                    uid
                })AS TEMP WHERE TEMP.RowNum > {offSet}");
            db.Close();
            return ds;
        }

        /// <summary>
        /// 获取用户平台待反(金币)记录
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="page">页码</param>
        /// <param name="pageSize">页面大小</param>
        /// <returns>数据集</returns>
        public DataSet GetUserPlateToReturnRecord(int uid, int page, int pageSize)
        {
            var pageId = Math.Max(page, 1);
            var offSet = (pageId - 1) * pageSize;
            //FAVOURITE_SHOP
            var db = new RRLDB();
            var ds = db.ExeQuery($@"SELECT TOP {pageSize} * FROM ( SELECT ROW_NUMBER () OVER (ORDER BY addtime DESC) AS RowNum ,* FROM ( SELECT * FROM rrl_user_plate_to_return_record WHERE uid = {uid} AND type <> 5) AS record ) AS TEMP WHERE TEMP.RowNum > {offSet}");
            db.Close();
            return ds;
        }

        /// <summary>
        /// 获取用户现金账户记录(x_money)
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="page">页码</param>
        /// <param name="pageSize">页面大小</param>
        /// <returns>数据集</returns>
        public DataSet GetUserXMoneyRecord(int uid, int page, int pageSize)
        {
            var pageId = Math.Max(page, 1);
            var offSet = (pageId - 1) * pageSize;
            var db = new RRLDB();
            var ds = db.ExeQuery($@"SELECT TOP {pageSize} * FROM ( SELECT ROW_NUMBER () OVER (ORDER BY addtime DESC) AS RowNum ,* FROM ( SELECT * FROM rrl_user_money_record WHERE uid = {uid} AND type IN (10,12,6)) AS record ) AS TEMP WHERE TEMP.RowNum > {offSet}");
            db.Close();
            return ds;
        }

        /// <summary>
        /// 获取用户消费账户记录(r_money)
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="page">页码</param>
        /// <param name="pageSize">页面大小</param>
        /// <returns>数据集</returns>
        public DataSet GetUserRMoneyRecord(int uid, int page, int pageSize)
        {
            var pageId = Math.Max(page, 1);
            var offSet = (pageId - 1) * pageSize;
            var db = new RRLDB();
            var ds = db.ExeQuery($@"SELECT TOP {pageSize} * FROM ( SELECT ROW_NUMBER () OVER (ORDER BY addtime DESC) AS RowNum ,* FROM ( SELECT * FROM rrl_user_money_record WHERE uid = {uid} AND type IN (2,4,7,9)) AS record ) AS TEMP WHERE TEMP.RowNum > {offSet}");
            db.Close();
            return ds;
        }

        /// <summary>
        /// 获取用户推荐账户记录(y_money)
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="page">页码</param>
        /// <param name="pageSize">页面大小</param>
        /// <returns>数据集</returns>
        public DataSet GetUserYMoneyRecord(int uid, int page, int pageSize)
        {
            var pageId = Math.Max(page, 1);
            var offSet = (pageId - 1) * pageSize;
            var db = new RRLDB();
            var ds = db.ExeQuery($@"SELECT TOP {pageSize} * FROM ( SELECT ROW_NUMBER () OVER (ORDER BY addtime DESC) AS RowNum ,* FROM ( SELECT * FROM rrl_user_money_record WHERE uid = {uid} AND type IN (3,11)) AS record ) AS TEMP WHERE TEMP.RowNum > {offSet}");
            db.Close();
            return ds;
        }

        /// <summary>
        /// 获取用户下线
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="page">页码</param>
        /// <param name="pageSize">页面大小</param>
        /// <returns>数据集</returns>
        public DataSet GetUserReferralsList(int uid, int page, int pageSize)
        {
            var pageId = Math.Max(page, 1);
            var offSet = (pageId - 1) * pageSize;
            var db = new RRLDB();
            var ds = db.ExeQuery($@"SELECT TOP {pageSize} * FROM ( SELECT ROW_NUMBER () OVER (ORDER BY addtime DESC) AS RowNum ,* FROM ( SELECT id,LEFT(username,3 ) + '****'+ RIGHT(username,4 ) AS username, addtime FROM rrl_user WHERE spreader_uid = {uid}) AS list ) AS TEMP WHERE TEMP.RowNum > {offSet}");
            db.Close();
            return ds;
        }

        /// <summary>
        /// 获取用户推荐数据
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <returns></returns>
        public DataSet GetUserRecommandCount(int uid)
        {
            var now = DateTime.Now;

            var start = new DateTime(now.Year, now.Month, 1);
            var end = start.AddMonths(1);

            var startStr = start.ToString(CultureInfo.InvariantCulture);
            var endStr = end.ToString(CultureInfo.InvariantCulture);

            var db = new RRLDB();
            var ds = db.ExeQuery($@"SELECT
	{uid} AS id,
	(
		SELECT
			is_shop_keeper
		FROM
			rrl_user
		WHERE
			id = {uid}
	) AS is_shop_keeper,
	(
		SELECT
			ISNULL(COUNT (*),0)
		FROM
			sperader_relation
		WHERE
			sperader_uid = {uid}
		OR sperader_sperader_uid = {uid}
		OR sperader_sperader_sperader_uid = {uid}
	) AS total_recommand_user_count,
	(
		SELECT
			ISNULL(COUNT (*),0)
		FROM
			sperader_relation
		WHERE
			sperader_uid = {uid}
	) AS recommand_user_first_class,
	(
		SELECT
			ISNULL(COUNT (*),0)
		FROM
			sperader_relation
		WHERE
			sperader_sperader_uid = {uid}
	) AS recommand_user_second_class,
	(
		SELECT
			ISNULL(COUNT (*),0)
		FROM
			sperader_relation
		WHERE
			sperader_sperader_sperader_uid = {uid}
	) AS recommand_user_third_class,
	(
		SELECT
			ISNULL(COUNT (*),0)
		FROM
			rrl_user
		WHERE
			id IN (
				SELECT
					uid
				FROM
					sperader_relation
				WHERE
					sperader_uid = {uid}
				OR sperader_sperader_uid = {uid}
				OR sperader_sperader_sperader_uid = {uid}
			)
		AND total_cash > 0
	) AS recommand_user_consumed,
	(
		SELECT
			ISNULL(COUNT (*),0)
		FROM
			rrl_user
		WHERE
			id IN (
				SELECT
					uid
				FROM
					sperader_relation
				WHERE
					sperader_uid = {uid}
				OR sperader_sperader_uid = {uid}
				OR sperader_sperader_sperader_uid = {uid}
			)
		AND total_cash = 0
	) AS recommand_user_not_consumed,
	(
		SELECT
			ISNULL(SUM(money), 0)
		FROM
			rrl_user_money_record
		WHERE
			uid = {uid}
		AND type = 3
	) AS total_recommand_award,
	(
		SELECT
			ISNULL(SUM(money), 0)
		FROM
			rrl_user_money_record
		WHERE
			uid = {uid}
		AND type = 3
		AND addtime BETWEEN '{startStr}'
		AND '{endStr}'
	) AS month_recommand_award,
	(
		SELECT
			ISNULL(COUNT (*),0)
		FROM
			rrl_order
		WHERE
			buyer_id IN (
				SELECT
					uid
				FROM
					sperader_relation
				WHERE
					sperader_uid = {uid}
				OR sperader_sperader_uid = {uid}
				OR sperader_sperader_sperader_uid = {uid}
			)
	) AS total_recommand_order_count,
	(
		SELECT
			ISNULL(COUNT (*),0)
		FROM
			rrl_order
		WHERE
			buyer_id IN (
				SELECT
					uid
				FROM
					sperader_relation
				WHERE
					sperader_uid = {uid}
				OR sperader_sperader_uid = {uid}
				OR sperader_sperader_sperader_uid = {uid}
			)
		AND addtime BETWEEN '{startStr}'
		AND '{endStr}'
	) AS month_recommand_order_count");
            db.Close();
            return ds;
        }

        /// <summary>
        /// 获取月度推荐用户列表
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="month">月份</param>
        /// <param name="page">页码</param>
        /// <param name="pagesize">页面尺寸</param>
        /// <returns></returns>
        public DataSet GetUserMonthRecommandUserList(int uid, int month, int page, int pagesize)
        {

            int pageid = Math.Max(page, 1);
            int offSet = (pageid - 1) * pagesize;

            var now = DateTime.Now;

            DateTime start, end;

            if (month > 0 && month < 13)
            {
                start = new DateTime(now.Year, month, 1);
                end = start.AddMonths(1);
            }
            else
            {
                start = new DateTime(now.Year, 1, 1);
                end = start.AddMonths(12);
            }

            var startStr = start.ToString(CultureInfo.InvariantCulture);
            var endStr = end.ToString(CultureInfo.InvariantCulture);

            //var datecondition = $@"addtime BETWEEN '{startStr}' AND '{endStr}'";

            var queryStr = $@"
SELECT TOP({pagesize}) * 
FROM
    (
        SELECT
            ROW_NUMBER () OVER (ORDER BY rec_list.addtime DESC) AS RowNum,
	        rec_list.*
        FROM
	        (select a.id,a.username,a.real_name,a.addtime,a.total_cash,1 as recommand_class, (select count(*) from rrl_user c where c.spreader_uid=a.id ) as recommand_count from 
              rrl_user a where a.spreader_uid={uid} and  addtime BETWEEN '{startStr}' AND '{endStr}'
                 ) AS rec_list        
    ) AS tmptab
WHERE tmptab.RowNum > {offSet}";

            var db = new RRLDB();

            var ds = db.ExeQuery(queryStr);
            db.Close();
            return ds;
        }

        /// <summary>
        /// 获取月度推荐用户统计数据
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="month">月份</param>
        /// <returns></returns>
        public DataSet GetUserMonthRecommandUserCount(int uid, int month)
        {
            var now = DateTime.Now;

            DateTime start, end;

            if (month > 0 && month < 13)
            {
                start = new DateTime(now.Year, month, 1);
                end = start.AddMonths(1);
            }
            else
            {
                start = new DateTime(now.Year, 1, 1);
                end = start.AddMonths(12);
            }

            var startStr = start.ToString(CultureInfo.InvariantCulture);
            var endStr = end.ToString(CultureInfo.InvariantCulture);

            var db = new RRLDB();
            var ds = db.ExeQuery($@"SELECT Top(1)
    base_table.id,
    base_table.is_shop_keeper,
	(
		base_table.month_first_class_recommand_user + base_table.month_second_class_recommand_user + base_table.month_third_class_recommand_user
	) AS month_total_recommand_user,
	base_table.month_first_class_recommand_user,
	base_table.month_second_class_recommand_user,
	base_table.month_third_class_recommand_user,
	base_table.month_consumed_recommand_user,
	base_table.month_consumed_not_consumed_user
FROM
	(
		SELECT
            {uid} AS id,
	        (
		        SELECT
			        is_shop_keeper
		        FROM
			        rrl_user
		        WHERE
			        id = {uid}
	        ) AS is_shop_keeper,
			(
				SELECT
					COUNT (*)
				FROM
					sperader_relation
				WHERE
					sperader_uid = {uid}
				AND addtime BETWEEN '{startStr}'
				AND '{endStr}'
			) AS month_first_class_recommand_user,
			(
				SELECT
					COUNT (*)
				FROM
					sperader_relation
				WHERE
					sperader_sperader_uid = {uid}
				AND addtime BETWEEN '{startStr}'
				AND '{endStr}'
			) AS month_second_class_recommand_user,
			(
				SELECT
					COUNT (*)
				FROM
					sperader_relation
				WHERE
					sperader_sperader_sperader_uid = {uid}
				AND addtime BETWEEN '{startStr}'
				AND '{endStr}'
			) AS month_third_class_recommand_user,
			(
				SELECT
					COUNT (*)
				FROM
					rrl_user
				WHERE
					id IN (
						SELECT
							uid
						FROM
							sperader_relation
						WHERE
							sperader_uid = {uid}
						OR sperader_sperader_uid = {uid}
						OR sperader_sperader_sperader_uid = {uid}
						AND addtime BETWEEN '{startStr}'
						AND '{endStr}'
					)
				AND total_cash > 0
			) AS month_consumed_recommand_user,
			(
				SELECT
					COUNT (*)
				FROM
					rrl_user
				WHERE
					id IN (
						SELECT
							uid
						FROM
							sperader_relation
						WHERE
							sperader_uid = {uid}
						OR sperader_sperader_uid = {uid}
						OR sperader_sperader_sperader_uid = {uid}
						AND addtime BETWEEN '{startStr}'
						AND '{endStr}'
					)
				AND total_cash = 0
			) AS month_consumed_not_consumed_user
	) AS base_table");
            db.Close();
            return ds;
        }

        /// <summary>
        /// 获取月度推荐订单列表
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="month">月份</param>
        /// <param name="page">页码</param>
        /// <param name="pagesize">页面尺寸</param>
        /// <returns></returns>
        public DataSet GetUserMonthRecommandOrderList(int uid, int month, int page, int pagesize)
        {
            int pageid = Math.Max(page, 1);
            int offSet = (pageid - 1) * pagesize;

            var now = DateTime.Now;

            DateTime start, end;

            if (month > 0 && month < 13)
            {
                start = new DateTime(now.Year, month, 1);
                end = start.AddMonths(1);
            }
            else
            {
                start = new DateTime(now.Year, 1, 1);
                end = start.AddMonths(12);
            }

            var startStr = start.ToString(CultureInfo.InvariantCulture);
            var endStr = end.ToString(CultureInfo.InvariantCulture);

            var queryStr = $@"
SELECT
	TOP ({pagesize}) *
FROM
	(
		SELECT
			ROW_NUMBER () OVER (

				ORDER BY
					rec_list.addtime DESC
			) AS RowNum,
			rec_list.*
		FROM
			(
				SELECT
					rrl_order.ordercode,
					rrl_user.username,
					rrl_order.addtime,
					rrl_order.checktime,
					rrl_order.money,
					rrl_order.status,
					1 AS recommand_class
				FROM
					rrl_order
				LEFT JOIN rrl_user ON rrl_order.buyer_id = rrl_user.id
				WHERE
					rrl_order.buyer_id IN (
						SELECT
							sperader_relation.uid
						FROM
							sperader_relation
						WHERE
							sperader_relation.sperader_uid = {uid}
					)
				AND rrl_order.status IN (2, 3)
				AND rrl_order.addtime BETWEEN '{startStr}'
				AND '{endStr}'
				UNION ALL
					SELECT
						rrl_order.ordercode,
						rrl_user.username,
						rrl_order.addtime,
						rrl_order.checktime,
						rrl_order.money,
						rrl_order.status,
						2 AS recommand_class
					FROM
						rrl_order
					LEFT JOIN rrl_user ON rrl_order.buyer_id = rrl_user.id
					WHERE
						rrl_order.buyer_id IN (
							SELECT
								sperader_relation.uid
							FROM
								sperader_relation
							WHERE
								sperader_relation.sperader_sperader_uid = {uid}
						)
					AND rrl_order.status IN (2, 3)
					AND rrl_order.addtime BETWEEN '{startStr}'
					AND '{endStr}'
					UNION ALL
						SELECT
							rrl_order.ordercode,
							rrl_user.username,
							rrl_order.addtime,
							rrl_order.checktime,
							rrl_order.money,
							rrl_order.status,
							3 AS recommand_class
						FROM
							rrl_order
						LEFT JOIN rrl_user ON rrl_order.buyer_id = rrl_user.id
						WHERE
							rrl_order.buyer_id IN (
								SELECT
									sperader_relation.uid
								FROM
									sperader_relation
								WHERE
									sperader_relation.sperader_sperader_sperader_uid = {uid}
							)
						AND rrl_order.status IN (2, 3)
                        AND rrl_order.addtime BETWEEN '{startStr}'
					    AND '{endStr}'
			) AS rec_list
	) AS tmptab
WHERE
	tmptab.RowNum > {offSet}";

            var db = new RRLDB();

            var ds = db.ExeQuery(queryStr);
            db.Close();
            return ds;
        }

        /// <summary>
        /// 获取月度推荐订单统计数据
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="month">月份</param>
        /// <returns></returns>
        public DataSet GetUserMonthRecommandOrderCount(int uid, int month)
        {
            var now = DateTime.Now;

            DateTime monthStart, monthEnd;
            var yearStart = new DateTime(now.Year, 1, 1);
            var yearEnd = yearStart.AddMonths(12);

            if (month > 0 && month < 13)
            {
                monthStart = new DateTime(now.Year, month, 1);
                monthEnd = monthStart.AddMonths(1);
            }
            else
            {
                monthStart = yearStart;
                monthEnd = yearEnd;
            }

            var monthStartStr = monthStart.ToString(CultureInfo.InvariantCulture);
            var monthEndStr = monthEnd.ToString(CultureInfo.InvariantCulture);
            var yearStartStr = yearStart.ToString(CultureInfo.InvariantCulture);
            var yearEndStr = yearEnd.ToString(CultureInfo.InvariantCulture);

            var db = new RRLDB();
            var ds = db.ExeQuery($@"SELECT
    {uid} AS id,
	(
		 SELECT
			  is_shop_keeper
		 FROM
			  rrl_user
		 WHERE
			  id = {uid}
	) AS is_shop_keeper,
	(
		SELECT
			COUNT (*)
		FROM
			rrl_order
		LEFT JOIN rrl_user ON rrl_order.buyer_id = rrl_user.id
		WHERE
			rrl_order.buyer_id IN (
				SELECT
					sperader_relation.uid
				FROM
					sperader_relation
				WHERE
					sperader_relation.sperader_uid = {uid}
				OR sperader_relation.sperader_sperader_uid = {uid}
				OR sperader_relation.sperader_sperader_sperader_uid = {uid}
			)
		AND rrl_order.status IN (2, 3)
		AND rrl_order.addtime BETWEEN '{yearStartStr}'
		AND '{yearEndStr}'
	) AS year_total_order,
	(
		SELECT
			COUNT (*)
		FROM
			rrl_order
		LEFT JOIN rrl_user ON rrl_order.buyer_id = rrl_user.id
		WHERE
			rrl_order.buyer_id IN (
				SELECT
					sperader_relation.uid
				FROM
					sperader_relation
				WHERE
					sperader_relation.sperader_uid = {uid}
				OR sperader_relation.sperader_sperader_uid = {uid}
				OR sperader_relation.sperader_sperader_sperader_uid = {uid}
			)
		AND rrl_order.status = 3
		AND rrl_order.addtime BETWEEN '{yearStartStr}'
		AND '{yearEndStr}'
	) AS year_settled_order,
	(
		SELECT
			COUNT (*)
		FROM
			rrl_order
		LEFT JOIN rrl_user ON rrl_order.buyer_id = rrl_user.id
		WHERE
			rrl_order.buyer_id IN (
				SELECT
					sperader_relation.uid
				FROM
					sperader_relation
				WHERE
					sperader_relation.sperader_uid = {uid}
				OR sperader_relation.sperader_sperader_uid = {uid}
				OR sperader_relation.sperader_sperader_sperader_uid = {uid}
			)
		AND rrl_order.status IN (2, 3)
		AND rrl_order.addtime BETWEEN '{monthStartStr}'
		AND '{monthEndStr}'
	) AS month_total_order,
	(
		SELECT
			COUNT (*)
		FROM
			rrl_order
		LEFT JOIN rrl_user ON rrl_order.buyer_id = rrl_user.id
		WHERE
			rrl_order.buyer_id IN (
				SELECT
					sperader_relation.uid
				FROM
					sperader_relation
				WHERE
					sperader_relation.sperader_uid = {uid}
				OR sperader_relation.sperader_sperader_uid = {uid}
				OR sperader_relation.sperader_sperader_sperader_uid = {uid}
			)
		AND rrl_order.status = 3
		AND rrl_order.addtime BETWEEN '{monthStartStr}'
		AND '{monthEndStr}'
	) AS month_settled_order");
            db.Close();
            return ds;
        }

        /// <summary>
        /// 获取月度推荐奖励金列表
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="month">月份</param>
        /// <param name="page">页码</param>
        /// <param name="pagesize">页面尺寸</param>
        /// <returns></returns>
        public DataSet GetUserMonthRecommandMoneyList(int uid, int month, int page, int pagesize)
        {
            int pageid = Math.Max(page, 1);
            int offSet = (pageid - 1) * pagesize;

            var now = DateTime.Now;

            DateTime start, end;

            if (month > 0 && month < 13)
            {
                start = new DateTime(now.Year, month, 1);
                end = start.AddMonths(1);
            }
            else
            {
                start = new DateTime(now.Year, 1, 1);
                end = start.AddMonths(12);
            }

            var startStr = start.ToString(CultureInfo.InvariantCulture);
            var endStr = end.ToString(CultureInfo.InvariantCulture);

//            var queryStr = $@"SELECT
//	TOP ({pagesize}) *
//FROM
//	(
//		SELECT
//			ROW_NUMBER () OVER (

//				ORDER BY
//					rec_list.addtime DESC
//			) AS RowNum,
//			*
//		FROM
//			(
//                SELECT
//	                rrl_user_money_record.addtime,
//	                rrl_user_money_record.money AS recommand_money,
//	                ISNULL(rrl_user.username,'') AS buyer_name,
//					ISNULL(rrl_order.is_settled,1) AS is_frz,
//					ISNULL(rrl_order.money,0) AS order_money,
//	                rrl_user_money_record.rec_class AS recommand_class
//                FROM
//	                rrl_user_money_record
//                LEFT JOIN rrl_order ON rrl_order.id = rrl_user_money_record.order_id
//                LEFT JOIN rrl_user ON rrl_user.id = rrl_order.buyer_id
//                WHERE
//	                rrl_user_money_record.uid = {uid}
//                AND rrl_user_money_record.type = 3
//                AND rrl_user_money_record.addtime BETWEEN '{startStr}'
//                AND '{endStr}'
//			) AS rec_list
//	) AS tmptab
//WHERE
//	tmptab.RowNum > {offSet}";

            // lcl 20180510 Modify
            var queryStr = $@"with money_record as
                              (
                                  select [uid] ,[type] ,[money] ,[order_id] ,[rec_class] ,[addtime]
	                                from rrl_user_money_record(nolock)
	                               where ([uid] = {uid})
	                                 and ([type] = 3)
	                                 and (addtime between '{startStr}' and '{endStr}')
                              )
                              select top ({pagesize}) *
                                from (
                                    select row_number() over (order by rec_list.addtime desc) as rownum, *
                              	    from (
                                          select mr.addtime
                              	              ,mr.[money] as recommand_money
                              	              ,isnull(rrl_user.username, '') as buyer_name
                              				  ,case when rrl_order.status = 3 or datediff(dd, rrl_order.addtime, getdate()) > 15 then 0 else 1 end as is_frz
                              				  ,isnull(order_detail.order_money, 0) as order_money
                              	              ,mr.rec_class as recommand_class
                                            from money_record mr
                                            left join rrl_order(nolock)
                              			      on rrl_order.id = mr.order_id
                              			    left join (
                              			      select g.order_id
                              				        ,sum(isnull(goods_price, 0) * isnull(goods_count, 0)) as order_money
                              				    from rrl_order_info_goods(nolock) g
                                                 inner join money_record on g.order_id = money_record.order_id
                              				   group by g.order_id
                              			    ) as order_detail
                              			      on rrl_order.id = order_detail.order_id
                                            left join rrl_user(nolock)
                              			      on rrl_user.id = rrl_order.buyer_id
                                           where (rrl_order.status in (2, 3))
                              		) as rec_list
                                ) as tmptab
                               where tmptab.rownum > {offSet}";

            var db = new RRLDB();

            var ds = db.ExeQuery(queryStr);
            db.Close();
            return ds;
        }

        /// <summary>
        /// 获取月度推荐奖励金统计数据
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="month">月份</param>
        /// <returns></returns>
        public DataSet GetUserMonthRecommandMoneyCount(int uid, int month)
        {
            var now = DateTime.Now;

            DateTime monthStart, monthEnd;
            var yearStart = new DateTime(now.Year, 1, 1);
            var yearEnd = yearStart.AddMonths(12);

            if (month > 0 && month < 13)
            {
                monthStart = new DateTime(now.Year, month, 1);
                monthEnd = monthStart.AddMonths(1);
            }
            else
            {
                monthStart = yearStart;
                monthEnd = yearEnd;
            }

            var monthStartStr = monthStart.ToString(CultureInfo.InvariantCulture);
            var monthEndStr = monthEnd.ToString(CultureInfo.InvariantCulture);
            var yearStartStr = yearStart.ToString(CultureInfo.InvariantCulture);
            var yearEndStr = yearEnd.ToString(CultureInfo.InvariantCulture);

            var db = new RRLDB();
            var ds = db.ExeQuery($@"
SELECT
	{uid} AS id,
	(
		SELECT
			is_shop_keeper
		FROM
			rrl_user
		WHERE
			id = {uid}
	) AS is_shop_keeper,
	(
		SELECT
			ISNULL(SUM(money), 0)
		FROM
			rrl_user_money_record
		WHERE
			uid = {uid}
		AND type = 3
		AND addtime BETWEEN '{monthStartStr}'
		AND '{monthEndStr}'
	) AS month_total_money,
	(
		SELECT
			ISNULL(SUM(rrl_user_money_record.money), 0)
		FROM
			rrl_user_money_record
		LEFT JOIN rrl_order ON rrl_order.id = rrl_user_money_record.order_id
		WHERE
			rrl_user_money_record.uid = {uid}
		AND rrl_user_money_record.type = 3
		AND rrl_order.status = 2
		AND rrl_user_money_record.addtime BETWEEN '{monthStartStr}'
		AND '{monthEndStr}'
	) AS month_frz_money,
	(
		SELECT
			ISNULL(SUM(money), 0)
		FROM
			rrl_user_money_record
		WHERE
			uid = {uid}
		AND type = 3
		AND addtime BETWEEN '{yearStartStr}'
		AND '{yearEndStr}'
	) AS year_total_money,
	(
		SELECT
			ISNULL(SUM(rrl_user_money_record.money), 0)
		FROM
			rrl_user_money_record
		LEFT JOIN rrl_order ON rrl_order.id = rrl_user_money_record.order_id
		WHERE
			rrl_user_money_record.uid = {uid}
		AND rrl_user_money_record.type = 3
		AND rrl_order.status = 2
		AND rrl_user_money_record.addtime BETWEEN '{yearStartStr}'
		AND '{yearEndStr}'
	) AS year_frz_money");
            db.Close();
            return ds;
        }

        /// <summary>
        /// 获取用户上线
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="page">页码</param>
        /// <param name="pageSize">页面大小</param>
        /// <returns>数据集</returns>
        public DataSet GetUserSpreadorList(int uid, int page, int pageSize)
        {
            var db = new RRLDB();
            var ds = db.ExeQuery($@"SELECT ROW_NUMBER () OVER (ORDER BY addtime DESC) AS RowNum ,* FROM ( SELECT id, LEFT (username, 3) + '****' + RIGHT (username, 4) AS username, addtime FROM rrl_user WHERE id = ( SELECT spreader_uid FROM rrl_user WHERE id = {uid} ) ) AS list");
            db.Close();
            return ds;
        }

        /// <summary>
        /// 重新申请短效token
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="shortTimeToken">长效token</param>
        /// <returns>申请结果</returns>
        public int ApplyShortTimeToken(int uid, out string shortTimeToken)
        {
            shortTimeToken = "";
            var user = new UserAuth();
            var res = user.Load(uid);
            if (res != MessageCode.SUCCESS)
                return res;
            shortTimeToken = user.EncryptShortTimeToken();
            res = user.Save();
            return res;
        }

        /// <summary>
        /// 使用短信验证码注册
        /// </summary>
        /// <param name="smscode"></param>
        /// <param name="username"></param>
        /// <param name="spreader"></param>
        /// <param name="devicecode"></param>
        /// <param name="spreaderCode"></param>
        /// <returns></returns>
        public int RegistWithSMSCode(string smscode, string username, string spreader, string devicecode, string spreaderCode,string fromcode=null)
        {
            if (string.IsNullOrWhiteSpace(spreader))
            {
                spreader = "99999999999";
            }
            //if (string.IsNullOrWhiteSpace(spreader))
            //{
            //    return MessageCode.ERROR_NO_SPERATER;
            //}
            if (!UserAuth.CheckSMS(username, smscode))
            {
                return MessageCode.ERROR_BAD_SMS;
            }
            if (UserAuth.Exists(username))
            {
                return MessageCode.ERROR_EXIST_USER;
            }

            //if (string.IsNullOrWhiteSpace(devicecode))
            //{
            //    devicecode = "";
            //}
            var db = new RRLDB();

            var ds = db.ExeQuery($@"select id from rrl_user where username =@spreader and isnull(is_locked_login,0)='0' ", new SqlParameter("@spreader", SqlDbType.NChar, 11) { Value = spreader });
            if (ds == null || ds.Tables[0].Rows.Count == 0)
            {
                db.Close();
                return MessageCode.ERROR_NO_SPERATER;
            }

            int effect=db.ExecSP(@"sp_USER_REGIST",
                new SqlParameter("@username", SqlDbType.NChar, 11) { Value = username },
                new SqlParameter("@spreader", SqlDbType.NChar, 11) { Value = spreader });
            
            ds = db.ExeQuery($@"select id from rrl_user where username = '{username}'");

            if (ds == null)
            {
                db.Close();
                return MessageCode.ERROR_EXECUTE_SQL;
            }
            if (!string.IsNullOrWhiteSpace(fromcode))
            {
                db.ExeCMD(@"update rrl_user set from_code=@from_code where   username = '{username}'", new SqlParameter("@from_code", fromcode));
            }
            //if (!string.IsNullOrWhiteSpace(devicecode))
            //{
            //    ds = db.ExeQuery($@"SELECT COUNT(*) FROM rrl_user WHERE [device_code] = {devicecode}");

            //    if (ds == null || Convert.ToInt32(ds.Tables[0].Rows[0][0]) >= 3)
            //    {
            //        db.Close();
            //        return MessageCode.ERROR_TOO_MANY_DEVICE;
            //    }
            //    db.ExeCMD($@"UPDATE rrl_user SET [device_code] = {devicecode} WHERE username = {username}");
            //}

            db.Close();


            string strShareMoneyPay = new ConfigManager().GetConfigValue("GameCenter_Share_Redpackage");

            try
            {
                if (!string.IsNullOrWhiteSpace(spreader))
                {


                    var user = new UserAuth(spreader);
                    if (!string.IsNullOrWhiteSpace(user.wx_mp_open_id))
                    {
                        // 推荐人收到的微信内容
                        var messageobj = new
                        {
                            touser = user.wx_mp_open_id,
                            template_id = "qfOtJT8K_ppRoH63GkmbfC3fcrr_pEZJnGZ2Cein8kI",
                            url = "https://e-shop.rrlsz.com.cn/#/my",
                            data = new
                            {
                                first = new
                                {
                                    value = "推荐注册奖励小红包",
                                    color = "#0000CC"
                                },
                                keyword1 = new
                                {
                                    value = $@"新用户新设备登录成功，即得{strShareMoneyPay}小红包",
                                    color = "#0000CC"
                                },
                                keyword2 = new
                                {
                                    value = DateTime.Now.ToString(CultureInfo.InvariantCulture),
                                    color = "#0000CC"
                                }
                            }
                        };

                        var wxMpMessage = new WxMpMessage();
                        wxMpMessage.SendTemplateMessage(messageobj);
                    }

                }
            }
            catch
            {
                // ignored
            }


            return MessageCode.SUCCESS;
        }

        /// <summary>
        /// 通过短信验证码登录
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="smsCode">短信码</param>
        /// <param name="deviceCode"></param>
        /// <param name="longTimeToken">长效token</param>
        /// <param name="shortTimeToken">短效token</param>
        /// <returns></returns>
        public int LoginWithSMSCode(string userName, string smsCode, string deviceCode, out string longTimeToken, out string shortTimeToken)
        {
            longTimeToken = "";
            shortTimeToken = "";
            if (userName.Trim() == "" || smsCode.Trim() == "")
            {
                return MessageCode.ERROR_NULL_UID;
            }
            var user = new UserAuth();
            var res = user.Load(userName);
            switch (res)
            {
                case MessageCode.ERROR_NO_UID:
                    return MessageCode.ERROR_NO_UID;
                case MessageCode.ERROR_EXECUTE_SQL:
                    return MessageCode.ERROR_EXECUTE_SQL;
            }
            if(res== MessageCode.SUCCESS)
            {
                if("1".Equals(user.is_locked_login))
                {
                    return MessageCode.ERROR_USER_LOCKED;
                }
            }

            if (!user.CheckDeviceCode(deviceCode))
            {
                return MessageCode.ERROR_TOO_MANY_DEVICE;
            }
            if (!UserAuth.CheckSMSForExistsUser(user.username, smsCode))
            {
                return MessageCode.ERROR_BAD_SMS;
            }
            longTimeToken = user.EncryptLongTimeToken();
            shortTimeToken = user.EncryptShortTimeToken();
            res = user.Save();
            return res < 0 ? MessageCode.ERROR_EXECUTE_SQL : MessageCode.SUCCESS;
        }

        /// <summary>
        /// 接收游戏分享赠送红包
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="hasreveive"></param>
        /// <param name="spreader"></param>
        /// <param name="spreaderMoney"></param>
        public void ReceiveHongMoney(int uid, bool hasreveive, string spreader, out double spreaderMoney,string remark= "分享游戏赠送红包")
        {
            // lcl 2018-07-12 启用新的自动发红包机制，取消新注册用户的红包规则
            //spreaderMoney = 0d;
            //return;

            ConfigManager configMgr = new ConfigManager();
            spreaderMoney = 0;
            decimal mMoneyPay = 0m;

            if (!hasreveive)
            {
                var db = new RRLDB();
                // lcl 2018-07-06 Modify
                // 获取配置中赠送的免费红包数额
                double.TryParse(configMgr.GetConfigValue("spreader_h_money"), out spreaderMoney);
                // 获取配置中赠送的V红包数额
                decimal.TryParse(configMgr.GetConfigValue("spreader_h_money_pay"), out mMoneyPay);

                int effect = db.ExeCMD(
                    $@"Update rrl_user 
                          Set h_money_free = h_money_free + {spreaderMoney}
                             ,h_money_pay = h_money_pay + {mMoneyPay}
                             ,has_received_spreader_h_money = 1
                             ,has_received_daily_free_h_money = 1
                        Where id = {uid} and isnull(has_received_spreader_h_money,0)=0");

                if (effect > 0)
                {
                    if (spreaderMoney > 0d)
                    {
                        db.ExeCMD(
                            $@"INSERT INTO  [rrl_user_money_record] ([uid], [money],[type], [remark]) VALUES ({
                                    uid
                                }, {spreaderMoney},{101} ,N'{remark}')");
                    }
                    if (mMoneyPay > 0m)
                    {
                        db.ExeCMD($@"insert into rrl_user_money_record
                                           ([uid], [money] ,[type], [remark])
                                     values 
                                           ({uid} ,{mMoneyPay} ,105 ,N'新用户注册送V红包')");
                    }

                    if (!string.IsNullOrWhiteSpace(spreader))
                        db.ExeCMD($@"UPDATE rrl_user SET [spreader_count] = [spreader_count] + 1 WHERE username = '{spreader}'");
                }
                db.Close();
            }
        }

        /// <summary>
        /// 用户登出
        /// </summary>
        /// <param name="token">任意有效token</param>
        public void Logout(string token)
        {
            var tokenObj = TokenObject.InitTokenObjFromString(token);
            if (DateTime.Compare(tokenObj.Expirtime, DateTime.Now) <= 0)
                return;
            var user = new UserAuth();
            var res = user.CheckToken(tokenObj);
            if (res != MessageCode.SUCCESS) return;
            user.ExpirLongTimeToken();
            user.ExpirShortTimeToken();
        }

        /// <summary>
        /// 绑定Openid
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="smscode">短信验证码</param>
        /// <param name="spreader">推荐人</param>
        /// <param name="openid">openid</param>
        /// <param name="type">openid类型</param>
        /// <param name="longtimetoken">长token</param>
        /// <param name="shorttimetoken">短token</param>
        /// <returns></returns>
        public int BindOpenId(string username, string smscode, string spreader, string openid, string type, out string longtimetoken, out string shorttimetoken)
        {
            longtimetoken = "";
            shorttimetoken = "";
            if (username.Trim() == "" || smscode.Trim() == "")
            {
                return MessageCode.ERROR_NULL_UID;
            }
            if (UserAuth.Exists(username))
            {
                var user = new UserAuth();
                var res = user.Load(username);
                if (res == MessageCode.ERROR_NO_UID)
                {
                    return MessageCode.ERROR_NO_UID;
                }
                if (res == MessageCode.ERROR_EXECUTE_SQL)
                {
                    return MessageCode.ERROR_EXECUTE_SQL;
                }
                if (!UserAuth.CheckSMSForExistsUser(user.username, smscode))
                {
                    return MessageCode.ERROR_BAD_SMS;
                }
                longtimetoken = user.EncryptLongTimeToken();
                shorttimetoken = user.EncryptShortTimeToken();
                res = user.Save();
                if (res < 0)
                {
                    return MessageCode.ERROR_EXECUTE_SQL;
                }
                res = user.SaveOpenid(openid, type);
                return res < 0 ? MessageCode.ERROR_EXECUTE_SQL : MessageCode.SUCCESS;
            }
            else
            {
                if (string.IsNullOrWhiteSpace(spreader))
                {
                    var db1 = new RRLDB();
                    var ds1 = db1.ExeQuery($@"select id from rrl_user where username = '{spreader}'");
                    db1.Close();
                    if (ds1 == null || ds1.Tables[0].Rows.Count == 0)
                    {
                        return MessageCode.ERROR_NO_SPERATER;
                    }
                }
                //if (string.IsNullOrWhiteSpace(spreader))
                //{
                //    var db1 = new RRLDB();
                //    var ds1 = db1.ExeQuery($@"select spreader_uid from rrl_user where username = '{username}'");
                //    db1.Close();
                //    if (ds1 == null || ds1.Tables[0].Rows.Count == 0 || ds1.Tables[0].Rows[0][0].ToString().Equals("0"))
                //    {
                //        return MessageCode.ERROR_NO_SPERATER;
                //    }
                //}
                if (!UserAuth.CheckSMS(username, smscode))
                {
                    return MessageCode.ERROR_BAD_SMS;
                }
                var db = new RRLDB();

                db.ExecSP(@"sp_USER_REGIST",
                    new SqlParameter("@username", SqlDbType.NChar, 11) { Value = username },
                    new SqlParameter("@spreader", SqlDbType.NChar, 11) { Value = spreader });
                var ds = db.ExeQuery($@"select id from rrl_user where username = '{username}'");
                db.Close();
                if (ds == null || ds.Tables[0].Rows.Count == 0)
                {
                    return MessageCode.ERROR_EXECUTE_SQL;
                }

                var user = new UserAuth();
                var res = user.Load(username);
                if (res == MessageCode.ERROR_NO_UID)
                {
                    return MessageCode.ERROR_NO_UID;
                }
                if (res == MessageCode.ERROR_EXECUTE_SQL)
                {
                    return MessageCode.ERROR_EXECUTE_SQL;
                }
                longtimetoken = user.EncryptLongTimeToken();
                shorttimetoken = user.EncryptShortTimeToken();
                res = user.Save();
                if (res < 0)
                {
                    return MessageCode.ERROR_EXECUTE_SQL;
                }
                res = user.SaveOpenid(openid, type);
                return res < 0 ? MessageCode.ERROR_EXECUTE_SQL : MessageCode.SUCCESS;
            }
        }

       

        /// <summary>
        /// 通过openid获取token
        /// </summary>
        /// <param name="openid">openid</param>
        /// <param name="type">类型</param>
        /// <param name="shorttimetoken">短</param>
        /// <param name="longtimetoken">长</param>
        /// <returns></returns>
        public int ApplyTokenViaOpenid(string openid, string type, out string shorttimetoken, out string longtimetoken)
        {
            shorttimetoken = "";
            longtimetoken = "";
            var user = new UserAuth();
            var res = user.Load(openid, type);
            if (res == MessageCode.ERROR_NO_UID)
            {
                return MessageCode.ERROR_NO_UID;
            }
            if (res == MessageCode.ERROR_EXECUTE_SQL)
            {
                return MessageCode.ERROR_EXECUTE_SQL;
            }
            longtimetoken = user.EncryptLongTimeToken();
            shorttimetoken = user.EncryptShortTimeToken();
            res = user.Save();
            if (res < 0)
            {
                return MessageCode.ERROR_EXECUTE_SQL;
            }
            if (res < 0)
            {
                return MessageCode.ERROR_EXECUTE_SQL;
            }
            return MessageCode.SUCCESS;
        }

        #region 获取我的券包
        /// <summary>
        /// 
        /// </summary>
        /// <param name="user_id"></param>
        /// <param name="page_index"></param>
        /// <param name="page_size"></param>
        /// <param name="errorCode"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public List<TradeManager.MyCoupons> GetMyCoupons(int user_id, int page_index, int page_size, out int errorCode, out string errMsg)
        {
            errorCode = 0;
            errMsg = "ok";
            errorCode = 1;
            errMsg = "";
            int OFFSET = 0;

            OFFSET = (page_index - 1) * page_size;

            List<TradeManager.MyCoupons> list = new List<TradeManager.MyCoupons>();
            string sql = $@"
select *, case when ('1900-01-01'=c.starttime and '1900-01-01'=c.endtime)  
or  (c.starttime<GETDATE() and c.endtime >GETDATE())  then '0' else '1' end  as is_outdate from rrl_coupons c where  c.uid={user_id} and is_used=0 and is_paid=1  
order by c. endtime asc
OFFSET {OFFSET} ROWS
FETCH NEXT {page_size} ROWS ONLY
";
            SqlDataBase db = new SqlDataBase();
            list = db.Select<TradeManager.MyCoupons>(sql, null);


            return list;
        
        }
        #endregion

        #region 获取随机红包数
        /// <summary>
        /// 获取随机红包数
        /// </summary>
        /// <returns></returns>
        public Tuple<int,string> GetRndRedPackage()
        {
            SqlDataBase db = new SqlDataBase();
            string sql = @"select * from rrl_goods_coupons_config order by endTime asc ";
            var list = db.Select<Models.Goods_Coupons_Config>(sql, null);
            if (list == null || list.Count == 0)
            {
                return new Tuple<int, string>(0,"2000-01-01 00:00:00");
            }
            else
            {
                var now=DateTime.Now;
                var nowTime = now.TimeOfDay;
                var nowDate = now.Date;
                foreach (var item in list)
                {
                    var startTime= item.startTime.TimeOfDay;
                    var endTime = item.endTime.TimeOfDay;
                    if(nowTime>=startTime&& nowTime< endTime)
                    {
                        var maxMoney = item.maxMoney / 1000;
                        var minMoney = item.minMoney / 1000;
                        var rnd = new Random().Next(minMoney, maxMoney + 1) * 1000;
                        return new Tuple<int, string>(rnd, nowDate.ToString("yyyy-MM-dd")+" "+ endTime.ToString());
                    }
                }
                //都找不到.找最接近的
                List<Tuple<int, TimeSpan>> closeTs = new List<Tuple<int, TimeSpan>>();
                for (int i = 0; i < list.Count; i++)
                {
                    var item = list[i];
                    var startTime = item.startTime.TimeOfDay;
                    var s = nowTime - startTime;
                    closeTs.Add(new Tuple<int, TimeSpan>(i, s));
                }
                var closeItem=closeTs.OrderBy(v => v.Item2).FirstOrDefault();
                return new Tuple<int, string>(0, nowDate.ToString("yyyy-MM-dd") + " " + closeItem.Item2.ToString());
            }
            
        }
        #endregion
    }
}