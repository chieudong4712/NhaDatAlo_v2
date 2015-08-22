using ResourceMetadata.Data.Infrastructure;
using ResourceMetadata.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResourceMetadata.Data.Repositories
{
    public class UserRepository : RepositoryBase<ApplicationUser>, IUserRepository
    {

        public UserRepository(IDatabaseFactory dbFactory, IWorkContext workContext)
            : base(dbFactory,workContext)
        {

        } 
    }


    public interface IUserRepository : IRepository<ApplicationUser>
    { 
    }
}
