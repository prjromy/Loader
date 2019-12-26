using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Loader.Helper
{
    public class CookiesHelper
    {

        public void UpdateCookie(string SessionName, string SessionValue)
        {
            HttpContext.Current.Session[SessionName] = SessionValue;
        }
        public object GetSessionValue(string SessionVariableName)
        {
            return HttpContext.Current.Session[SessionVariableName];

        }

    }
}