using ResourceMetadata.Model.Base;
using ResourceMetadata.Model;
using System.Collections.Generic;

namespace ResourceMetadata.Model.SearchModels
{
    public interface IPropertySE: IBaseSearchEntity<Property>
    {
        string Title { get; set; }
        string Value { get; set; }
        string Type { get; set; }
        long? ParentId { get; set; }
     
    }
    public class PropertySE : BaseSearchEntity<Property>, IPropertySE
    {
        public string Title { get; set; }
        public string Value { get; set; }
        public string Type { get; set; }

        private long? _parentId = null;
        /// <summary>
        /// ParentId = null: search all, ParentId = 0: is parent
        /// </summary>
        public long? ParentId {
            get {
                if (_parentId == null)
                {
                    _parentId = 0;
                }
                if (_parentId == 0)
                {
                    _parentId = null;
                }
                return _parentId; }
            set { _parentId = value; }
        }

        private IEnumerable<long> _parentIds = null;
        /// <summary>
        /// ParentId = 0: search all
        /// </summary>
        public IEnumerable<long> ParentIds
        {
            get { return _parentIds; }
            set { _parentIds = value; }
        }

        public string UserId { get; set; }
    }
}
