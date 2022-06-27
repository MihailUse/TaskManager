using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;

namespace TaskManager.Model.Database
{
    internal interface ITaskManagerContext
    {
        DbSet<Membership> Membership { get; set; }
        DbSet<Project> Project { get; set; }
        DbSet<Role> Role { get; set; }
        DbSet<Status> Status { get; set; }
        DbSet<Task> Task { get; set; }
        DbSet<User> User { get; set; }

        int SaveChanges();
        DbEntityEntry Entry(object entity);
    }
}