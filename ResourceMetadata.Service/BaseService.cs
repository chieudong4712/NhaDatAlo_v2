
using ResourceMetadata.Core.Common;
using ResourceMetadata.Data.Infrastructure;
using ResourceMetadata.Model.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using ResourceMetadata.Core.Util;

namespace ResourceMetadata.Service
{
    public interface IBaseService<T, SE> where T : BaseEntity where SE : BaseSearchEntity<T>
    {
        IEnumerable<T> Get();
        IEnumerable<T> GetPaged(int pageSize, int pageNumber, string sortField, ref int totalCount);
        T GetById(long id);
        T Update(T setting);
        T Add(T setting);
        void Delete(long id);
        void Delete(long[] ids);
        IQueryable<T> Query(Expression<Func<T, bool>> where);
        IQueryable<T> Search(SE se, bool isNoTracking = true, bool withoutDeleted = true);

        
    }

    public abstract class BaseService<T, SE> : IBaseService<T, SE>
        where T : BaseEntity
        where SE : BaseSearchEntity<T>
    {
        public readonly IUnitOfWork unitOfWork;
        public readonly IRepository<T> repository;

        public BaseService() { }
        
        public BaseService(IUnitOfWork unitOfWork, IRepository<T> repository)
        {
            this.unitOfWork = unitOfWork;
            this.repository = repository;
        }

        public IEnumerable<T> Get()
        {
            return repository.GetAll();
        }

        public IEnumerable<T> GetPaged(int pageSize, int pageNumber,string sortField, ref int totalCount)
        {
            var query = repository.TableNoTracking
                .Where(x => x.Status != StatusObject.Deleted)
                .OrderByDescending(x => x.Id);

            if (!String.IsNullOrEmpty(sortField))
            {
                query = query.OrderByPropertyName(sortField);
            }

            totalCount = query.Count();
            var settings = query.Skip((pageNumber - 1) * pageSize).Take(pageSize);
            return settings;
        }

        public T GetById(long id)
        {
            return repository.GetById(id);
        }

        public T Add(T t)
        {
            repository.Add(t);
            unitOfWork.SaveChanges();
            return t;
        }

        public T Update(T t)
        {
            try
            {
                repository.Update(t);
                unitOfWork.SaveChanges();
                return t;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public void Delete(long id)
        {
            var t = repository.GetById(id);
            repository.Delete(t);
            unitOfWork.SaveChanges();
        }

        public void Delete(long[] ids)
        {
            var items = repository.GetMany(x => ids.Contains(x.Id));
            repository.Delete(x=>ids.Contains(x.Id));
            unitOfWork.SaveChanges();
        }

        public IQueryable<T> Query(Expression<Func<T, bool>> where)
        {
            return repository.Query(where);
        }
        
        private IQueryable<T> Search(IQueryable<T> q, SE se)
        {
            if (se.Id != 0)
            {
                q = q.Where(x => x.Id == se.Id);
            }
            if (!String.IsNullOrEmpty(se.Attribute))
            {
                foreach (char c in se.Attribute)
                {
                    q = q.Where(x => x.Attribute.Contains(c.ToString()));
                }
            }
            if (!String.IsNullOrEmpty(se.Status))
            {
                q = q.Where(x => x.Status == se.Status);
            }
            if (!String.IsNullOrEmpty(se.CreatedBy))
            {
                q = q.Where(x => x.CreatedBy == se.CreatedBy);
            }
            if (!String.IsNullOrEmpty(se.LastUpdatedBy))
            {
                q = q.Where(x => x.LastUpdatedBy == se.LastUpdatedBy);
            }
            if (se.CreatedDateTime.Year > 2014)
            {
                q = q.Where(x => x.CreatedDateTime.CompareTo(se.CreatedDateTime) == 0);
            }
            if (se.LastUpdatedDateTime.Year > 2014)
            {
                q = q.Where(x => x.LastUpdatedDateTime.CompareTo(se.LastUpdatedDateTime) == 0);
            }
            if (se.CreatedDateTimeFrom.Year > 2014 && se.CreatedDateTimeTo.Year > 2014)
            {
                q = q.Where(x => x.CreatedDateTime >= se.CreatedDateTimeFrom && x.CreatedDateTime <= se.CreatedDateTimeTo);
            }
            if (se.LastUpdatedDateTimeFrom.Year > 2014 && se.LastUpdatedDateTimeTo.Year > 2014)
            {
                q = q.Where(x => x.LastUpdatedDateTime >= se.LastUpdatedDateTimeFrom && x.LastUpdatedDateTime <= se.LastUpdatedDateTimeTo);
            }


            return q;
        }

        public IQueryable<T> Search(SE se, bool isNoTracking = true, bool withoutDeleted = true)
        {
            var q = isNoTracking ? repository.TableNoTracking : repository.Table;
            if (withoutDeleted) q = q.Where(x => x.Status != StatusObject.Deleted);
            q = Search(q, se);
            return q;
        }
    }
}
