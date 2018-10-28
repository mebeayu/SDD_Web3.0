using RRL.Core.Utility;
using RRL.WEB.Ulity;
using System;
using System.Collections.Generic;
using System.Data;

namespace RRL.WEB.Models.Response
{
    #region 快递结果对象

    /// <summary>
    /// 快递查询结果
    /// </summary>
    public class TrackResult
    {
        /// <summary>
        /// 消息体
        /// </summary>
        public string message { get; set; }

        /// <summary>
        /// 签收状态
        /// 0在途中、1已揽收、2疑难、3已签收、4退签、5同城派送中、6退回、7转单
        /// </summary>
        public int state { get; set; }

        /// <summary>
        /// 通讯状态
        /// </summary>
        public int status { get; set; }

        /// <summary>
        /// 明细状态标记
        /// </summary>
        public string condition { get; set; }

        /// <summary>
        /// 是否签收，已弃用
        /// 这个快递100怕是石乐智
        /// </summary>
        public string ischeck { get; set; }

        /// <summary>
        /// 快递公司编码
        /// </summary>
        public string com { get; set; }

        /// <summary>
        /// 快递单号
        /// </summary>
        public string nu { get; set; }

        /// <summary>
        /// 查询结果
        /// </summary>
        public TrackNode[] data { get; set; }

        /// <summary>
        /// 获取第一条结果
        /// </summary>
        public TrackNode GetRecent()
        {
            return data[0];
        }
    }

    /// <summary>
    /// 快递节点
    /// </summary>
    public class TrackNode
    {
        /// <summary>
        /// 内容
        /// </summary>
        public string context { get; set; }

        /// <summary>
        /// 时间
        /// </summary>
        public DateTime time { get; set; }

        /// <summary>
        /// 格式化时间
        /// </summary>
        public DateTime ftime { get; set; }
    }

    #endregion 快递结果对象

    #region 普通结果对象

    /// <summary>
    /// 结果对象
    /// </summary>
    public class DataResult
    {
        /// <summary>
        /// 消息码
        /// </summary>
        public int status { get; set; }

        /// <summary>
        /// 消息信息
        /// </summary>
        public string message { get; set; }

        /// <summary>
        /// 结果数据集
        /// </summary>
        public object data { get; set; }

        /// <summary>
        /// 附加数据
        /// </summary>
        public object additional_data { get; set; }

        /// <summary>
        /// 其他附加数据
        /// </summary>
        public object add_data { get; set; }

        /// <summary>
        /// 使用DataSet初始化结果对象
        /// </summary>
        /// <param name="data">数据集</param>
        /// <returns>结果对象</returns>
        public static DataResult InitFromDataSet(DataSet data)
        {
            DataResult Resault = new DataResult();
            if (data == null)
            {
                Resault.status = MessageCode.ERROR_EXECUTE_SQL;
                Resault.message = MessageCode.TranslateMessageCode(MessageCode.ERROR_EXECUTE_SQL);
            }
            else if (data.Tables[0].Rows.Count == 0)
            {
                Resault.status = MessageCode.ERROR_NO_DATA;
                Resault.message = MessageCode.TranslateMessageCode(MessageCode.ERROR_NO_DATA);
            }
            else
            {
                Resault.status = MessageCode.SUCCESS;
                Resault.message = MessageCode.TranslateMessageCode(MessageCode.SUCCESS);
            }
            Resault.data = (data==null?null:data.Tables[0]);
            return Resault;
        }

        /// <summary>
        /// 使用消息码初始化结果对象
        /// </summary>
        /// <param name="code">消息码</param>
        /// <returns>结果对象</returns>
        public static DataResult InitFromMessageCode(int code)
        {
            DataResult Resault = new DataResult();
            Resault.status = code;
            Resault.message = MessageCode.TranslateMessageCode(code);
            return Resault;
        }

