using BugTracker.Models;
using BugTracker.Models.Helpers;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BugTracker.Controllers
{
    [Authorize]
    public class ApplicationUsersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: ApplicationUsers
        public ActionResult Index()
        {
            return View(db.Users.ToList());
        }
        public ActionResult ChangeRole(string id)
        {
            var model = new UserRoleViewModel();
            var userRoleHelper = new UserRoleHelper();

            model.Id = id;
            model.Name = User.Identity.Name;
            var roles = userRoleHelper.GetAllRoles();
            var userRoles = userRoleHelper.GetUserRoles(id);
            model.Roles = new MultiSelectList(roles, "Name", "Name", userRoles);
            return View(model);
        }
        [HttpPost]
        public ActionResult ChangeRole(UserRoleViewModel model)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            //STEP 1: Find the user
            var user = userManager.FindById(model.Id);
            //STEP 2: Get UserRoles:
            var userRoles = userManager.GetRoles(user.Id);
            //STEP 3: Remove the roles from the user
            foreach (var role in userRoles)
            {
                userManager.RemoveFromRole(user.Id, role);
            }
            //STEP 4: Add roles to the user
            foreach (var role in model.SelectedRoles)
            {
                userManager.AddToRole(user.Id, role);
            }
            var signInManager = HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            signInManager.SignIn(user, isPersistent: false, rememberBrowser: false);
            return RedirectToAction("Index");
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}