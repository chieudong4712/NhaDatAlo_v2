using System;
using ResourceMetadata.Model.Base;

namespace ResourceMetadata.Model
{
    public class Product: BaseEntity
    {
        

        #region Info Properties
        
        public string Title { get; set; }

        public string Description { get; set; }

        public string Keyword { get; set; }

        public string FriendURL { get; set; }
        public string Content { get; set; }

        #endregion

        #region Price
        public decimal? Price { get; set; }
        public decimal? OldPrice { get; set; }
        public decimal? Cost { get; set; }
        public decimal? SpecialPrice { get; set; }
        public DateTime? SpecialPriceStartDate { get; set; }
        public DateTime? SpecialPriceEndDate { get; set; }
        #endregion


        #region Other
        /// <summary>
        /// Primary picture url
        /// </summary>
        public string Picture { get; set; }
        public string UserId { get; set; }
        #endregion


        public Product() 
        {
 
        }

    }
}