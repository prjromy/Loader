using Loader.Models;
using Loader.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Loader.Service
{
    public class BranchSetupService
    {
        private GenericUnitOfWork uow = null;


        public BranchSetupService()
        {
            uow = new GenericUnitOfWork();
        }

        public List<Company> GetAll()
        {
            return uow.Repository<Company>().GetAll().ToList();
        }
        public Nullable<DateTime> GetTransactionDateOfCurrentBranch(int branchId)
        {
            Nullable<DateTime> transDate = null;
            var transObj = uow.Repository<Company>().FindBy(x => x.CompanyId == branchId).FirstOrDefault();
            if(transObj!=null)
            {
                transDate = transObj.TDate;
                
            }
            return transDate;

        }

        public List<Company> GetAllOfParent(int parentId)
        {


            //return uow.Repository<Company>().GetAll().Where(x => x.ParentId == parentId).ToList().ForEach(s =>
            //{
            //    s.IsGroup == true;
            //});
            var allList = uow.Repository<Company>().GetAll().Where(x => x.ParentId == parentId).ToList();
            foreach (var item in allList)
            {
                item.IsGroup = IsBranchGroup(item.CompanyId);
                
            }
            return allList;
        }
        public string WithHeirarchy()
        {
            var dValue = uow.Repository<ParamValue>().GetAll().Where(x => x.PId == 14).Select(x => x.PValue).SingleOrDefault();
            return dValue;
        }

        public Company GetSingle(int BranchSetupId)
        {
            Company BranchSetup = uow.Repository<Company>().GetSingle(c => c.CompanyId == BranchSetupId);
            return BranchSetup;
        }

        public bool IsBranchGroup(int brnchId)
        {
            return uow.Repository<Models.Company>().GetAll().Any(x => x.ParentId == brnchId);
        }

        public void Save(Company BranchSetup)
        {
            GenericUnitOfWork editUOW = new GenericUnitOfWork();
            int checkExists = editUOW.Repository<Company>().GetAll().Where(x => x.BranchName == BranchSetup.BranchName).Count();
            if (checkExists > 0)
            {
                throw new Exception("Duplicate Branch Name Found. BranchName Caption Not Valid");
            }
            if (BranchSetup.CompanyId == 0)
            {
                if(BranchSetup.IsBranch==null)
                {
                    BranchSetup.IsBranch = false;
                }
                
                uow.Repository<Company>().Add(BranchSetup);
            }
            else
            {

                uow.Repository<Company>().Edit(BranchSetup);
            }
            uow.Commit();
        }

        public bool Delete(int BranchSetupId)
        {
            Company BranchSetup = this.GetSingle(BranchSetupId);

            if (BranchSetup != null)
            {
                uow.Repository<Company>().Delete(BranchSetup);
                uow.Commit();
                return true;
            }
            else
            {
                return false;
            }
        }

        public string GetAddress(int BranchSetupId)
        {
            string result = "";

            if (BranchSetupId != 0)
            {
                Company mnu = new Company();
                mnu = GetSingle(BranchSetupId);

                List<string> lst = new List<string>();


                while (mnu != null)
                {
                    lst.Add(mnu.BranchName);
                    mnu = GetSingle(mnu.ParentId);
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
                result = "ChannakyaSoft";
            }
            return result;
        }

        #region Tree

        private List<Company> FilterTree(List<Company> list, string filter)
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
        public ViewModel.TreeView GetBranchSetupGroupTree(string filter = "")
        {
            var treelist = uow.Repository<Company>().GetAll().Where(x=>x.IsBranch==true);
            List<Company> list = treelist.ToList();

            list.Add(new Company { CompanyId = 1, BranchName = "ChannakyaSoft", ParentId = 0 });

            if (filter.Trim() != "")
            {
                list = FilterTree(list, filter);
            }
            ViewModel.TreeView tree = this.GenerateTree(list, 0);
            return tree;
        }

        public ViewModel.TreeView GetBranchSetupGroupTree(int BranchSetupIdExpect, string filter = "")
        {
            List<Company> list = uow.Repository<Company>().GetAll().Where(x => x.CompanyId != BranchSetupIdExpect).ToList();
            list.Add(new Company { CompanyId = 1, BranchName = "ChannakyaSoft", ParentId = 0 });

            if (filter.Trim() != "")
            {
                list = FilterTree(list, filter);
            }
            ViewModel.TreeView tree = this.GenerateTree(list, 0);
            return tree;
        }


        public ViewModel.TreeView GetBranchSetupGroupTree(int parentId, int BranchSetupIdExpect, string filter = "")
        {
            List<Company> list = uow.Repository<Company>().GetAll().Where(x => x.CompanyId != BranchSetupIdExpect).ToList();

            if (filter.Trim() != "")
            {
                list = FilterTree(list, filter);
            }

            ViewModel.TreeView tree = this.GenerateTree(list, parentId);
            return tree;
        }
        

        private ViewModel.TreeView GenerateTree(List<Company> list, int? parentBranchSetupId)
        {

            var parent = list.Where(x => x.ParentId == parentBranchSetupId);
            ViewModel.TreeView tree = new ViewModel.TreeView();
            tree.Title = "BranchSetup";
            foreach (var itm in parent)
            {
                tree.TreeData.Add(new ViewModel.TreeDTO
                {
                    Id = itm.CompanyId,
                    
                    PId = itm.ParentId,
                    Text = itm.BranchName,

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