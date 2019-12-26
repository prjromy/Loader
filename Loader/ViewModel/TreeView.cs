using System;
using System.Collections.Generic;

namespace Loader.ViewModel
{

    public class TreeDTO
    {
        public TreeDTO()
        {
            PId = null;
            Image = null;
            IsGroup = true;
            IsChecked = false;
        }
        public int Id { get; set; }
        public Nullable<int> PId { get; set; }
        public string Text { get; set; }
        public byte[] Image { get; set; }
        public bool IsGroup { get; set; }
        public bool IsChecked { get; set; }
        public List<ViewModel.TreeDTO> Children { get; set; }
    }
    public class LayoutTreeDTO
    {
        public LayoutTreeDTO()
        {
            PId = null;
            Image = null;
            IsGroup = true;
            IsChecked = false;
        }
        public int Id { get; set; }
        public Nullable<int> PId { get; set; }
        public string Text { get; set; }
        public byte[] Image { get; set; }
        public bool IsGroup { get; set; }
        public bool IsChecked { get; set; }
        public string Controler { get; set; }
        public string Acton { get; set; }
        public List<ViewModel.LayoutTreeDTO> Children { get; set; }
    }


    public class TreeView
    {
        public TreeView()
        {
            TreeData = new List<ViewModel.TreeDTO>();
            Title = "Treeview";
        }
        public List<ViewModel.TreeDTO> TreeData { get; set; }
        public string Title { get; set; }
    }
    public class LayoutTreeView
    {
        public LayoutTreeView()
        {
            LayoutTreedata = new List<ViewModel.LayoutTreeDTO>();
            Title = "LayoutTreeView";
        }
        public List<ViewModel.LayoutTreeDTO> LayoutTreedata { get; set; }
        public string Title { get; set; }
    }

    public class TreeViewParam {

        public TreeViewParam()
        {
     
            WithCheckBox = false;
            WithImageIcon = false;
            AllowSelectGroup = true;
            WithOutMe = 0;
            Title = "Select...";
            Filter = "";
            SelectedNodeText = "";
            SelectedNodeId = 0;
        }
        public TreeViewParam( int withoutMe, int selectedNodeId, string selectedNodeText, string title)
        {
            
            WithOutMe = withoutMe;
            SelectedNodeId = selectedNodeId;
            SelectedNodeText = selectedNodeText;
            Title = title;

            WithCheckBox = false;
            AllowSelectGroup = true;
            WithImageIcon = false;
            Filter = "";


        }
        public TreeViewParam( bool withCheckBox, bool allowSelectGroup, bool withImageIcon, int withoutMe, int selectedNodeId, string selectedNodeText, string title, string filter)
        {
            
            WithCheckBox = withCheckBox;
            AllowSelectGroup = allowSelectGroup;
            WithImageIcon = withImageIcon;
            WithOutMe = withoutMe;
            Title = title;
            SelectedNodeId = selectedNodeId;
            Filter = filter;
            SelectedNodeText = selectedNodeText;

        }
        public TreeViewParam(bool withCheckBox, bool allowSelectGroup, bool withImageIcon, int withoutMe, int selectedNodeId, string title)
        {
      
            WithCheckBox = withCheckBox;
            AllowSelectGroup = allowSelectGroup;
            WithImageIcon = withImageIcon;
            WithOutMe = withoutMe;
            Title = title;
            SelectedNodeId = selectedNodeId;
            SelectedNodeText = "";
            Filter = "";
        }
        
        public bool WithCheckBox { get; set; }
        public bool AllowSelectGroup { get; set; }
        public bool WithImageIcon { get; set; }
        public int WithOutMe { get; set; }
        public string Title { get; set; }
        public string Filter { get; set; }
        public string SelectedNodeText { get; set; }
        public int SelectedNodeId { get; set; }
        public string SearchFor { get; set; }
    }
}