using System;
using System.Linq;
using System.Linq.Expressions;

namespace DataStructures
{
    public interface ISpecification<TEntity> where TEntity : class
    {
        Expression<Func<TEntity, bool>> Predicate { get; }

        IQueryable<TEntity> SatisfyingEntitiesFrom(IQueryable<TEntity> query);
        TEntity SatisfyingEntityFrom(IQueryable<TEntity> query);
    }
}