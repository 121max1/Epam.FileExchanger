using Epam.FileExchanger.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epam.FileExchanger.DAL.Interfaces
{
    public interface IUserDAO
    {
        void Add(User user);

        void Delete(User user);

        void Update(User user);

        IEnumerable<User> GetALL();

        User GetById(long id);

        void AddToFriends(User userBase, User userToAdd);

        IEnumerable<User> GetFriends(User user);
    }
}
