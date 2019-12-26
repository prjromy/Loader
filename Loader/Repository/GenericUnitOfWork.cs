using Loader.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Threading.Tasks;


namespace Loader.Repository
{
    public sealed class GenericUnitOfWork : IGenericUnitOfWork, IDisposable
    {
        private ApplicationDbContext entities = null;



        public GenericUnitOfWork()
        {
            entities = new ApplicationDbContext();
        }


        public Dictionary<Type, object> repositories = new Dictionary<Type, object>();

        public IGenericRepository<T> Repository<T>() where T : class
        {
            if (repositories.Keys.Contains(typeof(T)) == true)
            {
                return repositories[typeof(T)] as IGenericRepository<T>;
            }
            IGenericRepository<T> repo = new GenericRepository<T>(entities);
            repositories.Add(typeof(T), repo);
            return repo;
        }
       

        public int Commit()
        {
            return entities.SaveChanges();
        }

        private bool disposed = false;

        private void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    entities.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        public ApplicationDbContext GetContext()
        {
            ApplicationDbContext ctx = new ApplicationDbContext();
        
            return ctx;
        }


        
    }

}