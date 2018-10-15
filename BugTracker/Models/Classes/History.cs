using System;

namespace BugTracker.Models.Classes
{
    public class History
    {
        public int Id { get; set; }

        public int TicketHistoryId { get; set; }
        public virtual Ticket TicketHistory { get; set; }

        public string HistoryUserId { get; set; }
        public virtual ApplicationUser HistoryUser { get; set; }

        public string Property { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public DateTimeOffset ChangedDate { get; set; }
    }
}