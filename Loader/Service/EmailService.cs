using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Loader.Repository;

namespace Loader.Service
{
    public class EmailService
    {
        GenericUnitOfWork uow = new GenericUnitOfWork();

        public  string Host;

        public  int Port;
        public  string FromEmail;

        public  string FromPassword;

        #region Constructors
        public EmailService()
        {
            var emailHost = uow.Repository<Models.ParamValue>().FindBy(x => x.PId == 27).FirstOrDefault();
            if(emailHost!=null)
            {
                Host = emailHost.PValue;
            }

            var emailPort = uow.Repository<Models.ParamValue>().FindBy(x => x.PId == 28).FirstOrDefault();
            if(emailPort!=null)
            {
                Port = Convert.ToInt32(emailPort.PValue);
            }

            var fromEmail = uow.Repository<Models.ParamValue>().FindBy(x => x.PId == 29).FirstOrDefault();
            if(fromEmail!=null)
            {
                FromEmail = fromEmail.PValue;
            }

            var fromPWD = uow.Repository<Models.ParamValue>().FindBy(x => x.PId == 30).FirstOrDefault();
            if(fromPWD!=null)
            {
                FromPassword = fromPWD.PValue;
            }
        }
        #endregion

    }
}