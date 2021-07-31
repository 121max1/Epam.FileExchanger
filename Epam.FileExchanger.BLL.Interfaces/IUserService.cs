using Epam.FileExchanger.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epam.FileExchanger.BLL.Interfaces
{
    public interface IUserService
    {
        void Add(User user);

        void Delete(User user);

        void Update(User user);

        IEnumerable<User> GetALL();

        bool CanLogin(string login,string password);

        User GetById(long id);

        User GetByLogin(string login);

        void AddToFriends(User userBase, User userToAdd);

        IEnumerable<User> GetFriends(User user);

        User GetByUsername(string username);


    }
}
