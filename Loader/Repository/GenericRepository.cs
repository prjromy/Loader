using Loader.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;

namespace Loader.Repository
{
    

    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected DbContext _entities;
        protected readonly IDbSet<T> _dbset;

        public GenericRepository(DbContext context)
        {
            _entities = context;
            _dbset = context.Set<T>();
        }
  

        public IEnumerable<T> GetAll()
        {
            return _dbset.AsEnumerable();
        }
        //public List<string[]> ExecuteQuery(string query)
        //{
        //    //var test= { new id};

        //    var data = _entities.Database.SqlQuery<string[]>(query).ToList();
        //    //var dd = _entities.Database.SqlQuery<>
        //    return data;
            
        //}

        public IEnumerable<T> FindBy(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
        {
            IEnumerable<T> query = _dbset.Where(predicate).AsEnumerable();
            return query;
        }
        public IEnumerable<T> SqlQuery(string query, params object[] parameters)
        {
            return _entities.Database.SqlQuery<T>(query, parameters);
        }

        public virtual T GetSingle(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
        {
            T query = _dbset.Where(predicate).FirstOrDefault();
            return query;
        }

        public virtual void Add(T entity)
        {
            _dbset.Add(entity);
        }
        

        public virtual void Delete(T entity)
        {
            _dbset.Remove(entity);            
        }

        public virtual void Edit(T entity)
        {
            _entities.Entry(entity).State = EntityState.Modified;
            

        }
        public virtual void Attach(T entity)
        {            
            _dbset.Attach(entity);
        }

        public virtual void Save()
        {
            _entities.SaveChanges();
        }
    }
}