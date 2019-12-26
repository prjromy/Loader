using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Loader.ViewModel
{
    public class UserBranchViewModel
    {
        public int UserId { get; set; }
        public List<Branch> Branch { get; set; }
        public int SelectedBranchId { get; set; }
        public List<BranchRoles> Role { get; set; }
       
    }

    public class Branch
    {
        public int BranchId { get; set; }
        public string BranchName { get; set; }
        public Nullable<DateTime> ToDate { get; set; }

    }

    public class BranchRoles
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }

    }
}