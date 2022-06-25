using System.Collections.Generic;
using System.Linq;

namespace TaskManager.Model.Database.Repository
{
    internal class RoleRepository
    {
        private ITaskManagerContext _context { get; }

        public RoleRepository(ITaskManagerContext context)
        {
            _context = context;
        }

        public Role Create(Role role)
        {
            Role newProject = _context.Role.Add(role);
            _context.SaveChanges();
            return newProject;
        }

        public Role Read(Roles role)
        {
            return _context.Role.Where(x => x.Id == (int)role).FirstOrDefault();
        }

        public List<Role> ReadAll()
        {
            return _context.Role.ToList();
        }
    }
}
