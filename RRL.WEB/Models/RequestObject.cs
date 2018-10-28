using System;

namespace RRL.WEB.Models.Request
{
    /// <summary>
    /// 红包登录请求
    /// </summary>
    public class HongRequest
    {
        /// <summary>
        /// 手机号码
        /// </summary>
        public string username { get; set; }
        /// <summary>
        /// 验证码
        /// </summary>
        public string smscode { get; set; }
        /// <summary>
        /// 设备码
        /// </summary>
        public string devicecode { get; set; }
        /// <summary>
        /// 推荐人token
        /// </summary>
        public string token { get; set; }

        /// <summary>
        /// 图片验证码
        /// </summary>
        public string picture_code { get; set; }

        /// <summary>
        /// 类型1=分享购物有优惠,2=红包裂变,4=,8=
        /// </summary>
        public string type { get; set; }
        /// <summary>
        /// d订单id
        /// </summary>
        public string orderid { get; set; }
        /// <summary>
        /// 红包裂变任务ID
        /// </summary>
        public string tid { get; set; }
    }

    /// <summary>
    /// 真实信息填写请求
    /// </summary>
    public class IdentityRequest
    {
        /// <summary>
        /// 真实姓名
        /// </summary>
        public string realname { get; set; }
        /// <summary>
        /// 身份证号
        /// </summary>
        public string identity { get; set; }
        /// <summary>
        /// token
        /// </summary>
        public string token { get; set; }
    }


    #region 设置用户头像请求

    /// <summary>
    /// 设置用户头像请求对象
    /// </summary>
    public class HeadPicRequest
    {
        /// <summary>
        /// 短效token
        /// </summary>
        public string token { get; set; }

        /// <summary>
        /// 头像base64编码字符串
        /// </summary>
        public string base64 { get; set; }
    }

    #endregion 设置用户头像请求

    #region 商品相关请求

    /// <summary>
    /// 商品基础请求对象
    /// </summary>
    public class BaseGoodsRequest
    {
        /// <summary>
        /// 短效token
        /// </summary>
        public string token { get; set; }

        /// <summary>
        /// 商品标识
        /// </summary>
        public int goods_id { get; set; }
    }

    #endregion 商品相关请求

    #region 商店相关请求
    /// <summary>
    /// 商店基础请求对象
    /// </summary>
    public class BaseShopRequest
    {
        /// <summary>
        /// 短效token
        /// </summary>
        public string token { get; set; }

        /// <summary>
        /// 商店标识
        /// </summary>
        public int shop_id { get; set; }
    }
    #endregion 商店相关请求

    #region 基础分页请求
    /// <summary>
    /// 基础分页请求
    /// </summary>
    public class BasePaginateRequest
    {
        /// <summary>
        /// 页码
        /// </summary>
        public int Page { get; set; }

        /// <summary>
        /// 页面大小
        /// </summary>
        public int PageSize { get; set; }
    }
    #endregion 基础分页请求


    #region 带tonen的分页请求
    /// <summary>
    /// 带tonen的分页请求
    /// </summary>
    public class PaginateTokenRequest : BasePaginateRequest
    {
        /// <summary>
        /// 短效token
        /// </summary>
        public string token { get; set; }
    }
    #endregion 带tonen的分页请求


    #region 基础令牌请求
    /// <summary>
    /// 基础令牌请求
    /// </summary>
    public class BaseTokenRequest
    {
        /// <summary>
        /// 短效token
        /// </summary>
        public string token { get; set; }
    }
    #endregion 基础令牌请求

    #region 带月份数据的令牌请求

    /// <summary>
    /// 带月份数据的令牌请求
    /// </summary>
    public class MonthPaginateRequest: PaginateTokenRequest
    {
        /// <summary>
        /// 请求月份
        /// </summary>
        public int month { get; set; }
    }

    #endregion

    /// <summary>
    /// 用于推荐统计的请求
    /// </summary>
    public class RecommandStatisticsRequest : BaseTokenRequest
    {
        /// <summary>
        /// 表示年月的日期字符创
        /// </summary>
        public string month { get; set; }
    }

    // STORY #18 lcl 20180516 Insert
    /// <summary>
    /// <para>包含基础令牌的分页请求</para>
    /// <para>当需要获取与指定用户相关的分页数据时使用</para>
    /// </summary>
    public class BasePaginationRequest : BaseTokenRequest
    {
        /// <summary>
        /// 页索引（即，显示第几页）
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// 页大小（即，一页显示多少行）
        /// </summary>
        public int PageSize { get; set; }
    }

