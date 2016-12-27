using Microsoft.EntityFrameworkCore;

namespace DataStructures
{
    public interface IObjectContext
    {
        /// <summary>
        /// Attaches the specified entity to object context in unmodified state.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="entity">The entity.</param>
        /// <param name="saveDependents">False  - only entity is saved. True - dependents objects exposed through navigation properties are saved as well.</param>
        /// <returns></returns>
        void Attach<TEntity>(TEntity entity, bool saveDependents = false) where TEntity : class;

        /// <summary>
        /// Attaches the specified entity to object context in modified state.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="entity">The entity.</param>
        /// <param name="saveDependents">False  - only entity is saved. True - dependents objects exposed through navigation properties are saved as well.</param>
        /// <returns></returns>
        void AttachAsModified<TEntity>(TEntity entity, bool saveDependents = false) where TEntity : class;

        /// <summary>
        /// Saves the changes in entities to database
        /// </summary>
        /// <returns></returns>
        int SaveChanges();

        /// <summary>
        /// Returns DbSet for specific entity.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <returns></returns>
        DbSet<TEntity> Set<TEntity>() where TEntity : class;

        /// <summary>
        /// Peforms db migration to follow application version.
        /// </summary>
        void MigrateDatabase();

        /// <summary>
        /// Adds the seed data.
        /// </summary>
        void GenerateSeedData();
    }
}