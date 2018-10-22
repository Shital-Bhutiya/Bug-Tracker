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
        public static void SendNotification(string UserId, string Details)
        {
            db = new ApplicationDbContext();
            var user = db.Users.FirstOrDefault(p => p.Id == UserId);
            var personalEmailService = new PersonalEmailService();
            var mailMessage = new MailMessage(WebConfigurationManager.AppSettings["emailto"], user.Email);
            mailMessage.IsBodyHtml = true;
           
            switch (Details)
            {
                case "Comment":
                    mailMessage.Body = "Your ticket has one new comment added";
                    mailMessage.Subject = "Tickets Comment";
                    break;
                case "Attechment":
                    mailMessage.Body = "Your ticket has one new comment added";
                    mailMessage.Subject = "Tickets Attechments";
                    break;
                case "Assign":
                    mailMessage.Body = "You are assign to a new ticket";
                    mailMessage.Subject = "Tickets Assign";
                    break;
                case "Edit":
                    mailMessage.Body = "There is a new update on your ticket";
                    mailMessage.Subject = "Tickets update";
                    break;
            }
            personalEmailService.Send(mailMessage);
        }
        
    }
}