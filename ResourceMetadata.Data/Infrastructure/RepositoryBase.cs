using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data;
using System.Linq.Expressions;
using System.Reflection;
using ResourceMetadata.Core.Util;
using ResourceMetadata.Core.Common;

namespace ResourceMetadata.Data.Infrastructure
{
    public abstract class RepositoryBase<T> where T : class, new()
    {

        private ResourceManagerEntities dataContext;
        private readonly IDbSet<T> dbset;
        protected RepositoryBase(IDatabaseFactory databaseFactory, IWorkContext workContext)
        {
            DatabaseFactory = databaseFactory;
            dbset = DataContext.Set<T>();
            WorkContext = workContext;
        }

        #region PersistenData
        private class PersistentData
        {
            public PropertyInfo[] PersistentMembers = null;
            public PropertyInfo[] UpdatedMembers = null;
            public PropertyInfo Status = null;
            public PropertyInfo CreatedBy = null;
            public PropertyInfo CreatedDateTime = null;
            public PropertyInfo LastUpdatedBy = null;
            public PropertyInfo LastUpdatedDateTime = null;
        }
        private static IDictionary<Type, PersistentData> _CachePersistantDatas = new Dictionary<Type, PersistentData>();
        private static IDictionary<Type, PersistentData> GetCachePersistantDatas()
        {
            if (_CachePersistantDatas == null)
                _CachePersistantDatas = new Dictionary<Type, PersistentData>();
            return _CachePersistantDatas;
        }
        private PersistentData CreatePersistentData(PropertyInfo[] meta)
        {
            PersistentData data = new PersistentData();
            foreach (var member in meta)
            {
                if (member.Name == "Status")
                {
                    data.Status = (PropertyInfo)member;
                }
                else if (member.Name == "CreatedBy")
                {
                    data.CreatedBy = (PropertyInfo)member;
                }
                else if (member.Name == "CreatedDateTime")
                {
                    data.CreatedDateTime = (PropertyInfo)member;
                }
                else if (member.Name == "LastUpdatedBy")
                {
                    data.LastUpdatedBy = (PropertyInfo)member;
                }
                else if (member.Name == "LastUpdatedDateTime")
                {
                    data.LastUpdatedDateTime = (PropertyInfo)member;
                }
            }
            return data;

        }
        #endregion

        protected IWorkContext WorkContext { get; set; }
        protected IDatabaseFactory DatabaseFactory
        {
            get;
            private set;
        }

