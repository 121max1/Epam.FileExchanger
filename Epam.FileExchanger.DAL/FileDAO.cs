using Epam.FileExchanger.Common.Entities;
using Epam.FileExchanger.DAL.Interfaces;
using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace Epam.FileExchanger.DAL
{
    public class FileDAO : IFileDAO
    {
        private readonly string _connectionString;

        public FileDAO()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["Epam.FileExchanger.DAL.Properties.Settings.FileExchangerDbConnectionString"].ConnectionString;
            //_connectionString = "Data Source=desktop-mbiluqg;Initial Catalog=FileExchangerDb;Integrated Security=True";
        }
        public void Add(File file)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string queryStringFile =
                        $"INSERT INTO [Files](Path, IdUser,AddTime,PrivacyType) " +
                        $"VALUES ('{file.Path}', '{file.Publisher.Id}', '{file.AddTime}','{(file.PrivacyType == PrivacyType.Public?0:1)}')";
                using (var command = new SqlCommand(queryStringFile, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }
        public void Delete(File file)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string queryString =
                        $"DELETE FROM [Files] Where Id = '{file.Id}'";


                using (var command = new SqlCommand(queryString, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        public IEnumerable<File> GetAll()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string queryString = $"SELECT * FROM Files";
                var command = new SqlCommand(queryString, connection);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string queryStringRoles = $"SELECT IdRole FROM UserAuthRoles WHERE IdUser = '{reader["IdUser"]}'";
                        var commandRoles = new SqlCommand(queryStringRoles, connection);
                        List<RoleUser> roles = new List<RoleUser>();
                        using (var readerRoles = commandRoles.ExecuteReader())
                        {
                            while (readerRoles.Read())
                            {
                                roles.Add(int.Parse(readerRoles["IdRole"].ToString()) == 1 ? RoleUser.Admin : RoleUser.User);
                            }
                        }
                        string queryStringUser = $"SELECT [Users].Id, [Users].Birthday, [Users].FullName, [Users].Username, UserAuth.[Login], UserAuth.[Password] FROM [Users] INNER JOIN UserAuth ON [Users].Id = UserAuth.IdUser WHERE [Users].Id = '{reader["IdUser"]}' ";
                        var commandUser = new SqlCommand(queryStringUser, connection);
                        var user = new User();
                        using (var readerUser = commandUser.ExecuteReader())
                        {
                            while (readerUser.Read())
                            {
                                user = new User(readerUser["Login"].ToString(),
                                    readerUser["Password"].ToString(),
                                    readerUser["FullName"].ToString(),
                                    readerUser["Username"].ToString(),
                                    DateTime.Parse(readerUser["Birthday"].ToString()),
                                    roles,
                                    new List<File>(),
                                    int.Parse(readerUser["Id"].ToString()));
                            }
                        }

                        yield return new File(reader["Path"].ToString(),
                            user,
                            DateTime.Parse(reader["AddTime"].ToString()),
                            int.Parse(reader["PrivacyType"].ToString()) == 0 ?PrivacyType.Public:PrivacyType.Private,
                            int.Parse(reader["Id"].ToString()));
                    }
                }
            }
        }
    

        public File GetById(long id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string queryString = $"SELECT * FROM Files WHERE Id = {id}";
                var command = new SqlCommand(queryString, connection);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string queryStringRoles = $"SELECT IdRole FROM UserAuthRoles WHERE IdUser = '{reader["IdUser"]}'";
                        var commandRoles = new SqlCommand(queryStringRoles, connection);
                        List<RoleUser> roles = new List<RoleUser>();
                        using (var readerRoles = commandRoles.ExecuteReader())
                        {
                            while (readerRoles.Read())
                            {
                                roles.Add(int.Parse(readerRoles["IdRole"].ToString()) == 1 ? RoleUser.Admin : RoleUser.User);
                            }
                        }
                        string queryStringUser = $"SELECT [Users].Id, [Users].Birthday, [Users].FullName, [Users].Username, UserAuth.[Login], UserAuth.[Password] FROM [Users] INNER JOIN UserAuth ON [Users].Id = UserAuth.IdUser WHERE [Users].Id = '{reader["IdUser"]}' ";
                        var commandUser = new SqlCommand(queryStringUser, connection);
                        var user = new User();
                        using (var readerUser = commandUser.ExecuteReader())
                        {
                            while (readerUser.Read())
                            {
                                user = new User(readerUser["Login"].ToString(),
                                    readerUser["Password"].ToString(),
                                    readerUser["FullName"].ToString(),
                                    readerUser["Username"].ToString(),
                                    DateTime.Parse(readerUser["Birthday"].ToString()),
                                    roles,
                                    new List<File>(),
                                    int.Parse(readerUser["Id"].ToString()));
                            }
                        }

                        return new File(reader["Path"].ToString(),
                            user,
                            DateTime.Parse(reader["AddTime"].ToString()),
                            int.Parse(reader["PrivacyType"].ToString()) == 0 ? PrivacyType.Public : PrivacyType.Private,
                            int.Parse(reader["Id"].ToString()));
                    }
                }
            }
            throw new Exception($"File with Id={id} doesn't exist");
        }
    

        public void Update(File file)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string queryStringUser =
                        $"UPDATE [Files] SET Path = '{file.Path}', IdUser = '{file.Publisher.Id}', AddTime = '{file.AddTime}',PrivacyType = '{(file.PrivacyType == PrivacyType.Public ? 0 : 1)}' " +
                        $"Where Id = '{file.Id}";
                using (var command = new SqlCommand(queryStringUser, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
