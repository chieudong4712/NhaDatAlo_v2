using ResourceMetadata.Data.Infrastructure;
using ResourceMetadata.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResourceMetadata.Data.Repositories
{
    public class UserProfileRepository : RepositoryBase<UserProfile>, IUserProfileRepository
    {
        public UserProfileRepository(IDatabaseFactory databaseFactory, IWorkContext workContext)
            : base(databaseFactory, workContext)
        {
        }
    }
    public interface IUserProfileRepository : IRepository<UserProfile>
    {
    }
}