        /// <summary>
        /// 转换单一列数据集为列表
        /// </summary>
        /// <param name="data">数据集</param>
        /// <returns>列表</returns>
        public static List<object> SingleColumnDataSetToList(DataSet data)
        {
            List<object> Resault = new List<object>();
            if (data != null)
            {
                for (int i = 0, count = data.Tables[0].Rows.Count; i < count; i++)
                {
                    Resault.Add(data.Tables[0].Rows[i][0]);
                }
            }
            return Resault;
        }
    }

    #endregion 普通结果对象

    #region 版本信息对象

    /// <summary>
    /// APP版本信息对象
    /// </summary>
    public class AppVersionObject
    {
        /// <summary>
        /// 下载地址
        /// </summary>
        public string url { get; set; }

        /// <summary>
        /// 版本号
        /// </summary>
        public int version { get; set; }

        /// <summary>
        /// 更新信息
        /// </summary>
        public string info { get; set; }
    }

    public class AppleVersionObject
    {
        /// <summary>
        /// 下载地址
        /// </summary>
        public string url { get; set; }

        /// <summary>
        /// 版本号
        /// </summary>
        public string version { get; set; }

        /// <summary>
        /// 更新信息
        /// </summary>
        public string info { get; set; }
    }

    #endregion 版本信息对象

    #region 结果视图模板

    /// <summary>
    /// 结果视图
    /// </summary>
    /// <typeparam name="T">类型占位符</typeparam>
    public class ResultView<T>
    {
        /// <summary>
        /// 消息码
        /// </summary>
        public int status { get; set; }

        /// <summary>
        /// 消息信息
        /// </summary>
        public string message { get; set; }

        /// <summary>
        /// 结果数据集
        /// </summary>
        public T data { get; set; }

        /// <summary>
        /// 使用DataSet初始化结果对象
        /// </summary>
        /// <param name="data">数据集</param>
        /// <returns>结果对象</returns>
        public static ResultView<List<T>> InitFromDataSet(DataSet data)
        {
            ResultView<List<T>> Resault = new ResultView<List<T>>();
            if (data == null)
            {
                Resault.status = MessageCode.ERROR_EXECUTE_SQL;
                Resault.message = MessageCode.TranslateMessageCode(MessageCode.ERROR_EXECUTE_SQL);
            }
            else if (data.Tables[0].Rows.Count == 0)
            {
                Resault.status = MessageCode.ERROR_NO_DATA;
                Resault.message = MessageCode.TranslateMessageCode(MessageCode.ERROR_NO_DATA);
            }
            else
            {
                Resault.status = MessageCode.SUCCESS;
                Resault.message = MessageCode.TranslateMessageCode(MessageCode.SUCCESS);
            }
            Resault.data = (List<T>)DataSetHelper.ConvertDataTableToList<T>(data.Tables[0]);
            return Resault;
        }

        /// <summary>
        /// 使用列表初始化结果对象
        /// </summary>
        /// <param name="List">列表</param>
        /// <returns>结果对象</returns>
        public static ResultView<List<T>> InitFromList(List<T> List)
        {
            ResultView<List<T>> Resault = new ResultView<List<T>>();

            if (List.Count == 0)
            {
                Resault.status = MessageCode.ERROR_NO_DATA;
                Resault.message = MessageCode.TranslateMessageCode(MessageCode.ERROR_NO_DATA);
            }
            else
            {
                Resault.status = MessageCode.SUCCESS;
                Resault.message = MessageCode.TranslateMessageCode(MessageCode.SUCCESS);
            }
            Resault.data = List;
            return Resault;
        }

        /// <summary>
        /// 使用消息码初始化结果对象
        /// </summary>
        /// <param name="code">消息码</param>
        /// <returns>结果对象</returns>
        public static ResultView<T> InitFromMessageCode(int code)
        {
            ResultView<T> Resault = new ResultView<T>();
            Resault.status = code;
            Resault.message = MessageCode.TranslateMessageCode(code);
            return Resault;
        }
    }

    #endregion 结果视图模板
}