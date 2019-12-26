using System.Linq;
using System.Web.Mvc;
using Loader.Models;
using System;
using System.Web;

namespace Loader.Controllers
{
    [MyAuthorize]
    public class BranchSetupController : Controller
    {

        private Service.BranchSetupService BranchSetupService = null;

        public BranchSetupController()
        {
            BranchSetupService = new Service.BranchSetupService();
        }
        public ActionResult Index()
        {
            if (Request.IsAjaxRequest() == true)
            {
                ViewModel.TreeView tree = BranchSetupService.GetBranchSetupGroupTree();
                ViewBag.treeview = tree;
                ViewBag.Address = BranchSetupService.GetAddress(0);
                ViewBag.ViewType = 1;
                ViewBag.ParentBranchSetupId = 0;
                ViewData["param"] = new Loader.ViewModel.TreeViewParam(false, true, true, 0, 0, "Available Branch");

                return View(BranchSetupService.GetAllOfParent(1).ToList());
            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult _Details(int parentId, bool allowSelectGroup, int ViewType = 1)
        {
            ViewBag.Address = BranchSetupService.GetAddress(parentId);
            ViewBag.ParentBranchSetupId = parentId;
            ViewBag.ViewType = ViewType;
            return PartialView(BranchSetupService.GetAllOfParent(parentId).ToList());
        }

        public ActionResult Details(int id = 0)
        {
            Company BranchSetup = BranchSetupService.GetSingle(id);
            if (BranchSetup == null)
            {
                return HttpNotFound();
            }
            return View(BranchSetup);
        }

        [HttpGet]
        public ActionResult Create(string activeText, int activeId, int BranchSetupId = 0)
        {
            if (Request.IsAjaxRequest())
            {
                Company BranchSetupDTO = new Company();
                if (BranchSetupId != 0)
                {

                    BranchSetupDTO = new Loader.Repository.GenericUnitOfWork().Repository<Company>().GetSingle(x => x.CompanyId == BranchSetupId);

                }
                else
                {
                    BranchSetupDTO.ParentId = activeId;
                }
                var withHeirarchy = BranchSetupService.WithHeirarchy();
                ViewBag.Heirarchy = 0;
                if (withHeirarchy == "true")
                {
                    ViewBag.Heirarchy = 1;
                }
                ViewBag.ActiveText = activeText;
                ViewData["param"] = new Loader.ViewModel.TreeViewParam(false, true, false, 0, 0, "Available BranchSetup Group");
                return PartialView(BranchSetupDTO);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }


        [HttpPost]
        public ActionResult Create(Company BranchSetup, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {

                try
                {
                    if (BranchSetup.CompanyId == 0)
                    {
                        BranchSetupService.Save(BranchSetup);
                        return RedirectToAction("Create", new { activeText = "ChannakyaSoft", activeId = 1 });
                    }
                    else
                    {
                        BranchSetupService.Save(BranchSetup);
                        var parentNode = new Loader.Repository.GenericUnitOfWork().Repository<Company>().GetSingle(x => x.CompanyId == BranchSetup.ParentId);
                        if (parentNode == null)
                        {
                            return RedirectToAction("Create", new { activeText = "ChannakyaSoft", activeId = BranchSetup.ParentId });
                        }


                        return RedirectToAction("Create", new { activeText = parentNode.BranchName, activeId = parentNode.CompanyId });
                    }


                }
                catch (Exception ex)
                {
                    return JavaScript(ex.Message);
                }


            }
            return View(BranchSetup);

        }

        [HttpGet]
        public ActionResult _UpdateBranchSetupTree(int selectedNode, bool allowSelectGroupNode, bool withImageIcon, bool withCheckBox, int withOutMe = 0, int rootNode = -1)
        {
            ViewData["AllowSelectGroup"] = allowSelectGroupNode;
            ViewData["WithImageIcon"] = withImageIcon;
            ViewData["WithCheckBox"] = withCheckBox;
            ViewData["param"] = new Loader.ViewModel.TreeViewParam(false, true, true, 0, selectedNode, "Available BranchSetup Group");

            ViewModel.TreeView tree = new ViewModel.TreeView();
            if (rootNode < 0)
            {
                tree = BranchSetupService.GetBranchSetupGroupTree(withOutMe);

            }
            else
            {
                tree = BranchSetupService.GetBranchSetupGroupTree(rootNode, withOutMe);
            }
            return PartialView("_TreeViewBody", tree);
        }

        public ActionResult _CreateAction()
        {
            return PartialView();
        }


        [HttpPost]
        public ActionResult _GetBranchSetupTreePopup(Loader.ViewModel.TreeViewParam param)
        {

            ViewData["param"] = param;

            var filter = param.Filter == null ? "" : param.Filter;

            ViewModel.TreeView tree = new ViewModel.TreeView();

            if (param.WithOutMe == 0)
            {
                tree = BranchSetupService.GetBranchSetupGroupTree(filter);
            }
            else
            {
                tree = BranchSetupService.GetBranchSetupGroupTree(param.WithOutMe, filter);
            }
            return PartialView("_TreeViewPopup", tree);

        }

        [HttpPost]
        public ActionResult _GetBranchSetupTree(Loader.ViewModel.TreeViewParam param)
        {
            ViewData["param"] = param;
            var filter = param.Filter == null ? "" : param.Filter;

            ViewModel.TreeView tree = new ViewModel.TreeView();

            if (param.WithOutMe == 0)
            {
                tree = BranchSetupService.GetBranchSetupGroupTree(filter);
            }
            else
            {
                tree = BranchSetupService.GetBranchSetupGroupTree(param.WithOutMe, filter);
            }
            return PartialView("_TreeViewBody", tree);
        }

        public ActionResult Edit(int id = 0)
        {
            Company BranchSetup = BranchSetupService.GetSingle(id);
            if (BranchSetup == null)
            {
                return HttpNotFound();
            }
            return View(BranchSetup);
        }


        [HttpPost]
        public ActionResult Edit(Company BranchSetup)
        {
            if (ModelState.IsValid)
            {
                BranchSetupService.Save(BranchSetup);

                return RedirectToAction("Index");
            }
            return View(BranchSetup);
        }

        [HttpGet]
        public JsonResult Delete(int id = 0)
        {
            Company BranchSetup = new Loader.Repository.GenericUnitOfWork().Repository<Company>().GetSingle(x => x.CompanyId == id);
            bool deleteConfirm = true;

            //var checkinEmployee = new Loader.Repository.GenericUnitOfWork().Repository<Employee>().GetSingle(x => x.Id == id);
            if (BranchSetup == null)
            {
                deleteConfirm = false;
            }
            return Json(deleteConfirm, JsonRequestBehavior.AllowGet);
            //return View(BranchSetup);
        }
        [HttpPost]
        public ActionResult DeleteConfirm(int BranchSetupId)
        {
            BranchSetupService.Delete(BranchSetupId);
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
