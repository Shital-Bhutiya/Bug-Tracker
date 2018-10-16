using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BugTracker.Models;
using BugTracker.Models.Helpers;
using BugTracker.Models.Classes;
using Microsoft.AspNet.Identity;
using PagedList;
using static BugTracker.Models.Helpers.DocumentUploder;
using System.IO;

namespace BugTracker.Controllers
{
    [Authorize]
    public class TicketsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Tickets
        [Authorize(Roles = "Admin,Project Manager")]
        public ActionResult AssignDevelopers(int ticketId)
        {
            var model = new TicketAssignViewModel();
            var ticket = db.Tickets.FirstOrDefault(p => p.Id == ticketId);
            var userRoleHelper = new UserRoleHelper();
            var users = userRoleHelper.UsersInRole("Developer");
            model.Id = ticketId;
            model.UserList = new SelectList(users, "Id", "Name");
            return View(model);
        }
        [HttpPost]
        [Authorize(Roles = "Admin,Project Manager")]
        public ActionResult AssignDevelopers(TicketAssignViewModel model)
        {
            var ticket = db.Tickets.FirstOrDefault(p => p.Id == model.Id);
            ticket.AssignId = model.SelectedUser;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult Index(string id)
        {
            if (!string.IsNullOrWhiteSpace(id))
            {
                var userId = User.Identity.GetUserId();
                var userRoleHelper = new UserRoleHelper();
                var role = userRoleHelper.GetUserRoles(userId);
                ViewBag.User = "User";

                if (role.Contains("Developer"))
                {
                    if (id == "myTicket")
                    {
                        return View(db.Tickets.Include(t => t.Comments).Include(t => t.TicketPriority).Include(t => t.TicketProject).Include(t => t.TicketStatus).Include(t => t.TicketType).Where(p => p.AssignId == userId).ToList());
                    }
                    else if (id == "myProjectsTicket")
                    {
                        var dbUSer = db.Users.FirstOrDefault(p => p.Id == userId);
                        var myProject = dbUSer.Project.Select(p => p.Id);
                        var ticket = db.Tickets.Where(p => myProject.Contains(p.TicketProjectId)).ToList();
                        return View(ticket);
                    }
                }
                else if (role.Contains("Project Manager"))
                {
                    var dbUSer = db.Users.FirstOrDefault(p => p.Id == userId);
                    var myProject = dbUSer.Project.Select(p => p.Id);
                    var ticket = db.Tickets.Where(p => myProject.Contains(p.TicketProjectId)).ToList();
                    return View(ticket);
                }
                else if (role.Contains("Submitter"))
                {
                    return View(db.Tickets.Include(t=>t.Comments).Include(t => t.TicketPriority).Include(t => t.TicketProject).Include(t => t.TicketStatus).Include(t => t.TicketType).Where(p => p.CreatorId == userId).ToList());
                }
            }
            ViewBag.User = "";
            return View(db.Tickets.Include(t => t.TicketPriority).Include(t => t.Comments).Include(t => t.TicketProject).Include(t => t.TicketStatus).Include(t => t.TicketType).ToList());
        }

        // GET: Tickets/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Tickets.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            return View(ticket);
        }

        // GET: Tickets/Create
        public ActionResult Create()
        {
            ViewBag.TicketProjectId = new SelectList(db.Projects, "Id", "Name");
            ViewBag.TicketPriorityId = new SelectList(db.Prorities, "Id", "Name");
            ViewBag.TicketStatusId = new SelectList(db.Statues, "Id", "Name");
            ViewBag.TicketTypeId = new SelectList(db.Types, "Id", "Name");
            return View();
        }

        // POST: Tickets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Description,AssignId,TicketTypeId,TicketProjectId,TicketPriorityId,TicketStatusId")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                ticket.TicketStatusId = 1;
                ticket.CreatorId = User.Identity.GetUserId();
                db.Tickets.Add(ticket);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.TicketPriorityId = new SelectList(db.Prorities, "Id", "Name", ticket.TicketPriorityId);
            ViewBag.TicketStatusId = new SelectList(db.Statues, "Id", "Name", ticket.TicketStatusId);
            ViewBag.TicketTypeId = new SelectList(db.Types, "Id", "Name", ticket.TicketTypeId);
            return View(ticket);
        }

        // GET: Tickets/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Tickets.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            ViewBag.TicketProjectId = new SelectList(db.Projects, "Id", "Name", ticket.TicketProjectId);
            ViewBag.TicketPriorityId = new SelectList(db.Prorities, "Id", "Name", ticket.TicketPriorityId);
            ViewBag.TicketStatusId = new SelectList(db.Statues, "Id", "Name", ticket.TicketStatusId);
            ViewBag.TicketTypeId = new SelectList(db.Types, "Id", "Name", ticket.TicketTypeId);
            return View(ticket);
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Description,AssignId,TicketTypeId,TicketProjectId,TicketPriorityId,TicketStatusId")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                var dbTicket = db.Tickets.FirstOrDefault(p => p.Id == ticket.Id);
                dbTicket.Title = ticket.Title;
                dbTicket.Description = ticket.Description;
                dbTicket.AssignId = ticket.AssignId;
                dbTicket.Updated = DateTime.Now;
                dbTicket.TicketPriorityId = ticket.TicketPriorityId;
                dbTicket.TicketProjectId = ticket.TicketProjectId;
                dbTicket.TicketStatusId = ticket.TicketStatusId;
                dbTicket.TicketTypeId = ticket.TicketTypeId;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(ticket);
        }

        // GET: Tickets/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Tickets.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            return View(ticket);
        }

        // POST: Tickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Ticket ticket = db.Tickets.Find(id);
            db.Tickets.Remove(ticket);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpPost]
        [Authorize]
        public ActionResult CreateComment(int id, string body)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var ticket = db.Tickets
               .Where(p => p.Id == id)
               .FirstOrDefault();
            if (ticket == null)
            {
                return HttpNotFound();
            }
            if (string.IsNullOrWhiteSpace(body))
            {
                TempData["ErrorMessage"] = "Comment is required";
                return RedirectToAction("Details", new { ticket.Id });
            }
            var comment = new Comment();
            comment.UserId = User.Identity.GetUserId();
            comment.TicketId = ticket.Id;
            comment.Created = DateTime.Now;
            comment.CommentDescription = body;
            db.Comments.Add(comment);
            db.SaveChanges();
            return RedirectToAction("Details", new { id });
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UploadDocument(int id ,[Bind(Include = "Id,Description,FilePath")]Attechments attechments, HttpPostedFileBase document)
        {
            if (ModelState.IsValid)
            {
                if (DocumentUploder.IsWebFriendlyImage(document))
                {
                    var fileName = Path.GetFileName(document.FileName);
                    document.SaveAs(Path.Combine(Server.MapPath("~/Uploads/"), fileName));
                    attechments.FilePath = "/Uploads/" + fileName;
                    attechments.TicketId = attechments.Id;
                    attechments.UserId = User.Identity.GetUserId();
                    attechments.Created = DateTime.Now;
                    db.Attechments.Add(attechments);
                }

                db.SaveChanges();
                return RedirectToAction("Details", new { id });
            }

            return View();
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
