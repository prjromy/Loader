using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;

namespace Loader
{
    #region Authorize Attribute


    public class MyAuthorizeAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            HttpContext context = System.Web.HttpContext.Current;
            var userId = Loader.Models.Global.UserId;
            var browser = HttpContext.Current.Session["BrowserName"];
            if ((HttpContext.Current.Session["UserName"] == null && (string)HttpContext.Current.Session["BrowserName"] != context.Request.Url.OriginalString))
            {
                var routeValues = new RouteValueDictionary(new
                {
                    controller = "Account",
                    action = "Login",
                });
                filterContext.Result = new RedirectToRouteResult(routeValues);
            }
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            var url = filterContext.HttpContext.Request.Url;

            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                filterContext.HttpContext.Response.StatusCode = 401;
                filterContext.Result = new JsonResult
                {
                    Data = new
                    {
                        Error = "NotAuthorized",
                        LogOnUrl = FormsAuthentication.LoginUrl
                    },
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary { { "controller", "home" }, { "action", "index" } });

                // filterContext.HttpContext.Response.End();
            }
            else
            {
                base.HandleUnauthorizedRequest(filterContext);
            }
        }

        private bool IsProfileCompleted(string user)
        {
            // You know what to do here => go hit your database to verify if the
            // current user has already completed his profile by checking
            // the corresponding field
            throw new NotImplementedException();
        }

        public static bool VerifyMe(string password)
         {
            bool isValid = false;
            var myEncryptedPW = MyDecrypter(password);
            if (myEncryptedPW == "69!@#$%22!@#$%35!@#$%fc!@#$%20!@#$%a4!@#$%43!@#$%54!@#$%4f!@#$%9b!@#$%f7!@#$%a8!@#$%12!@#$%e2!@#$%8f!@#$%25!@#$%")
            {
                isValid = true;
            }
            return isValid;
        }


        private static string MyDecrypter(string input)
        {
            StringBuilder hash = new StringBuilder();
            MD5CryptoServiceProvider md5provider = new MD5CryptoServiceProvider();
            byte[] bytes = md5provider.ComputeHash(new UTF8Encoding().GetBytes(input));

            for (int i = 0; i < bytes.Length; i++)
            {
                hash.Append(bytes[i].ToString("x2") + "!@#$%");
            }
            return hash.ToString();
        }
    }


    
    #endregion

}