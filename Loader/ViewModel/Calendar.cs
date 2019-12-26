using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Loader.ViewModel.Calendar
{
    public enum eDateType {
        Default = 0,
        AD = 1,
        BS = 2
    }
    public enum eDateFormat
    {
        ddMMyyyy = 1,
        MMddyyyy = 2,
        yyyyddMM = 3,
        yyyyMMdd = 4

    }

    public class DateDTO
    {
        public DateDTO()
        {
            Date = ((DateTime)CHGlobal.TransactionDate).ToShortDateString();
        }
        public string Date { get; set; }
        public string DateAD { get; set; }
        public string DateBS { get; set; }
    }

    public class CalendarDTO
    {
        public CalendarDTO()
        {
            DateType = CHGlobal.DefaultDateType;
            Date = CHGlobal.TransactionDate;
        }
        public DateTime Date { get; set; }
        public eDateType DateType { get; set; }
        public Int16 FirstDayOfWeeek { get; set; }
        public Int16 CurrentDay { get; set; }
        public Int16 CurrentMonth { get; set; }
        public Int32 CurrentYear { get; set; }
        public Int16 NoOfDays { get; set; }
        public Int16 NoOfWeeks { get; set; }
    }
    public class YearMonthListDTO
    {
        public IEnumerable<SelectListItem> MonthList { get; set; }
        public IEnumerable<SelectListItem> YearList { get; set; }
        public CalendarDTO Calendar { get; set; }
    }
}