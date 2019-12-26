using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Loader.ViewModel
{
    public class SearchDTO
    {
        public SearchDTO()
        {
        }
        public int Id { get; set; }
        public string EmpNo { get; set; }
        public string Text { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string DeptId { get; set; }
        public string DGId { get; set; }
        public List<ViewModel.SearchDTO> Children { get; set; }
    }
    public class SearchViewModel
    {
        public SearchViewModel()
        {
            SearchData = new List<ViewModel.SearchDTO>();
            Id = 1;
            PhoneNumber = "11";
            Address = "ChannakyaSoft";
            Title = "Select ....";
            Filter = "";
        }
        public SearchViewModel(int id, string phoneNumber, string address,string name,string title,string filter)
        {
            Id = id;
            PhoneNumber = phoneNumber;
            Address = address;
            Name = name;
            Title = title;
            Filter = filter;
        }
        public SearchViewModel(int id, string phoneNumber, string address, string name, string title, string filter,string dGId, string dept)
        {
            Id = id;
            PhoneNumber = phoneNumber;
            Address = address;
            Name = name;
            Title = title;
            Filter = filter;
            DGId = dGId;
            DeptId = DeptId;
            

        }
        public SearchViewModel(int id, string phoneNumber, string address, string name, string title, string filter, string dGId, string dept, string searchFor)
        {
            Id = id;
            PhoneNumber = phoneNumber;
            Address = address;
            Name = name;
            Title = title;
            Filter = filter;
            DGId = dGId;
            DeptId = DeptId;
            SearchFor = searchFor;

        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string Title { get; set; }
        public string Filter { get; set; }
        public string DGId { get; set; }
        public string DeptId { get; set; }
        public string SearchFor { get; set; } //"Employee" and "Collector" searches available simply pass "Employee" and "Collector" as needed in ViewBag 
        public List<ViewModel.SearchDTO> SearchData { get; set; }


    }
}