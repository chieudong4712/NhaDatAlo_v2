using ResourceMetadata.Model;
using ResourceMetadata.Model.Base;

namespace ResourceMetadata.Model.SearchModels
{
    public interface IFunctionSE: IBaseSearchEntity<Function>
    {
        string Title { get; set; }
        string Description { get; set; }
        string ActionName { get; set; }
        string ControllerName { get; set; }
    }
    public class FunctionSE : BaseSearchEntity<Function>, IFunctionSE
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string ActionName { get; set; }
        public string ControllerName { get; set; }

        private long? _parentId = 0;
        /// <summary>
        /// ParentId = 0: search all
        /// </summary>
        public long? ParentId {
            get { return _parentId; }
            set { _parentId = value; }
        }
    }
}
