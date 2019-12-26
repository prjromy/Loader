using Loader.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Loader.Repository
{
    public interface IGenericRepository<T> where T : class
    {
        //IEnumerable<T> GetAll(Func<T, bool> predicate = null);
        //T Get(Func<T, bool> predicate);
        //void Add(T entity);
        //void Attach(T entity);
        //void Delete(T entity);

        IEnumerable<T> GetAll();
        IEnumerable<T> SqlQuery(string query, params object[] parameters);
        IEnumerable<T> FindBy(Expression<Func<T, bool>> predicate);
        T GetSingle(Expression<Func<T, bool>> predicate);
        void Attach(T entity);
        void Add(T entity);
        void Delete(T entity);
        void Edit(T entity);
       //List<string[]> ExecuteQuery(string query);
        void Save();
    }
    //public interface IGenericRepository1<T> where T : BaseEntity
    //{
    //    IEnumerable<T> GetAll();
    //    IEnumerable<T> FindBy(Expression<Func<T, bool>> predicate);
    //    T Add(T entity);
    //    T Delete(T entity);
    //    void Edit(T entity);
    //    void Save();
    //}

    //public interface IGenericRepositoryI<T> where T : class
    //{
    //    IEnumerable<T> GetAll();
    //    IEnumerable<T> FindBy(Expression<Func<T, bool>> predicate);
    //    T Add(T entity);
    //    T Delete(T entity);
    //    void Edit(T entity);
    //    void Save();
    //}

    //public interface IGenericRepositoryQ<T> where T : class
    //{
    //    IQueryable<T> GetAll();
    //    IQueryable<T> FindBy(Expression<Func<T, bool>> predicate);
    //    void Add(T entity);
    //    void Delete(T entity);
    //    void Edit(T entity);
    //    void Save();
    //}

    //public interface IGenericRepository<T> where T : class
    //{
    //    IEnumerable<T> GetAll();
    //    IEnumerable<T> FindBy(Expression<Func<T, bool>> predicate);
    //  //  T GetSingle(Expression<Func<T, bool>> predicate);
    //    T Add(T entity);
    //    T Delete(T entity);
    //    //void Attach(T entity);
    //    void Edit(T entity);       
    //    void Save();
    //}
}