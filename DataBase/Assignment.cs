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
    
    public partial class Assignment
    {
        public int id { get; set; }
        public int membership_id { get; set; }
        public int task_id { get; set; }
    
        public virtual Membership Membership { get; set; }
        public virtual Task Task { get; set; }
    }
}
