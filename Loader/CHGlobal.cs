using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Loader
{
    public class CHGlobal
    {
        private static Helper.CookiesHelper CH = new Helper.CookiesHelper();

        public static DateTime TransactionDate
        {
            get
            {
                DateTime DTime;
                try
                {
                    if (CH.GetSessionValue("TDate").ToString().Trim() == "")
                    {
                        DTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                    }
                    else
                    {
                        DTime = Convert.ToDateTime(CH.GetSessionValue("TDate"));
                        DTime = new DateTime(DTime.Year, DTime.Month, DTime.Day);
                    }

                }
                catch
                {
                    DTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                }
                return DTime;
            }
        }

        public static ViewModel.Calendar.eDateType  DefaultDateType
        {
            get
            {
                try
                {
                    return (ViewModel.Calendar.eDateType)Enum.Parse(typeof(ViewModel.Calendar.eDateType), CH.GetSessionValue("DDType").ToString());
                }
                catch
                {
                    HttpContext.Current.Session["DDType"] = "2";
                    //CH.UpdateCookie("DDType", "2");
                    return (ViewModel.Calendar.eDateType)Enum.Parse(typeof(ViewModel.Calendar.eDateType), CH.GetSessionValue("DDType").ToString());
                }
            }
        }

        public static ViewModel.Calendar.eDateFormat DefaultDateFormatAD
        {
            get
            {
                try
                {
                    return (ViewModel.Calendar.eDateFormat)Enum.Parse(typeof(ViewModel.Calendar.eDateType), CH.GetSessionValue("ADDFormat").ToString());
                }
                catch
                {
                    HttpContext.Current.Session["ADDFormat"] = "1";
                    //CH.UpdateCookie("ADDFormat", "1");
                    return (ViewModel.Calendar.eDateFormat)Enum.Parse(typeof(ViewModel.Calendar.eDateType), CH.GetSessionValue("ADDFormat").ToString());
                }
            }
        }

        public static ViewModel.Calendar.eDateFormat DefaultDateFormatBS
        {
            get
            {
                try
                {
                    return (ViewModel.Calendar.eDateFormat)Enum.Parse(typeof(ViewModel.Calendar.eDateType), CH.GetSessionValue("BSDFormat").ToString());
                }
                catch
                {
                    HttpContext.Current.Session["BSDFormat"] = "4";
                    //CH.UpdateCookie("BSDFormat", "4");
                    return (ViewModel.Calendar.eDateFormat)Enum.Parse(typeof(ViewModel.Calendar.eDateType), CH.GetSessionValue("BSDFormat").ToString());
                }
            }

        }
    }
}