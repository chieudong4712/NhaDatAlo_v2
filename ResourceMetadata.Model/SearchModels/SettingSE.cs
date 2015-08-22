using ResourceMetadata.Model;
using ResourceMetadata.Model.Base;

namespace ResourceMetadata.Model.SearchModels
{
    public interface ISettingSE: IBaseSearchEntity<Setting>
    {
        string Name { get; set; }
        string Value { get; set; }
        long StoreId { get; set; }
     
    }
    public class SettingSE : BaseSearchEntity<Setting>, ISettingSE
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public long StoreId { get; set; }
    }
}
