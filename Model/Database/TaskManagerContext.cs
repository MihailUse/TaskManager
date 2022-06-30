using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace TaskManager.Model.Database
{
    enum Roles
    {
        Owner = 1,
        Administrator,
        Tester,
        Developer
    }

    enum Statuses
    {
        TODO = 1,
        InProgress,
        ToTest,
        Testing,
        Done
    }

    internal class TaskManagerContext : DbContext, ITaskManagerContext
    {
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<Task> Task { get; set; }
        public virtual DbSet<Role> Role { get; set; }
        public virtual DbSet<Status> Status { get; set; }
        public virtual DbSet<Project> Project { get; set; }
        public virtual DbSet<Membership> Membership { get; set; }

        //public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }

        public TaskManagerContext() : base("name=TaskManagerDatabase")
        {
            this.Database.CreateIfNotExists();
        }
        ~TaskManagerContext() => this.Dispose(false);

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("public"); // set schema for pg
            base.OnModelCreating(modelBuilder);
        }

        private static TaskManagerContext _instance;
        public static TaskManagerContext GetInstance()
        {
            if (_instance == null)
                _instance = new TaskManagerContext();

            return _instance;
        }
    }
}
