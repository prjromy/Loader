using Loader.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Loader.Controllers
{
    [MyAuthorize]
    public class ParameterController : Controller
    {
        private Service.ParameterService parameterService = null;

        ReturnBaseMessageModel returnBaseMessageModel = null;

        public ParameterController()
        {
            parameterService = new Service.ParameterService();
            returnBaseMessageModel = new ReturnBaseMessageModel();
        }


        public ActionResult Index()
        {
            if (Request.IsAjaxRequest())
            {
                ViewModel.TreeView tree = parameterService.GetParamGroupTree();
                ViewBag.treeview = tree;
                ViewBag.Address = parameterService.GetAddress(0);
                ViewBag.ViewType = 1;
                ViewBag.ParentParamId = 0;
                ViewData["param"] = new Loader.ViewModel.TreeViewParam(false, true, true, 0, 0, "Available Param Group");

                return View(parameterService.GetAllOfParent(0).ToList());
            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult _Details(int parentId, bool allowSelectGroup, int ViewType = 1)
        {
            ViewBag.Address = parameterService.GetAddress(parentId);
            ViewBag.ParentParamId = parentId;
            ViewBag.ViewType = ViewType;
            return PartialView(parameterService.GetAllOfParent(parentId).ToList());
        }

        public ActionResult Details(int id = 0)
        {
            Models.Param Param = parameterService.GetSingle(id);
            if (Param == null)
            {
                return HttpNotFound();
            }
            return View(Param);
        }

        [HttpGet]
        public ActionResult Create(string activeText, int activeId, int ParamId = 0)
        {
            if (Request.IsAjaxRequest())
            {
                Models.Param ParamDTO = new Models.Param();
                ParamDTO.paramValue = new Models.ParamValue();
                ParamDTO.paramValue.paramScript = new Models.ParamScript();
                ViewBag.ParamId = ParamId;
                if (ParamId != 0)
                {
                    ParamDTO = new Loader.Repository.GenericUnitOfWork().Repository<Models.Param>().GetSingle(x => x.PId == ParamId);
                }
                else
                {
                    ParamDTO.ParentId = activeId;
                    ParamDTO.IsGroup = true;
                }
                var dataType = new Loader.Repository.GenericUnitOfWork().Repository<Models.Datatype>().GetAll();
                List<Models.Datatype> lst = dataType.ToList();
                ViewBag.DataTypeList = new SelectList(lst, "DTId", "DType");

                ViewBag.ActiveText = activeText;
                ViewData["param"] = new Loader.ViewModel.TreeViewParam(false, true, false, 0, 0, "Available Param Group");
                return PartialView(ParamDTO);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }


        [HttpPost]
        public ActionResult Create(Models.Param Param, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {

                //if (file != null)
                //{
                //    using (var reader = new System.IO.BinaryReader(file.InputStream))
                //    {
                //        Param.Image = reader.ReadBytes(file.ContentLength);
                //    }
                //}
                try
                {
                    if (Param.PId == 0)
                    {
                        parameterService.Save(Param);
                        return RedirectToAction("Create", new { activeText = "Root", activeId = 0 });
                    }
                    else
                    {
                        parameterService.Save(Param);
                        var parentNode = new Loader.Repository.GenericUnitOfWork().Repository<Models.Param>().GetSingle(x => x.PId == Param.ParentId);
                        if (parentNode == null)
                        {
                            return RedirectToAction("Create", new { activeText = "Root", activeId = 0 });
                        }
                        else
                        {
                            return RedirectToAction("Create", new { activeText = parentNode.PName, activeId = parentNode.PId });
                        }

                    }


                }
                catch (Exception ex)
                {
                    return JavaScript(ex.Message);
                }


            }
            return View(Param);

        }

        [HttpGet]
        public ActionResult _UpdateParamTree(int selectedNode, bool allowSelectGroupNode, bool withImageIcon, bool withCheckBox, int withOutMe = 0, int rootNode = -1)
        {
            ViewData["AllowSelectGroup"] = allowSelectGroupNode;
            ViewData["WithImageIcon"] = withImageIcon;
            ViewData["WithCheckBox"] = withCheckBox;
            ViewData["param"] = new Loader.ViewModel.TreeViewParam(false, true, true, 0, selectedNode, "Available Param Group");

            ViewModel.TreeView tree = new ViewModel.TreeView();
            if (rootNode < 0)
            {
                tree = parameterService.GetParamGroupTree(withOutMe);

            }
            else
            {
                tree = parameterService.GetParamGroupTree(rootNode, withOutMe);
            }
            return PartialView("_TreeViewBody", tree);
        }

        public ActionResult _CreateAction()
        {
            return PartialView();
        }


        [HttpPost]
        public ActionResult _GetParamTreePopup(Loader.ViewModel.TreeViewParam param)
        {

            ViewData["param"] = param;

            var filter = param.Filter == null ? "" : param.Filter;

            ViewModel.TreeView tree = new ViewModel.TreeView();

            if (param.WithOutMe == 0)
            {
                tree = parameterService.GetParamGroupTree(filter);
            }
            else
            {
                tree = parameterService.GetParamGroupTree(param.WithOutMe, filter);
            }
            return PartialView("_TreeViewPopup", tree);

        }

        [HttpPost]
        public ActionResult _GetParamTree(Loader.ViewModel.TreeViewParam param)
        {
            ViewData["param"] = param;
            var filter = param.Filter == null ? "" : param.Filter;

            ViewModel.TreeView tree = new ViewModel.TreeView();

            if (param.WithOutMe == 0)
            {
                tree = parameterService.GetParamGroupTree(filter);
            }
            else
            {
                tree = parameterService.GetParamGroupTree(param.WithOutMe, filter);
            }
            return PartialView("_TreeViewBody", tree);
        }

        public ActionResult Edit(int id = 0)
        {
            Models.Param Param = parameterService.GetSingle(id);
            if (Param == null)
            {
                return HttpNotFound();
            }
            return View(Param);
        }


        [HttpPost]
        public ActionResult Edit(Models.Param Param)
        {
            if (ModelState.IsValid)
            {
                parameterService.Save(Param);

                return RedirectToAction("Index");
            }
            return View(Param);
        }

        [HttpGet]
        public JsonResult Delete(int id = 0)
        {
            Models.Param Param = new Loader.Repository.GenericUnitOfWork().Repository<Models.Param>().GetSingle(x => x.ParentId == id);
            bool deleteConfirm = false;
            if (Param == null)
            {
                deleteConfirm = true;
            }
            return Json(deleteConfirm, JsonRequestBehavior.AllowGet);
            //return View(Param);
        }
        [HttpPost]
        public ActionResult DeleteConfirm(int ParamId)
        {
            parameterService.Delete(ParamId);
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
        [HttpGet]
        public ActionResult _EditParameterValue(int id, int selectedId = 0)
        {

            var paramValues = parameterService.ParameterValues(id);
            ViewData["SelectedId"] = selectedId;
            return PartialView("_EditParameters", paramValues);
        }

        [HttpPost]
        public ActionResult _EditParametersValue(int id, string value)
        {


            parameterService.SaveParamValues(id, value);


            return RedirectToAction("SystemCustomize");
        }

        [HttpPost]
        public ActionResult UploadFiles()
        {

            // Checking no of files injected in Request object  
            try
            {
                if (Request.Files.Count > 0)
                {

                    // string ImageName = System.IO.Path.GetFileName(file.FileName); //file2 to store path and url  
                    //  Get all files from Request object  
                    HttpFileCollectionBase files = Request.Files;
                    for (int i = 0; i < files.Count; i++)
                    {
                        //string path = AppDomain.CurrentDomain.BaseDirectory + "Uploads/";  
                        //string filename = Path.GetFileName(Request.Files[i].FileName);  

                        HttpPostedFileBase file = files[i];
                        string fname;

                        // Checking for Internet Explorer  
                        if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                        {
                            string[] testfiles = file.FileName.Split(new char[] { '\\' });
                            fname = testfiles[testfiles.Length - 1];
                        }
                        else
                        {
                            fname = file.FileName;
                        }
                        string physicalPath = Server.MapPath("~/img/" + fname);
                        file.SaveAs(physicalPath);

                        string theFileName = System.IO.Path.GetFileName(file.FileName);
                        byte[] thePictureAsBytes = new byte[file.ContentLength];
                        using (System.IO.BinaryReader theReader = new System.IO.BinaryReader(file.InputStream))
                        {
                            thePictureAsBytes = theReader.ReadBytes(file.ContentLength);
                        }
                        string thePictureDataAsString = Convert.ToBase64String(thePictureAsBytes);
                        
                        string databasepath = "img/" + fname;
                        string value = databasepath;
                        int id = 8;
                        parameterService.SaveParamValues(id, thePictureDataAsString);
                        returnBaseMessageModel.Value = thePictureDataAsString;
                        returnBaseMessageModel.Msg = "File Uploaded Successfully!";
                        returnBaseMessageModel.Success = true;
                        //ViewBag.fileName = value;
                    }
                    return Json(returnBaseMessageModel, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    throw new System.ArgumentException("Parameter cannot be null", "original");
                }
            }
            catch (Exception ex)
            {
                return JavaScript("Error occurred. Error details: " + ex.Message);
            }
        }


        public ActionResult _GetParamValue()
        {
            var dataType = new Loader.Repository.GenericUnitOfWork().Repository<Models.Datatype>().GetAll();
            List<Models.Datatype> lst = dataType.ToList();
            ViewBag.DataTypeList = new SelectList(lst, "DTId", "DType");
            Models.Param model = new Models.Param();
            model.paramValue = new Models.ParamValue();
            model.paramValue.paramScript = new Models.ParamScript();
            return PartialView("_GetDataType", model);
        }
        public ActionResult _GetScript()
        {
            Models.Param model = new Models.Param();
            model.paramValue = new Models.ParamValue();
            model.paramValue.paramScript = new Models.ParamScript();
            return PartialView("_Script", model);
        }
        public ActionResult SystemCustomize()
        {
            List<Models.Param> param = parameterService.ListAllParameters(1);
            ViewBag.Distinct = param.Select(x => x.ParentId).Distinct();
            ViewBag.Title = "System Parameters";

            return View("_ParamCustomize", param);
        }
        public ActionResult UserCustomize()
        {
            List<Models.Param> param = parameterService.ListAllParameters(2);
            ViewBag.Distinct = param.Select(x => x.ParentId).Distinct();
            ViewBag.Title = "User Parameters";
            return View("_ParamCustomize", param);
        }




    }
}
