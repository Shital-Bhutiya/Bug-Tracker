using BugTracker.Hubs;
using BugTracker.Models.Classes;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Configuration;

namespace BugTracker.Models.Helpers
{
    public class EmailSendingExtensions
    {
      public static ApplicationDbContext db { get; set; }
        public static void SendNotification(Ticket ticket, string Details)
        {
            db = new ApplicationDbContext();
            var user = db.Users.FirstOrDefault(p => p.Id == ticket.AssignId);
            var personalEmailService = new PersonalEmailService();
            var mailMessage = new MailMessage(WebConfigurationManager.AppSettings["emailto"], user.Email);
            mailMessage.IsBodyHtml = true;
           
            switch (Details)
            {
                case "Comment":
                    mailMessage.Body = "Your '"+ticket.TicketProject.Name+"' ticket Has a new comment added by other user";
                    mailMessage.Subject = "Tickets Comment";
                    break;
                case "Attechment":
                    mailMessage.Body = "Your '" + ticket.TicketProject.Name + "' Has a new Attechments added by other user";
                    mailMessage.Subject = "Tickets Attechments";
                    break;
                case "Assign":
                    mailMessage.Body = "You are assign to a new ticket '"+ ticket.TicketProject.Name+"'";
                    mailMessage.Subject = "Tickets Assign";
                    break;
                case "Edit":
                    mailMessage.Body = "There is a new update on your '" + ticket.TicketProject.Name + "' ticket";
                    mailMessage.Subject = "Tickets update";
                    break;
            }
            personalEmailService.Send(mailMessage);
            TrackerHub.SendNotificationToUser(ticket.Assign.UserName, mailMessage.Body);
        }
        
    }
}