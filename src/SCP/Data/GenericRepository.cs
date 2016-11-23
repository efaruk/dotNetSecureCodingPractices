using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using SCP.Data.Entities;

namespace SCP.Data
{
    public class GenericRepository<TEntity, TId> : IRepository<TEntity, TId> where TEntity : class, IEntity<TId>
        where TId : struct
    {
        protected DbContext Context;
        protected DbSet<TEntity> Set;

        public GenericRepository(DbContext context, bool disableChangeTracking = false,
            bool disableLazyLoading = false,
            bool disableProxyCreation = false, bool useDatabaseNullSemantics = false, bool disableValidateOnSave = false)
        {
            Context = context;
            Set = context.Set<TEntity>();

            Context.Configuration.AutoDetectChangesEnabled = !disableChangeTracking;
            Context.Configuration.LazyLoadingEnabled = !disableLazyLoading;
            Context.Configuration.ProxyCreationEnabled = !disableProxyCreation;
            Context.Configuration.UseDatabaseNullSemantics = !useDatabaseNullSemantics;
            Context.Configuration.ValidateOnSaveEnabled = !disableValidateOnSave;
        }

        public int? SearchTimeOut { get; set; }

        #region Private Methods

        private object GetPrimaryKeyValue(DbContext context, TEntity entity)
        {
            object result = null;
            if (context == null)
                throw new ArgumentNullException("context");

            var set = ((IObjectContextAdapter) context).ObjectContext.CreateObjectSet<TEntity>();
            var entitySet = set.EntitySet;
            var propertyName = entitySet.ElementType.KeyMembers.Select(k => k.Name).FirstOrDefault();
            if (propertyName != null)
                result = entity.GetType().GetProperty(propertyName).GetValue(entity);
            return result;
        }

        #endregion

        #region Public Methods

        public IQueryable<TEntity> AsQueryable()
        {
            return Set.AsQueryable();
        }

        public void Delete(TId id, bool saveAfter = false, bool async = false)
        {
            var entityToDelete = Set.Find(id);
            Delete(entityToDelete);
            if (saveAfter) Save(async);
        }

        public void Delete(TEntity entity, bool saveAfter = false, bool async = false)
        {
            if (Context.Entry(entity).State == EntityState.Detached)
                Set.Attach(entity);
            Set.Remove(entity);
            if (saveAfter) Save(async);
        }

        public void Insert(TEntity entity, bool saveAfter = false, bool async = false)
        {
            Set.Add(entity);
            if (saveAfter) Save(async);
        }

        public void Update(TEntity entity, bool saveAfter = false, bool async = false)
        {
            var existing = GetLocalExistingEntity(entity);
            if (existing != null)
                Context.Entry(existing).State = EntityState.Detached;
            Set.Attach(entity);
            Context.Entry(entity).State = EntityState.Modified;
            if (saveAfter) Save(async);
        }

        public void BulkInsert(IEnumerable<TEntity> entities, bool saveAfter = false, bool async = false)
        {
            Set.AddRange(entities);
            if (saveAfter) Save(async);
        }

        public void BulkDelete(IEnumerable<object> ids, bool saveAfter = false, bool async = false)
        {
            foreach (var id in ids)
            {
                var entityToDelete = Set.Find(id);
                if (entityToDelete != null)
                    Delete(entityToDelete);
            }
            if (saveAfter) Save(async);
        }

        public void BulkDelete(IEnumerable<TEntity> entities, bool saveAfter = false, bool async = false)
        {
            foreach (var entity in entities)
            {
                if (Context.Entry(entity).State == EntityState.Detached)
                    Set.Attach(entity);
                Set.Remove(entity);
            }
            if (saveAfter) Save(async);
        }

        public void BulkUpdate(IEnumerable<TEntity> entities, bool saveAfter = false, bool async = false)
        {
            foreach (var entity in entities)
            {
                var existing = GetLocalExistingEntity(entity);
                if (existing != null)
                    Context.Entry(existing).State = EntityState.Detached;
                Set.Attach(entity);
                Context.Entry(entity).State = EntityState.Modified;
            }
            if (saveAfter) Save(async);
        }

        public TEntity GetLocalExistingEntity(TEntity entity)
        {
            if (!Set.Local.Any()) return null;
            var primaryKey = GetPrimaryKeyValue(Context, entity);
            var existing = Set.Local.FirstOrDefault(
                f => GetPrimaryKeyValue(Context, f).Equals(primaryKey));
            return existing;
        }

        public TEntity Get(TId id)
        {
            return Set.Find(id);
        }

        public IEnumerable<TEntity> GetAll()
        {
            var query = AsQueryable();
            var list = query.ToList();
            return list;
        }

        public void Save(bool async = false)
        {
            if (async)
                Context.SaveChangesAsync();
            else
                Context.SaveChanges();
        }

        #endregion

        #region IDisposable

        private bool _disposed;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
                if (disposing)
                    Context.Dispose();
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}