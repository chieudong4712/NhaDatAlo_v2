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
    public class SettingService : BaseService<Setting, BaseSearchEntity<Setting>>, ISettingService
    {
        public SettingService(ISettingRepository repository, IUnitOfWork unitOfWork)
            : base(unitOfWork, repository)
        {
        }

        #region CRUD
        

        public IQueryable<Setting> Search(SettingSE se, bool isNoTracking = true, bool withoutDeleted = true)
        {
            var q = base.Search(se, isNoTracking, withoutDeleted);

            if (!String.IsNullOrEmpty(se.Name))
            {
                q = q.Where(x => x.Name.Contains(se.Name));
            }
            if (!String.IsNullOrEmpty(se.Value))
            {
                q = q.Where(x => x.Value.Contains(se.Value));
            }
            
            return q;
        }
        #endregion

        

    }

    public interface ISettingService : IBaseService<Setting, SettingSE>
    {

    }
}
