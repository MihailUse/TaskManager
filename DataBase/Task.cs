//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TaskManager.DataBase
{
    using System;
    using System.Collections.Generic;
    
    public partial class Task
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Task()
        {
            this.Assignment = new HashSet<Assignment>();
        }
    
        public int id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public int project_id { get; set; }
        public int status_id { get; set; }
        public System.DateTime createdAt { get; set; }
        public Nullable<System.DateTime> updatedAt { get; set; }
        public Nullable<System.DateTime> detetedAt { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Assignment> Assignment { get; set; }
        public virtual Project Project { get; set; }
        public virtual Status Status { get; set; }
    }
}
