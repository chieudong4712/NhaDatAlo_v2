
using ResourceMetadata.Data.Infrastructure;
using ResourceMetadata.Model;
namespace ResourceMetadata.Data.Repositories
{
    public class PictureRepository : RepositoryBase<Picture>, IPictureRepository
        {
        public PictureRepository(IDatabaseFactory databaseFactory, IWorkContext workContext)
            : base(databaseFactory, workContext)
            {
            }
      
        }
    public interface IPictureRepository : IRepository<Picture>
    {    

    }
}