using Loader.Models;
using Loader.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Loader.Service
{
    public class DepartmentService
    {
        private GenericUnitOfWork uow = null;


        public DepartmentService()
        {
            uow = new GenericUnitOfWork();
        }

        public List<Department> GetAll()
        {
            return uow.Repository<Department>().GetAll().ToList();
        }

        public List<Department> GetAllOfParent(int parentId)
        {


            return uow.Repository<Department>().GetAll().Where(x => x.PDeptId == parentId).ToList();
        }
        public string WithHeirarchy()
        {
            var dValue = uow.Repository<ParamValue>().GetAll().Where(x => x.PId == 14).Select(x => x.PValue).SingleOrDefault();
            return dValue;
        }

        public Department GetSingle(int DepartmentId)
        {
            Department Department = uow.Repository<Department>().GetSingle(c => c.DeptId == DepartmentId);
            return Department;
        }

        public void Save(Department Department)
        {
            GenericUnitOfWork editUOW = new GenericUnitOfWork();
            string departmentName = Department.DeptName.ToLower();
            int checkExists = editUOW.Repository<Department>().GetAll().Where(x => x.DeptId != Department.DeptId && x.DeptName.ToLower().Equals(departmentName) && x.PDeptId == Department.PDeptId).Count();
            if (checkExists > 0)
            {
                throw new Exception("Duplicate Department Found. Department Caption Not Valid");
            }
            if (Department.DeptId == 0)
            {
                Department.PostedBy = Global.UserId;
                Department.PostedOn = DateTime.Now;
                uow.Repository<Department>().Add(Department);
            }
            else
            {

                uow.Repository<Department>().Edit(Department);
            }
            uow.Commit();
        }

        public bool Delete(int DepartmentId)
        {
            Department Department = this.GetSingle(DepartmentId);

            if (Department != null)
            {
                uow.Repository<Department>().Delete(Department);
                uow.Commit();
                return true;
            }
            else
            {
                return false;
            }
        }

        public string GetAddress(int DepartmentId)
        {
            string result = "";

            if (DepartmentId != 0)
            {
                Department mnu = new Department();
                mnu = GetSingle(DepartmentId);

                List<string> lst = new List<string>();


                while (mnu != null)
                {
                    lst.Add(mnu.DeptName);
                    mnu = GetSingle(mnu.PDeptId);
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
                        result = result + "/" + item.Key;
                    }

                }
            }
            else
            {
                result = "Root";
            }
            return result;
        }

        #region Tree

        private List<Department> FilterTree(List<Department> list, string filter)
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

            list.Add(new Department { DeptId = 0, DeptName = "Root", PDeptId = -1 });

            if (filter.Trim() != "")
            {
                list = FilterTree(list, filter);
            }
            ViewModel.TreeView tree = this.GenerateTree(list, -1);
            return tree;
        }

        public ViewModel.TreeView GetDepartmentGroupTree(int DepartmentIdExpect, string filter = "")
        {
            //List<Department> list = new List<Department>();
            //if (DepartmentIdExpect!=0)
            //{
            //    list=uow.Repository<Department>().GetAll().Where(x=>!)
            //}
            //    else
            //{
                List<Department> list  = uow.Repository<Department>().GetAll().Where(x => x.DeptId != DepartmentIdExpect).ToList();
          
           
            list.Add(new Department { DeptId = 0, DeptName = "Root", PDeptId = -1 });

            if (filter.Trim() != "")
            {
                list = FilterTree(list, filter);
            }
            ViewModel.TreeView tree = this.GenerateTree(list, -1);
            return tree;
        }


        public ViewModel.TreeView GetDepartmentGroupTree(int parentId, int DepartmentIdExpect, string filter = "")
        {
            List<Department> list = uow.Repository<Department>().GetAll().Where(x => x.DeptId != DepartmentIdExpect).ToList();

            if (filter.Trim() != "")
            {
                list = FilterTree(list, filter);
            }

            ViewModel.TreeView tree = this.GenerateTree(list, parentId);
            return tree;
        }

        private ViewModel.TreeView GenerateTree(List<Department> list, int? parentDepartmentId)
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
                itm.Children = GenerateTree(list, itm.Id).TreeData.ToList();
            }
            return tree;
        }
        #endregion
    }
}