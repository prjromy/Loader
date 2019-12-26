using System.Linq;
using System.Web.Mvc;
using Loader.Models;
using System;
using System.Web;

namespace Loader.Controllers
{
    [MyAuthorize]
    public class DesignationController : Controller
    {

        private Service.DesignationService DesignationService = null;

        public DesignationController()
        {
            DesignationService = new Service.DesignationService();
        }


        public ActionResult Index()
        {
            if (Request.IsAjaxRequest() == true)
            {
                ViewModel.TreeView tree = DesignationService.GetDesignationGroupTree();
                ViewBag.treeview = tree;
                ViewBag.Address = DesignationService.GetAddress(0);
                ViewBag.ViewType = 1;
                ViewBag.ParentDesignationId = 0;

                var withHeirarchy = DesignationService.WithHeirarchy();
                ViewBag.Heirarchy = 0;
                if (withHeirarchy == "true")
                {
                    ViewBag.Heirarchy = 1;
                }
                ViewData["param"] = new Loader.ViewModel.TreeViewParam(false, true, true, 0, 0, "Available Designation Group");

                return View(DesignationService.GetAllOfParent(0).ToList());
            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult _Details(int parentId, bool allowSelectGroup, int ViewType = 1)
        {
            ViewBag.Address = DesignationService.GetAddress(parentId);
            ViewBag.ParentDesignationId = parentId;
            ViewBag.ViewType = ViewType;
            ViewBag.Heirarchy = 0;
            var withHeirarchy = DesignationService.WithHeirarchy();
            if (withHeirarchy == "true")
            {
                ViewBag.Heirarchy = 1;
            }
            return PartialView(DesignationService.GetAllOfParent(parentId).ToList());
        }

        public ActionResult Details(int id = 0)
        {
            Designation Designation = DesignationService.GetSingle(id);
            if (Designation == null)
            {
                return HttpNotFound();
            }
            return View(Designation);
        }

        [HttpGet]
        public ActionResult Create(string activeText, int activeId, int DesignationId = 0, int DesignationParentid = 0)
        {
            if (Request.IsAjaxRequest())
            {
                Designation DesignationDTO = new Designation();

                var isEdit = false;
                var tData = Convert.ToBoolean(  TempData["isEdit"] );
                if (tData == true)
                {

                    isEdit = tData;
                }

                if (DesignationId != 0)
                {
                    DesignationDTO = new Loader.Repository.GenericUnitOfWork().Repository<Designation>().GetSingle(x => x.DGId == DesignationId);
                    TempData["isEdit"] = true;
                }
                else
                {
                    DesignationDTO.PDGId = activeId;
                    TempData["isEdit"] = isEdit;
                }
                ViewBag.parentNodeId = DesignationParentid;
                ViewBag.isEdit = 0;
                if (Convert.ToBoolean( TempData["isEdit"] )== true)
                {
                    ViewBag.isEdit = 1;
                }
                TempData.Keep();

                var withHeirarchy = DesignationService.WithHeirarchy();
                ViewBag.Heirarchy = 0;
                if (withHeirarchy == "true")
                {
                    ViewBag.Heirarchy = 1;
                }
                ViewBag.ActiveText = activeText;
                ViewData["param"] = new Loader.ViewModel.TreeViewParam(false, true, false, 0, 0, "Available Designation Group");
                return PartialView(DesignationDTO);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }


        [HttpPost]
        public ActionResult Create(Designation Designation, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {

                try
                {
                    if (Designation.DGId == 0)
                    {
                        DesignationService.Save(Designation);
                        return RedirectToAction("Create", new { activeText = "Root", activeId = 0 });
                    }
                    else
                    {
                        DesignationService.Save(Designation);
                        var parentNode = new Loader.Repository.GenericUnitOfWork().Repository<Designation>().GetSingle(x => x.DGId == Designation.PDGId);
                        TempData["isEdit"] = true;
                        if (parentNode == null)
                        {
                            return RedirectToAction("Create", new { activeText = "Root", activeId =0});
                        }
                        TempData["parentNodeId"] = parentNode.PDGId;

                        return RedirectToAction("Create", new { activeText = parentNode.DGName, activeId = parentNode.DGId, DesignationParentid = parentNode.PDGId });
                    }


                }
                catch (Exception ex)
                {
                    return JavaScript(ex.Message);
                }


            }
            return View(Designation);

        }

        [HttpGet]
        public ActionResult _UpdateDesignationTree(int selectedNode, bool allowSelectGroupNode, bool withImageIcon, bool withCheckBox,  int withOutMe = 0, int rootNode = -1)
        {
            //var isEdit = TempData["isEdit"];

            ViewData["AllowSelectGroup"] = allowSelectGroupNode;
            ViewData["WithImageIcon"] = withImageIcon;
            ViewData["WithCheckBox"] = withCheckBox;
            ViewData["param"] = new Loader.ViewModel.TreeViewParam(false, true, true, 0, selectedNode, "Available Designation Group");

            ViewModel.TreeView tree = new ViewModel.TreeView();
            if (rootNode < 0)
            {
                tree = DesignationService.GetDesignationGroupTree(withOutMe);

            }
            else
            {
                tree = DesignationService.GetDesignationGroupTree(rootNode, withOutMe);
            }
            return PartialView("_TreeViewBody", tree);
        }

        public ActionResult _CreateAction()
        {
            return PartialView();
        }


        [HttpPost]
        public ActionResult _GetDesignationTreePopup(Loader.ViewModel.TreeViewParam param)
        {

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
        public ActionResult _GetDesignationTree(Loader.ViewModel.TreeViewParam param)
        {
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

        public ActionResult Edit(int id = 0)
        {
            Designation Designation = DesignationService.GetSingle(id);
            if (Designation == null)
            {
                return HttpNotFound();
            }
            return View(Designation);
        }


        [HttpPost]
        public ActionResult Edit(Designation Designation)
        {
            if (ModelState.IsValid)
            {
                DesignationService.Save(Designation);

                return RedirectToAction("Index");
            }
            return View(Designation);
        }

        [HttpGet]
        public JsonResult Delete(int id = 0)
        {
            Designation Designation = new Loader.Repository.GenericUnitOfWork().Repository<Designation>().GetSingle(x => x.PDGId == id);
            bool deleteConfirm = false;
          
            var checkMenuTemplate = new Loader.Repository.GenericUnitOfWork().Repository<MenuTemplate>().GetSingle(x => x.DesignationId == id);
            var checkinEmployee = new Loader.Repository.GenericUnitOfWork().Repository<Employee>().GetSingle(x => x.DGId == id);
            if (Designation == null && checkMenuTemplate==null && checkinEmployee==null)
            {
                deleteConfirm = true;
            }
            return Json(deleteConfirm, JsonRequestBehavior.AllowGet);
            //return View(Designation);
        }
        [HttpPost]
        public ActionResult DeleteConfirm(int DesignationId)
        {
            DesignationService.Delete(DesignationId);
            return RedirectToAction("Index");
        }
        public ActionResult DisplayImage(HttpPostedFileBase file)
        {
            using (var reader = new System.IO.BinaryReader(file.InputStream))
            {
                byte[] ContentImage = reader.ReadBytes(file.ContentLength);
                var ImageContent = Convert.ToBase64String(ContentImage, Base64FormattingOptions.None);
                return Json(ImageContent, JsonRequestBehavior.AllowGet);
            }


        }

    }
}
