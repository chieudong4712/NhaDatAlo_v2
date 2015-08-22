using System.Collections.Generic;
using ResourceMetadata.Core.Common;
using ResourceMetadata.Model.Base;


namespace SocialGoal.Model.AttributeModels
{
    public class CategoryAttribute: IAttribute
    {
        /// <summary>
        /// Auto add product to category if be content with some conditions
        /// </summary>
        private bool _isAutoAddProduct = false;
        public bool IsAutoAddProduct
        {
            get
            {
                return _isAutoAddProduct;
            }
            set
            {
                _isAutoAddProduct = value;
            }
        }


        private FilterCondition _autoAddProductConditions = new FilterCondition();
        public FilterCondition AutoAddProductConditions
        {
            get
            {
                return _autoAddProductConditions;
            }
            set
            {
                _autoAddProductConditions = value;
            }
        }
    }
}
