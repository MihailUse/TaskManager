using ImageLibrary;
using Isopoh.Cryptography.Argon2;
using Moq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace TaskManager.Model.Database
{
    internal class MockTaskManagerContext : DbContext, ITaskManagerContext
    {
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<Task> Task { get; set; }
        public virtual DbSet<Role> Role { get; set; }
        public virtual DbSet<Status> Status { get; set; }
        public virtual DbSet<Project> Project { get; set; }
        public virtual DbSet<Membership> Membership { get; set; }

        //public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }

        private static DbSet<T> GetQueryableMockDbSet<T>(List<T> sourceList) where T : class
        {
            IQueryable<T> queryable = sourceList.AsQueryable();
            Mock<DbSet<T>> dbSet = new Mock<DbSet<T>>();

            dbSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryable.Provider);
            dbSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
            dbSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            dbSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(() => queryable.GetEnumerator());
            dbSet.Setup(d => d.Add(It.IsAny<T>())).Callback<T>((s) => sourceList.Add(s));

            return dbSet.Object;
        }

        public MockTaskManagerContext()
        {
            // roles
            List<Role> roles = new List<Role>()
            {
                new Role((int)Roles.Owner, "Owner", "A person who created the project"),
                new Role((int)Roles.Administrator, "Administrator", "A person who can add/remove members, edit entries, set the priority of tasks"),
                new Role((int)Roles.Tester, "Tester", "A person who can change the status of tasks and create new tasks"),
                new Role((int)Roles.Developer, "Developer", "A person who develops software"),
            };

            // statuses
            List<Status> statuses = new List<Status>()
            {
                new Status((int)Statuses.ToTest, "To Do"),
                new Status((int)Statuses.InProgress, "In Progress"),
                new Status((int)Statuses.ToTest, "To Test"),
                new Status((int)Statuses.Testing, "Testing"),
                new Status((int)Statuses.Done, "Done"),
            };

            // users
            List<User> users = new List<User>();
            for (int i = 0; i < 10; i++)
            {
                users.Add(new User()
                {
                    Id = i,
                    Login = $"testLogin{i}",
                    About = $"someAbout{i}",
                    Avatar = ImageConvertor.BitmapToBytes(ImageGenerator.GenerateImage()),
                    Password = Argon2.Hash($"test{i}", timeCost: 1, parallelism: Environment.ProcessorCount, hashLength: 128),
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                });
            }

            // memberships
            List<Membership> memberships = new List<Membership>();
            for (int p = 0; p < 10; p++)
            {
                for (int u = 0; u < 10; u++)
                {
                    User user = users.Find(x => x.Id == u);

                    if (u == p)
                    {
                        memberships.Add(new Membership()
                        {
                            //Id = p,
                            UserId = user.Id,
                            User = user,
                            ProjectId = p,
                            RoleId = (int)Roles.Owner,
                            Role = roles.Find(x => x.Id == (int)Roles.Owner)
                        });
                        continue;
                    }

                    memberships.Add(new Membership()
                    {
                        //Id = (p * 10) + u,
                        UserId = user.Id,
                        User = user,
                        ProjectId = p,
                        RoleId = (int)Roles.Developer,
                        Role = roles.Find(x => x.Id == (int)Roles.Developer)
                    });
                }
            }

            // projects
            List<Project> projects = new List<Project>();
            for (int i = 0; i < 10; i++)
            {
                projects.Add(new Project()
                {
                    Id = i,
                    Name = $"testProject{i}",
                    Description = $"someDescription{i}",
                    Logo = ImageConvertor.BitmapToBytes(ImageGenerator.GenerateImage()),
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    Membership = memberships.Where(x => x.ProjectId == i).ToList(),
                    Task = new List<Task>()
                });
            }

            // Tasks
            List<Task> tasks = new List<Task>();
            for (int i = 0; i < 10; i++)
            {
                Project project = projects.First();

                tasks.Add(new Task()
                {
                    Id = i,
                    Title = $"someTitle{i}",
                    Description = "someDescrissddddddddddddddfjdshfj\ndshfahfjks\ndfhjklfhjhsdakjfhaptionsomeDescrissddddddddddddddfjdshfj\ndshfahfjks\ndfhjklfhjhsdakjfhaptionsomeDescrissddddddddddddddfjdshfj\ndshfahfjks\ndfhjklfhjhsdakjfhaptionsomeDescrissddddddddddddddfjdshfj\ndshfahfjks\ndfhjklfhjhsdakjfhaptionsomeDescrissddddddddddddddfjdshfj\ndshfahfjks\ndfhjklfhjhsdakjfhaptionsomeDescrissddddddddddddddfjdshfj\ndshfahfjks\ndfhjklfhjhsdakjfhaptionsomeDescrissddddddddddddddfjdshfj\ndshfahfjks\ndfhjklfhjhsdakjfhaptionsomeDescrissddddddddddddddfjdshfj\ndshfahfjks\ndfhjklfhjhsdakjfhaptionsomeDescrissddddddddddddddfjdshfj\ndshfahfjks\ndfhjklfhjhsdakjfhaptionsomeDescrissddddddddddddddfjdshfj\ndshfahfjks\ndfhjklfhjhsdakjfhaptionsomeDescrissddddddddddddddfjdshfj\ndshfahfjks\ndfhjklfhjhsdakjfhaptionsomeDescrissddddddddddddddfjdshfj\ndshfahfjks\ndfhjklfhjhsdakjfhaption",
                    ProjectId = project.Id,
                    Project = project,
                    UserId = i,
                    User = users.Where(x => x.Id == i).First(),
                    StatusId = i % 5,
                    Status = statuses[i % 5],
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                });
            }

            User = GetQueryableMockDbSet(users);
            Project = GetQueryableMockDbSet(projects);
            Membership = GetQueryableMockDbSet(memberships);
            Role = GetQueryableMockDbSet(roles);
            Task = GetQueryableMockDbSet(tasks);
            Status = GetQueryableMockDbSet(statuses);
        }
        ~MockTaskManagerContext() => this.Dispose(false);


        private static MockTaskManagerContext _instance;
        public static MockTaskManagerContext GetInstance()
        {
            if (_instance == null)
                _instance = new MockTaskManagerContext();

            return _instance;
        }
    }
}
