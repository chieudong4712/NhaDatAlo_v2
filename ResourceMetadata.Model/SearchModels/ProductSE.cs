using ResourceMetadata.Model.Base;
using ResourceMetadata.Model;

namespace ResourceMetadata.Model.SearchModels
{
    public interface IProductSE: IBaseSearchEntity<Product>
    {
        string Title { get; set; }
        string Description { get; set; }

        long? CategoryId { get; set; }
        string UserId { get; set; }
     
    }
    public class ProductSE : BaseSearchEntity<Product>, IProductSE
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public long? CategoryId { get; set; }
        public string UserId { get; set; }
    }
}
