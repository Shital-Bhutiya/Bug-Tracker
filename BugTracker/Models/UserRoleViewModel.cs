using System.Web.Mvc;
namespace BugTracker.Models
{
    public class UserRoleViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public MultiSelectList Roles { get; set; }
        public string[] SelectedRoles { get; set; }
    }
}