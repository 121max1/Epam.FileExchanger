using Epam.FileExchanger.BLL.Interfaces;
using Epam.FileExchanger.Common.Dependencies;
using Epam.FileExchanger.Common.Entities;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace Epam.FileExchanger.PL.WebPagess.Models
{
    public class FileExchangerRoleProvider : RoleProvider
    {
        public override string ApplicationName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

        public override string[] GetRolesForUser(string username)
        {
            var userService = DependenciesResolver.Kernel.GetService<IUserService>();
            User user = userService.GetByLogin(username);
            List<string> roles = new List<string>();
            foreach(var role in user.Roles)
            {
                roles.Add(role == RoleUser.Admin? "admin" : "user");
            }
            return roles.ToArray();

        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            var userService = DependenciesResolver.Kernel.GetService<IUserService>();
            User user = userService.GetByLogin(username);
            foreach(var role in user.Roles)
            {
                string currentRole = role == RoleUser.Admin ? "admin" : "user";
                if (currentRole == roleName)
                {
                    return true;
                }
            }
            return false;
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }
    }
}