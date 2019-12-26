using Loader.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Loader.Repository
{
    public interface IGenericUnitOfWork : IDisposable
    {
        int Commit();
        ApplicationDbContext GetContext();
    }
}