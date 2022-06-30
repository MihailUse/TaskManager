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

        public List<Status> ReadAll()
        {
            return _context.Status.ToList();
        }

        public List<Status> ReadAllExclude(Statuses status)
        {
            return _context.Status.Where(x => x.Id != (int)status).ToList();
        }
    }
}
