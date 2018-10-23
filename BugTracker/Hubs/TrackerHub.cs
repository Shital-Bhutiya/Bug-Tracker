using Microsoft.AspNet.Identity;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.Hubs
{
    public class TrackerHub: Hub
    {
        public void Send(string id, string message)
        {   
            Clients.All.addNewMessageToPage(id, message);
        }

        public static void SendNotificationToUser(string username, string message)
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<TrackerHub>();
            context.Clients.User(username).addNewMessageToPage(message);
        }
    }
}