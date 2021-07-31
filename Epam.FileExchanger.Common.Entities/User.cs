using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epam.FileExchanger.Common.Entities
{
    public enum RoleUser
    {
        Admin,
        User
    }
    
    public class User
    {
        public User(string login, string password, string fullName, string username, DateTime birthday, IEnumerable<RoleUser> roles, IEnumerable<File> files, long id = -1)
        {
            Id = id;
            Login = login;
            Password = password;
            FullName = fullName;
            Username = username;
            Birthday = birthday;
            Roles = roles;
            Files = files;
        }
        public User()
        {

        }

        public long Id { get; private set; }
        public string Login { get; private set; }
        public string Password { get; private set; }
        public string FullName { get; private set; }

        public string Username { get; private set; }

        public DateTime Birthday { get; private set; }

        public IEnumerable<RoleUser> Roles { get; private set; }
        public IEnumerable<File> Files { get; private set; }

    }
}
