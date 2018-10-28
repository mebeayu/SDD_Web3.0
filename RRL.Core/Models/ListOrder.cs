using RRL.Core.Utility;
using RRL.DB;
using System;
using System.Data;
using System.Data.SqlClient;

namespace RRL.Core.Models
{
    public class ListOrder
    {
        #region 字段定义

        private int _goods_count;
        private int _id;
        private double _money;
        private string _order_code;
        private double _return_money;
        private string _send_time;
        private int _shop_id;
        private string _shop_name;
        private int _status;
        private string _track_com;
        private string _track_num;
        private int _uid;
        private string _receive_name;
        private string _receive_address;
        private string _receive_mobile;
        private double _discount;
        private double _postage;
        private string _check_code;
        private string _addtime;
        
        private DataTable _goods_list;

        public int goods_count
        {
            get
            {
                return _goods_count;
            }

            private set
            {
                _goods_count = value;
            }
        }

        public int id
        {
            get
            {
                return _id;
            }

            private set
            {
                _id = value;
            }
        }

        public double money
        {
            get
            {
                return _money;
            }

            private set
            {
                _money = value;
            }
        }

        public string order_code
        {
            get
            {
                return _order_code;
            }

            private set
            {
                _order_code = value;
            }
        }

        public double return_money
        {
            get
            {
                return _return_money;
            }

            private set
            {
                _return_money = value;
            }
        }

        public string send_time
        {
            get
            {
                return _send_time;
            }

            private set
            {
                _send_time = value;
            }
        }

        public int shop_id
        {
            get
            {
                return _shop_id;
            }

            private set
            {
                _shop_id = value;
            }
        }

        public string shop_name
        {
            get
            {
                return _shop_name;
            }

            private set
            {
                _shop_name = value;
            }
        }
        public string real_pay_text { get; set; }
        public string reward_coin_text { get; set; }
        public int status
        {
            get
            {
                return _status;
            }

            private set
            {
                _status = value;
            }
        }

        public string track_com
        {
            get
            {
                return _track_com;
            }

            private set
            {
                _track_com = value;
            }
        }

        public string track_num
        {
            get
            {
                return _track_num;
            }

            private set
            {
                _track_num = value;
            }
        }

        public int uid
        {
            get
            {
                return _uid;
            }

            private set
            {
                _uid = value;
            }
        }

        public DataTable goods_list
        {
            get
            {
                return _goods_list;
            }

            private set
            {
                _goods_list = value;
            }
        }

        public string receive_name
        {
            get
            {
                return _receive_name;
            }

            private set
            {
                _receive_name = value;
            }
        }

        public string receive_address
        {
            get
            {
                return _receive_address;
            }

            private set
            {
                _receive_address = value;
            }
        }

        public string receive_mobile
        {
            get
            {
                return _receive_mobile;
            }

            private set
            {
                _receive_mobile = value;
            }
        }

        public double discount
        {
            get
            {
                return _discount;
            }

            private set
            {
                _discount = value;
            }
        } 

        public double postage
        {
            get
            {
                return _postage;
            }

            private set
            {
                _postage = value;
            }
        }

        public string check_code
        {
            get { return _check_code; }

            private set { _check_code = value; }
        }

        public string addtime
        {
            get { return _addtime; }
            private set { _addtime = value; }
        }


        public string sh_sell_status { get; set; }

        public int shared_count { get; set; }

        public int remain_second { get; set; }


        /// <summary>
        /// 是否能转卖
        /// </summary>
        public bool is_can_resell { get; set; } = false;

        #endregion 字段定义

        public ListOrder(DataRow OrderRow)
        {
            InitListOrder(OrderRow);
            InitOrderGoods(this.id);
        }

        public ListOrder(int id)
        {
            Load(id);
        }

        public int Load(int id)
        {
            RRLDB db = new RRLDB();
            DataSet ds = db.ExeQuery("select * from [ORDER_LIST_VIEW] where id = @id",
                new SqlParameter("@id", SqlDbType.Int, 4) { Value = id });
            db.Close();
            if (ds == null)
                return MessageCode.ERROR_EXECUTE_SQL;//执行查询失败
            if (ds.Tables[0].Rows.Count == 0)
                return MessageCode.ERROR_NO_UID;//没查到数据
            DataRow Order = ds.Tables[0].Rows[0];
            InitListOrder(Order);
            InitOrderGoods(this.id);
            return MessageCode.SUCCESS;
        }

