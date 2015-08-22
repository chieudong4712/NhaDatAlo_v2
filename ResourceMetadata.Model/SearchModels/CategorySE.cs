using ResourceMetadata.Model.Base;
using ResourceMetadata.Model;
using ResourceMetadata.Core.Common;

namespace ResourceMetadata.Model.SearchModels
{
    public interface ICategorySE: IBaseSearchEntity<Category>
    {
        string Title { get; set; }
        string Description { get; set; }
        CategoryType CategoryType { get; set; }
     
    }
    public class CategorySE : BaseSearchEntity<Category>, ICategorySE
    {
        public string Title { get; set; }
        public string Description { get; set; }

        private CategoryType _categoryType = CategoryType.SysCategory;

        public CategoryType CategoryType
        {
            get
            {
                return _categoryType;
            }
            set
            {
                _categoryType = value;
            }
        }

        private long? _parentId = 0;
        /// <summary>
        /// ParentId = 0: search all
        /// </summary>
        public long? ParentId {
            get { return _parentId; }
            set { _parentId = value; }
        }

        public string UserId { get; set; }
    }
}
