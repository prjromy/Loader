using Loader.Models;
using Loader.Repository;
using Loader.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Loader.Service
{
    public class MenuTemplateService
    {
        private GenericUnitOfWork uow = null;
        private ReturnBaseMessageModel returnBaseMessage = null;

        public MenuTemplateService()
        {
            uow = new GenericUnitOfWork();
            returnBaseMessage = new ReturnBaseMessageModel();
        }

        public List<MenuTemplate> GetAll()
        {
            return uow.Repository<MenuTemplate>().GetAll().ToList();
        }

        public List<MenuTemplate> GetAllOfParent(int parentId)
        {
            return uow.Repository<MenuTemplate>().GetAll().Where(x => x.MTId == parentId).ToList();
        }

        public MenuTemplate GetSingle(int MTId)
        {
            MenuTemplate MenuTemplate = uow.Repository<MenuTemplate>().GetSingle(c => c.MTId == MTId);
            return MenuTemplate;
        }
        public string GetUserName(int userId)
        {
            var usrName = "";
            var userName = uow.Repository<Models.ApplicationUser>().GetSingle(x => x.Id == userId);
            if (userName != null)
            {
                usrName = userName.UserName;
            }

            return usrName;
        }
        public bool IsSuperAdmin()
        {
            bool isSuper = false;
            int userId = Loader.Models.Global.UserId;
            var menuTempObj = uow.Repository<Models.ApplicationUser>().GetSingle(x => x.Id == userId);
            if (menuTempObj != null)
            {
                if (userId == 1)
                {
                    isSuper = true;
                }
            }
            return isSuper;
        }
       // Menu template assigned to
        public IEnumerable<Models.ApplicationUser> TemplateAssignedUsers(int mtId)
        {
            var menuTempObj = uow.Repository<Models.ApplicationUser>().GetAll().Where(x => x.MTId == mtId).ToList();
            return menuTempObj;
        }
        public ReturnBaseMessageModel Save(MenuTemplate TemplateData, TreeView data)
        {
            GenericUnitOfWork editUOW = new GenericUnitOfWork();

            int checkExists = editUOW.Repository<MenuTemplate>().GetAll().Where(x => x.MTId != TemplateData.MTId && x.MTName == TemplateData.MTName).Count();
            if (checkExists > 0)
            {
                returnBaseMessage.Msg = "MenuTemplate Name already exist ";
                returnBaseMessage.Success = false;
                return returnBaseMessage;
            }
            var templateList = CheckedMenuTemplate(data);
            if (templateList.Count == 0)
            {
                returnBaseMessage.Msg = "Please Assign Menu";
                returnBaseMessage.Success = false;
                return returnBaseMessage;
            }

            using (var transaction = uow.GetContext().Database.BeginTransaction())
            {
                try
                {

                    if (TemplateData.MTId == 0)
                    {
                        TemplateData.PostedOn = DateTime.Now;
                        TemplateData.PostedBy = Global.UserId;
                        uow.Repository<MenuTemplate>().Add(TemplateData);

                        uow.Commit();

                        foreach (var item in templateList)
                        {
                            MenuVsTemplate menuvsTemplate = new MenuVsTemplate();
                            menuvsTemplate.MenuId = item;
                            menuvsTemplate.TemplateId = TemplateData.MTId;
                            uow.Repository<MenuVsTemplate>().Add(menuvsTemplate);
                            uow.Commit();
                        }


                        //For Role

                        Role role = new Role();
                        var temp = TemplateData.MTId;
                        //role.Id = temp;
                      
                        role.Name = TemplateData.MTName;
                        if (TemplateData.DesignationId != 0)
                        {
                            role.DGId = TemplateData.DesignationId;
                        }
                        role.MTId = temp;
                      
                        uow.Repository<Role>().Add(role);
                        uow.Commit();
                        returnBaseMessage.Msg = "Menu Template Added Succesfully";
                        returnBaseMessage.Success = true;
                        return returnBaseMessage;


                    }
                    else
                    {
                        // List<MenuVsTemplate> menuLst = new List<MenuVsTemplate>();

                        TemplateData.PostedOn = DateTime.Now;
                        TemplateData.PostedBy = Global.UserId;

                        uow.Repository<MenuTemplate>().Edit(TemplateData);

                        var menuData = uow.Repository<MenuVsTemplate>().GetAll().Where(x => x.TemplateId == TemplateData.MTId).ToList();
                        foreach (var item in menuData)
                        {
                            uow.Repository<MenuVsTemplate>().Delete(item);
                        }
                        uow.Commit();

                        foreach (var item in templateList)
                        {
                            MenuVsTemplate menuvsTemplate = new MenuVsTemplate();
                            menuvsTemplate.TemplateId = TemplateData.MTId;
                            menuvsTemplate.MenuId = item;
                            uow.Repository<MenuVsTemplate>().Add(menuvsTemplate);
                            uow.Commit();

                            //For Role

                        }
                        Role role = uow.Repository<Models.Role>().FindBy(x => x.MTId == TemplateData.MTId).SingleOrDefault();
                        if (role != null)
                        {
                            role.Name = TemplateData.MTName;
                            uow.Repository<Role>().Edit(role);
                            uow.Commit();
                        }
                      
                        returnBaseMessage.Msg = "Menu Template Edited Successfully";
                        returnBaseMessage.Success = true;
                        return returnBaseMessage;
                    }


                }


                catch (Exception e)
                {
                    returnBaseMessage.Msg = "error";
                    returnBaseMessage.Success = false;
                    return returnBaseMessage;

                }

            }
        }

        public bool Delete(int MTId)
        {
            MenuTemplate MenuTemplate = this.GetSingle(MTId);
            Role role = this.GetRole(MTId);

            if (MenuTemplate != null && role != null)
            {
                uow.Repository<MenuTemplate>().Delete(MenuTemplate);
                uow.Repository<Role>().Delete(role);
                uow.Commit();
                return true;
            }
            else
            {
                return false;
            }
        }


        public Role GetRole(int MTId)
        {
            Role role = uow.Repository<Role>().FindBy(x => x.MTId == MTId).SingleOrDefault();

            return role;
        }
        //public string GetAddress(int MTId)
        //{
        //    string result = "";

        //    if (MTId != 0)
        //    {
        //        MenuTemplate mnu = new MenuTemplate();
        //        mnu = GetSingle(MTId);

        //        List<string> lst = new List<string>();


        //        while (mnu != null)
        //        {
        //            lst.Add(mnu.MTName);
        //            mnu = GetSingle(mnu.PMTId);
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
        public ViewModel.TreeView GetMenuTemplateGroupTree(int? templateId, string filter = "")
        {
            var treelist = uow.Repository<Menu>().FindBy(x => x.IsEnable == true).ToList();
            List<Menu> list = treelist;

            if (filter.Trim() != "")
            {
                list = FilterTree(list, filter);
            }
          //  var menuVsTemplateList1 = uow.Repository<MenuVsTemplate>().GetAll().ToList();
            var menuVsTemplateList = uow.Repository<MenuVsTemplate>().FindBy(x => x.TemplateId == templateId).ToList();
            
            ViewModel.TreeView tree = this.GenerateTree(list, 0, templateId, menuVsTemplateList);
            return tree;
        }

        #region Tree

        List<int> templateList = new List<int>();
        public List<int> CheckedMenuTemplate(TreeView data)
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

        public string WithDesignation()
        {
            string withDes = uow.Repository<ParamValue>().GetAll().Where(x => x.PId == 16).Select(x => x.PValue).SingleOrDefault();
            return withDes;
        }
        public string AllowSelectDesignationGroup()
        {
            string withGrp = uow.Repository<ParamValue>().FindBy(x => x.PId == 18).Select(x => x.PValue).SingleOrDefault();
            return withGrp;
        }

        private ViewModel.TreeView GenerateTree(List<Menu> list, int? parentMTId, int? templateId, List<MenuVsTemplate> menuVsTemplateList)
        {
            int? menuTemplateId = templateId;
            //all permission granted menu list
            var parent = list.Where(x => x.PMenuId == parentMTId);
            ViewModel.TreeView tree = new ViewModel.TreeView();
            tree.Title = "MenuTemplate";

           

            foreach (var itm in parent)
            {
                bool isChecked = false;
                //isChecked =menuVsTemplateList.Find(x => x.MenuId == itm.MenuId);
                //isChecked = menuVsTemplateList.Where(x => x.MTId == itm.MenuId);
                //isChecked = (bool)menuVsTemplateList.Where(x => x.MenuId == itm.MenuId);

                var Checked = menuVsTemplateList.Find(x => x.MenuId == itm.MenuId);

                if (Checked!=null)
                    isChecked = true;

                if (itm.MenuId == 2 || itm.MenuId == 3 || itm.MenuId == 14)
                {
                    if (Loader.Models.Global.UserId == 1)
                    {
                        tree.TreeData.Add(new ViewModel.TreeDTO
                        {
                            Id = itm.MenuId,
                            PId = itm.PMenuId,
                            Text = itm.MenuCaption,
                            IsGroup = itm.IsGroup,
                            //  IsChecked = IsChecked(itm, menuVsTemplateList.Where(x=>x.TemplateId==templateId).ToList()),
                            //IsChecked = IsChecked(itm, templateId),
                            IsChecked=isChecked,
                            Image = itm.Image,

                        });
                    }

                }
                else
                {
                    tree.TreeData.Add(new ViewModel.TreeDTO
                    {
                        Id = itm.MenuId,
                        PId = itm.PMenuId,
                        Text = itm.MenuCaption,
                        IsGroup = itm.IsGroup,
                        //  IsChecked = IsChecked(itm, menuVsTemplateList.Where(x=>x.TemplateId==templateId).ToList()),
                        //IsChecked = IsChecked(itm, templateId),
                        IsChecked = isChecked,
                        Image = itm.Image,

                    });
                }

            }

            foreach (var itm in tree.TreeData)
            {
                itm.Children = GenerateTree(list, itm.Id, templateId, menuVsTemplateList).TreeData.ToList();
            }
            return tree;
        }

        //private bool IsChecked(int menuId, List<MenuVsTemplate> menuVsTemplateList)
        //{

        //   var a = menuVsTemplateList.Find(x => x.MenuId == menuId);
        //  // menuVsTemplateList.where(x => x.menuId == menuId)
           
        //}
            

            
            

        //// private bool IsChecked(Menu data, List<MenuVsTemplate> checkedList)
        //private bool IsChecked(Menu data, int? templateId)
        //{
        //    var checkedList = uow.Repository<MenuVsTemplate>().FindBy(x => x.TemplateId == templateId);
        //    var returnVal = false;
        //    foreach (var item in checkedList)
        //    {
        //        if (item.MenuId == data.MenuId)
        //        {
        //            returnVal = true;
        //        }

        //    }
        //    return returnVal;
        //}
        #endregion
    }
}