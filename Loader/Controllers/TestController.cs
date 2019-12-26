using Loader.Hubs;
using Loader.Models;
using Loader.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Loader.Controllers
{
    public class TestController : Controller
    {

       
        // GET: Test
        public ActionResult Index()
        {
            //         var ser = uow.Repository<Menu>().GetAll(); 
            //uow.Repository<Menu>().FindBy(x => x.Address == "A").ToList();
       
            
            return View();
        }
        public void SendNotification()
        {
            NotificationHub objNotify = new NotificationHub();
            objNotify.SendNotification(3045);
        }
    }
}