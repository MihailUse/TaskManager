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
            return _context.User.Any(x => (x.Login == login) && !x.DetetedAt.HasValue);
        }

        public User Create(User user)
        {
            User newUser = _context.User.Add(user);
            _context.SaveChanges();
            return newUser;
        }

        public User GetByLogin(string login)
        {
            return _context.User.Where(x => (x.Login == login) && !x.DetetedAt.HasValue).FirstOrDefault();
        }

        public User Update(User newUser)
        {
            User user = _context.User.Find(newUser.Id);
            if (user == null)
                return null;

            _context.Entry(user).CurrentValues.SetValues(newUser);
            _context.SaveChanges();
            return user;
        }

        public void Delete(User user)
        {
            _context.User.Remove(user);
            _context.SaveChanges();
        }

        public List<User> GetAllByLogin(string login)
        {
            return _context.User
                .Where(x => x.Login.ToLower().Contains(login.ToLower()))
                .ToList();
        }
    }
}
