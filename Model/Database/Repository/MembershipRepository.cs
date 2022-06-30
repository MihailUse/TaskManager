using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace TaskManager.Model.Database.Repository
{
    internal class MembershipRepository
    {
        private ITaskManagerContext _context { get; }

        public MembershipRepository(ITaskManagerContext context)
        {
            _context = context;
        }

        public IEnumerable<Membership> CreateOrUpdate(IEnumerable<Membership> memberships)
        {
            foreach (Membership membership in memberships)
            {
                if (membership.Id == 0)
                    _context.Membership.Add(membership);
                else
                    _context.Entry(membership).CurrentValues.SetValues(membership);
            }

            _context.SaveChanges();
            return memberships;
        }

        public Membership Read(Expression<Func<Membership, bool>> predicate)
        {
            return _context.Membership.Where(predicate).FirstOrDefault();
        }

        public void Delete(Membership membership)
        {
            _context.Membership.Remove(membership);
            _context.SaveChanges();
        }

        public List<User> GetProjectUsers(Project project)
        {
            return _context.Membership
                .Where(x => x.ProjectId == project.Id)
                //.Include(x => x.User)
                .Select(x => x.User)
                .ToList();
        }

        public List<Membership> GetProjectMemberships(long projectId)
        {
            return _context.Membership
                .Where(x => x.ProjectId == projectId)
                .Include(x => x.User)
                .ToList();
        }

        public List<User> GetProjectUsers(long projectId)
        {
            return _context.Membership
                .Where(x => x.ProjectId == projectId)
                //.Include(x => x.User)
                .Select(x => x.User)
                .ToList();
        }
    }
}
