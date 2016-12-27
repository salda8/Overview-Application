using System;
using System.Linq;
using System.Linq.Expressions;
using DataStructures;

namespace DataAccess
{
    /// <summary>
    /// Base class for specification
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public class Specification<TEntity> : ISpecification<TEntity> where TEntity : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Specification{TEntity}"/> class.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        public Specification(Expression<Func<TEntity, bool>> predicate)
        {
            Predicate = predicate;
        }

        /// <summary>
        /// Returns single entity that satisfies <see cref="Predicate"/>. Only one entity must satisfy <see cref="Predicate"/> otherwise exception is thrown.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns></returns>
        public TEntity SatisfyingEntityFrom(IQueryable<TEntity> query)
        {
            return query.Where(Predicate).SingleOrDefault();
        }

        /// <summary>
        /// Returns the entities satisfying <see cref="Predicate"/>.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns></returns>
        public IQueryable<TEntity> SatisfyingEntitiesFrom(IQueryable<TEntity> query)
        {
            return query.Where(Predicate);
        }

        /// <summary>
        /// Gets or sets the predicate (condition).
        /// </summary>
        /// <value>
        /// The predicate.
        /// </value>
        public Expression<Func<TEntity, bool>> Predicate { get; protected set; }
    }

}
