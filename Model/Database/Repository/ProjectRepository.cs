using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using TaskManager.Model.SubModels;

namespace TaskManager.Model.Database.Repository
{
    internal class ProjectRepository
    {
        private ITaskManagerContext _context { get; }

        public ProjectRepository(ITaskManagerContext context)
        {
            _context = context;
        }

        public bool IsExist(Project project)
        {
            return _context.Project.Any(x => (x.Id != project.Id) && (x.Name == project.Name));
        }

        public Project Find(long projectId)
        {
            return _context.Project.Where(x => x.Id == projectId).FirstOrDefault();
        }

        public Project Read(Expression<Func<Project, bool>> predicate)
        {
            return _context.Project.Where(predicate).FirstOrDefault();
        }

        public List<ProjectItem> GetProjectItems(long userId, bool userOwer = false)
        {
            IQueryable<Project> query = userOwer
                ? _context.Project.Where(x => x.Membership.Where(m => (m.UserId == userId) && (m.Role.Id == (int)Roles.Owner)).Any())
                : _context.Project.Where(x => x.Membership.Any(m => m.UserId == userId));

            return query
                .Select<Project, ProjectItem>(x => new ProjectItem()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Logo = x.Logo,
                    Description = x.Description,
                    UserCount = x.Membership.Count(),
                    TaskCount = x.Task.Count(),
                    Role = (Roles)x.Membership.Where(m => m.UserId == userId).FirstOrDefault().RoleId,
                })
                .ToList();
        }

        public Project CreateOrUpdate(Project newProject)
        {
            Project project = _context.Project.Find(newProject.Id);
            if (project == null)
                project = _context.Project.Add(newProject);
            else
                _context.Entry(project).CurrentValues.SetValues(newProject);

            _context.SaveChanges();
            return project;
        }

        public void Delete(long id)
        {
            Project project = _context.Project.Find(id);
            _context.Project.Remove(project);
            _context.SaveChanges();
        }

        public List<Project> GetUserProjects(User user)
        {
            return _context.Project
                .Where(x => x.Membership.Where(m => m.UserId == user.Id).Any())
                .ToList();
        }
    }
}
