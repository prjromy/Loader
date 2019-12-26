using System.Linq;
using System.Web.Mvc;
using Loader.Models;
using System;
using System.Web;
using System.Collections.Generic;
using Loader.ViewModel;
using Loader.Service;

namespace Loader.Controllers
{
    [MyAuthorize]
    public class MenuTemplateController : Controller
    {

        private Service.MenuTemplateService MenuTemplateService = null;

        private ReturnBaseMessageModel returnMessage = null;

        public MenuTemplateController()
        {
            MenuTemplateService = new Service.MenuTemplateService();
        }


        public ActionResult Index()
        {
            if (Request.IsAjaxRequest() == true)
            {

                ViewModel.TreeView tree = MenuTemplateService.GetMenuTemplateGroupTree(null);
                ViewBag.treeview = tree;
                ViewBag.AllowInGroup = MenuTemplateService.AllowSelectDesignationGroup();
                ViewBag.Designation = 1;
                if (MenuTemplateService.WithDesignation() == "false")
                {
                    ViewBag.Designation = 0;
                }
                //ViewBag.Address = MenuTemplateService.GetAddress(0);
                ViewBag.ViewType = 1;
                ViewBag.ParentMenuTemplateId = 0;
                ViewData["param"] = new Loader.ViewModel.TreeViewParam(true, true, true, 0, 0, "Available MenuTemplate Group");
                //var check = MenuTemplateService.GetAll().Where(x => x.MTId != 1).ToList();
                if(Loader.Models.Global.IsSuperAdmin)
                    return View(MenuTemplateService.GetAll().ToList());
                else
                    return View(MenuTemplateService.GetAll().Where(x=>x.MTId!=1).ToList());

            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult _Details(bool allowSelectGroup, int ViewType = 1)
        {
            // var templateAssignedUsers = MenuTemplateService.TemplateAssignedUsers(int mtId);
            //  ViewBag.templateAssignedUsers = MenuTemplateService.TemplateAssignedUsers(mtId);
            //ViewBag.Address = MenuTemplateService.GetAddress(parentId);
            ViewBag.ViewType = ViewType;
            return PartialView(MenuTemplateService.GetAll().ToList());
        }

        public ActionResult GetUserAssignedToTemplete(int Mtid)
        {
            try
            {
                MenuTemplateService menuTemplateService = new MenuTemplateService();
                var result = menuTemplateService.TemplateAssignedUsers(Mtid);
                return PartialView("GetUserAssignedToTemplete", result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        //public ActionResult Details(int id = 0)
        //{
        //    MenuTemplate MenuTemplate = MenuTemplateService.GetSingle(id);
        //    if (MenuTemplate == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(MenuTemplate);
        //}

        [HttpGet]
        public ActionResult Create(int MenuTemplateId = 0)
        {
            if (MenuTemplateId != 0)
            {
                ViewModel.TreeView tree = MenuTemplateService.GetMenuTemplateGroupTree(MenuTemplateId);
                ViewBag.treeview = tree;
            }
            else
            {
                ViewModel.TreeView tree = MenuTemplateService.GetMenuTemplateGroupTree(null);
                ViewBag.treeview = tree;
            }

            ViewBag.AllowInGroup = MenuTemplateService.AllowSelectDesignationGroup();
            ViewBag.WithDesignation = 1;

            if (MenuTemplateService.WithDesignation() == "false")
            {
                ViewBag.WithDesignation = 0;
            }
            if (Request.IsAjaxRequest())
            {
                MenuTemplate MenuTemplateDTO = new MenuTemplate();
                if (MenuTemplateId != 0)
                {

                    MenuTemplateDTO = new Loader.Repository.GenericUnitOfWork().Repository<MenuTemplate>().GetSingle(x => x.MTId == MenuTemplateId);

                }
                else
                {
                    //MenuTemplateDTO.DGId = activeId;
                }
                if (MenuTemplateDTO != null)
                {
                    ViewBag.ActiveText = new Loader.Repository.GenericUnitOfWork().Repository<Designation>().FindBy(x=>x.DGId== MenuTemplateDTO.DesignationId).Select(x => x.DGName).SingleOrDefault();
                }
                else
                {
                    return RedirectToAction("Index", "MenuTemplate");
                }
                ViewData["param"] = new Loader.ViewModel.TreeViewParam(true, true, false, 0, 0, "Available MenuTemplate Group");

                return PartialView(MenuTemplateDTO);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }


        [HttpPost]
        public ActionResult Create(MenuTemplate TemplateData, TreeView data)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (TemplateData.MTId == 0)
                    {
                        
                        var returnmessage= MenuTemplateService.Save(TemplateData, data);
                        return Json(returnmessage, JsonRequestBehavior.AllowGet);
                        //return RedirectToAction("Create", new { MenuTemplateId = 0 });
                    }
                    else
                    {
                        var returnmessage = MenuTemplateService.Save(TemplateData, data);
                        //MenuTemplateService.Save(TemplateData, data);
                        //var parentNode = new Loader.Repository.GenericUnitOfWork().Repository<MenuTemplate>().GetSingle(x => x.MTId == TemplateData.MTId);
                        return Json(returnmessage, JsonRequestBehavior.AllowGet);
                        //return RedirectToAction("Create", new { MenuTemplateId = 0 });
                    }


                }
                catch (Exception ex)
                {
                    return JavaScript(ex.Message);
                }

            }
            return View();

        }

        //[HttpGet]
        //public ActionResult _UpdateMenuTemplateTree(int selectedNode, bool allowSelectGroupNode, bool withImageIcon, bool withCheckBox, int withOutMe = 0, int rootNode = -1)
        //{
        //    ViewData["AllowSelectGroup"] = allowSelectGroupNode;
        //    ViewData["WithImageIcon"] = withImageIcon;
        //    ViewData["WithCheckBox"] = withCheckBox;
        //    ViewData["param"] = new Loader.ViewModel.TreeViewParam(false, true, true, 0, selectedNode, "Available MenuTemplate Group");

        //    ViewModel.TreeView tree = new ViewModel.TreeView();
        //    if (rootNode < 0)
        //    {
        //        tree = MenuTemplateService.GetMenuTemplateGroupTree(withOutMe);

        //    }
        //    else
        //    {
        //        tree = MenuTemplateService.GetMenuTemplateGroupTree(rootNode, withOutMe);
        //    }
        //    return PartialView("_TreeViewBody", tree);
        //}

        //public ActionResult _CreateAction()
        //{
        //    return PartialView();
        //}
        [HttpPost]
        public ActionResult _GetDesignationTreePopup(Loader.ViewModel.TreeViewParam param)
        {

            Service.DesignationService DesignationService = new Service.DesignationService();
            param.AllowSelectGroup = true;
            ViewData["param"] = param;

            var filter = param.Filter == null ? "" : param.Filter;

            ViewModel.TreeView tree = new ViewModel.TreeView();

            if (param.WithOutMe == 0)
            {
                tree = DesignationService.GetDesignationGroupTree(filter);
            }
            else
            {
                tree = DesignationService.GetDesignationGroupTree(param.WithOutMe, filter);
            }
            return PartialView("_TreeViewPopup", tree);

        }

        [HttpPost]
        public ActionResult _GetMenuTemplateTreePopup(Loader.ViewModel.TreeViewParam param)
        {

            ViewData["param"] = param;

            var filter = param.Filter == null ? "" : param.Filter;

            ViewModel.TreeView tree = new ViewModel.TreeView();

            if (param.WithOutMe == 0)
            {
                tree = MenuTemplateService.GetMenuTemplateGroupTree(null, filter);
            }
            else
            {
                // tree = MenuTemplateService.GetMenuTemplateGroupTree(param.WithOutMe, filter);
            }
            return PartialView("_TreeViewPopup", tree);

        }

        [HttpPost]
        public ActionResult _GetMenuTemplateTree(Loader.ViewModel.TreeViewParam param)
        {
            ViewData["param"] = param;
            var filter = param.Filter == null ? "" : param.Filter;
            param.WithCheckBox = true;
            ViewModel.TreeView tree = new ViewModel.TreeView();

            if (param.WithOutMe == 0)
            {
                tree = MenuTemplateService.GetMenuTemplateGroupTree(null, filter);
            }
            else
            {
                tree = MenuTemplateService.GetMenuTemplateGroupTree(param.WithOutMe, filter);
            }
            return PartialView("_TreeViewBody", tree);
        }
        [HttpPost]
        public ActionResult _GetDesignationTree(Loader.ViewModel.TreeViewParam param)
        {
            Service.DesignationService DesignationService = new Service.DesignationService();
            ViewData["param"] = param;
            var filter = param.Filter == null ? "" : param.Filter;

            ViewModel.TreeView tree = new ViewModel.TreeView();

            if (param.WithOutMe == 0)
            {
                tree = DesignationService.GetDesignationGroupTree(filter);
            }
            else
            {
                tree = DesignationService.GetDesignationGroupTree(param.WithOutMe, filter);
            }
            return PartialView("_TreeViewBody", tree);
        }

        //public ActionResult Edit(int id = 0)
        //{
        //    MenuTemplate MenuTemplate = MenuTemplateService.GetSingle(id);
        //    if (MenuTemplate == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(MenuTemplate);
        //}


        //[HttpPost]
        //public ActionResult Edit(MenuTemplate MenuTemplate)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        MenuTemplateService.Save(MenuTemplate);

        //        return RedirectToAction("Index");
        //    }
        //    return View(MenuTemplate);
        //}

        [HttpGet]
        public JsonResult Delete(int id = 0)
        {
            MenuTemplate MenuTemplate = new Loader.Repository.GenericUnitOfWork().Repository<MenuTemplate>().GetSingle(x => x.MTId == id);
            bool deleteConfirm = false;
            var checkinUsers = new Loader.Repository.GenericUnitOfWork().Repository<ApplicationUser>().GetSingle(x => x.MTId == id);
            if (checkinUsers == null)
            {
                deleteConfirm = true;
            }

            return Json(deleteConfirm, JsonRequestBehavior.AllowGet);
            //return View(MenuTemplate);
        }
        [HttpPost]
        public ActionResult DeleteConfirm(int MenuTemplateId)
        {
            MenuTemplateService.Delete(MenuTemplateId);
            return RedirectToAction("Index");
        }
        //public ActionResult DisplayImage(HttpPostedFileBase file)
        //{
        //    using (var reader = new System.IO.BinaryReader(file.InputStream))
        //    {
        //        byte[] ContentImage = reader.ReadBytes(file.ContentLength);
        //        var ImageContent = Convert.ToBase64String(ContentImage, Base64FormattingOptions.None);
        //        return Json(ImageContent, JsonRequestBehavior.AllowGet);
        //    }


        //}

    }
}