    #region 兜兜运动

    // STORY #18 lcl 20180515 Insert
    /// <summary>
    /// 竞猜出资操作的请求类
    /// </summary>
    public class GuessBettingRequest : BaseTokenRequest
    {
        /// <summary>
        /// 出资金额
        /// </summary>
        public decimal money { get; set; }

        /// <summary>
        /// 预测结果("1":单数; "2":双数)
        /// </summary>
        public string forecast { get; set; }
    }

    // STORY #18 lcl 20180515 Insert
    /// <summary>
    /// 运动步数据维护的请求类
    /// </summary>
    public class SportStepRequest : BaseTokenRequest
    {
        public int step { get; set; }
    }

    // STORY #18 lcl 20180515 Insert
    /// <summary>
    /// 用户点赞或回赞操作的请求类
    /// </summary>
    public class UserToLikeRequest : BaseTokenRequest
    {
        public int to_uid { get; set; }

        public int type { get; set; }
    }

    // STORY #18 lcl 20180516 Insert
    /// <summary>
    /// 点赞和回赞数据查询的请求类
    /// </summary>
    public class LikeQueryRequest : BasePaginationRequest
    {
        public string date { get; set; }
    }

    // STORY #18 lcl 20180516 Insert
    /// <summary>
    /// 用户运动数据查询的请求类
    /// </summary>
    public class SportDataQueryRequest : BaseTokenRequest
    {
        public string date { get; set; }
    }

    #endregion

    // lcl 20180526 Insert
    /// <summary>
    /// 用来记录用户在页面访问行为的请求类
    /// </summary>
    public class VisitDataRequest : BaseTokenRequest
    {
        /// <summary>
        /// 访问页面时间
        /// </summary>
        public long visitTime { get; set; }

        /// <summary>
        /// 离开页面时间
        /// </summary>
        public long leaveTime { get; set; }

        /// <summary>
        /// 访问时长（秒）
        /// </summary>
        public int visitInterval { get; set; }

        /// <summary>
        /// 页面类型（index,list,banner,detail）
        /// </summary>
        public string pageType { get; set; }

        /// <summary>
        /// 商品ID
        /// </summary>
        public int goodsId { get; set; }

        /// <summary>
        /// 访问的页面地址
        /// </summary>
        public string visitPath { get; set; }

        /// <summary>
        /// 页面深度（即，当前页面在站点中的层级）
        /// </summary>
        public int deep { get; set; }
    }

    /// <summary>
    /// 用于公众号操作的请求基类
    /// </summary>
    public class WeChatBaseRequest : BaseTokenRequest
    {
        public string openId { get; set; }
    }

    public class GamePlayTimesRequest : BaseTokenRequest
    {
        public string moneyType { get; set; }
    }

    public class RedPackageToBeansRequest : BaseTokenRequest
    {
        public string moneyType { get; set; }
    }

    // lcl 20180716 Insert
    public class TaskRedpackageRequest : BaseTokenRequest
    {
        public string taskType { get; set; }
    }

    // lcl 20180717 Insert
    public class ProxyStatisticsByMonthlyRequest : BaseTokenRequest
    {
        public int year { get; set; }

        public int month { get; set; }
    }

    // lcl 20180717 Insert
    public class ProxyStatisticsByDayRequest : BaseTokenRequest
    {
        public string date { get; set; }
    }

    public class SecondHandAutoOrderRequest : BaseTokenRequest
    {
        public string payAccount { get; set; }

        public string name { get; set; }
    }

    #region 红包裂变

    public class RedpackageFissionRequest : BaseTokenRequest
    {
        public string taskId { get; set; }
    }

    #endregion

    public class GameAutoRedpackageRequest : BaseTokenRequest
    {
        /// <summary>
        /// 来源
        /// </summary>
        public string source { get; set; }
    }

    #region 记录设备信息

    public class AndroidInfoRequest : BaseTokenRequest
    {
        public string imei { get; set; }
        public string serial { get; set; }
        public string mac_wifi { get; set; }
        public string providersName { get; set; }
        public string networkKoperatorName { get; set; }
        public string phoneNumber { get; set; }
    }

    public class IOSInfoRequest : BaseTokenRequest
    {
        public string deviceName { get; set; }
        public string deviceIPAdress { get; set; }
        public string uuid { get; set; }
    }

    #endregion
}