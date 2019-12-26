using Autofac;
using Autofac.Integration.Mvc;
using Loader.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Loader
{
    public class MvcApplication : System.Web.HttpApplication
    {
        GenericUnitOfWork uow = new GenericUnitOfWork();
        public static int GUserId { get; set; }
        public static int GBranchId { get; set; }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);




        }



        void Session_OnEnd(object sender, EventArgs e)
        {
            int userId = GUserId;
            int branchId = GBranchId;

            Models.LoginLogs sessionObj = uow.Repository<Models.LoginLogs>().FindBy(x => x.UserId == userId && x.BranchId == branchId).LastOrDefault();

            if (sessionObj != null)
            {

                sessionObj.To = DateTime.Now;

                uow.Repository<Models.LoginLogs>().Edit(sessionObj);
                uow.Commit();
            }

        }
    }
}
