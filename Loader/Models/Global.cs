using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using System.Reflection;

namespace Loader.Models
{

    public static class Global
    {
        private static int fyId = 0;

        public static int UserId { get { return Convert.ToInt32(HttpContext.Current.Session["UserID"]); } }
        public static int BranchId { get  { return Convert.ToInt32(HttpContext.Current.Session["BranchId"]); } }
        public static bool IsSuperAdmin { get { Loader.Service.MenuTemplateService param = new Loader.Service.MenuTemplateService(); return param.IsSuperAdmin(); } }
        public static string UserName { get { return Convert.ToString(HttpContext.Current.Session["UserName"]); } }

        public static string Image { get { return Convert.ToString(HttpContext.Current.Session["UserName"]); } }


        public static int CurrentFYID
        {
            get { return Convert.ToInt32(HttpContext.Current.Session["CurrentFYID"]); }
            set { HttpContext.Current.Session["CurrentFYID"] = value; }
        }
        public static int SelectedFYID {
            get { return Convert.ToInt32(HttpContext.Current.Session["SelectedFYID"]); }
            set { HttpContext.Current.Session["SelectedFYID"] = value; }
        }
        public static string CurrentFiscalYear
        {
            get
            {
                Loader.Service.ParameterService param = new Service.ParameterService();
                var check = param.GetCurrentFiscalYear(CurrentFYID);//== "" ? DateTime.Now.ToShortDateString() : param.GetCurrentFiscalYear();
                return check;
            }
        }
       
        public static Nullable<DateTime> TransactionDate {
            get { return Convert.ToDateTime(HttpContext.Current.Session["TransactionDate"]); }
            set { HttpContext.Current.Session["TransactionDate"] = value; }
        }

        //change for session
        public static int getCurrentFYID(int BranchId)
        {
            int fyId;
            {
                Loader.Service.ParameterService param = new Service.ParameterService();
                fyId = param.GetCurrentFYID(BranchId);
            }
            return fyId;
        }
        public static string getCurrentFiscalYear(int CurrentFYID)
        {
            
                Loader.Service.ParameterService param = new Service.ParameterService();
                return param.GetCurrentFiscalYear(CurrentFYID) == "" ? DateTime.Now.ToShortDateString() : param.GetCurrentFiscalYear();
            
        }
        public static Nullable<DateTime> getTransactionDate(int BranchId) {
            
                Loader.Service.BranchSetupService param = new Loader.Service.BranchSetupService();
                return param.GetTransactionDateOfCurrentBranch(BranchId);
             }

        //public static string Logo (int paramid)
        //{
        //    Loader.Service.ParameterService param = new Service.ParameterService();
           
        //    var logoget=param.GetLogo(paramid);
        //    get { return Convert.ToInt32(HttpContext.Current.Session["logoget"]); }
        //    set { HttpContext.Current.Session["Logo"] = logoget; }
        //}
    }


}