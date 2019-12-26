using Loader.Models;
using Loader.Repository;
using Loader.ViewModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;

namespace Loader.Service
{
    public class EmployeeService
    {
        private GenericUnitOfWork uow = null;
        Loader.Service.UserVSBranchService usrBrnchService = new UserVSBranchService();


        public EmployeeService()
        {
            uow = new GenericUnitOfWork();
        }
        public bool AllowDepartmentInGroup()
        {
            var dValue = Convert.ToBoolean(uow.Repository<ParamValue>().GetAll().Where(x => x.PId == 12).Select(x => x.PValue).SingleOrDefault());
            return dValue;
        }

        public List<Employee> GetAll()
        {
            return uow.Repository<Employee>().GetAll().ToList();
        }

        public Employee GetSingle(int EmployeeId)
        {
            Employee Employee = uow.Repository<Employee>().GetSingle(c => c.EmployeeId == EmployeeId);
            return Employee;
        }
        public bool HasDeptChilds(int deptId)
        {
            var Employee = uow.Repository<Department>().FindBy(x => x.PDeptId == deptId);
            bool hasChilds = false;
            if (Employee != null)
            {
                hasChilds = true;
            }
            return hasChilds;
        }


        public List<SelectListItem> GetEmployeeStatusList()
        {
            var statusList = uow.Repository<Status>().GetAll();

            List<SelectListItem> lst = new List<SelectListItem>();
            foreach (var item in statusList)
            {

                lst.Add(new SelectListItem { Text = item.StatusName, Value = item.StatusId.ToString() });


            }
            return lst;
        }


        public List<SelectListItem> GetBloodGroupOption()
        {
            List<SelectListItem> lst = new List<SelectListItem>();
            var list = uow.Repository<BloodGroup>().GetAll();
            foreach (var item in list)
            {
                lst.Add(new SelectListItem { Text = item.BGName, Value = item.BGId.ToString() });
            }
            return lst;
        }
        public List<SelectListItem> GetReligionOption()
        {
            List<SelectListItem> lst = new List<SelectListItem>();
            var list = uow.Repository<Religion>().GetAll().OrderBy(x=>x.ReligionName);
            foreach (var item in list)
            {
                lst.Add(new SelectListItem { Text = item.ReligionName, Value = item.RId.ToString() });
            }
            return lst;
        }

