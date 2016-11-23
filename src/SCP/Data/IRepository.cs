using System;
using System.Collections.Generic;
using SCP.Data.Entities;

namespace SCP.Data
{
    public interface IRepository<TEntity, in TId> : IDisposable where TEntity : IEntity<TId>
        where TId : struct
    {
        void Delete(TId id, bool saveAfter = false, bool async = false);

        void Delete(TEntity entity, bool saveAfter = false, bool async = false);

        void Insert(TEntity entity, bool saveAfter = false, bool async = false);

        void Update(TEntity entity, bool saveAfter = false, bool async = false);

        void BulkInsert(IEnumerable<TEntity> entities, bool saveAfter = false, bool async = false);

        void BulkDelete(IEnumerable<object> ids, bool saveAfter = false, bool async = false);

        void BulkDelete(IEnumerable<TEntity> entities, bool saveAfter = false, bool async = false);

        void BulkUpdate(IEnumerable<TEntity> entities, bool saveAfter = false, bool async = false);

        TEntity GetLocalExistingEntity(TEntity entity);

        /// <summary>
        ///     Get <see cref="TEntity">TEntity</see> by Tid.
        /// </summary>
        TEntity Get(TId id);

        IEnumerable<TEntity> GetAll();

        /// <summary>
        ///     Save to commit changes to persistent store...
        /// </summary>
        void Save(bool async = false);
    }
}