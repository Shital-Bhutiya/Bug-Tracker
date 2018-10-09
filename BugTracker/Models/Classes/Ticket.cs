using System;
using System.Web.Mvc;

namespace BugTracker.Models.Classes
{
    public class Ticket
    {
        public int Id  { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset Updated { get; set; }
        public Ticket()
        {
            Created = DateTime.Now;
        }
    
    }
}