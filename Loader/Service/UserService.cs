using Loader.Models;
using Loader.Repository;
using Loader.ViewModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using PagedList;
using PagedList.Mvc;
using System.Web.Mvc;
using Loader.Hubs;

namespace Loader.Service
{
    public class UsersService
    {
        private GenericUnitOfWork uow = null;

        public UsersService()
        {
            uow = new GenericUnitOfWork();


        }

        public List<Users> GetAll()
        {
            return uow.Repository<Users>().GetAll().ToList();
        }

        public Users GetSingle(int UsersId)
        {
            Users Users = uow.Repository<Users>().GetSingle(c => c.UserId == UsersId);
            return Users;
        }
        public string GetMenuTemplateName(int MTId)
        {
            var name = uow.Repository<MenuTemplate>().GetSingle(x => x.MTId == MTId).MTName;
            return name;

        }
        public string GetDepartmentName(int? Id)
        {
            var name = "";
            if (Id != 0)
            {
                name = uow.Repository<Department>().GetSingle(x => x.DeptId == Id).DeptName;
            }
            return name;

        }
        public string GetDesignationName(int? Id)
        {
            var name = "";
            if (Id != 0)
            {
                name = uow.Repository<Designation>().GetSingle(x => x.DGId == Id).DGName;
            }
            return name;

        }

