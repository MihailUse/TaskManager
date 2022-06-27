namespace TaskManager.Migrations
{
    using System.Data.Entity.Migrations;
    using TaskManager.Model;
    using TaskManager.Model.Database;

    internal sealed class Configuration : DbMigrationsConfiguration<TaskManagerContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(TaskManager.Model.Database.TaskManagerContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.

            Role[] roles = new Role[]
            {
                new Role((int)Roles.Owner, "Owner", "A person who created the project"),
                new Role((int)Roles.Administrator, "Administrator", "A person who can add/remove members, edit entries, set the priority of tasks"),
                new Role((int)Roles.Tester, "Tester", "A person who can change the status of tasks and create new tasks"),
                new Role((int)Roles.Developer, "Developer", "A person who develops software"),
            };

            Status[] statuses = new Status[]
            {
                new Status((int)Statuses.ToTest, "To Do"),
                new Status((int)Statuses.InProgress, "In Progress"),
                new Status((int)Statuses.ToTest, "To Test"),
                new Status((int)Statuses.Testing, "Testing"),
                new Status((int)Statuses.Done, "Done"),
            };

            context.Role.AddOrUpdate(x => x.Id, roles);
            context.Status.AddOrUpdate(x => x.Id, statuses);
        }
    }
}
