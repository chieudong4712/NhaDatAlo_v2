using ResourceMetadata.Model.Base;
using ResourceMetadata.Model;

namespace ResourceMetadata.Model.SearchModels
{
    public interface IPictureSE: IBaseSearchEntity<Picture>
    {
        string Title { get; set; }
        string Description { get; set; }
        string PictureType { get; set; }
     
    }
    public class PictureSE : BaseSearchEntity<Picture>, IPictureSE
    {
        public string Title { get; set; }
        public string Description { get; set; }

        private string _pictureType;
        public string PictureType
        {
            get
            {
                return _pictureType;
            }
            set
            {
                _pictureType = value;
            }
        }

        private string _refType;
        public string RefType
        {
            get { return _refType; }
            set { _refType = value; }
        }

        private long _refId;
        public long RefId
        {
            get { return _refId; }
            set { _refId = value; }
        }

    }
}
