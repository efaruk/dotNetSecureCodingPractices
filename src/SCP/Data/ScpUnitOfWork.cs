using SCP.Data.Repositories;

namespace SCP.Data
{
    //TODO: Do NOT use single UOW for whole app, we should have misson specific seperated UOW's and connection strings
    public class ScpUnitOfWork : UnitOfWorkBase<ScpDbContext>, IScpUnitOfWork
    {
        private readonly IDateTimeProvider _dateTimeProvider;

        public ScpUnitOfWork(ScpDbContext context, IDateTimeProvider dateTimeProvider)
        {
            _dateTimeProvider = dateTimeProvider;
            Context = context;
            SetupRepositories();
        }

        public UserRepository UserRepository { get; set; }

        private void SetupRepositories()
        {
            UserRepository = new UserRepository(_dateTimeProvider, Context);
        }
    }
}