using DataStructures;

namespace DataAccess
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IObjectContext objectContext;

        public UnitOfWork(IObjectContext objectContext)
        {
            this.objectContext = objectContext;
        }

        /// <summary>
        /// Saves all changes to all data sources. In my case database only
        /// </summary>
        public void Commit()
        {
            objectContext.SaveChanges();
        }
    }
}