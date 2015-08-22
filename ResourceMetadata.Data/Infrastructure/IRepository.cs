using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace ResourceMetadata.Data.Infrastructure
{
    public interface IRepository<T> where T : class
    {
        T Get(Expression<Func<T, bool>> where);
        T GetById(long id);
        T GetById(string id);
        IEnumerable<T> GetAll();
        IEnumerable<T> GetMany(Expression<Func<T, bool>> where);
        IQueryable<T> Query(Expression<Func<T, bool>> where);
        T Add(T entity);

        void Update(T entity);

        void Delete(T entity);

        void Delete(Expression<Func<T, bool>> where);

        /// <summary>
        /// Gets a table
        /// </summary>
        IQueryable<T> Table { get; }

        /// <summary>
        /// Gets a table with "no tracking" enabled (EF feature) Use it only when you load record(s) only for read-only operations
        /// </summary>
        IQueryable<T> TableNoTracking { get; }
    }

}
