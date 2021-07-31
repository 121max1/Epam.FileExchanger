using Epam.FileExchanger.BLL.Interfaces;
using Epam.FileExchanger.Common.Entities;
using Epam.FileExchanger.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epam.FileExchanger.BLL
{
    public class UserService : IUserService
    {
        private readonly IUserDAO _userDAO;
        public UserService(IUserDAO userDAO)
        {
            _userDAO = userDAO;
        }
        public void Add(User user)
        {
            _userDAO.Add(user);
        }

        public void AddToFriends(User userBase, User userToAdd)
        {
            _userDAO.AddToFriends(userBase, userToAdd);
        }

        public bool CanLogin(string login, string password)
        {
            User user = _userDAO.GetALL().Where(u => u.Login == login).FirstOrDefault();
            if (user != null)
            {
                if(user.Password == password)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public void Delete(User user)
        {
            _userDAO.Delete(user);
        }

        public IEnumerable<User> GetALL()
        {
            return _userDAO.GetALL();
        }

        public User GetById(long id)
        {
            return _userDAO.GetById(id);
        }

        public User GetByLogin(string login)
        {
            return _userDAO.GetALL().Where(u => u.Login == login).FirstOrDefault();
        }

        public User GetByUsername(string username)
        {
            return _userDAO.GetALL().Where(u => u.Username == username).FirstOrDefault();
        }

        public IEnumerable<User> GetFriends(User user)
        {
            return _userDAO.GetFriends(user);
        }

        public void Update(User user)
        {
            _userDAO.Update(user);
        }
    }
}
