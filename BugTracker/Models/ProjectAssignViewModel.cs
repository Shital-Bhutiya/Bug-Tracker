using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BugTracker.Models
{
    public class ProjectAssignViewModel
    {
        public int Id { get; set; }
        public MultiSelectList UserList { get; set; }
        public string[] SelectedUsers { get; set; }
    }
}