using ResourceMetadata.Core.Common;
using ResourceMetadata.Model.Base;
using System.Collections.Generic;

namespace ResourceMetadata.Model
{
    public class Category : BaseEntity
    {

        #region SEO 
        public string Title { get; set; }

        public string Description { get; set; }

        public string Keyword { get; set; }

        public string FriendURL { get; set; }

        #endregion

        public string Icon { get; set; }

        public string Picture { get; set; }

        public int OrderNumber { get; set; }

        /// <summary>
        /// System Category, Shop Category, System Menu, Shop Menu
        /// </summary>
        public CategoryType CategoryType { get; set; }

        //Foreign key
        public string RefType { get; set; }
        public long RefId { get; set; }


        public long? ParentId { get; set; }
        
        public virtual Category Parent { get; set; }

        public string UserId { get; set; }


        public Category() 
        {
 
        }

    }
}