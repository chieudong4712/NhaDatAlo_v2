using System.Collections.Generic;
using ResourceMetadata.Core.Common;
using ResourceMetadata.Model.Base;


namespace SocialGoal.Model.AttributeModels
{
    public class ProductAttribute: IAttribute
    {
        private List<string> _collectionProduct;
        public List<string> CollectionProduct
        {
            get { return _collectionProduct; }
            set { _collectionProduct = value; }
        }


    }
}
