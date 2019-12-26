using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Loader.Controllers
{
    public class CalendarController : Controller
    {
        // GET: Calendar
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GetCalendar(DateTime dateAD, Int16 dateType)
        {

            return PartialView();
        }
    }
}