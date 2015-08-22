using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ResourceMetadata.Model;
using ResourceMetadata.Data.Infrastructure;

namespace ResourceMetadata.Data.Repositories
{
    public class CategoryRepository : RepositoryBase<Category>, ICategoryRepository
    {
        public CategoryRepository(IDatabaseFactory databaseFactory, IWorkContext workContext)
            : base(databaseFactory, workContext)
        {
          
        }
    }

    public interface ICategoryRepository : IRepository<Category>
    {

    }
}
