
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
    
public partial class Membership
{

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public Membership()
    {

        this.Assignment = new HashSet<Assignment>();

    }


    public int id { get; set; }

    public int user_id { get; set; }

    public int project_id { get; set; }

    public int role_id { get; set; }



    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<Assignment> Assignment { get; set; }

    public virtual Project Project { get; set; }

    public virtual Role Role { get; set; }

    public virtual User User { get; set; }

}

}
