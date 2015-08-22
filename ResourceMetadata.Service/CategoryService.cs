using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ResourceMetadata.Data.Repositories;
using ResourceMetadata.Data.Infrastructure;
using ResourceMetadata.Model;
using ResourceMetadata.Core.Util;
using ResourceMetadata.Core.Common;
using ResourceMetadata.Model.SearchModels;
using ResourceMetadata.Model.Base;

namespace ResourceMetadata.Service
{
    public class CategoryService : BaseService<Category, BaseSearchEntity<Category>>, ICategoryService
    {
        public CategoryService(ICategoryRepository repository, IUnitOfWork unitOfWork)
            : base(unitOfWork, repository)
        {
        }

        #region CRUD
        

        public IQueryable<Category> Search(CategorySE se, bool isNoTracking = true, bool withoutDeleted = true)
        {
            var q = base.Search(se, isNoTracking, withoutDeleted);

            if (!String.IsNullOrEmpty(se.Title))
            {
                q = q.Where(x => x.Title.Contains(se.Title));
            }
            if (se.ParentId != 0)
            {
                q = q.Where(x => x.ParentId == se.ParentId);
            }
            
            
            return q;
        }
        #endregion

        

    }

    public interface ICategoryService : IBaseService<Category, CategorySE>
    {

    }
}
