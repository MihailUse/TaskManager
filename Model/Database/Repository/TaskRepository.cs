using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace TaskManager.Model.Database.Repository
{
    internal class TaskRepository
    {
        private ITaskManagerContext _context { get; }

        public TaskRepository(ITaskManagerContext context)
        {
            _context = context;
        }

        public bool IsExist(Task task, Project project)
        {
            return _context.Task
                .Where(x => (x.Id != task.Id) && (x.ProjectId == project.Id) && (x.Title == task.Title))
                .Any();
        }

        public void CreateOrUpdate(Task newTask)
        {
            Task task = _context.Task.Find(newTask.Id);
            if (task == null)
                task = _context.Task.Add(newTask);
            else
                _context.Entry(task).CurrentValues.SetValues(newTask);

            _context.SaveChanges();
        }

        public List<Task> GetTaskItems(long projectId, User user = null, string status = null)
        {
            IQueryable<Task> query = _context.Task.Where(x => (x.ProjectId == projectId));

            if (user != null)
                query = query.Where(x => x.UserId == user.Id);

            if (status != null)
                query = query.Where(x => x.Status.Name == status);

            return query
                .Include(x => x.Status)
                .ToList();
        }

        public Task Update(Task newTask)
        {
            Task task = _context.Task.Find(newTask.Id);
            if (task == null)
                return null;

            _context.Entry(task).CurrentValues.SetValues(newTask);
            _context.SaveChanges();
            return task;
        }
    }
}
