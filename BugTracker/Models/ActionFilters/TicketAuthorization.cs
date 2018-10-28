using System;
using System.Linq;
using System.Web.Mvc;
using BugTracker.Models.Helpers;
using Microsoft.AspNet.Identity;
namespace BugTracker.Models.ActionFilters
{
    public class TicketAuthorization : ActionFilterAttribute
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private UserRoleHelper userRoleHelper = new UserRoleHelper();
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var ticketId = Convert.ToInt32(filterContext.ActionParameters.FirstOrDefault(p => p.Key == "id").Value);
            var actionMethod = filterContext.ActionDescriptor.ActionName;
            var userId = filterContext.HttpContext.User.Identity.GetUserId();
            var ticket = db.Tickets.Where(i => i.Id == ticketId).FirstOrDefault();
            var Roles = userRoleHelper.GetUserRoles(userId);
            var url = new UrlHelper(filterContext.RequestContext);
            var passed = false;
            if (Roles.Contains("Project Manager"))
            {
                if (ticket.TicketProject.Users.Select(p => p.Id).Contains(userId))
                {
                    passed = true;
                }
            }
            if (Roles.Contains("Admin"))
            {
                passed = true;
            }
            else if (Roles.Contains("Developer"))
            {
                if (actionMethod == "Details")
                {
                    if (ticket.AssignId == userId || ticket.TicketProject.Users.Select(p => p.Id).Contains(userId))
                    {
                        passed = true;
                    }
                }
                else if (actionMethod == "Edit")
                {
                    if (ticket.AssignId==userId)
                    {
                        passed = true;
                    }
                }

            }
            else if (Roles.Contains("Submitter"))
            {
                if (ticket.CreatorId == userId)
                {
                    passed = true;
                }
            }

            if (!passed)
            {
                filterContext.Result = new RedirectResult(url.Action("Error_403", "Home"));
            }
        }
    }
}