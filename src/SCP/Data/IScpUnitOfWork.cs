using SCP.Data.Repositories;

namespace SCP.Data
{
    public interface IScpUnitOfWork : IUnitOfWork<ScpDbContext>
    {
        UserRepository UserRepository { get; set; }
    }
}