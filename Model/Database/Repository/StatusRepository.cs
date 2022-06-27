using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace TaskManager.Model.Database.Repository
{
    internal class StatusRepository
    {
        private ITaskManagerContext _context { get; }

        public StatusRepository(ITaskManagerContext context)
        {
            _context = context;
        }

        public Status Create(Status status)
        {
            Status newStatus = _context.Status.Add(status);
            _context.SaveChanges();
            return newStatus;
        }

        public Status Read(string name)
        {
            return _context.Status.Where(x => x.Name == name).FirstOrDefault();
        }

        public List<Status> ReadAll()
        {
            return _context.Status.ToList();
        }

        public List<Status> ReadAll(Expression<Func<Status, bool>> predicate)
        {
            return _context.Status.Where(predicate).ToList();
        }
    }
}
