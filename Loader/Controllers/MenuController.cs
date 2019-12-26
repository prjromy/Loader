using System.Linq;
using System.Web.Mvc;
using Loader.Models;
using System;
using System.Web;

namespace Loader.Controllers
{
    [MyAuthorize]
    public class MenuController : Controller
    {

        private Service.MenuService menuService = null;

        public MenuController()
        {
            menuService = new Service.MenuService();
        }


        public ActionResult Index()
        {
            if (Request.IsAjaxRequest())
            {
                ViewModel.TreeView tree = menuService.GetMenuGroupTree();
                ViewBag.treeview = tree;
                ViewBag.Address = menuService.GetAddress(0);
                ViewBag.ViewType = 1;
                ViewBag.ParentMenuId = 0;
                ViewData["param"] = new Loader.ViewModel.TreeViewParam(false, true, true, 0, 0, "Available Menu Group");

                return View(menuService.GetAllOfParent(0).ToList());
            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult _Details(int parentId, bool allowSelectGroup, int ViewType = 1)
        {
            ViewBag.Address = menuService.GetAddress(parentId);
            ViewBag.ParentMenuId = parentId;
            ViewBag.ViewType = ViewType;
       
            return PartialView(menuService.GetAllOfParent(parentId).ToList());
        }

        public ActionResult Details(int id = 0)
        {
            Menu menu = menuService.GetSingle(id);
            if (menu == null)
            {
                return HttpNotFound();
            }
            return View(menu);
        }

        [HttpGet]
        public ActionResult Create(string activeText, int activeId, int menuId = 0)
        {
            if (Request.IsAjaxRequest())
            {
                Menu menuDTO = new Menu();
                if (menuId != 0)
                {

                    menuDTO = new Loader.Repository.GenericUnitOfWork().Repository<Menu>().GetSingle(x => x.MenuId == menuId);
                    if (menuDTO.Image != null)
                    {
                        ViewBag.Image = Convert.ToBase64String(menuDTO.Image, Base64FormattingOptions.None);
                    }
                }
                else
                {
                    menuDTO.PMenuId = activeId;
                }

                ViewBag.ActiveText = activeText;
                ViewData["param"] = new Loader.ViewModel.TreeViewParam(false, true, false, 0, 0, "Available Menu Group");
                return PartialView(menuDTO);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }


        [HttpPost]
        public ActionResult Create(Menu menu, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {

                if (file != null)
                {
                    using (var reader = new System.IO.BinaryReader(file.InputStream))
                    {
                        menu.Image = reader.ReadBytes(file.ContentLength);
                    }
                }
                try
                {
                    if (menu.MenuId == 0)
                    {
                        menuService.Save(menu);
                        return RedirectToAction("Create", new { activeText = "Root", activeId = 0 });
                    }
                    else
                    {
                        menuService.Save(menu);
                        var parentNode = new Loader.Repository.GenericUnitOfWork().Repository<Menu>().GetSingle(x => x.MenuId == menu.PMenuId);
                        if (parentNode == null)
                        {
                            return RedirectToAction("Create", new { activeText = "Root", activeId = 1 });
                        }
                        else
                        {
                            return RedirectToAction("Create", new { activeText = parentNode.MenuCaption, activeId = parentNode.PMenuId });
                        }
                    }


                }
                catch (Exception ex)
                {
                    return JavaScript(ex.Message);
                }


            }
            return View(menu);

        }

        [HttpGet]
        public ActionResult _UpdateMenuTree(int selectedNode, bool allowSelectGroupNode, bool withImageIcon, bool withCheckBox, int withOutMe = 0, int rootNode = -1)
        {
            ViewData["AllowSelectGroup"] = allowSelectGroupNode;
            ViewData["WithImageIcon"] = withImageIcon;
            ViewData["WithCheckBox"] = withCheckBox;
            ViewData["param"] = new Loader.ViewModel.TreeViewParam(false, true, true, 0, selectedNode, "Available Menu Group");

            ViewModel.TreeView tree = new ViewModel.TreeView();
            if (rootNode < 0)
            {
                tree = menuService.GetMenuGroupTree(withOutMe);

            }
            else
            {
                tree = menuService.GetMenuGroupTree(rootNode, withOutMe);
            }
            return PartialView("_TreeViewBody", tree);
        }

        public ActionResult _CreateAction(int id)
        {
            var menuActions = new Loader.Repository.GenericUnitOfWork().Repository<Menu>().GetSingle(x => x.MenuId == id);
            return PartialView(menuActions);
        }


        [HttpPost]
        public ActionResult _GetMenuTreePopup(Loader.ViewModel.TreeViewParam param)
        {

            ViewData["param"] = param;

            var filter = param.Filter == null ? "" : param.Filter;

            ViewModel.TreeView tree = new ViewModel.TreeView();

            if (param.WithOutMe == 0)
            {
                tree = menuService.GetMenuGroupTree(filter);
            }
            else
            {
                tree = menuService.GetMenuGroupTree(param.WithOutMe, filter);
            }
            return PartialView("_TreeViewPopup", tree);

        }

        [HttpPost]
        public ActionResult _GetMenuTree(Loader.ViewModel.TreeViewParam param)
        {
            ViewData["param"] = param;
            var filter = param.Filter == null ? "" : param.Filter;

            ViewModel.TreeView tree = new ViewModel.TreeView();

            if (param.WithOutMe == 0)
            {
                tree = menuService.GetMenuGroupTree(filter);
            }
            else
            {
                tree = menuService.GetMenuGroupTree(param.WithOutMe, filter);
            }
            return PartialView("_TreeViewBody", tree);
        }



        [HttpPost]
        public ActionResult _GetLayoutSideBarTree(Loader.ViewModel.TreeViewParam param)
        {
            ViewData["param"] = param;
            var filter = param.Filter == null ? "" : param.Filter;

            ViewModel.LayoutTreeView tree = new ViewModel.LayoutTreeView();

            tree = menuService.GetLayoutGroupTree(filter);


            return PartialView("_GetLayoutFillterTree", tree);
        }
        public ActionResult Edit(int id = 0)
        {
            Menu menu = menuService.GetSingle(id);
            if (menu == null)
            {
                return HttpNotFound();
            }
            return View(menu);
        }


        [HttpPost]
        public ActionResult Edit(Menu menu)
        {
            if (ModelState.IsValid)
            {
                menuService.Save(menu);

                return RedirectToAction("Index");
            }
            return View(menu);
        }

        [HttpGet]
        public JsonResult Delete(int id = 0)
        {
            Menu menu = new Loader.Repository.GenericUnitOfWork().Repository<Menu>().GetSingle(x => x.PMenuId == id);
            bool deleteConfirm = false;
            if (menu == null)
            {
                deleteConfirm = true;
            }
            return Json(deleteConfirm, JsonRequestBehavior.AllowGet);
            //return View(menu);
        }
        [HttpPost]
        public ActionResult DeleteConfirm(int menuId)
        {
            menuService.Delete(menuId);
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult DisplayImage(HttpPostedFileBase imagefile)
        {

            using (var reader = new System.IO.BinaryReader(imagefile.InputStream))
            {
                byte[] ContentImage = reader.ReadBytes(imagefile.ContentLength);
                var ImageContent = Convert.ToBase64String(ContentImage, Base64FormattingOptions.None);
                return Json(ImageContent, JsonRequestBehavior.AllowGet);
            }


        }
        public ActionResult GetLayoutMenu()
        {
            Service.LayoutMenuMainService layoutmain = new Service.LayoutMenuMainService();
            return PartialView("_Layout-Sidebar", layoutmain.GetLayoutMenuGroupTree());

        }

    }
}
