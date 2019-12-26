using Loader.Hubs;
using Loader.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Loader.Controllers
{
    
    [MyAuthorize]
    
    //[CheckSessionOut]
    public class HomeController : Controller
    {
        Loader.Service.UserVSBranchService _usrVSBrnchService = new Service.UserVSBranchService();
        Loader.Service.EmployeeService _TDate = new Service.EmployeeService();

        public ActionResult Index()
        {
            var branchId = Loader.Models.Global.BranchId;
            //Loader.Models.Global.nu = "acd";
            if (branchId==0)
            {
                UserBranchViewModel allRoles = _usrVSBrnchService.HasAnotherRole(Loader.Models.Global.UserId);
                if (allRoles.Branch.Count() > 0)
                {
                    return RedirectToAction("BranchSelect", "Account");
                }

            }
            return View();
            //}
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

    }
}