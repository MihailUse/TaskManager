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

        public Membership Create(Membership membership)
        {
            Membership newProject = _context.Membership.Add(membership);
            _context.SaveChanges();
            return newProject;
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

        public List<Membership> ReadAll(Expression<Func<Membership, bool>> predicate = null) //, Expression<Func<Project, string>> keySelectorctor)
        {
            return _context.Membership.Where(predicate).ToList();
        }

        public Membership Update(Membership newMembership)
        {
            Membership membership = _context.Membership.Find(newMembership.Id);
            if (membership == null)
                return null;

            _context.Entry(membership).CurrentValues.SetValues(newMembership);
            _context.SaveChanges();
            return membership;
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
