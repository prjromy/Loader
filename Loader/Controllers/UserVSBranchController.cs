using System.Linq;
using System.Web.Mvc;
using Loader.Models;
using System;
using System.Web;
using Loader.ViewModel;
using PagedList;
using System.Collections.Generic;

namespace Loader.Controllers
{
    //[MyAuthorize]
    public class UserVSBranchController : Controller
    {

        private Service.UserVSBranchService UserVSBranchService = null;

        public UserVSBranchController()
        {
            UserVSBranchService = new Service.UserVSBranchService();
        }
        public ActionResult Index()
        {
            if (Request.IsAjaxRequest() == true)
            {
                ViewBag.ViewType = 1;
                // added part
                int? page = 1;
                const int pageSize = 12;
                var userBranchList = UserVSBranchService.GetUserVsBranchList();
                var userBranchListQueryable = userBranchList.AsQueryable();
                var dataList = userBranchListQueryable.OrderBy(m => m.Id);
                var pagedList = new Pagination<UserVSBranch>(dataList, page ?? 0, pageSize);
                ViewBag.CountPager = new Pagination<UserVSBranch>(dataList, page ?? 0, pageSize).TotalPages;
                ViewBag.ActivePager = 1;
                return View(pagedList);
              //  return View(UserVSBranchService.GetAll().ToList());
            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult _Details(int parentId, bool allowSelectGroup, int ViewType = 1)
        {
            ViewBag.ViewType = ViewType;
            return PartialView(UserVSBranchService.GetAll().ToList());
        }

        public ActionResult Details(int id = 0)
        {
            UserVSBranch UserVSBranch = UserVSBranchService.GetSingle(id);
            if (UserVSBranch == null)
            {
                return HttpNotFound();
            }
            return View(UserVSBranch);
        }

        public ActionResult GetUserDetails(string query = null, int? userId = 0, int? page = 1)
        {
            const int pageSize = 10;
            var userList = UserVSBranchService.GetAllUsers();
            ApplicationDbContext ctx = new ApplicationDbContext();
            IPagedList<ApplicationUser> listUser;
            if (!string.IsNullOrEmpty(query))
            {
                listUser = new Loader.Repository.GenericUnitOfWork().Repository<Models.ApplicationUser>().FindBy(m => m.UserName.ToUpper().Contains(query.ToUpper())).OrderBy(x => x.UserName).ToPagedList(page ?? 0, pageSize);
            }
            else
            {
                listUser = ctx.Users.OrderBy(x => x.UserName).ToPagedList(page ?? 0, pageSize);
            }

            int totalCount = listUser.TotalItemCount;
            List<ApplicationUser> finalList = listUser.ToList();

            var pagedList = new Pagination<ApplicationUser>(finalList.AsQueryable(), page ?? 0, pageSize, totalCount);
            ViewBag.CountPager = new Pagination<ApplicationUser>(finalList.AsQueryable(), page ?? 0, pageSize, totalCount).TotalPages;
            ViewBag.ActivePager = 1;


            return PartialView(pagedList);
        }
        public ActionResult _GetUserDetailsPartial(int ViewType = 2, string query = null, int? page = 1)
        {
            const int pageSize = 10;
            var list = new Loader.Repository.GenericUnitOfWork().Repository<ApplicationUser>().GetAll().AsQueryable();
            if (!string.IsNullOrEmpty(query))
            {
                list = list.Where(m => m.UserName.Contains(query));
                ViewBag.Query = query;
            }
            var dataList = list.OrderBy(m => m.Id);
            var pagedList = new Pagination<ApplicationUser>(dataList, page ?? 0, pageSize);
            ViewBag.CountPager = new Pagination<ApplicationUser>(dataList, page ?? 0, pageSize).TotalPages;
            ViewBag.ViewType = ViewType;
            ViewBag.ActivePager = page;
            return PartialView(pagedList);
        }

        public ActionResult _GUDetails(string query = null, int? userId = 0, int? page = 1)
        {
            const int pageSize = 10;
            IQueryable<ApplicationUser> list;
            if(Models.Global.UserId != 1)
            {
                list = new Loader.Repository.GenericUnitOfWork().Repository<ApplicationUser>().GetAll().Where(x => x.IsActive == true && x.Id != 1).AsQueryable();
            }
            else
            {
                list = new Loader.Repository.GenericUnitOfWork().Repository<ApplicationUser>().GetAll().Where(x => x.IsActive == true).AsQueryable();

            }

            if (!string.IsNullOrEmpty(query))
            {
                list = list.Where(m => m.UserName.Contains(query));
                ViewBag.Query = query;
            }
            var dataList = list.OrderBy(m => m.Id);
            var pagedList = new Pagination<ApplicationUser>(dataList, page ?? 0, pageSize);
            ViewBag.CountPager = new Pagination<ApplicationUser>(dataList, page ?? 0, pageSize).TotalPages;
            ViewBag.ViewType = 2;
            ViewBag.ActivePager = page;
            return PartialView(pagedList);
        }
        public ActionResult _GUDetailPartial(string query = null, int? userId = 0, int? page = 1)
        {
            const int pageSize = 10;

            IQueryable<ApplicationUser> list;
            if (Models.Global.UserId != 1)
            {
                list = new Loader.Repository.GenericUnitOfWork().Repository<ApplicationUser>().GetAll().Where(x => x.IsActive == true && x.Id != 1).AsQueryable();
            }
            else
            {
                list = new Loader.Repository.GenericUnitOfWork().Repository<ApplicationUser>().GetAll().Where(x => x.IsActive == true).AsQueryable();

            }
            if (!string.IsNullOrEmpty(query))
            {
                list = list.Where(m => m.UserName.Contains(query));
                ViewBag.Query = query;
            }
            var dataList = list.OrderBy(m => m.Id);
            var pagedList = new Pagination<ApplicationUser>(dataList, page ?? 0, pageSize);
            ViewBag.CountPager = new Pagination<ApplicationUser>(dataList, page ?? 0, pageSize).TotalPages;
            ViewBag.ViewType = 2;
            ViewBag.ActivePager = page;
            return PartialView(pagedList);
        }


        [HttpGet]
        public ActionResult Create(string activeText, int activeId, int UserVSBranchId = 0)
        {
            if (Request.IsAjaxRequest())
            {
                UserVSBranch UserVSBranchDTO = new UserVSBranch();

                if (UserVSBranchId != 0)
                {

                    UserVSBranchDTO = new Loader.Repository.GenericUnitOfWork().Repository<UserVSBranch>().GetSingle(x => x.Id == UserVSBranchId);

                }
                else
                {
                    UserVSBranchDTO.Id = activeId;
                }

                ViewBag.ActiveText = activeText;

                return PartialView(UserVSBranchDTO);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }


        [HttpPost]
        public ActionResult Create(UserVSBranch UserVSBranch, HttpPostedFileBase file)
        {
            ModelState modelstate = new ModelState();
            try
            {
                if (ModelState.IsValid)
                {

                    if (UserVSBranch.Id == 0)
                    {
                        UserVSBranchService.Save(UserVSBranch);
                        return RedirectToAction("Create", new { activeText = "Root", activeId = 0 });
                    }
                    else
                    {
                        UserVSBranchService.Save(UserVSBranch);
                        var parentNode = new Loader.Repository.GenericUnitOfWork().Repository<UserVSBranch>().GetSingle(x => x.Id == UserVSBranch.Id);
                        if (parentNode == null)
                        {
                            return RedirectToAction("Create", new { activeText = "Root" });
                        }


                        return RedirectToAction("Create", new { activeId = parentNode.Id });
                    }
                }

                else
                {

                    var err = ModelState.Values.SelectMany(v => v.Errors);
                    return JavaScript(err.ToString());

                }
                
            }
            catch (Exception ex)
            {
                return JavaScript(ex.Message);
            }


          //  return View(UserVSBranch);

        }



        public ActionResult _CreateAction()
        {
            return PartialView();
        }



        public ActionResult Edit(int id = 0)
        {
            UserVSBranch UserVSBranch = UserVSBranchService.GetSingle(id);
            if (UserVSBranch == null)
            {
                return HttpNotFound();
            }
            return View(UserVSBranch);
        }


        [HttpPost]
        public ActionResult Edit(UserVSBranch UserVSBranch)
        {
            if (ModelState.IsValid)
            {
                UserVSBranchService.Save(UserVSBranch);

                return RedirectToAction("Index");
            }
            return View(UserVSBranch);
        }

        [HttpGet]
        public JsonResult Delete(int id = 0)
        {
            UserVSBranch UserVSBranch = new Loader.Repository.GenericUnitOfWork().Repository<UserVSBranch>().GetSingle(x => x.Id == id);
            bool deleteConfirm = true;

            //var checkinEmployee = new Loader.Repository.GenericUnitOfWork().Repository<Employee>().GetSingle(x => x.Id == id);
            if (UserVSBranch == null)
            {
                deleteConfirm = false;
            }
            return Json(deleteConfirm, JsonRequestBehavior.AllowGet);
            //return View(UserVSBranch);
        }
        [HttpPost]
        public ActionResult DeleteConfirm(int UserVSBranchId)
        {
            UserVSBranchService.Delete(UserVSBranchId);
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

        public ActionResult _DetailPartial(int ViewType = 2, string query = null, int? page = 1, int? status = 2)
        {
            //var list;
            IQueryable<UserVSBranch> list;
            const int pageSize = 12;

            //      var list = new Loader.Repository.GenericUnitOfWork().Repository<ApplicationUser>().GetAll().Where(x=>x.IsActive==Convert.ToBoolean(status)).AsQueryable();
          
                list = new Loader.Repository.GenericUnitOfWork().Repository<UserVSBranch>().GetAll().AsQueryable();
       


            //if (!string.IsNullOrEmpty(query))
            //{
            //    list = list.Where(m => m.UserName.Contains(query));
            //    ViewBag.Query = query;
            //}
            var dataList = list.OrderBy(m => m.Id);
            var pagedList = new Pagination<UserVSBranch>(dataList, page ?? 0, pageSize);
            ViewBag.CountPager = new Pagination<UserVSBranch>(dataList, page ?? 0, pageSize).TotalPages;
            //       ViewBag.ViewType = ViewType;
            ViewBag.ViewType = 2;
            ViewBag.ActivePager = page;
            return PartialView(pagedList);
        }

    }
}
