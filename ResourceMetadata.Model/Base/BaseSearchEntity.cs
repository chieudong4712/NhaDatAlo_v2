using ResourceMetadata.Model.Base;
using System;
namespace ResourceMetadata.Model.Base
{
    
    public interface IBaseSearchEntity<T> where T: BaseEntity
    {
        int PageSize { get; set; }
        int PageNumber { get; set; }
        string SortField { get; set; }
        string SortOrder { get; set; }
        DateTime CreatedDateTimeFrom { get; set; }
        DateTime CreatedDateTimeTo { get; set; }
        DateTime LastUpdatedDateTimeFrom { get; set; }
        DateTime LastUpdatedDateTimeTo { get; set; }
    }

    public abstract class BaseSearchEntity<T> : BaseEntity, IBaseSearchEntity<T> where T : BaseEntity
    {
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public string SortField { get; set; }
        public string SortOrder { get; set; }
        public DateTime CreatedDateTimeFrom { get; set; }
        public DateTime CreatedDateTimeTo { get; set; }
        public DateTime LastUpdatedDateTimeFrom { get; set; }
        public DateTime LastUpdatedDateTimeTo { get; set; }
    }
}
