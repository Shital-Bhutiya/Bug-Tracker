﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.Models.Classes
{
    public class Attechments
    {
        public int Id { get; set; }
        public string FilePath { get; set; }
        public string Description { get; set; }
        public DateTimeOffset Created { get; set; }
        public string FileUrl { get; set; }

        public int UserId { get; set; }
        public ApplicationUser User { get; set; }

        public int TicketId { get; set; }
        public Ticket Ticket { get; set; }
    }
}