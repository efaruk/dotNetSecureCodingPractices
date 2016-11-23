using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Data.Entity.ModelConfiguration.Conventions;
using SCP.Data.Entities;
using System.Data.SQLite;
using System.Data.SQLite.EF6;

namespace SCP.Data
{
    public class ScpDbContext : DbContext
    {
        public ScpDbContext() : base("ScpDbContext")
        {
            Configure();
        }

        public ScpDbContext(string contextNameOrConnectionString) : base(contextNameOrConnectionString)
        {
            Configure();
        }

        public virtual DbSet<User> Users { get; set; }

        private void Configure()
        {
            //Database.SetInitializer(new NullDatabaseInitializer<ScpDbContext>());
            //Database.SetInitializer(new CreateDatabaseIfNotExists<ScpDbContext>());

            Configuration.AutoDetectChangesEnabled = false;
            Configuration.LazyLoadingEnabled = false;
            Configuration.ProxyCreationEnabled = false;
            Configuration.ValidateOnSaveEnabled = true;
            Configuration.UseDatabaseNullSemantics = true;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //throw new UnintentionalCodeFirstException();
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            Database.SetInitializer(new ScpDropCreateOnChange(modelBuilder));
            //Database.SetInitializer(new ScpDropCreateAlways(modelBuilder));
            base.OnModelCreating(modelBuilder);
        }
    }

    #region Migration Configuration

    public class ScpDbContextMigrationConfiguration : DbMigrationsConfiguration<ScpDbContext>
    {
        public ScpDbContextMigrationConfiguration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }
    }

    #endregion

}