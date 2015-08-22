using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ResourceMetadata.Model;
using ResourceMetadata.Data.Infrastructure;

namespace ResourceMetadata.Data.Repositories
{
    public class ResourceRepository : RepositoryBase<Resource>, IResourceRepository
    {
        public ResourceRepository(IDatabaseFactory databaseFactory, IWorkContext workContext)
            : base(databaseFactory,workContext)
        {
          
        }
    }

    public interface IResourceRepository : IRepository<Resource>
    {

    }
}
