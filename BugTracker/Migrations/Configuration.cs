namespace BugTracker.Migrations
{
    using BugTracker.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<BugTracker.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(BugTracker.Models.ApplicationDbContext context)
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            if (!context.Roles.Any(r => r.Name == "Admin"))
            {
                roleManager.Create(new IdentityRole { Name = "Admin" });
            }
            if (!context.Roles.Any(r => r.Name == "Project Manager"))
            {
                roleManager.Create(new IdentityRole { Name = "Project Manager" });
            }
            if (!context.Roles.Any(r => r.Name == "Developer"))
            {
                roleManager.Create(new IdentityRole { Name = "Developer" });
            }
            if (!context.Roles.Any(r => r.Name == "Submitter"))
            {
                roleManager.Create(new IdentityRole { Name = "Submitter" });
            }

            ApplicationUser adminUser = null;
            if (!context.Users.Any(p => p.UserName == "admin@mybugtracker.com"))
            {
                adminUser = new ApplicationUser();
                adminUser.UserName = "admin@mybugtracker.com";
                adminUser.Email = "admin@mybugtracker.com";
                adminUser.Name = "Admin";
                adminUser.LastName = "User";
                adminUser.DisplayName = "Admin User";
                userManager.Create(adminUser, "Password-1");
            }
            else
            {
                adminUser = context.Users.Where(p => p.UserName == "admin@mybugtracker.com")
                    .FirstOrDefault();
            }
            if (!userManager.IsInRole(adminUser.Id, "Admin"))
            {
                userManager.AddToRole(adminUser.Id, "Admin");
            }
            context.Types.AddOrUpdate(x => x.Id,
               new Models.Classes.Type(){Id = 1,Name = "Bug Fixes"},
               new Models.Classes.Type(){Id = 2,Name = "Software Update"},
               new Models.Classes.Type(){Id = 3,Name = "Adding Helpers"},
               new Models.Classes.Type(){Id = 4,Name = "Database Error"}
            );
            context.Prorities.AddOrUpdate(x => x.Id,
              new Models.Classes.Priority() { Id = 1, Name = "High" },
              new Models.Classes.Priority() { Id = 2, Name = "Low" },
              new Models.Classes.Priority() { Id = 3, Name = "Medium" },
              new Models.Classes.Priority() { Id = 4, Name = "Urgent" }
           );
            context.Statues.AddOrUpdate(x => x.Id,
              new Models.Classes.Status() { Id = 1, Name = "Not Started" },
              new Models.Classes.Status() { Id = 2, Name = "Finished" },
              new Models.Classes.Status() { Id = 3, Name = "On Hold" },
              new Models.Classes.Status() { Id = 4, Name = "In Progress" }
           );
        }
    }
}
