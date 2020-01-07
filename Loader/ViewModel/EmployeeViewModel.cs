using PagedList;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Loader.ViewModel
{
    public class EmployeeViewModel
    {
        public int EmployeeId { get; set; }

        [DisplayName("Code")]
        public string EmployeeNo { get; set; }

        [DisplayName("Full Name")]
        public string EmployeeName { get; set; }

        [DisplayName("Departmet")]
        public string DeptName { get; set; }

        [DisplayName("Designation")]
        public string DGName { get; set; }

        public string SearchEmployee { get; set; }
        public string EmployeeOption { get; set; }
        public int BranchId { get; set; }
        public string BrnhNam { get; set; }

        public DateTime StartFrom { get; set; }

        public int MapId { get; set; }
        public string RoleName { get; set; }
        public int UserId { get; set; }

        public int TotalCount { get; set; }
        public string UserName { get; set; }
        public string LoadType { get; set; }
        public string SearchType { get; set; }
        public IPagedList<EmployeeViewModel> EmployeeList { get; set; }
        public List<EmployeeViewModel> Employee { get; set; }
    }
}