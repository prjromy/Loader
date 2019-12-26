using Loader.Models;
using Loader.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Loader.Service
{
    public class DesignationService
    {
        private GenericUnitOfWork uow = null;


        public DesignationService()
        {
            uow = new GenericUnitOfWork();
        }

        public List<Designation> GetAll()
        {
            return uow.Repository<Designation>().GetAll().ToList();
        }

        public List<Designation> GetAllOfParent(int parentId)
        {
            
           
            return uow.Repository<Designation>().GetAll().Where(x => x.PDGId == parentId).ToList();
        }
        public string WithHeirarchy()
        {
            var dValue = uow.Repository<ParamValue>().GetAll().Where(x => x.PId == 10).Select(x => x.PValue).SingleOrDefault();
            return dValue;
        }

        public Designation GetSingle(int DesignationId)
        {
            Designation Designation = uow.Repository<Designation>().GetSingle(c => c.DGId == DesignationId);
            return Designation;
        }

        public void Save(Designation Designation)
        {
            GenericUnitOfWork editUOW = new GenericUnitOfWork();
            int checkExists = editUOW.Repository<Designation>().GetAll().Where(x => x.DGId != Designation.DGId && x.DGName == Designation.DGName && x.PDGId == Designation.PDGId).Count();
            if (checkExists > 0)
            {
                throw new Exception("Duplicate Designation Found. Designation Caption Not Valid");
            }
            if (Designation.DGId == 0)
            {
                Designation.PostedOn = DateTime.Now ;
                Designation.PostedBy = Global.UserId;
                uow.Repository<Designation>().Add(Designation);
            }
            else
            {
                uow.Repository<Designation>().Edit(Designation);
            }
            uow.Commit();
        }

        public bool Delete(int DesignationId)
        {
            Designation Designation = this.GetSingle(DesignationId);

            if (Designation != null)
            {
                uow.Repository<Designation>().Delete(Designation);
                uow.Commit();
                return true;
            }
            else
            {
                return false;
            }
        }

        public string GetAddress(int DesignationId)
        {
            string result = "";

            if (DesignationId != 0)
            {
                Designation mnu = new Designation();
                mnu = GetSingle(DesignationId);

                List<string> lst = new List<string>();


                while (mnu != null)
                {
                    lst.Add(mnu.DGName);
                    mnu = GetSingle(mnu.PDGId);
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

        private List<Designation> FilterTree(List<Designation> list, string filter)
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
            var treelist = uow.Repository<Designation>().GetAll();
            List<Designation> list = treelist.ToList();

            list.Add(new Designation { DGId = 0, DGName = "Root", PDGId = -1 });

            if (filter.Trim() != "")
            {
                list = FilterTree(list, filter);
            }
            ViewModel.TreeView tree = this.GenerateTree(list, -1);
            return tree;
        }

        public ViewModel.TreeView GetDesignationGroupTree(int DesignationIdExpect, string filter = "")
        {
            List<Designation> list = uow.Repository<Designation>().GetAll().Where(x => x.DGId != DesignationIdExpect).ToList();
            list.Add(new Designation { DGId = 0,  DGName = "Root", PDGId = -1 });

            if (filter.Trim() != "")
            {
                list = FilterTree(list, filter);
            }
            ViewModel.TreeView tree = this.GenerateTree(list, -1);
            return tree;
        }


        public ViewModel.TreeView GetDesignationGroupTree(int parentId, int DesignationIdExpect, string filter = "")
        {
            List<Designation> list = uow.Repository<Designation>().GetAll().Where(x => x.DGId != DesignationIdExpect).ToList();

            if (filter.Trim() != "")
            {
                list = FilterTree(list, filter);
            }

            ViewModel.TreeView tree = this.GenerateTree(list, parentId);
            return tree;
        }

        private ViewModel.TreeView GenerateTree(List<Designation> list, int? parentDesignationId)
        {

            var parent = list.Where(x => x.PDGId == parentDesignationId);
            ViewModel.TreeView tree = new ViewModel.TreeView();
            tree.Title = "Designation";
            foreach (var itm in parent)
            {
                bool isGrp = false;
                int childs = IsGroup(itm.DGId,list);
                if(childs>0)
                {
                    isGrp = true;
                }
                if(itm.DGId==1)
                {
                    if(Loader.Models.Global.UserId==1)
                    {
                        tree.TreeData.Add(new ViewModel.TreeDTO
                        {
                            Id = itm.DGId,
                            PId = itm.PDGId,
                            Text = itm.DGName,
                            IsGroup = isGrp

                        });
                    }
                }
                else
                {
                    tree.TreeData.Add(new ViewModel.TreeDTO
                    {
                        Id = itm.DGId,
                        PId = itm.PDGId,
                        Text = itm.DGName,
                        IsGroup = isGrp

                    });
                }
              
            }

            foreach (var itm in tree.TreeData)
            {
                itm.Children = GenerateTree(list, itm.Id).TreeData.ToList();
            }
            return tree;
        }

        private int IsGroup(int DGid,List<Designation> list)
        {
            int childs = list.Where(x => x.PDGId == DGid).Select(x => x.DGId).Count();
            return childs;
        } 
        #endregion
    }
}