using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace TaskManager.Model.Database.Repository
{
    internal class UserRepository
    {
        private ITaskManagerContext _context { get; }

        public UserRepository(ITaskManagerContext context)
        {
            _context = context;
        }

        public bool IsExist(string login)
        {
            return _context.User.Any(x => (x.Login == login));
        }

        public User Create(User user)
        {
            User newUser = _context.User.Add(user);
            _context.SaveChanges();
            return newUser;
        }

        public User GetByLogin(string login)
        {
            return _context.User.Where(x => (x.Login == login)).FirstOrDefault();
        }

        public List<User> GetAllByLogin(string login)
        {
            return _context.User
                .Where(x => x.Login.ToLower().Contains(login.ToLower()))
                .ToList();
        }
    }
}
