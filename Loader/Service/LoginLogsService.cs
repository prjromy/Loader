using Loader.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Loader.Service
{
    public class LoginLogsService
    {
        private GenericUnitOfWork uow = null;


        public LoginLogsService()
        {
            uow = new GenericUnitOfWork();
        }
        public void AddLogs(Models.LoginLogs loginLogs)
        {
            MvcApplication.GUserId = loginLogs.UserId;
            MvcApplication.GBranchId = loginLogs.BranchId;


            GenericUnitOfWork editUOW = new GenericUnitOfWork();
            
            if (loginLogs.Id == 0)
            {

                uow.Repository<Models.LoginLogs>().Add(loginLogs);
            }
           
            uow.Commit();
        }

        public void EndSession()
        {
            int userId = Loader.Models.Global.UserId;
            int branchId = Loader.Models.Global.BranchId;

            Models.LoginLogs sessionObj = uow.Repository<Models.LoginLogs>().FindBy(x => x.UserId == userId && x.BranchId == branchId).LastOrDefault();

            if (sessionObj != null)
            {

                sessionObj.To = DateTime.Now;

                uow.Repository<Models.LoginLogs>().Edit(sessionObj);
                uow.Commit();
            }

        }
    }
}