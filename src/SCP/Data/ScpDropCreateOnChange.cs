using System;
using System.Data.Entity;
using SCP.Data.Entities;
using SQLite.CodeFirst;

namespace SCP.Data
{

    public class ScpDropCreateAlways : SqliteDropCreateDatabaseAlways<ScpDbContext>
    {
        public ScpDropCreateAlways(DbModelBuilder modelBuilder) : base(modelBuilder)
        {
        }

        protected override void Seed(ScpDbContext context)
        {
            var dataInitializer = new DataInitializer(context);
            dataInitializer.InitializeData();

            base.Seed(context);
        }
    }

    public class ScpDropCreateOnChange : SqliteDropCreateDatabaseWhenModelChanges<ScpDbContext>
    {
        public ScpDropCreateOnChange(DbModelBuilder modelBuilder) : base(modelBuilder)
        {
        }

        protected override void Seed(ScpDbContext context)
        {
            var dataInitializer = new DataInitializer(context);
            dataInitializer.InitializeData();

            base.Seed(context);
        }
        
        
    }

    public class DataInitializer
    {
        private readonly ScpDbContext _context;

        public DataInitializer(ScpDbContext context)
        {
            _context = context;
        }

        public void InitializeData()
        {
            AddUsers();
        }

        private void AddUsers()
        {
            var admin = new User
            {
                Name = "System Admin",
                EMail = "admin@exploit.net",
                IsAdmin = true,
                Password = "123456",
                UserName = "admin",
                IsStaff = true
            };

            var sysadmin = new User
            {
                Name = "Trusted Admin",
                EMail = "sysadmin@exploit.net",
                IsAdmin = true,
                Password = "426ff20b0e4e9fb8b0d2cdf616e0f3877c127b5c03fbc564ef0768bde1162069",
                UserName = "sysadmin",
                IsStaff = true
            };

            var staff = new User
            {
                Name = "System Member",
                EMail = "staff@exploit.net",
                IsAdmin = false,
                Password = "123456",
                UserName = "staff",
                IsStaff = true
            };

            var user = new User
            {
                Name = "Trusted Member",
                EMail = "user@exploit.net",
                IsAdmin = false,
                Password = "426ff20b0e4e9fb8b0d2cdf616e0f3877c127b5c03fbc564ef0768bde1162069",
                UserName = "user",
                IsStaff = true
            };

            _context.Users.Add(admin);
            _context.Users.Add(sysadmin);
            _context.Users.Add(staff);
            _context.Users.Add(user);
        }
    }
}