        //Email Duplication check
        public bool CheckEmail(string email, int Id = 0)
        {
            int count = uow.Repository<ApplicationUser>().FindBy(x => x.Email.Trim().ToLower() == email.Trim().ToLower() && x.Id != Id).Count();
            if (count == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //Username Dulication check
        public bool CheckUsername(string username, int Id = 0)
        {
            int count = uow.Repository<ApplicationUser>().FindBy(x => x.UserName.Trim().ToLower() == username.Trim().ToLower() && x.Id != Id).Count();
            if (count == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //user status dropdownlist 
        public List<SelectListItem> GetUserStatusList()
        {
            List<SelectListItem> lst = new List<SelectListItem>();

                lst.Add(new SelectListItem { Text = "Active", Value = "1"});
                lst.Add(new SelectListItem { Text = "Inactive", Value = "0"});
                return lst;

            //List<SelectListItem> lst = new List<SelectListItem>();
            //lst.Add(new SelectListItem
            //{
            //    Text = "Select Status",
            //    Value = ""
            //});
            //lst.Add(new SelectListItem
            //{
            //    Text = "Active",
            //    Value = "1",
            //    //Selected=(Model.IsActive==true )
            //    Selected = true
            //});
            //lst.Add(new SelectListItem
            //{
            //    Text = "InActive",
            //    Value = "0",
            //    //Selected=(Model.IsActive==false)
            //    Selected = false
            //});
           // return lst;


        }



        public void Save(Users userData)
        {
            try
            {
                RegisterViewModel createDTO = new RegisterViewModel();
                ApplicationUser usersDTO = new ApplicationUser();
                GenericUnitOfWork editUOW = new GenericUnitOfWork();
                int checkExists = editUOW.Repository<Users>().GetAll().Where(x => x.UserId != userData.UserId && x.UserName == userData.UserName).Count();
                if (checkExists > 0)
                {
                    throw new Exception("Duplicate Users Found. Users Caption Not Valid");
                }
                if (userData.UserId == 0)
                {
                    if (userData.IsUnlimited == true)
                    {
                        userData.From = Convert.ToDateTime(null);
                        userData.To = Convert.ToDateTime(null);

                    }
                    RegisterViewModel regObj = new RegisterViewModel();
                    regObj.Email = userData.Email;
                    regObj.IsUnlimited = userData.IsUnlimited;
                    regObj.Password = userData.Password;
                    regObj.ConfirmPassword = userData.ReEnterPassword;
                    createDTO.IsActive = usersDTO.IsActive;
                    //uow.Repository<Users>().Add(userData);
                }
                else
                {
                    Users userEdit = new Users();
                    var password = editUOW.Repository<Users>().GetSingle(x => x.UserId == userData.UserId).Password;
                    userData.Password = password;
                    userData.ReEnterPassword = password;
                    createDTO.IsActive = usersDTO.IsActive;

                    uow.Repository<Users>().Edit(userData);
                }
                uow.Commit();
            }
            catch (Exception ex)
            {
                throw ex;

            }
        }
        public List<Users> GetUsersList()
        {
            List<Users> lstUsers = new List<Users>();
            return lstUsers;
        }
        public bool Delete(int UsersId)
        {
            Users Users = this.GetSingle(UsersId);

            if (Users != null)
            {
                uow.Repository<Users>().Delete(Users);
                uow.Commit();
                return true;
            }
            else
            {
                return false;
            }
        }

        //public string GetAddress(int UsersId)
        //{
        //    string result = "";

        //    if (UsersId != 0)
        //    {
        //        Users mnu = new Users();
        //        mnu = GetSingle(UsersId);

        //        List<string> lst = new List<string>();


        //        while (mnu != null)
        //        {
        //            lst.Add(mnu.UsersName);
        //            mnu = GetSingle(mnu.PUsersId);
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
        public int ConnectUser(string ConnectionID, string Username)
        {
            try
            {
                Users objUserDetails = uow.Repository<Users>().SqlQuery("select UserId from [LG].[User] where UserName='" + Username + "'").First();

                int checkExists = uow.Repository<UserConnection>().GetAll().Where(x => x.UserId == objUserDetails.UserId).Count();

                //UserConnection objUser = new UserConnection
                //{
                //  //  UserId = objUserDetails.UserId,
                //    checkExist.ConnectionID = ConnectionID
                //};

                if (checkExists > 0)
                {
                    var objEdit = uow.Repository<UserConnection>().GetAll().Where(x => x.UserId == objUserDetails.UserId).SingleOrDefault();
                    objEdit.ConnectionID = ConnectionID;
                    uow.Repository<UserConnection>().Edit(objEdit);
                }
                else
                {

                    UserConnection objUser = new UserConnection
                    {
                        UserId = objUserDetails.UserId,
                        ConnectionID = ConnectionID
                    };
                    uow.Repository<UserConnection>().Add(objUser);
                }
                uow.Commit();
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }


        }


        public string WithDesignation()
        {
            string withDes = uow.Repository<ParamValue>().GetAll().Where(x => x.PId == 16).Select(x => x.PValue).SingleOrDefault();
            return withDes;
        }

        public string WithDesignationForUser()
        {
            string withDes = uow.Repository<ParamValue>().GetAll().Where(x => x.PId == 9073).Select(x => x.PValue).SingleOrDefault();
            return withDes;
        }

        public string AllowSelectDesignationGroup()
        {
            string withGrp = uow.Repository<ParamValue>().FindBy(x => x.PId == 9073).Select(x => x.PValue).SingleOrDefault();
            return withGrp;
        }

        #region Tree

        List<int> templateList = new List<int>();
        public List<int> CheckedUsers(TreeView data)
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
        private List<MenuTemplate> FilterMenuTemplateTree(List<MenuTemplate> list, string filter)
        {
            bool lLoop = true;

            var filteredList = list.Where(x => x.MTName.ToLower().Contains(filter.ToLower())).ToList();

            while (lLoop)
            {
                //select all parents of filtered list
                var allParent = (from mainList in list
                                 join selList in filteredList on mainList.MTId equals selList.MTId
                                 select mainList).ToList();

                //Select unique parent only
                var parentList = (from p in allParent
                                  join c in filteredList on p.MTId equals c.MTId into gj
                                  from uniqueParent in gj.DefaultIfEmpty()
                                  where uniqueParent == null
                                  select p).ToList();

                if (parentList.Count() == 0)
                {
                    lLoop = false;
                }

                filteredList = filteredList.Union(parentList).OrderBy(x => x.MTId).ToList();
            }
            list = filteredList;
            return list;

        }
        public ViewModel.TreeView GetMenuTemplateGroupTree(string filter = "")
        {
            var treelist = uow.Repository<MenuTemplate>().GetAll();
            // var treelist = uow.Repository<MenuTemplate>().GetAll().Where(x => x.DesignationId == designationId);
            List<MenuTemplate> list = treelist.ToList();

            //list.Add(new MenuTemplate { DeptId = 0, DeptName = "Root", PDeptId = -1 });

            if (filter.Trim() != "")
            {
                list = FilterMenuTemplateTree(list, filter);
            }
            ViewModel.TreeView tree = this.GenerateMenuTemplateTree(list, -1);
            return tree;
        }

        public ViewModel.TreeView GetMenuTemplateGroupTreeForSpecificDesignation(string filter = "",int designationId=0)
        {
            //var treelist = uow.Repository<MenuTemplate>().GetAll();
            var treelist = uow.Repository<MenuTemplate>().GetAll().Where(x => x.DesignationId == designationId).Distinct();
            List<MenuTemplate> list = treelist.ToList();

            //list.Add(new MenuTemplate { DeptId = 0, DeptName = "Root", PDeptId = -1 });

            if (filter.Trim() != "")
            {
                list = FilterMenuTemplateTree(list, filter);
            }
            ViewModel.TreeView tree = this.GenerateMenuTemplateTree(list, -1);
            return tree;
        }




        private ViewModel.TreeView GenerateMenuTemplateTree(List<MenuTemplate> list, int? parentMenuTemplate)
        {

            //var parent = list.Where(x => x.MTId == parentDepartmentId);
            ViewModel.TreeView tree = new ViewModel.TreeView();
            tree.Title = "Menu Template";
            foreach (var itm in list)
            {
                if (itm.MTId == 1)
                {
                    if (Loader.Models.Global.UserId == 1)
                    {
                        tree.TreeData.Add(new ViewModel.TreeDTO
                        {
                            Id = itm.MTId,
                            Text = itm.MTName,

                        });
                    }
                }
                else
                {
                    tree.TreeData.Add(new ViewModel.TreeDTO
                    {
                        Id = itm.MTId,
                        Text = itm.MTName,

                    });
                }

            }

            return tree;
        }

        private List<Employee> FilterEmployeeTree(List<Employee> list, string filter)
        {
            bool lLoop = true;

            var filteredList = list.Where(x => x.EmployeeName.ToLower().Contains(filter.ToLower())).ToList();

            while (lLoop)
            {
                //select all parents of filtered list
                var allParent = (from mainList in list
                                 join selList in filteredList on mainList.DGId equals selList.EmployeeId
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
        public ViewModel.SearchViewModel GetEmployeeGroupTree(string filter = "", List<Employee> list = null)
        {


            //var treelist = uow.Repository<Employee>().GetAll().Skip(pageFinal * PageSize).Take(PageSize);

            if (filter.Trim() != "")
            {
                list = FilterEmployeeTree(list, filter);
            }
            ViewModel.SearchViewModel tree = this.GenerateEmployeeTree(list, 2);
            return tree;
        }

        private ViewModel.SearchViewModel GenerateEmployeeTree(List<Employee> list, int? parentDesignationId)
        {
            ViewModel.SearchViewModel tree = new ViewModel.SearchViewModel();
            tree.Title = "Designation";

            foreach (var itm in list)
            {

                tree.SearchData.Add(new SearchDTO
                {
                    Address = "",
                    Id = itm.EmployeeId,
                    EmpNo = itm.EmployeeNo,
                    PhoneNumber = "",
                    Text = itm.EmployeeName,

                    DeptId = GetDepartmentName(itm.DeptId == null ? 0 : itm.DeptId),

                    DGId = GetDesignationName(itm.DGId == null ? 0 : itm.DGId)
                });

            }
            return tree;
        }

        public bool WithEmployee()
        {
            string withDes = uow.Repository<ParamValue>().GetAll().Where(x => x.PId == 21).Select(x => x.PValue).SingleOrDefault();
            bool desc = withDes.Equals("true") ? true : false;
            return desc;
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
            var treelist = uow.Repository<Designation>().GetAll().Where(x=>x.DGId!=1);
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

        #endregion
    }
}