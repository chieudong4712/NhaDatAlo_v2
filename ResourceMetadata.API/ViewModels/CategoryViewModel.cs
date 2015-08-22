using ResourceMetadata.Core.Common;
using ResourceMetadata.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace ResourceMetadata.API.ViewModels
{
    public class CategoryViewModel
    {
        #region SEO
        public long Id { get; set; }
        public string Title { get; set; }
        public int OrderNumber { get; set; }
        public string Description { get; set; }

        public string Keyword { get; set; }

        public string FriendURL { get; set; }

        #endregion

        #region Picture
        public string Icon { get; set; }

        public string Picture { get; set; }
        #endregion
        

        /// <summary>
        /// System Category, Shop Category, System Menu, Shop Menu
        /// </summary>
        public CategoryType CategoryType { get; set; }

        #region Foreign
        public string RefType { get; set; }
        public long RefId { get; set; }
        public long? ParentId { get; set; }
        public string UserId { get; set; }
        #endregion

    }
}