using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace DataStructures
{
    public interface IRepository
    {
        void Add<T>(T entity) where T : class;
        int Count<T>(ISpecification<T> where) where T : class;
        void Delete<T>(T entity) where T : class;
        IEnumerable<T> Find<T>(ISpecification<T> where, params Expression<Func<T, object>>[] includes) where T : class;
        IEnumerable<T> GetAll<T>(params Expression<Func<T, object>>[] includes) where T : class;
        T Single<T>(ISpecification<T> where, params Expression<Func<T, object>>[] includes) where T : class;
        void Update<T>(T entity) where T : class;
    }
}