using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Loader.Controllers
{
    public class LayoutMenuController : Controller
    {
        private Service.LayoutMenuMainService LayoutMenuMainService;
        public LayoutMenuController()
        {
            LayoutMenuMainService=new Service.LayoutMenuMainService();
        }
        //GET: LayoutMenu
        //public ActionResult Index()
        //{
        //    //ViewModel.TreeView tree = LayoutMenuMainService.GetMenuGroupTree();
        //    //ViewBag.treeview = tree;
        //    //ViewBag.Address = menuService.GetAddress(0);
        //    //ViewBag.ViewType = 1;
        //    //ViewBag.ParentMenuId = 0;
        //    //ViewData["param"] = new Loader.ViewModel.TreeViewParam(false, true, true, 0, 0, "Available Menu Group");

        //    //return View(menuService.GetAllOfParent(0).ToList());
        //}

    }

}