        public bool checkDoubleEmployeeNo(string employeeNo, int employeeId)
        {
            int count = uow.Repository<Employee>().FindBy(x => x.EmployeeName.Trim().ToLower() == employeeNo.Trim().ToLower() && x.EmployeeId != employeeId).Count();
            if (count == 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public List<SelectListItem> GetNationalityOption()
        {
            List<SelectListItem> lst = new List<SelectListItem>();
            var list = uow.Repository<Nationality>().GetAll().OrderBy(x=>x.NName);
            foreach (var item in list)
            {
                lst.Add(new SelectListItem { Text = item.NName, Value = item.NId.ToString() });
            }
            return lst;
        }
        public List<SelectListItem> GetMaritialStatusOption()
        {
            List<SelectListItem> lst = new List<SelectListItem>();
            var list = uow.Repository<MaritialStatus>().GetAll().OrderBy(x=>x.MSName);
            foreach (var item in list)
            {
                lst.Add(new SelectListItem { Text = item.MSName, Value = item.MSId.ToString() });
            }
            return lst;
        }
        public List<SelectListItem> GetStatusOption()
        {
            List<SelectListItem> lst = new List<SelectListItem>();
            var list = uow.Repository<Status>().GetAll().OrderBy(x=>x.StatusName);
            foreach (var item in list)
            {
                lst.Add(new SelectListItem { Text = item.StatusName, Value = item.StatusId.ToString() });
            }
            return lst;
        }
        public List<SelectListItem> GetGenderOption()
        {
            List<SelectListItem> lst = new List<SelectListItem>();
            var list = uow.Repository<Gender>().GetAll().OrderBy(x=>x.GenderName);
            foreach (var item in list)
            {
                lst.Add(new SelectListItem { Text = item.GenderName, Value = item.GenderId.ToString() });
            }
            return lst;
        }
        public void Save(Employee TemplateData)
        {
            GenericUnitOfWork editUOW = new GenericUnitOfWork();

            var withDesg = WithDesignation();
            var withDept = WithDepartment();
            if (TemplateData.EmployeeNo == null || TemplateData.EmployeeName == null || (withDesg == true && TemplateData.DGId == 0) || (withDept == true && TemplateData.DeptId == 0) || TemplateData.BranchId == 0)
            {
                throw new Exception("The Required Feild are Empty");
            }

            if (TemplateData.EmployeeId == 0)
            {
                //int checkExists = editUOW.Repository<Employee>().GetAll().Where(x => x.EmployeeNo == TemplateData.EmployeeNo || x.EmployeeId == TemplateData.EmployeeId).Count();
                //if (checkExists > 0)
                //{
                //    throw new Exception("Duplicate Employee Found. Employee Number Not Valid");
                //}
                TemplateData.DateOfJoin = Convert.ToDateTime(TemplateData.DateOfJoin);
                TemplateData.PostedOn = DateTime.Now;
                TemplateData.PostedBy = Global.UserId;
          //      TemplateData.ModifiedOn = DateTime.Now;
                uow.Repository<Employee>().Add(TemplateData);
            }
            else
            {
                //checking the newly entered employee number
                var checkEmployeeNo = editUOW.Repository<Employee>().GetAll().Where(x => x.EmployeeNo == TemplateData.EmployeeNo).ToList();
                foreach (var item in checkEmployeeNo)
                {
                    if (TemplateData.EmployeeId != item.EmployeeId)
                    {
                        throw new Exception("New Employee Number Not Valid");
                    }
                }

                //updating the user table for isActivee status
                var userDetails = uow.Repository<ApplicationUser>().GetAll().Where(x => x.EmployeeId == TemplateData.EmployeeId);
                foreach (var item in userDetails)
                {
                    if (TemplateData.Status == 1)
                    {
                        item.IsActive = true;
                    }
                    else
                    {
                        item.IsActive = false;
                    }
                    uow.Repository<ApplicationUser>().Edit(item);
                }

                if (TemplateData.Photo == null)
                {
                    byte[] ImageContent = editUOW.Repository<Employee>().GetSingle(x => x.EmployeeId == TemplateData.EmployeeId).Photo;
                    TemplateData.Photo = ImageContent;
                }

                TemplateData.ModifiedBy = Global.UserId;
                TemplateData.ModifiedOn = DateTime.Now;

                uow.Repository<Employee>().Edit(TemplateData);
            }
            uow.Commit();
        }
        public List<Employee> GetEmployeeList()
        {
            List<Employee> lstEmployee = new List<Employee>();
            return lstEmployee;
        }
        public bool Delete(int EmployeeId)
        {
            Employee Employee = this.GetSingle(EmployeeId);
            //string query = "select count(cid) as Count from acc.SubsiDetail where CId=" + EmployeeId + " ";
            ////var count = uow.Repository<Employee>().SqlQuery(query);
            //var alreadyMapped = uow.GetContext().Database.SqlQuery<int>(query).ToArray()[0];
            var alreadyMapped = mappedCount(EmployeeId);
            if (Employee != null && alreadyMapped == 0)
            {
                uow.Repository<Employee>().Delete(Employee);
                uow.Commit();
                return true;
            }
            else
            {
                return false;
            }
        }
        public int mappedCount(int EmployeeId)
        {
            string query = "select count(cid) as Count from acc.SubsiDetail where CId=" + EmployeeId + " ";
            int alreadyMapped = uow.GetContext().Database.SqlQuery<int>(query).ToArray()[0];
            return alreadyMapped;
        }


        public string GetStatusName(int statusId)
        {
            //string name = uow.Repository<Status>().GetSingle(x => x.StatusId == statusId).StatusName;
            string name = uow.Repository<Status>().FindBy(x => x.StatusId == statusId).Select(x => x.StatusName).FirstOrDefault();
            return name;
        }
        public string GetDepartmentName(int? deptId)
        {
            string name = uow.Repository<Department>().FindBy(x => x.DeptId == deptId).Select(x => x.DeptName).FirstOrDefault();
            return name;
        }
        public string GetDesignationName(int? DGId)
        {
            string name = uow.Repository<Designation>().FindBy(x => x.DGId == DGId).Select(x => x.DGName).FirstOrDefault();
            return name;
        }
        //public string GetAddress(int EmployeeId)
        //{
        //    string result = "";

        //    if (EmployeeId != 0)
        //    {
        //        Employee mnu = new Employee();
        //        mnu = GetSingle(EmployeeId);

        //        List<string> lst = new List<string>();


        //        while (mnu != null)
        //        {
        //            lst.Add(mnu.EmployeeName);
        //            mnu = GetSingle(mnu.PEmployeeId);
        //        };

        //        var sorted = lst.Select((x, i) => new KeyValuePair<string, int>(x, i)).OrderByDescending(x => x.Value).ToList();

        //        foreach (var item in sorted)
        //        {
        //            if (result == "")
        //            {
        //                result = result + item.Key;
        //            }
        //            else
        //            {
        //                result = result + "/" + item.Key;
        //            }

        //        }
        //    }
        //    else
        //    {
        //        result = "Root";
        //    }
        //    return result;
        //}

        //to update Transcation date
        public string GetTranscationDate(int FYID)
        {
            int BrnhID = Loader.Models.Global.BranchId;

            string query = "select * from fin.LicenseBranch where BrnhID=" + BrnhID + "";
            LicenseBranch GetSingle = uow.Repository<LicenseBranch>().GetAll().Where(x => x.BrnhID == BrnhID).SingleOrDefault();
            int BrnhFYID = Convert.ToInt32(GetSingle.FYID);
            DateTime TDate = (DateTime)GetSingle.TDate;
            DateTime MigDate = (DateTime)GetSingle.MigDate;
            var Currentdate = uow.Repository<FiscalYear>().GetAll().Where(x => x.StartDt <= TDate && x.EndDt >= TDate).SingleOrDefault();
            var MigrationDate = uow.Repository<FiscalYear>().GetAll().Where(x => x.StartDt <= MigDate && x.EndDt >= MigDate).SingleOrDefault();
            //var allFyearLists = uow.Repository<FiscalYear>().SqlQuery("select * from lg.FiscalYears where FYID between '" + MigrationDate.FYID + "' and '" + Currentdate.FYID + "' order by FYID desc").ToList();
            var allFyearLists = uow.Repository<FiscalYear>().GetAll().Where(x => x.FYID >= MigrationDate.FYID && x.FYID <= Currentdate.FYID).OrderByDescending(x => x.FYID).ToList();

            var Tlist = "";
            if (FYID == allFyearLists.FirstOrDefault().FYID)
            {
                Tlist = Convert.ToDateTime(TDate).ToShortDateString();
            }
            else
            {
                Tlist = allFyearLists.Find(x => x.FYID == FYID).EndDt.ToShortDateString().ToString();
            }

            return Tlist;
        }

        public TreeView GetDepartmentTreeWithoutGroup(string filter = "")
        {
            List<Department> treelist = uow.Repository<Department>().FindBy(x=>x.PDeptId==0).ToList();


            //list.Add(new Department { DeptId = 0, DeptName = "Root", PDeptId = -1 });

            if (filter.Trim() != "")
            {
                treelist = FilterDepartmentTree(treelist, filter);
            }
            ViewModel.TreeView tree = this.GenerateDepartmentTree(treelist, 0);
            return tree;
        }

        #region Tree

        List<int> templateList = new List<int>();
        public List<int> CheckedEmployee(TreeView data)
        {

            foreach (var item in data.TreeData)
            {
                if (item.IsChecked == true)
                {
                    templateList.Add(item.Id);
                }
                if (item.Children != null)
                {
                    TemplateChild(item);
                }

            }
            return templateList;

        }
        private void TemplateChild(TreeDTO data)
        {
            foreach (var item in data.Children)
            {
                if (item.IsChecked == true)
                {
                    templateList.Add(item.Id);

                }
                if (item.Children != null)
                {
                    data.Children = item.Children;
                    TemplateChild(data);
                }
            }

        }

        private List<Company> FilterBranchTree(List<Company> list, string filter)
        {
            bool lLoop = true;

            var filteredList = list.Where(x => x.BranchName.ToLower().Contains(filter.ToLower())).ToList();

            while (lLoop)
            {
                //select all parents of filtered list
                var allParent = (from mainList in list
                                 join selList in filteredList on mainList.CompanyId equals selList.ParentId
                                 select mainList).ToList();

                //Select unique parent only
                var parentList = (from p in allParent
                                  join c in filteredList on p.CompanyId equals c.CompanyId into gj
                                  from uniqueParent in gj.DefaultIfEmpty()
                                  where uniqueParent == null
                                  select p).ToList();

                if (parentList.Count() == 0)
                {
                    lLoop = false;
                }

                filteredList = filteredList.Union(parentList).OrderBy(x => x.CompanyId).ToList();
            }
            list = filteredList;
            return list;

        }
        private List<Department> FilterDepartmentTree(List<Department> list, string filter)
        {
            bool lLoop = true;

            var filteredList = list.Where(x => x.DeptName.ToLower().Contains(filter.ToLower())).ToList();

            while (lLoop)
            {
                //select all parents of filtered list
                var allParent = (from mainList in list
                                 join selList in filteredList on mainList.DeptId equals selList.PDeptId
                                 select mainList).ToList();

                //Select unique parent only
                var parentList = (from p in allParent
                                  join c in filteredList on p.DeptId equals c.DeptId into gj
                                  from uniqueParent in gj.DefaultIfEmpty()
                                  where uniqueParent == null
                                  select p).ToList();

                if (parentList.Count() == 0)
                {
                    lLoop = false;
                }

                filteredList = filteredList.Union(parentList).OrderBy(x => x.DeptId).ToList();
            }
            list = filteredList;
            return list;

        }
        public ViewModel.TreeView GetDepartmentGroupTree(string filter = "")
        {
            var treelist = uow.Repository<Department>().GetAll();
            List<Department> list = treelist.ToList();

            //list.Add(new Department { DeptId = 0, DeptName = "Root", PDeptId = -1 });

            if (filter.Trim() != "")
            {
                list = FilterDepartmentTree(list, filter);
            }
            ViewModel.TreeView tree = this.GenerateDepartmentTree(list, 0);
            return tree;
        }

        public ViewModel.TreeView GetBranchGroupTree(string filter = "")
        {
            var treelist = uow.Repository<Company>().GetAll();
            List<Company> list = treelist.ToList();

            //list.Add(new Department { DeptId = 0, DeptName = "Root", PDeptId = -1 });

            if (filter.Trim() != "")
            {
                list = FilterBranchTree(list, filter);
            }
            ViewModel.TreeView tree = this.GenerateBranchTree(list, 0);
            return tree;
        }
        private ViewModel.TreeView GenerateBranchTree(List<Company> list, int? parentDepartmentId)
        {

            var parent = list.Where(x => x.ParentId == parentDepartmentId);
            ViewModel.TreeView tree = new ViewModel.TreeView();
            tree.Title = "Department";
            foreach (var itm in parent)
            {
                tree.TreeData.Add(new ViewModel.TreeDTO
                {
                    Id = itm.CompanyId,
                    PId = itm.ParentId,
                    Text = usrBrnchService.GetAddressInCompany(itm.CompanyId),

                });
            }

            foreach (var itm in tree.TreeData)
            {
                itm.Children = GenerateBranchTree(list, itm.Id).TreeData.ToList();
            }
            return tree;
        }
        private ViewModel.TreeView GenerateDepartmentTree(List<Department> list, int? parentDepartmentId)
        {

            var parent = list.Where(x => x.PDeptId == parentDepartmentId);
            ViewModel.TreeView tree = new ViewModel.TreeView();
            tree.Title = "Department";
            foreach (var itm in parent)
            {
                tree.TreeData.Add(new ViewModel.TreeDTO
                {
                    Id = itm.DeptId,
                    PId = itm.PDeptId,
                    Text = itm.DeptName,

                });
            }

            foreach (var itm in tree.TreeData)
            {
                itm.Children = GenerateDepartmentTree(list, itm.Id).TreeData.ToList();
            }
            return tree;
        }

        public bool CheckEmployeeNumberDuplication(string employee, int myId = 0)
        {
            int count = uow.Repository<Employee>().GetAll().Where(x => x.EmployeeNo == employee).Count();
            if (count == 0)
            {
                return true;
            }
            else
            {

                return false;
            }
        }



        private List<Designation> FilterDesignationTree(List<Designation> list, string filter)
        {
            bool lLoop = true;

            var filteredList = list.Where(x => x.DGName.ToLower().Contains(filter.ToLower())).ToList();

            while (lLoop)
            {
                //select all parents of filtered list
                var allParent = (from mainList in list
                                 join selList in filteredList on mainList.DGId equals selList.PDGId
                                 select mainList).ToList();

                //Select unique parent only
                var parentList = (from p in allParent
                                  join c in filteredList on p.DGId equals c.DGId into gj
                                  from uniqueParent in gj.DefaultIfEmpty()
                                  where uniqueParent == null
                                  select p).ToList();

                if (parentList.Count() == 0)
                {
                    lLoop = false;
                }

                filteredList = filteredList.Union(parentList).OrderBy(x => x.DGId).ToList();
            }
            list = filteredList;
            return list;

        }
        public ViewModel.TreeView GetDesignationGroupTree(string filter = "")
        {
           // var superAdminDesignation = uow.Repository<Designation>().FindBy(x => x.DGId == 1).Select(x => x.DGName).SingleOrDefault();
            var treelist = uow.Repository<Designation>().GetAll();
            List<Designation> list = treelist.ToList();

            //list.Add(new Designation { DGId = 0, DGName = "Root", PDGId = -1 });

            if (filter.Trim() != "")
            {
                list = FilterDesignationTree(list, filter);
            }
            ViewModel.TreeView tree = this.GenerateDesignationTree(list, 0);
            return tree;
        }
        private ViewModel.TreeView GenerateDesignationTree(List<Designation> list, int? parentDesignationId)
        {

            var parent = list.Where(x => x.PDGId == parentDesignationId);
            ViewModel.TreeView tree = new ViewModel.TreeView();
            tree.Title = "Designation";
            foreach (var itm in parent)
            {
                bool isGrp = false;
                int childs = IsGroup(itm.DGId, list);
                if (childs > 0)
                {
                    isGrp = true;
                }
                tree.TreeData.Add(new ViewModel.TreeDTO
                {
                    Id = itm.DGId,
                    PId = itm.PDGId,
                    Text = itm.DGName,
                    IsGroup = isGrp

                });
            }

            foreach (var itm in tree.TreeData)
            {
                itm.Children = GenerateDesignationTree(list, itm.Id).TreeData.ToList();
            }
            return tree;
        }
        private int IsGroup(int DGid, List<Designation> list)
        {
            int childs = list.Where(x => x.PDGId == DGid).Select(x => x.DGId).Count();
            return childs;
        }
        public bool WithPayrollParameters()
        {
            string withDes = uow.Repository<ParamValue>().GetAll().Where(x => x.PId == 19).Select(x => x.PValue).SingleOrDefault();
            bool desc = withDes.Equals("true") ? true : false;
            return desc;
        }
        public bool WithDesignation()
        {
            string withDes = uow.Repository<ParamValue>().GetAll().Where(x => x.PId == 18).Select(x => x.PValue).SingleOrDefault();
            bool desc = withDes.Equals("true") ? true : false;
            return desc;
        }
        public bool WithDepartment()
        {
            string withGrp = uow.Repository<ParamValue>().GetAll().Where(x => x.PId == 17).Select(x => x.PValue).SingleOrDefault();
            bool desc = withGrp.Equals("true") ? true : false;
            return desc;
        }

        public string getCompanyName()
        {
            string CompanyName = uow.Repository<ParamValue>().FindBy(x => x.PId == 26).Select(x => x.PValue).SingleOrDefault();
            return CompanyName;
        }

        public Employee GetEmployeePhoto(int id)


        {

            try
            {
                var EmployeeID = uow.Repository<ApplicationUser>().FindBy(x => x.Id == id).Select(x=>x.EmployeeId). SingleOrDefault();
                var Photo = uow.Repository<Employee>().FindBy(x => x.EmployeeId == EmployeeID).Select(x => new Employee()
                {
                    StringPhoto = string.Format("data:image/png;base64,{0}", Convert.ToBase64String(x.Photo))

                }).SingleOrDefault();
               

                return Photo;

            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion
    }
}