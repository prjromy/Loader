using Loader.Models;
using Loader.Repository;
using Loader.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Loader.Service
{
    public class UserVSBranchService
    {
        private GenericUnitOfWork uow = null;


        public UserVSBranchService()
        {
            uow = new GenericUnitOfWork();
        }

        public List<UserVSBranch> GetAll()
        {
            return uow.Repository<UserVSBranch>().GetAll().ToList();
        }
        public List<Company> GetAllCompany()
        {
            return uow.Repository<Company>().GetAll().ToList();
        }

        public List<UserVSBranch> GetAllOfParent(int parentId)
        {


            return uow.Repository<UserVSBranch>().GetAll().Where(x => x.Id == parentId).ToList();
        }

        public List<ApplicationUser> GetAllUsers()
        {
            return uow.Repository<Models.ApplicationUser>().GetAll().ToList();
        }

        public string WithHeirarchy()
        {
            var dValue = uow.Repository<ParamValue>().GetAll().Where(x => x.PId == 14).Select(x => x.PValue).SingleOrDefault();
            return dValue;
        }

        public UserVSBranch GetSingle(int UserVSBranchId)
        {
            UserVSBranch UserVSBranch = uow.Repository<UserVSBranch>().GetSingle(c => c.Id == UserVSBranchId);
            return UserVSBranch;
        }
        public string GetEmployeeName(int empId)
        {
            string employeeName = "";
            var empObj = uow.Repository<Models.Employee>().GetSingle(x => x.EmployeeId == empId);
            if (empObj != null)
            {
                employeeName = empObj.EmployeeName;
            }
            return employeeName;
        }


        public string GetUserName(int userId)
        {
            string userName = "";
            var userObj = uow.Repository<Models.ApplicationUser>().GetSingle(x => x.Id == userId);
            if (userObj != null)
            {
                userName = userObj.UserName;
            }
            return userName;
        }

        public List<SelectListItem> GetCompanyList()
        {
            List<SelectListItem> ls = new List<SelectListItem>();
            var companyList = uow.Repository<Company>().SqlQuery("select * from dbo.FGetCompanyList() order by CompanyId").ToList();
            foreach (var item in companyList)
            {
                //var parentId = companyList.Where(x => x.ParentId == item.CompanyId).FirstOrDefault();
                //if (parentId != null)
                //{
                //    var parentName = parentId.BranchName;
                //    ls.Add(new SelectListItem { Text = item.BranchName + "(" + parentName + ")", Value = item.CompanyId.ToString() });
                //}
                //else
                //{
                //    ls.Add(new SelectListItem { Text = item.BranchName, Value = item.CompanyId.ToString() });
                //}
                ls.Add(new SelectListItem { Text = GetAddressInCompany(item.CompanyId), Value = item.CompanyId.ToString() });

            }
            return ls;
        }


        public List<SelectListItem> GetBranchListForUserVsBranch()
        {
            List<SelectListItem> ls = new List<SelectListItem>();
            var branchList = uow.Repository<LicenseBranch>().SqlQuery("select * from fin.LicenseBranch order by BrnhId").ToList();
            foreach (var item in branchList)
            {

                //ls.Add(new SelectListItem { Text = GetAddressInCompany(item.CompanyId), Value = item.CompanyId.ToString() });
                ls.Add(new SelectListItem { Text = item.BrnhNam, Value = item.BrnhID.ToString() });

            }
            return ls;
        }


        public Company GetSingleCompany(int companyId)
        {
            return uow.Repository<Company>().SqlQuery("select * from dbo.FGetCompanyList() order by CompanyId").ToList().Where(x => x.CompanyId == companyId).FirstOrDefault();
        }
        public string GetAddressInCompany(int BranchSetupId)
        {
            string result = "";

            if (BranchSetupId != 0)
            {
                Company mnu = new Company();
                mnu = GetSingleCompany(BranchSetupId);

                List<string> lst = new List<string>();


                while (mnu != null)
                {
                    lst.Add(mnu.BranchName);
                    mnu = GetSingleCompany(mnu.ParentId);
                };

                var sorted = lst.Select((x, i) => new KeyValuePair<string, int>(x, i)).OrderByDescending(x => x.Value).ToList();

                foreach (var item in sorted)
                {
                    if (result == "")
                    {
                        result = result + item.Key;
                    }
                    else
                    {
                        result = result + "-" + item.Key;
                    }

                }
            }
            else
            {
                result = "ChannakyaSoft";
            }
            return result;
        }

        public string GetCurrentBranchString(int userId)
        {
            string branchName = "";
            var hasBranchsAssigned = uow.Repository<UserVSBranch>().FindBy(x => x.UserId == userId).ToList();
            if (hasBranchsAssigned.Count() > 0)
            {
                branchName = GetBranchName(hasBranchsAssigned.LastOrDefault().BranchId);
            }
            else
            {
                var userObj = uow.Repository<Models.ApplicationUser>().GetSingle(x => x.Id == userId);
                if (userObj != null)
                {
                    int empId = (int)userObj.EmployeeId;

                    int branchId = Convert.ToInt32(uow.Repository<Models.Employee>().GetSingle(x => x.EmployeeId == empId).BranchId);
                    branchName = GetBranchName(branchId);
                }

            }
            return branchName;
        }

        public string GetBranchName(int branchId)
        {
            string branchName = "";
            var branchObj = uow.Repository<Models.LicenseBranch>().GetSingle(x => x.BrnhID == branchId);
            if (branchObj != null)
            {
                branchName = branchObj.BrnhNam;
            }
            return branchName;
        }
        public int GetBranchRoleId(int branchId, int userId)
        {
            int roleId = 0;
            var branchObj = uow.Repository<Models.UserVSBranch>().FindBy(x => x.BranchId == branchId && x.UserId == userId).LastOrDefault();
            if (branchObj != null)
            {
                roleId = branchObj.RoleId;
            }
            else
            {
                var empObj = uow.Repository<Models.ApplicationUser>().FindBy(x => x.Id == userId).FirstOrDefault();
                if (empObj != null)
                {
                    roleId = empObj.MTId;
                }
            }

            return roleId;
        }

        public void Save(UserVSBranch UserVSBranch)
        {
            GenericUnitOfWork editUOW = new GenericUnitOfWork();
            int checkExists = editUOW.Repository<UserVSBranch>().GetAll().Where(x => x.Id != UserVSBranch.Id && x.BranchId == UserVSBranch.BranchId && x.UserId == UserVSBranch.UserId).Count();
            if (checkExists > 0)
            {
                throw new Exception("Already Mapped User and Branch");
            }
            var parmanentTransferExist = editUOW.Repository<UserVSBranch>().FindBy(x => x.UserId == UserVSBranch.UserId && x.To == null).ToList();
            if (UserVSBranch.IsPermanent == true)
            {
                if (parmanentTransferExist.Count == 1)
                {
                    int branchId = parmanentTransferExist.SingleOrDefault().BranchId;
                    var branchName = editUOW.Repository<Company>().FindBy(x => x.CompanyId == branchId).SingleOrDefault();
                    throw new Exception("User Is Already Permanently Allocated to " + branchName.BranchName);
                }
            }

            if (UserVSBranch.Id == 0)
            {
                if (UserVSBranch.IsPermanent == true)
                {
                    UserVSBranch.To = null;
                }
                UserVSBranch.PostedBy = Loader.Models.Global.UserId;
                UserVSBranch.PostedOn = DateTime.Now;
                if (UserVSBranch.IsPermanent == true)
                {
                    UserVSBranch.To = null;
                }
                uow.Repository<UserVSBranch>().Add(UserVSBranch);
            }
            else
            {

                uow.Repository<UserVSBranch>().Edit(UserVSBranch);
            }
            uow.Commit();
        }

        public int GetCurrentBranchInt(int userId)
        {
            int currentBranch = 0;
            var branchObj = uow.Repository<Models.ApplicationUser>().FindBy(x => x.Id == userId).FirstOrDefault();
            if (branchObj != null)
            {
                if (branchObj.EmployeeId != 0) //superAdmin condition
                {
                    var employeeObj = uow.Repository<Models.Employee>().FindBy(x => x.EmployeeId == branchObj.EmployeeId).FirstOrDefault();
                    if (employeeObj != null)
                    {
                        if (employeeObj.BranchId != null && employeeObj.BranchId != 0)
                        {
                            currentBranch = Convert.ToInt32(employeeObj.BranchId);
                        }
                    }
                }
                else
                {
                    var UserVSBranch = uow.Repository<Models.UserVSBranch>().FindBy(x => x.UserId == branchObj.Id).FirstOrDefault();
                    if (UserVSBranch != null)
                    {
                        if (UserVSBranch.BranchId != 0)
                        {
                            currentBranch = Convert.ToInt32(UserVSBranch.BranchId);
                        }
                    }

                }
            }
            return currentBranch;
        }

        public UserBranchViewModel HasAnotherRole(int userId)
        {
            UserBranchViewModel _usrBrnchViewModel = new UserBranchViewModel();
            //problem is if user is superadmin, its branch isnt included in employee table thus userVsBranch must have its branch
            var allbranchList = uow.Repository<Models.UserVSBranch>().FindBy(x => x.UserId == userId).Where(x => x.To == null || x.To.Value.Date >= DateTime.Now.Date && x.IsEnable == true).ToList();
            //var allbranchList = uow.Repository<Models.UserVSBranch>().FindBy(x => x.UserId == userId && (x.To == null || x.To.Value.Date >= DateTime.Now.Date && x.IsEnable == true)).ToList();

            if (allbranchList.Count == 0)
            {
                var userDetails = uow.Repository<ApplicationUser>().FindBy(x => x.Id == userId).FirstOrDefault();

                var employeeid = userDetails.EmployeeId;
                var allbranchList2 = uow.Repository<Models.Employee>().FindBy(x => x.EmployeeId == employeeid).ToList();

                List<Branch> BranchList = new List<Branch>();
                foreach (var item in allbranchList2)
                {
                    BranchList.Add(new Branch { BranchId = item.BranchId.GetValueOrDefault(), BranchName = GetBranchName(item.BranchId.GetValueOrDefault()) });
                }
                _usrBrnchViewModel.Branch = BranchList;
            }
            else
            {
                List<Branch> BranchList = new List<Branch>();

                var userDetails = uow.Repository<ApplicationUser>().FindBy(x => x.Id == userId).FirstOrDefault();

                var employeeid = userDetails.EmployeeId;
                var allbranchList2 = uow.Repository<Models.Employee>().FindBy(x => x.EmployeeId == employeeid).ToList();
                //adding branch from employee
                if (userId != 1)
                {
                    foreach (var item in allbranchList2)
                    {
                        BranchList.Add(new Branch { BranchId = item.BranchId.GetValueOrDefault(), BranchName = GetBranchName(item.BranchId.GetValueOrDefault()) });
                    }
                    //adding branch from userVsbranch
                    List<Branch> TempBranchList = new List<Branch>(BranchList);

                    foreach (var item in allbranchList)
                    {
                        foreach (var branchitems in BranchList)
                        {
                            if (branchitems.BranchId != item.BranchId)
                            {
                                if (item.IsEnable == true)
                                {
                                    if (item.To == null || item.To >= DateTime.Now)
                                    {
                                        TempBranchList.Add(new Branch { BranchId = item.BranchId, BranchName = GetBranchName(item.BranchId), ToDate = item.To });
                                    }
                                }
                            }
                        }
                    }
                    BranchList.Clear();

                    foreach (var item in TempBranchList)
                    {
                        BranchList.Add(new Branch { BranchId = item.BranchId, BranchName = GetBranchName(item.BranchId), ToDate = item.ToDate });
                    }
                }
                else
                {
                    //adding branch from userVsbranch
                    foreach (var item in allbranchList)
                    {
                        if (item.IsEnable == true)
                        {
                            if (item.To == null || item.To >= DateTime.Now)
                            {
                                BranchList.Add(new Branch { BranchId = item.BranchId, BranchName = GetBranchName(item.BranchId), ToDate = item.To });
                            }
                        }
                    }
                }

                _usrBrnchViewModel.Branch = BranchList;
            }

            return _usrBrnchViewModel;
        }

        public bool Delete(int UserVSBranchId)
        {
            UserVSBranch UserVSBranch = this.GetSingle(UserVSBranchId);

            if (UserVSBranch != null)
            {
                uow.Repository<UserVSBranch>().Delete(UserVSBranch);
                uow.Commit();
                return true;
            }
            else
            {
                return false;
            }
        }

        public static int GetEmployeeIdByUserId()
        {
            using (GenericUnitOfWork uow = new GenericUnitOfWork())
            {
                var userId = Global.UserId;
                //ApplicationUser lai userid gareko xa
                var employeeId = uow.Repository<ApplicationUser>().FindBy(x => x.Id == userId).Select(x => x.EmployeeId).Single();
                var retEmp = employeeId ?? default(int);
                return retEmp;
            }
        }

        public static String GetEmployeeBranchName(int? branchId)
        {
            using (GenericUnitOfWork uow = new GenericUnitOfWork())
            {
               
                //ApplicationUser lai userid gareko xa
                var branchName = uow.Repository<LicenseBranch>().FindBy(x => x.BrnhID == branchId).Select(x => x.BrnhNam).Single();
                //var retEmp = employeeId ?? default(int);
                return branchName;
            }
        }

        public static SelectList GetBranchList()
        {
            List<LicenseBranch> BranchListName = new List<LicenseBranch>();
            //IEnumerable<SelectListItem> BranchListName;
            using (GenericUnitOfWork uow = new GenericUnitOfWork())
            {
                //Loader.Repository.GenericUnitOfWork luow = new Loader.Repository.GenericUnitOfWork();
                BranchSetupService branchService = new BranchSetupService();

                var userId = Global.UserId;
                var employeeId = uow.Repository<ApplicationUser>().FindBy(x => x.Id == userId).Select(x => x.EmployeeId).Single();

                //var branchListFromUserVsBranch = uow.Repository<UserVSBranch>().GetAll().Where(x => x.UserId == userId).Select(x => x.BranchId).ToList();


                var branchListIdFromUserVsBranch = uow.Repository<UserVSBranch>().GetAll().Where(x => (x.UserId == userId) && (x.IsEnable == true)).Select(x => x.BranchId).ToList();

                //  IEnumerable<int>

                var branchListIdFromEmployeenew = uow.Repository<Employee>().FindBy(x => x.EmployeeId == employeeId).Select(x => x.BranchId).ToList();
                var branchListIdFromUserVsBrancha = branchListIdFromEmployeenew.Cast<int>();
                //  IEnumerable<int?>

                var unionBranchIdList = branchListIdFromUserVsBranch.Union(branchListIdFromUserVsBrancha);


                //private List<int> unionBranchList=new List<int>;


                foreach (var item in unionBranchIdList)
                {
                    var lst = uow.Repository<LicenseBranch>().GetAll().Where(x => x.BrnhID == item).SingleOrDefault();
                    //  var lst = uow.Repository<LicenseBranch>().FindBy(x => x.BrnhID==item).Select(x => x.BrnhNam).Single();
                    if (lst != null)
                    {
                        BranchListName.Add(lst);
                    }

                }


                //unionBranchList = branchListFromUserVsBranch.Union(branchListFromEmployee);

                // var branchListFromEmployeeId = uow.Repository<Employee>().GetAll().Where(x=);

                //var BranchList = from u in uow.Repository<Users>().GetAll()
                //                 join e in uow.Repository<Employee>().FindBy(x => x.EmployeeId == employeeId).ToList()
                //                  // join e in uow.Repository<Employee>().GetAll() 
                //                  on u.EmployeeId equals e.EmployeeId
                //                 join ub in uow.Repository<UserVSBranch>().FindBy(x => x.UserId == userId).ToList()
                //                 on u.UserId equals ub.UserId
                //                 select new
                //                 {
                //                     employeebranchId = e.BranchId,
                //                     userBranchId = ub.BranchId
                //                 };






                //    List<SelectListItem> options = new List<SelectListItem>();

                //    options.Add(new SelectListItem { Text = "Optional", Value = "1" });
                //    options.Add(new SelectListItem { Text = "Compulsory", Value = "2" });
                //    return options;


                //foreach (var item in BranchList)
                //{

                //    var BranchNameFromUserBranch = uow.Repository<Loader.Models.LicenseBranch>().FindBy(x => x.BrnhID == item.userBranchId).Select(x => x.BrnhNam).Single();
                //    var BranchNameFromEmployee = uow.Repository<Loader.Models.LicenseBranch>().FindBy(x => x.BrnhID == item.employeebranchId).Select(x => x.BrnhNam).Single();

                //    li.Add(BranchNameFromUserBranch);
                //.Add(new SelectListItem {Text= uow.Repository<Loader.Models.LicenseBranch>().FindBy(x => x.BrnhID == item.branchId).Select(x => x.BrnhNam),Value=item.branchId.ToString()})  })

            }



            //var getAllCompanyBranch = branchService.GetAll();
            //var mainBranch = getAllCompanyBranch.Where(x => x.ParentId == 0);
            //var branchList = getAllCompanyBranch.Except(mainBranch).ToList();
            //return new SelectList(branchName, "CompanyId", "BranchName");
            return new SelectList(BranchListName, "BrnhID", "BrnhNam");

        }

        //UserBranchList
        public List<UserVSBranch> UserBranchList()
        {
            using (GenericUnitOfWork uow = new GenericUnitOfWork())
            {

                //ApplicationUser lai userid gareko xa
                // var branchName = uow.Repository<LicenseBranch>().FindBy(x => x.BrnhID == branchId).Select(x => x.BrnhNam).Single();
                //var retEmp = employeeId ?? default(int);
                return uow.Repository<UserVSBranch>().SqlQuery("select * from [ChannakyabaseMigrationTesting].[LG].[UserVSBranch] UB"
                + "inner join[ChannakyabaseMigrationTesting].[LG].[User] U on UB.Id = U.UserId where U.MTId <> 1 ").ToList();
               
            }
        }

        // GetUserVsBranchList

        public List<UserVSBranch> GetUserVsBranchList()
        {
            using (GenericUnitOfWork uow = new GenericUnitOfWork())
            {

                
                return uow.Repository<UserVSBranch>().SqlQuery("select * from [ChannakyabaseMigrationTesting].[LG].[UserVSBranch] UB").ToList();

            }
        }

    }
}