using Loader.Models;
using Loader.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Loader.Service
{
    public class LayoutMenuMainService
    {
        private GenericUnitOfWork uow = null;
        Loader.Service.UserVSBranchService _usrBrnchService = new UserVSBranchService();


        public LayoutMenuMainService()
        {
            uow = new GenericUnitOfWork();
        }

        //public List<LayoutMenuMain> GetAll()
        //{
        //    return uow.Repository<LayoutMenuMain>().GetAll().ToList();
        //}

        //public List<LayoutMenuMain> GetAllOfParent(int parentId)
        //{
        //    return uow.Repository<LayoutMenuMain>().GetAll().Where(x => x.ParentId == parentId).ToList();
        //}
        public ViewModel.LayoutTreeView GetLayoutMenuGroupTree(string filter = "")
        {
            int userId = Global.UserId;
            if(userId==0)
            {
                throw new Exception("User Session Expired");
            }
            int branchId = Global.BranchId;
            List<int> menuTemplateList;
            var menuTemplateId=0;
            if (Loader.Models.Global.IsSuperAdmin)
            {
                var MenuIdList = uow.Repository<MenuVsTemplate>().GetAll().Select(x =>x.MenuId ).Distinct().ToList();
                menuTemplateList= uow.Repository<Menu>().FindBy(x => MenuIdList.Contains(x.MenuId) && x.IsContextMenu==false).Select(x=>x.MenuId).ToList();
                //menuTemplateList= uow.Repository<MenuVsTemplate>().GetAll().Select(x => x.MenuId).Distinct().ToList();
              
            }
            else
            {
                var currentBranchId = _usrBrnchService.GetCurrentBranchInt(userId);
                if(currentBranchId==branchId)
                {
                    menuTemplateId = uow.Repository<ApplicationUser>().GetSingle(x => x.Id == userId).MTId;
                }
                else
                {
                    menuTemplateId = _usrBrnchService.GetBranchRoleId(branchId, userId);
                }
                menuTemplateList = uow.Repository<MenuVsTemplate>().FindBy(x => x.TemplateId == menuTemplateId).Select(x => x.MenuId).ToList();
                
            }
            
            var menuAll = uow.Repository<Menu>().GetAll();
            var list = GetMenuList(menuAll.ToList(), menuTemplateList); 
            
            if (filter.Trim() != "")
            {
                list = FilterTree(list, filter);
            }
            ViewModel.LayoutTreeView tree = this.GenerateTree(list, 0);
            return tree;
        }
        private List<Menu> GetMenuList(List<Menu> list,List<int> menutemplateList)
        {
            List<Menu> finallst = new List<Menu>();
            if (Loader.Models.Global.IsSuperAdmin)
            {
                //var check = uow.Repository<Menu>().GetAll().ToList(); //OPTIMIZATION COMMENT
                List<Menu> forAdmin = list.Where(x => x.IsContextMenu == false).ToList();
                return forAdmin;
                //foreach (var item in list)
                //{
                //    if (item.MenuId == 1004)
                //    {
                //    }
                //    foreach (var item1 in menutemplateList)
                //    {
                //        if (item1 == 1004)
                //        {
                //        }
                //        if (item.MenuId == item1 && item.IsEnable == true && item.IsContextMenu != true )
                //        {
                //            finallst.Add(item);
                //        }
                //        else if (item.MenuId == 2 && item.MenuId == 3 && item.MenuId == 14)
                //        {
                //            finallst.Add(item);
                //        }
                //    }
                //}
                //return finallst;
            }
            foreach (var item in list)
            {
                foreach(var item1 in menutemplateList)
                {
                    if(item.MenuId==item1 && item.IsEnable==true && item.IsContextMenu!=true && item.MenuId!=2 && item.MenuId!=3 && item.MenuId!=14 )
                    {
                        finallst.Add(item);
                    }
                }
            }
            return finallst;
        }
        private List<Menu> FilterTree(List<Menu> list, string filter)
        {
            bool lLoop = true;

            var filteredList = list.Where(x => x.MenuCaption.ToLower().Contains(filter.ToLower())).ToList();

            while (lLoop)
            {
                //select all parents of filtered list
                var allParent = (from mainList in list
                                 join selList in filteredList on mainList.MenuId equals selList.PMenuId
                                 select mainList).ToList();

                //Select unique parent only
                var parentList = (from p in allParent
                                  join c in filteredList on p.MenuId equals c.MenuId into gj
                                  from uniqueParent in gj.DefaultIfEmpty()
                                  where uniqueParent == null
                                  select p).ToList();

                if (parentList.Count() == 0)
                {
                    lLoop = false;
                }

                filteredList = filteredList.Union(parentList).OrderBy(x => x.MenuId).ToList();
            }
            list = filteredList;
            return list;

        }

        private ViewModel.LayoutTreeView GenerateTree(List<Menu> list, int? parentMenuId)
        {

            var parent = list.Where(x => x.PMenuId == parentMenuId);
            ViewModel.LayoutTreeView tree = new ViewModel.LayoutTreeView();
            tree.Title = "Menu";
            foreach (var itm in parent)
            {
                tree.LayoutTreedata.Add(new ViewModel.LayoutTreeDTO
                {
                    Id = itm.MenuId,
                    PId = itm.PMenuId,
                    Text = itm.MenuCaption,
                    IsGroup = itm.IsGroup,
                    Controler=itm.Controler,
                    Acton=itm.Acton,
                    Image = itm.Image

                });
            }

            foreach (var itm in tree.LayoutTreedata)
            {
                itm.Children = GenerateTree(list, itm.Id).LayoutTreedata.ToList();
            }
            return tree;
        }

    }
}