        protected ResourceManagerEntities DataContext
        {
            get { return dataContext ?? (dataContext = DatabaseFactory.Get()); }
        }
        public virtual T Add(T entity)
        {
            DateTime now = DateTime.Now;
            Type type = typeof(T);

            IDictionary<Type, PersistentData> cachePersistantDatas = GetCachePersistantDatas();
            PersistentData persistentData = null;
            if (!cachePersistantDatas.TryGetValue(type, out persistentData))
            {
                persistentData = CreatePersistentData(type.GetProperties());
                cachePersistantDatas.Add(type, persistentData);
            }

            if (ReflectionUtil.GetPropertyValue(entity, "Status") == null ||
                ReflectionUtil.GetPropertyValue(entity, "Status") != null && ReflectionUtil.GetPropertyValue(entity, "Status").ToString() == String.Empty)
                persistentData.Status.SetValue(entity, StatusObject.Enable, null);
            if (persistentData.CreatedBy != null)
                persistentData.CreatedBy.SetValue(entity, WorkContext.CurrentUsername, null);
            if (persistentData.CreatedBy != null)
                persistentData.CreatedBy.SetValue(entity, WorkContext.CurrentUsername, null);
            if (persistentData.LastUpdatedBy != null)
                persistentData.LastUpdatedBy.SetValue(entity, WorkContext.CurrentUsername, null);
            if (persistentData.CreatedDateTime != null)
                persistentData.CreatedDateTime.SetValue(entity, now, null);
            if (persistentData.LastUpdatedDateTime != null)
                persistentData.LastUpdatedDateTime.SetValue(entity, now, null);

            dbset.Add(entity);
            return entity;
        }
        public virtual void Update(T entity)
        {
            Type type = typeof(T);

            IDictionary<Type, PersistentData> cachePersistantDatas = GetCachePersistantDatas();
            PersistentData persistentData = null;
            if (!cachePersistantDatas.TryGetValue(type, out persistentData))
            {
                persistentData = CreatePersistentData(type.GetProperties());
                cachePersistantDatas.Add(type, persistentData);
            }

            if (persistentData.LastUpdatedBy != null)
                persistentData.LastUpdatedBy.SetValue(entity, WorkContext.CurrentUsername, null);
            if (persistentData.LastUpdatedDateTime != null)
                persistentData.LastUpdatedDateTime.SetValue(entity, DateTime.Now, null);

            dbset.Attach(entity);
            dataContext.Entry(entity).State = EntityState.Modified;

            //Ignore create information
            if (persistentData.CreatedDateTime != null)
                dataContext.Entry(entity).Property(persistentData.CreatedDateTime.Name).IsModified = false;
            if (persistentData.CreatedBy != null)
                dataContext.Entry(entity).Property(persistentData.CreatedBy.Name).IsModified = false;
        }
        public virtual void Delete(T entity)
        {
            Type type = typeof(T);

            IDictionary<Type, PersistentData> cachePersistantDatas = GetCachePersistantDatas();
            PersistentData persistentData = null;
            if (!cachePersistantDatas.TryGetValue(type, out persistentData))
            {
                persistentData = CreatePersistentData(type.GetProperties());
                cachePersistantDatas.Add(type, persistentData);
            }

            if (persistentData.LastUpdatedBy != null)
                persistentData.LastUpdatedBy.SetValue(entity, WorkContext.CurrentUsername, null);
            if (persistentData.LastUpdatedDateTime != null)
                persistentData.LastUpdatedDateTime.SetValue(entity, DateTime.Now, null);
            if (persistentData.Status != null)
                persistentData.Status.SetValue(entity, StatusObject.Deleted, null);

            dbset.Attach(entity);
            dataContext.Entry(entity).State = EntityState.Modified;

            //Ignore create information
            if (persistentData.CreatedDateTime != null)
                dataContext.Entry(entity).Property(persistentData.CreatedDateTime.Name).IsModified = false;
            if (persistentData.CreatedBy != null)
                dataContext.Entry(entity).Property(persistentData.CreatedBy.Name).IsModified = false;
        }
        public virtual void Delete(Expression<Func<T, bool>> where)
        {
            IEnumerable<T> objects = dbset.Where<T>(where).AsEnumerable();
            foreach (T obj in objects)
                Delete(obj);
        }

        public virtual void DeletePermanent(T entity)
        {
            if (dataContext.Entry(entity).State == EntityState.Detached)
                dbset.Attach(entity);
            dbset.Remove(entity);
        }

        public virtual void DeletePermanent(Expression<Func<T, bool>> where)
        {
            IEnumerable<T> objects = dbset.Where<T>(where).AsEnumerable();
            foreach (T obj in objects)
                dbset.Remove(obj);
        }
        public virtual T GetById(long id)
        {
            return dbset.Find(id);
        }
        public virtual T GetById(string id)
        {
            return dbset.Find(id);
        }
        public virtual IEnumerable<T> GetAll()
        {
            //default Status!=Delete
            return dbset.WhereByCustom("Status", BinaryExpressionName.NotEqual, StatusObject.Deleted).ToList();
        }
        public virtual IEnumerable<T> GetMany(Expression<Func<T, bool>> where)
        {
            return dbset.Where(where).ToList();
        }
        public virtual IQueryable<T> Query(Expression<Func<T, bool>> where)
        {
            return dbset.Where(where);
        }

        public T Get(Expression<Func<T, bool>> where)
        {
            return dbset.Where(where).FirstOrDefault<T>();
        }

        /// <summary>
        /// Gets a table
        /// </summary>
        public virtual IQueryable<T> Table
        {
            get
            {
                return dbset;
            }
        }


        /// <summary>
        /// Gets a table with "no tracking" enabled (EF feature) Use it only when you load record(s) only for read-only operations
        /// </summary>
        public virtual IQueryable<T> TableNoTracking
        {
            get
            {
                dataContext.Configuration.LazyLoadingEnabled = false;
                return dbset.AsNoTracking();
            }
        }
    }
}
