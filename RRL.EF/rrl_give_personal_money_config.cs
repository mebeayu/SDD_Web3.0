//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace RRL.EF
{
    using System;
    using System.Collections.Generic;
    
    public partial class rrl_give_personal_money_config
    {
        public int id { get; set; }
        public Nullable<int> uid { get; set; }
        public Nullable<decimal> amount { get; set; }
        public Nullable<System.DateTime> add_time { get; set; }
        public Nullable<int> type { get; set; }
        public Nullable<int> isReceived { get; set; }
        public Nullable<System.DateTime> effective_time { get; set; }
        public Nullable<int> CreateUserId { get; set; }
        public string CreateUserName { get; set; }
        public Nullable<System.DateTime> CreateTime { get; set; }
        public Nullable<int> ModifyUserId { get; set; }
        public string ModifyUserName { get; set; }
        public Nullable<System.DateTime> ModifyTime { get; set; }
        public Nullable<int> isSendSMS { get; set; }
    }
}
