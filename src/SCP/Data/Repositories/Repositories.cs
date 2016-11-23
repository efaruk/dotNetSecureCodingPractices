using System.Data.Entity;
using System.Linq;
using SCP.Data.Entities;

namespace SCP.Data.Repositories
{
    public class UserRepository : GenericRepository<User, int>
    {
        private readonly IDateTimeProvider _dateTimeProvider;

        public UserRepository(IDateTimeProvider dateTimeProvider, DbContext context) : base(context)
        {
            _dateTimeProvider = dateTimeProvider;
        }

        public User GetUserByUsernameAndPassword(string userName, string password)
        {
            var query = AsQueryable().Where(u => (u.UserName == userName) && (u.Password == password));
            var user = query.FirstOrDefault();
            return user;
        }
    }
}