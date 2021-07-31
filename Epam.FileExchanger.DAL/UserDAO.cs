using Epam.FileExchanger.Common.Entities;
using Epam.FileExchanger.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epam.FileExchanger.DAL
{
    public class UserDAO : IUserDAO
    {
        private readonly string _connectionString;

        public UserDAO()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["Epam.FileExchanger.DAL.Properties.Settings.FileExchangerDbConnectionString"].ConnectionString;
        }
        public void Add(User user)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string queryStringUser =
                        $"INSERT INTO [Users](Birthday, FullName, Username) " +
                        $"OUTPUT Inserted.Id "+
                        $"VALUES ('{user.Birthday}', '{user.FullName}','{user.Username}')";
                int? insertedUserId = -1;
                using (var command = new SqlCommand(queryStringUser, connection))
                {
                    insertedUserId = int.Parse(command.ExecuteScalar().ToString());
                }
                string queryStringUserAuth =
                        $"INSERT INTO [UserAuth](IdUser, Login, Password) " +
                        $"VALUES ('{insertedUserId}', '{user.Login}','{user.Password}')";
                using (var command = new SqlCommand(queryStringUserAuth, connection))
                {
                    command.ExecuteNonQuery();
                }
                foreach (var role in user.Roles)
                {
                    int idToInsert = role == RoleUser.Admin ? 1 : 2;
                    string queryStringUserRoles =
                        $"INSERT INTO [UserAuthRoles](IdRole, IdUser) " +
                        $"VALUES ('{idToInsert}', '{insertedUserId}')";
                    using (var command = new SqlCommand(queryStringUserRoles, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                } 
            }
        }

        public void AddToFriends(User userBase, User userToAdd)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string queryStringUser =
                        $"INSERT INTO Friends(IdFirstUser, IdSecondUser) " +
                        $"VALUES ('{userBase.Id}', '{userToAdd.Id}')";
                using (var command = new SqlCommand(queryStringUser, connection))
                {
                    command.ExecuteNonQuery();

                }
            }
        }
        public void Delete(User user)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string queryStringDeleteFriends =
                        $"DELETE FROM Friends Where IdFirstUser = '{user.Id}'";

                using (var command = new SqlCommand(queryStringDeleteFriends, connection))
                {
                    command.ExecuteNonQuery();
                }
                string queryStringDeleteFriendsSecond =
                        $"DELETE FROM Friends Where IdSecondUser = '{user.Id}'";

                using (var command = new SqlCommand(queryStringDeleteFriendsSecond, connection))
                {
                    command.ExecuteNonQuery();
                }
                string queryStringDeleteRoles =
                        $"DELETE FROM UserAuthRoles Where IdUser = '{user.Id}'";

                using (var command = new SqlCommand(queryStringDeleteRoles, connection))
                {
                    command.ExecuteNonQuery();
                }

               

                string queryStringUserAuth =
                        $"DELETE FROM UserAuth Where IdUser = '{user.Id}'";

                using (var command = new SqlCommand(queryStringUserAuth, connection))
                {
                    command.ExecuteNonQuery();
                }

                string queryStringUser =
                        $"DELETE FROM [Users] Where Id = '{user.Id}'";


                using (var command = new SqlCommand(queryStringUser, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        public IEnumerable<User> GetALL()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string queryString = $"SELECT [Users].Id, [Users].Birthday, [Users].FullName, [Users].Username, UserAuth.Login, UserAuth.Password FROM [Users] " +
                                     $"INNER JOIN UserAuth ON [Users].Id = UserAuth.IdUser";
                var command = new SqlCommand(queryString, connection);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string queryStringRoles = $"SELECT IdRole FROM UserAuthRoles WHERE IdUser = '{reader["Id"]}'";
                        var commandRoles = new SqlCommand(queryStringRoles, connection);
                        List<RoleUser> roles = new List<RoleUser>();
                        using (var readerRoles = commandRoles.ExecuteReader())
                        {
                            while (readerRoles.Read())
                            {
                                roles.Add(int.Parse(readerRoles["IdRole"].ToString()) == 1 ? RoleUser.Admin : RoleUser.User);
                            }
                        }
                        string queryStringFiles = $"SELECT * FROM Files WHERE IdUser = '{reader["Id"]}'";
                        var commandFiles = new SqlCommand(queryStringFiles, connection);
                        List<File> files = new List<File>();
                        using (var readerFiles = commandFiles.ExecuteReader())
                        {
                            while (readerFiles.Read())
                            {
                                files.Add(new File(
                                    readerFiles["Path"].ToString(),
                                    new User(reader["Login"].ToString(),
                                    reader["Password"].ToString(),
                                    reader["FullName"].ToString(),
                                    reader["UserName"].ToString(),
                                    DateTime.Parse(reader["Birthday"].ToString()),
                                    roles,
                                    new List<File>(),
                                    int.Parse(reader["Id"].ToString())),
                                    DateTime.Parse(readerFiles["AddTime"].ToString()),
                                    int.Parse(readerFiles["PrivacyType"].ToString()) == 1 ? PrivacyType.Private : PrivacyType.Public,
                                    long.Parse(readerFiles["Id"].ToString())));


                            }
                        }
                        yield return new User(reader["Login"].ToString(),
                                    reader["Password"].ToString(),
                                    reader["FullName"].ToString(),
                                    reader["Username"].ToString(),
                                    DateTime.Parse(reader["Birthday"].ToString()),
                                    roles,
                                    files,
                                    int.Parse(reader["Id"].ToString()));
                    }
                }
            }
        }

        public User GetById(long id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string queryString = $"SELECT [Users].Id, [Users].Birthday, [Users].FullName, [Users].Username, UserAuth.Login, UserAuth.Password FROM [Users] " +
                                     $"INNER JOIN UserAuth ON [Users].Id = UserAuth.IdUser " +
                                     $"WHERE [Users].Id = '{id}'";
                var command = new SqlCommand(queryString, connection);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string queryStringRoles = $"SELECT IdRole FROM UserAuthRoles WHERE IdUser = '{reader["Id"]}'";
                        var commandRoles = new SqlCommand(queryStringRoles, connection);
                        List<RoleUser> roles = new List<RoleUser>();
                        using (var readerRoles = commandRoles.ExecuteReader())
                        {
                            while (readerRoles.Read())
                            {
                                roles.Add(int.Parse(readerRoles["IdRole"].ToString()) == 1 ? RoleUser.Admin : RoleUser.User);
                            }
                        }
                        string queryStringFiles = $"SELECT * FROM Files WHERE IdUser = '{reader["Id"]}'";
                        var commandFiles = new SqlCommand(queryStringFiles, connection);
                        List<File> files = new List<File>();
                        using (var readerFiles = commandFiles.ExecuteReader())
                        {
                            while (readerFiles.Read())
                            {
                                files.Add(new File(
                                    readerFiles["Path"].ToString(),
                                    new User(reader["Login"].ToString(),
                                    reader["Password"].ToString(),
                                    reader["FullName"].ToString(),
                                    reader["Username"].ToString(),
                                    DateTime.Parse(reader["Birthday"].ToString()),
                                    roles,
                                    new List<File>(),
                                    int.Parse(reader["Id"].ToString())),
                                    DateTime.Parse(readerFiles["AddTime"].ToString()),
                                    int.Parse(readerFiles["PrivacyType"].ToString()) == 1 ? PrivacyType.Private : PrivacyType.Public,
                                    long.Parse(readerFiles["Id"].ToString())));


                            }
                        }
                        return new User(reader["Login"].ToString(),
                                    reader["Password"].ToString(),
                                    reader["FullName"].ToString(),
                                    reader["Username"].ToString(),
                                    DateTime.Parse(reader["Birthday"].ToString()),
                                    roles,
                                    files,
                                    int.Parse(reader["Id"].ToString()));
                    }
                }
            }
            throw new Exception($"User with Id={id} doesn't exist");
        }

        public IEnumerable<User> GetFriends(User user)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var queryStringSelect = $"SELECT * FROM Friends WHERE IdFirstUser = '{user.Id}'";
                var command = new SqlCommand(queryStringSelect, connection);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        yield return GetById(long.Parse(reader["IdSecondUser"].ToString()));
                    }
                }
            }
        }

        public void Update(User user)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string queryStringUser =
                        $"UPDATE [Users] SET Birthday = '{user.Birthday}', Fullname = '{user.FullName}'" +
                        $"Where Id = '{user.Id}";
                using (var command = new SqlCommand(queryStringUser, connection))
                {
                    command.ExecuteNonQuery();
                }
                string queryStringUserAuth =
                        $"UPDATE [UserAuth] SET Login = '{user.Login}', Password = '{user.Password}'" +
                        $"WHERE IdUser = {user.Id}";
                using (var command = new SqlCommand(queryStringUserAuth, connection))
                {
                    command.ExecuteNonQuery();
                }
                string queryStringUserAuthDelete =
                        $"DELETE FROM UserAuthRoles WHERE IdUser = '{user.Id}'";
                using (var command = new SqlCommand(queryStringUserAuthDelete, connection))
                {
                    command.ExecuteNonQuery();
                }
                foreach (var role in user.Roles)
                {
                    int idToInsert = role == RoleUser.Admin ? 1 : 2;
                    string queryStringUserRoles =
                        $"INSERT INTO [UserAuthRoles](IdRole, IdUser)" +
                        $"VALUES ('{idToInsert}', '{user.Id}')";
                    using (var command = new SqlCommand(queryStringUserRoles, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }
            }
        }
    }
}
