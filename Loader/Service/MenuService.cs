using Loader.Models;
using Loader.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Loader.Service
{
    public class MenuService
    {
        private GenericUnitOfWork uow = null;


        public MenuService()
        {
            uow = new GenericUnitOfWork();
            
        }

        public List<Menu> GetAll()
        {
            return uow.Repository<Menu>().GetAll().ToList();
        }

        public List<Menu> GetAllOfParent(int parentId)
        {
            return uow.Repository<Menu>().GetAll().Where(x => x.PMenuId == parentId).ToList();
        }

        public Menu GetSingle(int menuId)
        {
            Menu menu = uow.Repository<Menu>().GetSingle(c => c.MenuId == menuId);
            return menu;
        }

        public void Save(Menu menu)
        {
            if ((menu.IsContextMenu == false && menu.IsGroup == false) && (menu.Controler == null || menu.Acton == null))
            {
                throw new Exception("Controller and Action Required");
            }
            GenericUnitOfWork editUOW = new GenericUnitOfWork();
            int checkExists = editUOW.Repository<Menu>().GetAll().Where(x => x.MenuId != menu.MenuId && x.MenuCaption == menu.MenuCaption && x.PMenuId == menu.PMenuId).Count();
            if (checkExists > 0)
            {
                throw new Exception("Duplicate Menu Found. Menu Caption Not Valid");
            }
            if (menu.MenuId == 0)
            {
                uow.Repository<Menu>().Add(menu);
            }
            else
            {
                uow.Repository<Menu>().Edit(menu);
            }
            uow.Commit();
        }

        public bool Delete(int menuId)
        {
            Menu menu = this.GetSingle(menuId);

            if (menu != null)
            {
                uow.Repository<Menu>().Delete(menu);
                uow.Commit();
                return true;
            }
            else
            {
                return false;
            }
        }

        public string GetAddress(int menuId)
        {
            string result = "";

            if (menuId != 0)
            {
                Menu mnu = new Menu();
                mnu = GetSingle(menuId);

                List<string> lst = new List<string>();


                while (mnu != null)
                {
                    lst.Add(mnu.MenuCaption);
                    mnu = GetSingle(mnu.PMenuId);
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
        public ViewModel.TreeView GetMenuGroupTree(string filter = "")
        {
            var treelist = uow.Repository<Menu>().GetAll().Where(x => x.IsGroup == true);
            List<Menu> list = treelist.ToList();

            list.Add(new Menu { MenuId = 0, IsGroup = true, MenuCaption = "Root", PMenuId = -1 });

            if (filter.Trim() != "")
            {
                list = FilterTree(list, filter);
            }
            ViewModel.TreeView tree = this.GenerateTree(list, -1);
            return tree;
        }
        public ViewModel.LayoutTreeView GetLayoutGroupTree(string filter = "")
        {
            var userId = Loader.Models.Global.UserId;
          
            var menuTemplateId = uow.Repository<Models.ApplicationUser>().GetSingle(x => x.Id == userId).MTId;
            var menuList = uow.Repository<MenuVsTemplate>().FindBy(x => x.TemplateId == menuTemplateId).Select(x => x.MenuId).ToList();
            var treelist = uow.Repository<Menu>().GetAll().Where(x => x.IsGroup == true).Select(x => x.MenuId);
            var menuListFinal = from m in uow.Repository<Menu>().GetAll() join p in menuList on m.MenuId equals p where m.IsContextMenu==false select new Menu { MenuId = m.MenuId, MenuCaption = m.MenuCaption, IsGroup = m.IsGroup, PMenuId = m.PMenuId, IsEnable = m.IsEnable, Acton = m.Acton, Controler = m.Controler, Image = m.Image, IsContextMenu = m.IsContextMenu };
            //var check=uow.Repository<Menu>().
            List<Menu> list = menuListFinal.ToList();
            //if (userId == 1)
            if (Loader.Models.Global.IsSuperAdmin)
            {
                list = uow.Repository<Models.Menu>().GetAll().Where(x=>x.IsContextMenu==false).ToList();
            }

            if (filter.Trim() != "")
            {
                list = FilterTree(list, filter);
            }
            ViewModel.LayoutTreeView tree = this.GenerateLayoutTree(list, 0);
            return tree;
        }

        public ViewModel.TreeView GetMenuGroupTree(int menuIdExpect, string filter = "")
        {
            List<Menu> list = uow.Repository<Menu>().GetAll().Where(x => x.IsGroup == true).Where(x => x.MenuId != menuIdExpect).ToList();
            list.Add(new Menu { MenuId = 0, IsGroup = true, MenuCaption = "Root", PMenuId = -1 });

            if (filter.Trim() != "")
            {
                list = FilterTree(list, filter);
            }
            ViewModel.TreeView tree = this.GenerateTree(list, -1);
            return tree;
        }


        public ViewModel.TreeView GetMenuGroupTree(int parentId, int menuIdExpect, string filter = "")
        {
            List<Menu> list = uow.Repository<Menu>().GetAll().Where(x => x.IsGroup == true).Where(x => x.MenuId != menuIdExpect).ToList();

            if (filter.Trim() != "")
            {
                list = FilterTree(list, filter);
            }

            ViewModel.TreeView tree = this.GenerateTree(list, parentId);
            return tree;
        }

        private ViewModel.TreeView GenerateTree(List<Menu> list, int? parentMenuId)
        {

            var parent = list.Where(x => x.PMenuId == parentMenuId);
            ViewModel.TreeView tree = new ViewModel.TreeView();
            tree.Title = "Menu";
            foreach (var itm in parent)
            {
                
                tree.TreeData.Add(new ViewModel.TreeDTO
                {
                    Id = itm.MenuId,
                    PId = itm.PMenuId,
                    Text = itm.MenuCaption,
                    IsGroup = itm.IsGroup,
                    Image = itm.Image

                });
            }

            foreach (var itm in tree.TreeData)
            {
                itm.Children = GenerateTree(list, itm.Id).TreeData.ToList();
            }
            return tree;
        }

        private ViewModel.LayoutTreeView GenerateLayoutTree(List<Menu> list, int? parentMenuId)
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
                    Acton = itm.Acton,
                    Controler = itm.Controler,
                    IsChecked = itm.IsEnable,
                    Image = itm.Image

                });
            }

            foreach (var itm in tree.LayoutTreedata)
            {
                itm.Children = GenerateLayoutTree(list, itm.Id).LayoutTreedata.ToList();
            }
            return tree;
        }
        #endregion
    }
}