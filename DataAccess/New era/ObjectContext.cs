
//using DataStructures;
//using Microsoft.EntityFrameworkCore;


//namespace DataAccess
//{
//    public class ObjectContext : DbContext, IObjectContext
//    {
//        private readonly string connectionString;

//        //for db migrations only
//        public ObjectContext() : this(@"Server=.\SQLEXPRESS;Database=CentralServerLocal;Trusted_Connection=True;MultipleActiveResultSets=true")
//        {
            
//        }

//        /// <summary>
//        /// Initializes a new instance of the <see cref="ObjectContext"/> class for unit testing with in memory database
//        /// </summary>
//        /// <param name="options">The options.</param>
//        public ObjectContext(DbContextOptions<ObjectContext> options)
//            : base(options)

//        {

//        }

//        public ObjectContext(string connectionString)
//        {
//            this.connectionString = connectionString;
//        }

//        /// <summary>
//        /// Configures database engine to use and connection string.
//        /// </summary>
//        /// <param name="options">The options.</param>
//        protected override void OnConfiguring(DbContextOptionsBuilder options)
//        {
//            if (options.IsConfigured) return;

            

//            options.UseSqlServer();
//        }

        

//        /// <summary>
//        /// Maps entities to database.
//        /// </summary>
//        /// <param name="modelBuilder">The model builder.</param>
//        protected override void OnModelCreating(ModelBuilder modelBuilder)
//        {
//            ModelConfigurator.ConfigureModel(ref modelBuilder);
//        }

//        /// <summary>
//        /// Attaches the specified entity to object context in unmodified state.
//        /// </summary>
//        /// <typeparam name="TEntity">The type of the entity.</typeparam>
//        /// <param name="entity">The entity.</param>
//        /// <param name="saveDependents">False  - only entity is saved. True - dependents objects exposed through navigation properties are saved as well.</param>
//        /// <returns></returns>
//        public void Attach<TEntity>(TEntity entity, bool saveDependents = false) where TEntity : class
//        {
//            base.Attach(entity, BoolToGraphBehavior(saveDependents));
//        }

//        /// <summary>
//        /// Attaches the specified entity to object context in modified state.
//        /// </summary>
//        /// <typeparam name="TEntity">The type of the entity.</typeparam>
//        /// <param name="entity">The entity.</param>
//        /// <param name="saveDependents">False  - only entity is saved. True - dependents objects exposed through navigation properties are saved as well.</param>
//        /// <returns></returns>
//        public void AttachAsModified<TEntity>(TEntity entity, bool saveDependents = false) where TEntity : class
//        {
//            base.Update(entity, BoolToGraphBehavior(saveDependents));
//        }

//        public Microsoft.Data.Entity.DbSet<TEntity> Set<TEntity>() where TEntity : class
//        {
//            return null;
//        }

//        /// <summary>
//        /// Peforms db migration to follow application version.
//        /// </summary>
//        public void MigrateDatabase()
//        {
//            Database.Migrate();   
//        }

//        /// <summary>
//        /// Adds the seed data.
//        /// </summary>
//        public void GenerateSeedData()
//        {
//            SeedData.Generate(this);
//        }

//        private static GraphBehavior BoolToGraphBehavior(bool saveDependents)
//        {
//            return saveDependents ? GraphBehavior.IncludeDependents : GraphBehavior.SingleObject;
//        }

        
//    }
//}
