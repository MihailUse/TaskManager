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

        public bool IsExist(string title, long projectId)
        {
            return _context.Task.Where(x => (x.Title == title) && (x.ProjectId == projectId)).Any();
        }

        public Task Create(Task task)
        {
            Task newProject = _context.Task.Add(task);
            _context.SaveChanges();
            return newProject;
        }

        public Task Read(Expression<Func<Task, bool>> predicate)
        {
            return _context.Task.Where(predicate).FirstOrDefault();
        }

        public List<Task> GetTaskItems(long projectId, User user = null, string status = null)
        {
            IQueryable<Task> query = _context.Task.Where(x => (x.ProjectId == projectId) && !x.DetetedAt.HasValue);

            if (user != null)
                query = query.Where(x => x.UserId == user.Id);

            if (status != null)
                query = query.Where(x => x.Status.Name == status);

            return query
                .Include(x => x.Status)
                .ToList();
        }

        public List<Task> ReadAll(Expression<Func<Task, bool>> predicate = null)
        {
            return _context.Task.Where(predicate).ToList();
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

        public void Delete(Task task)
        {
            _context.Task.Remove(task);
            _context.SaveChanges();
        }
    }
}
