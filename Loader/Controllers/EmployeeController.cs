using System.Linq;
using System.Web.Mvc;
using Loader.Models;
using System;
using System.Web;
using System.Collections.Generic;
using Loader.ViewModel;
using Loader.Service;
using static Loader.Controllers.AccountController;
using Loader;

namespace Loader.Controllers
{
    [MyAuthorize]
    public class EmployeeController : Controller
    {

        private Service.EmployeeService EmployeeService = null;

        public EmployeeController()
        {
            EmployeeService = new Service.EmployeeService();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult Index()
        {
            if (Request.IsAjaxRequest() == true)
            {
                ViewModel.TreeView designationTree = EmployeeService.GetDesignationGroupTree();
                ViewBag.DesignationTree = designationTree;
                ViewModel.TreeView departmentTree = EmployeeService.GetDepartmentGroupTree();
                ViewBag.DepartmentTree = departmentTree;
                ViewBag.WithDepartment = EmployeeService.WithDepartment();
                ViewBag.Designation = EmployeeService.WithDesignation();
                ViewBag.PayrollParam = EmployeeService.WithPayrollParameters();
                ViewBag.ViewType = 1;

                int? page = 1;
                const int pageSize = 12;
                // var list = new Loader.Repository.GenericUnitOfWork().Repository<Employee>().GetAll().AsQueryable();
                var list = new Loader.Repository.GenericUnitOfWork().Repository<Employee>().GetAll().Where(x => x.Status != 3).AsQueryable();
                var dataList = list.OrderByDescending(m => m.EmployeeId);
                var pagedList = new Pagination<Employee>(dataList, page ?? 0, pageSize);
                ViewBag.CountPager = pagedList.TotalPages;
                ViewBag.ActivePager = 1;

                return View(pagedList);
            }
            return RedirectToAction("Index", "Home");

        }

        public JsonResult CheckDoubleEmployeeNo(string EmployeeNo,int EmployeeId)
        {
            bool ifExists = EmployeeService.checkDoubleEmployeeNo(EmployeeNo, EmployeeId);
            return Json(ifExists, JsonRequestBehavior.AllowGet);
        }


        public ActionResult _Details(int ViewType = 1, string query=null, int? page = 1,int? status=1)
        {
            const int pageSize = 12;
            
            var list = new Loader.Repository.GenericUnitOfWork().Repository<Employee>().GetAll().Where(x => x.Status == status).AsQueryable();

            if (!string.IsNullOrEmpty(query))
            {
                list = list.Where(m => m.EmployeeName.Contains(query));
                ViewBag.Query = query;
            }

            var dataList = list.OrderByDescending(m => m.EmployeeId);

            var pagedList = new Pagination<Employee>(dataList, page ?? 0, pageSize);
            ViewBag.CountPager = new Pagination<Employee>(dataList, page ?? 0, pageSize).TotalPages;
            ViewBag.ViewType = ViewType;
            ViewBag.ActivePager = page;
            return PartialView("_Details", pagedList);
            //ViewBag.Address = EmployeeService.GetAddress(parentId);

        }
        public ActionResult _DetailPartial(int ViewType = 1, string query = null, int? page = 1, int? status = 1)
        {
            const int pageSize = 12;

            var list = new Loader.Repository.GenericUnitOfWork().Repository<Employee>().GetAll().Where(x => x.Status == status).AsQueryable();

            if (!string.IsNullOrEmpty(query))
            {
                list = list.Where(m => m.EmployeeName.Contains(query));
                ViewBag.Query = query;
            }

            var dataList = list.OrderByDescending(m => m.EmployeeId);

            var pagedList = new Pagination<Employee>(dataList, page ?? 0, pageSize);
            ViewBag.CountPager = new Pagination<Employee>(dataList, page ?? 0, pageSize).TotalPages;
            ViewBag.ViewType = ViewType;
            ViewBag.ActivePager = page;
            return PartialView(pagedList);

        }

        //public ActionResult Details(int id = 0)
        //{
        //    Employee Employee = EmployeeService.GetSingle(id);
        //    if (Employee == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(Employee);
        //}

        [HttpGet]
        public ActionResult Create(int EmployeeId = 0)
        {
            if (Request.IsAjaxRequest() == true)
            {
                ViewModel.TreeView designationTree = EmployeeService.GetDesignationGroupTree();
                ViewBag.DesignationTree = designationTree;
                ViewModel.TreeView departmentTree = EmployeeService.GetDepartmentGroupTree();
                ViewBag.DepartmentTree = departmentTree;
                ViewBag.WithDepartment = EmployeeService.WithDepartment();
                ViewBag.Designation = EmployeeService.WithDesignation();
                ViewBag.PayrollParam = EmployeeService.WithPayrollParameters();

                ViewBag.GetBloodGroup = EmployeeService.GetBloodGroupOption();
                ViewBag.GetReligion = EmployeeService.GetReligionOption();
                ViewBag.GetNationality = EmployeeService.GetNationalityOption();
                ViewBag.GetGender = EmployeeService.GetGenderOption();
                ViewBag.GetMaritialStatus = EmployeeService.GetMaritialStatusOption();
                ViewBag.GetStatusOption = EmployeeService.GetStatusOption();

                ViewBag.AllowDepartmentInGroup = EmployeeService.AllowDepartmentInGroup();

                if (EmployeeId != 0)
                {
                    ViewBag.messageToDisableEditBranchName = 1;
                }
                if (Request.IsAjaxRequest())
                {
                    Employee EmployeeDTO = new Employee();

                  


                    if (EmployeeId != 0)
                    {
                        Loader.Service.EmployeeService empObj = new Loader.Service.EmployeeService();
                        bool withDepartment = empObj.WithDepartment();
                        bool withDesg = empObj.WithDesignation();

                        EmployeeDTO = new Loader.Repository.GenericUnitOfWork().Repository<Employee>().GetSingle(x => x.EmployeeId == EmployeeId);
                        if (withDepartment == true)
                        {
                            if (EmployeeDTO.DeptId != null)
                            {
                                var deptName = new Loader.Repository.GenericUnitOfWork().Repository<Department>().FindBy(x => x.DeptId == EmployeeDTO.DeptId).FirstOrDefault().DeptName;
                                ViewBag.DeptText = deptName;
                            }
                        }
                        //if(EmployeeDTO.BranchId!=null)
                        //{
                        //    if(EmployeeDTO.BranchId!=0)
                        //    {
                        //        Service.UserVSBranchService usrBrnchService = new UserVSBranchService();
                        //        EmployeeDTO.b
                        //    }
                        //}
                        if (withDesg == true)
                        {
                            if (EmployeeDTO.DGId != null)
                            {
                                var designationName = new Loader.Repository.GenericUnitOfWork().Repository<Designation>().FindBy(x => x.DGId == EmployeeDTO.DGId).FirstOrDefault().DGName;
                                ViewBag.DesgText = designationName;
                            }
                        }

                        if (EmployeeDTO.BranchId != null)
                        {
                            Service.UserVSBranchService usrBrnchService = new UserVSBranchService();
                            if (EmployeeDTO.BranchId != 0)
                            {
                                ViewBag.BranchText = usrBrnchService.GetAddressInCompany((int)EmployeeDTO.BranchId);
                            }

                        }
                        if (EmployeeDTO.Photo != null)
                        {
                            ViewBag.Image = Convert.ToBase64String(EmployeeDTO.Photo, Base64FormattingOptions.None);
                        }

                    }
                    //ViewData["param"] = new Loader.ViewModel.TreeViewParam(true, true, false, 0, 0, "Available MenuTemplate Group");
                    return PartialView(EmployeeDTO);
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            return RedirectToAction("Index", "Home");
        }




        [HttpPost]
        public ActionResult Create(Employee employee, HttpPostedFileBase file)
        {
            
            if (ModelState.IsValid)
            {
                employee.BranchId = Convert.ToInt32(Request.Form["BranchId"]);
                if (file != null)
                {
                    using (var reader = new System.IO.BinaryReader(file.InputStream))
                    {
                        employee.Photo = reader.ReadBytes(file.ContentLength);
                    }
                }
                try
                {
                    if (employee.EmployeeId == 0)
                    {
                        EmployeeService.Save(employee);
                        return RedirectToAction("Create", new { EmployeeId = 0 });
                    }
                    else
                    {
                        EmployeeService.Save(employee);
                        return RedirectToAction("Create", new { EmployeeId = employee.EmployeeId });
                    }


                }
                catch (Exception ex)
                {
                    return JavaScript(ex.Message);
                }


            }
            else
            {
                var err = ModelState.Values.SelectMany(v => v.Errors);
            }
            return View(employee);

        }




        public ActionResult _CreateAction()
        {
            return PartialView();
        }


        [HttpPost]
        public ActionResult _GetEmployeeTreePopup(Loader.ViewModel.TreeViewParam param)
        {

            ViewData["param"] = param;

            var filter = param.Filter == null ? "" : param.Filter;

            ViewModel.TreeView tree = new ViewModel.TreeView();

            if (param.WithOutMe == 0)
            {
                if (param.Title == "Select Department")
                {
                    if (param.AllowSelectGroup == false)
                    {
                        tree = EmployeeService.GetDepartmentTreeWithoutGroup(filter);
                    }
                    else
                    {
                        tree = EmployeeService.GetDepartmentGroupTree(filter);
                    }
                   
                }
                else if (param.Title == "Select Designation")
                {
                    tree = EmployeeService.GetDesignationGroupTree(filter);
                }

            }
            else
            {
                // tree = EmployeeService.GetEmployeeGroupTree(param.WithOutMe, filter);
            }
            return PartialView("_TreeViewPopup", tree);

        }

        [HttpPost]
        public ActionResult _GetBranchTreePopUp(Loader.ViewModel.TreeViewParam param)
        {

            ViewData["param"] = param;

            var filter = param.Filter == null ? "" : param.Filter;

            ViewModel.TreeView tree = new ViewModel.TreeView();

            if (param.WithOutMe == 0)
            {
                tree = EmployeeService.GetBranchGroupTree(filter);
            }
            else
            {
                // tree = EmployeeService.GetEmployeeGroupTree(param.WithOutMe, filter);
            }
            return PartialView("_TreeViewPopup", tree);

        }
        public ActionResult PagingData(int id, int next = 0, int previous = 0)
        {
            int listCount = new Loader.Repository.GenericUnitOfWork().Repository<Employee>().GetAll().Count();
            var pagingCount = listCount % 10;
            List<Employee> empList = EmployeeService.GetEmployeeList();
            return View();
        }
        //public ActionResult PagerIndex(int page = 1)
        //   {
        //   int listCount = new Loader.Repository.GenericUnitOfWork().Repository<Employee>().GetAll().Count();
        //   const int PageSize = 1;
        //       var pageCount = (listCount + PageSize - 1) / PageSize;

        //       ViewData["page"] = page;
        //       ViewData["count"] = pageCount;

        //       return View(listCount((page - 1) * PageSize).Take(PageSize));
        //   }
        public ActionResult PagerIndex(String query, int? page)
        {
            const int pageSize = 4;

            var list = new Loader.Repository.GenericUnitOfWork().Repository<Employee>().GetAll().AsQueryable();

            if (!string.IsNullOrEmpty(query))
            {
                list = list.Where(m => m.EmployeeName.Contains(query));
            }

            var dataList = list.OrderByDescending(m => m.EmployeeId);

            var pagedList = new Pagination<Employee>(dataList, page ?? 0, pageSize);

            return View(pagedList);
        }


        [HttpPost]
        public ActionResult _GetEmployeeTree(Loader.ViewModel.TreeViewParam param)
        {
            ViewData["param"] = param;
            var filter = param.Filter == null ? "" : param.Filter;

            ViewModel.TreeView tree = new ViewModel.TreeView();

            if (param.WithOutMe == 0)
            {
                if (param.Title == "Select Department")
                {
                    tree = EmployeeService.GetDepartmentGroupTree(filter);
                }
                else if (param.Title == "Select Designation")
                {
                    tree = EmployeeService.GetDesignationGroupTree(filter);
                }

            }
            return PartialView("_TreeViewBody", tree);
        }

        [HttpPost]
        public ActionResult _GetBranchTree(Loader.ViewModel.TreeViewParam param)
        {
            ViewData["param"] = param;
            var filter = param.Filter == null ? "" : param.Filter;

            ViewModel.TreeView tree = new ViewModel.TreeView();

            if (param.WithOutMe == 0)
            {

                tree = EmployeeService.GetBranchGroupTree(filter);



            }
            return PartialView("_TreeViewBody", tree);
        }

        //public ActionResult Edit(int id = 0)
        //{
        //    Employee Employee = EmployeeService.GetSingle(id);
        //    if (Employee == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(Employee);
        //}


        //[HttpPost]
        //public ActionResult Edit(Employee Employee)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        EmployeeService.Save(Employee);

        //        return RedirectToAction("Index");
        //    }
        //    return View(Employee);
        //}

        [HttpGet]
        public JsonResult Delete(int id = 0)
        {
            Employee Employee = new Loader.Repository.GenericUnitOfWork().Repository<Employee>().GetSingle(x => x.EmployeeId == id);
            bool deleteConfirm = false;
            var checkinUsers = new Loader.Repository.GenericUnitOfWork().Repository<ApplicationUser>().GetSingle(x => x.EmployeeId == id);
            int alreadyMapped = EmployeeService.mappedCount(id);
            if (checkinUsers == null && alreadyMapped == 0)
            {
                deleteConfirm = true;
            }
            return Json(deleteConfirm, JsonRequestBehavior.AllowGet);
            //return View(Employee);
        }
        [HttpPost]
        public ActionResult DeleteConfirm(int EmployeeId)
        {
            EmployeeService.Delete(EmployeeId);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public JsonResult CheckResignedEmployee(int id = 0)
        {
            bool isResigned = false;
            if (id == 0)
            {
                isResigned = false;
                return Json(isResigned, JsonRequestBehavior.AllowGet);
            }
            else
            {
                Employee Employee = new Loader.Repository.GenericUnitOfWork().Repository<Employee>().GetSingle(x => x.EmployeeId == id);
                if (Employee.Status == 3)
                {
                    isResigned = true;
                }
                return Json(isResigned, JsonRequestBehavior.AllowGet);

            }
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

        public JsonResult SetFiscalYear(int FYId)
        {
            string fyName = "";
            if (FYId != 0)
            {
                Loader.Models.Global.SelectedFYID = FYId;
                Loader.Service.ParameterService paramService = new Loader.Service.ParameterService();
                fyName = paramService.GetCurrentFiscalYear(FYId);
            }
            return Json(fyName, JsonRequestBehavior.AllowGet);

        }
        public JsonResult SetTranscationDate(int FYId)
        {
            //Loader.Models.Global.TransactionDate
            string TDate = "";
            if (FYId != 0)
            {
                TDate = EmployeeService.GetTranscationDate(FYId);

            }
            Loader.Models.Global.TransactionDate = Convert.ToDateTime(TDate);
            return Json(TDate, JsonRequestBehavior.AllowGet);

        }

        public JsonResult CheckEmployeeNumber(string EmployeeNo, int EmployeeId = 0)
        {
            bool ifExists = EmployeeService.CheckEmployeeNumberDuplication(EmployeeNo, EmployeeId);
            return Json(ifExists, JsonRequestBehavior.AllowGet);

        }

    }
}