        private void InitListOrder(DataRow OrderRow)
        {
            goods_count = Convert.ToInt32(OrderRow["goods_count"]);
            id = Convert.ToInt32(OrderRow["id"]);
            money = Convert.ToDouble(OrderRow["money"].ToString());
            order_code = Convert.ToString(OrderRow["order_code"]);
 
            return_money = Convert.ToDouble(OrderRow["return_money"].ToString()==""?"0": OrderRow["return_money"].ToString());
 
            send_time = Convert.ToString(OrderRow["send_time"]);
            shop_id = Convert.ToInt32(OrderRow["shop_id"]);
            shop_name = Convert.ToString(OrderRow["shop_name"]);
            status = Convert.ToInt32(OrderRow["status"]);
            track_com = Convert.ToString(OrderRow["track_com"]);
            track_num = Convert.ToString(OrderRow["track_num"]);
            receive_name = Convert.ToString(OrderRow["receive_name"]);
            receive_mobile = Convert.ToString(OrderRow["receive_mobile"]);
            receive_address = Convert.ToString(OrderRow["receive_address"]);
            uid = Convert.ToInt32(OrderRow["uid"]);
            shared_count = Convert.ToInt32(OrderRow["shared_count"]);
            remain_second = Convert.ToInt32(OrderRow["remain_second"]);

            string discount_str= OrderRow["discount"].ToString();
            //discount
            if (!string.IsNullOrWhiteSpace(discount_str))
            {
                discount = Convert.ToDouble(discount_str);
            }
            //discount = Convert.ToDouble(OrderRow["discount"].ToString());
            string postage_string = OrderRow["postage"].ToString();
            if(!string.IsNullOrWhiteSpace(postage_string))
            {
                postage = Convert.ToDouble(postage_string);
            }
            //postage = Convert.ToDouble(OrderRow["postage"]);
            check_code = Convert.ToString(OrderRow["check_code"]);
            addtime = Convert.ToString(OrderRow["addtime"]);
            this. sh_sell_status= OrderRow["sh_sell_status"].ToString();
            var add_time=Convert.ToDateTime(addtime);
             
            this.is_can_resell = isCanResell(add_time,this.status,this.sh_sell_status);
            this.real_pay_text = OrderRow["real_pay_text"].ToString();


        }

        private void InitOrderGoods(int order_id)
        {
            RRLDB db = new RRLDB();
            DataSet ds = db.ExeQuery("select * from [ORDER_LIST_GOODS] where order_id = @order_id",
                new SqlParameter("@order_id", SqlDbType.Int, 4) { Value = order_id });
            db.Close();
            ds.Tables[0].Columns.Add("is_can_resell", typeof(bool));
            foreach (DataRow oneRow in ds.Tables[0].Rows)
            {
                var add_time = Convert.ToDateTime(oneRow["addtime"].ToString());
                oneRow["is_can_resell"] = isCanResell(add_time,this.status,this.sh_sell_status);  
            }
               
            goods_list = ds.Tables[0];
        }
        public static bool isCanResell(DateTime add_time, int status, string sh_resell_status)
        {
            if(status==3)
            {
                //return true;
                if("99".Equals(sh_resell_status)|| "1".Equals(sh_resell_status)|| "2".Equals(sh_resell_status))
                {
                    return false;
                }
                return true;
            }else if (status != 2)
            {
                return false;
            }
             

            if (!(string.IsNullOrWhiteSpace(sh_resell_status) || "0" == sh_resell_status))
            {
                return false;
            }

             
           
            var now = DateTime.Now;
            var now_date = now.Date;
            var next_date = now_date.AddDays(1);
            if (add_time > now_date && add_time < new DateTime(now_date.Year, now_date.Month, now_date.Day, 9, 0, 0))
            {
                return   true;
            }

            if (add_time > new DateTime(now_date.Year, now_date.Month, now_date.Day, 9, 0, 0)
                && add_time < new DateTime(next_date.Year, next_date.Month, next_date.Day, 9, 0, 0)
                )
            {
                return   true;
            }

            return false;
        }
    }
}