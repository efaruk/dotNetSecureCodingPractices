using System;

namespace SCP.Data
{
    public abstract class UnitOfWorkBase<TContext> : IUnitOfWork<TContext> where TContext : IDisposable
    {
        public TContext Context { get; set; }

        #region IDisposable

        private bool _disposed;

        /// <summary>
        ///     Custom dispose method for internal use
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
                if (disposing)
                    if (Context != null)
                        Context.Dispose();
            _disposed = true;
        }

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}