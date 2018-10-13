using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.Models.Helpers
{
    public class UserRoleHelper
    {
        private ApplicationDbContext Db;
        private UserManager<ApplicationUser> UserManager;
        private RoleManager<IdentityRole> RoleManager;
        public UserRoleHelper()
        {
            Db = new ApplicationDbContext();
            UserManager = new
            UserManager<ApplicationUser>(new UserStore<ApplicationUser>(Db));
            RoleManager = new
            RoleManager<IdentityRole>(new RoleStore<IdentityRole>(Db));
        }
        public List<IdentityRole> GetAllRoles()
        {
            return RoleManager.Roles.ToList();
        }
        public List<string> GetUserRoles(string id)
        {
            return UserManager.GetRoles(id).ToList();
        }
        public ICollection<ApplicationUser> UsersInRole(string role)
        {
            var roleId = Db.Roles.Where(p => p.Name == role).Select(p => p.Id).FirstOrDefault();

            return Db.Users.Where(p => p.Roles.Any(t => t.RoleId == roleId)).ToList();
        }
    }
}