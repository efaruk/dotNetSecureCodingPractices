using System;

namespace SCP.Data
{
    public interface IUnitOfWork<TContext> : IDisposable where TContext : IDisposable
    {
        TContext Context { get; set; }
    }
}