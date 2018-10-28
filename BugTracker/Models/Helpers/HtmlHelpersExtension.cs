using System;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
namespace BugTracker.Models.Helpers
{
    public static class HtmlHelpersExtensions
    {
        
        public static IHtmlString DisplayDate(this HtmlHelper helper, DateTimeOffset? date)
        {
            var returnData = "";
            if (!date.HasValue)
            {
                returnData = "-";
            }
            else
            {
                returnData = date.Value.ToString("yyyy-MM-dd HH:mm");
            }
            return new HtmlString(returnData);
        }
        public static IHtmlString DisplayLinks(this HtmlHelper helper, string Userstr,int itemId)
        {
            var urlHelper = new UrlHelper(helper.ViewContext.RequestContext);
            var userId = HttpContext.Current.User.Identity.GetUserId();
            var userRoleHelper = new UserRoleHelper();
            var Roles = userRoleHelper.GetUserRoles(userId);
            var Detailsurl = urlHelper.Action("Details", "Tickets", new { id = itemId });
            var EditUrl = urlHelper.Action("Edit", "Tickets", new { id = itemId });
            if (Roles.Contains("Project Manager"))
            {
                var result = "<td><a href=\"" + Detailsurl + "\">Details</a></td><td><a href=\"" + EditUrl + "\">Edit</a></td>";
                return new HtmlString(result);
            }
            else if (Roles.Contains("Developer"))
            {
                if(Userstr == "myTicket")
                {
                    var result = "<td><a href=\"" + Detailsurl + "\">Details</a></td><td><a href=\"" + EditUrl + "\">Edit</a></td>";
                    return new HtmlString(result);
                }
            }
            else if (Roles.Contains("Submitter"))
            {
                var result = "<td><a href=\"" + Detailsurl + "\">Details</a></td><td><a href=\"" + EditUrl + "\">Edit</a></td>";
                return new HtmlString(result);
            }

            return new HtmlString("");
        }
    }
}
