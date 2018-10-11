using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using BugTracker.Models.Classes;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace BugTracker.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public ICollection<Project> Project { get; set; }
        [InverseProperty("Creator")]
        public ICollection<Ticket> CreatorTickets { get; set; }
        [InverseProperty("Assign")]
        public ICollection<Ticket> AssigneeTickets { get; set; }

        public string Name { get;  set; }
        public string LastName { get; set; }
        public string DisplayName { get; set; }
        public ApplicationUser()
        {
            Project = new HashSet<Project>();
            
        }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            userIdentity.AddClaim(new Claim("DisplayName", DisplayName));
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public DbSet<Project> Projects { get; set; }

        public DbSet<Ticket> Tickets { get; set; }

        public DbSet<Type> Types { get; set; }

        public DbSet<Status> Statues { get; set; }

        public DbSet<Priority> Prorities { get; set; }

        public DbSet<Comment> Comments { get; set; }
        public DbSet<Attechments> Attechments { get; set; }
    }
}