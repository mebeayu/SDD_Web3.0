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
    
    public partial class admin_module
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public admin_module()
        {
            this.admin_role_power = new HashSet<admin_role_power>();
        }
    
        public int MId { get; set; }
        public string Logo { get; set; }
        public string ModuleName { get; set; }
        public string Url { get; set; }
        public string LevelStr { get; set; }
        public Nullable<int> ParentId { get; set; }
        public Nullable<int> Layer { get; set; }
        public string ActionMethod { get; set; }
        public Nullable<int> Sort { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<admin_role_power> admin_role_power { get; set; }
    }
}
