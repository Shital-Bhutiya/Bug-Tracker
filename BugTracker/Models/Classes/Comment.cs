using BugTracker.Models.Classes;
using System;

namespace BugTracker.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string CommentDescription { get; set; }
        public DateTimeOffset Created { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public int TicketId { get; set; }
        public Ticket Ticket { get; set; }
    }
}