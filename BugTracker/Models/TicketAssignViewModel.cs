using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BugTracker.Models
{
    public class TicketAssignViewModel
    {
        public int Id { get; set; }
        public string UserNameId { get; set; }
        public SelectList UserList { get; set; }
        public string SelectedUser { get; set; }
    }
}