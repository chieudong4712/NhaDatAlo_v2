using ResourceMetadata.Core.Common;
using ResourceMetadata.Model.Base;
using ResourceMetadata.Model.Base;

namespace ResourceMetadata.Model
{
    public class Picture: BaseEntity
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public string FileName { get; set; }

        /// <summary>
        /// Ref to PictureType enum: Avatar, Background...
        /// </summary>
        public string PictureType { get; set; }

        public int OrderNumber { get; set; }

        public string RefType { get; set; }
        public long RefId { get; set; }

        public Picture() 
        {
 
        }

    }
}