using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;
using Loader.Repository;
using System.ComponentModel.DataAnnotations.Schema;
using Loader.ViewModel;
using Loader.Models;
using Loader.Service;

namespace Loader.Hubs
{
    public class UserContext
    {
        public List<UserConnection> Users { get; set; }
    }
    [Table("UserConnection", Schema = "LG")]
    public class UserConnection
    {
        [Key,DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int UserId { get; set; }
        public string ConnectionID { get; set; }
    }
    public class NotificationHub : Hub
    {
        private GenericUnitOfWork uow = null;
        public NotificationHub()
        {
            uow = new GenericUnitOfWork();
        }
        public void SendNotification(int TaskID)
        {
            UserContext objUser = new UserContext();
            var context = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
            var userList=uow.Repository<NotificationModel>().SqlQuery("select * from LG.FGetConnectionIDFromTask("+TaskID+")").ToList();
            if (userList.Count()>0)
            {
                foreach (var item in userList)
                {
                    if (item.ConnectionID!="")
                    {
                        context.Clients.Client(item.ConnectionID).SendNotification(item.CountNotification);
                    }
                }
            }
           
            //using (var db = new UserContext())
            //{

            //    var receiverlist = db.Users.Select(s=>s.ConnectionID).Where();
            //    if (user == null)
            //    {
            //        Clients.Caller.showErrorMessage("Could not find that user.");
            //    }
            //    else
            //    {
            //        UserConnection objusers = new UserConnection();
            //        var conlist = db.Users.Select(s => user.UserId ).ToList(); 

            //        if (conlist.Count() == 0)
            //        {
            //            Clients.Caller.showErrorMessage("The user is no longer connected.");
            //        }
            //        else
            //        {
            //            foreach (var connection in conlist)
            //            {
            //                Clients.Client(connection)
            //                    .addChatMessage(name + ": " + message);
            //            }
            //        }
            //    }
            //}
        }

        public override Task OnConnected()
        {
            
            var name = Context.User.Identity.Name;
            var ConnectionID = Context.ConnectionId;
            UsersService objUser = new UsersService();
            int status=objUser.ConnectUser(ConnectionID, name);
            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            return base.OnDisconnected(stopCalled);
        }
    }
}

