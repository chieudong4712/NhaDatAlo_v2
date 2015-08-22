using ResourceMetadata.Core.Common;
using ResourceMetadata.Core.Util;
using System;

namespace ResourceMetadata.Model.Base
{
    public interface IBaseModel
    {
        long Id { get; set; }
        DateTime CreatedDateTime { get; set; }
        DateTime LastUpdatedDateTime { get; set; }
        string CreatedBy { get; set; }
        string LastUpdatedBy { get; set; }
        string Status { get; set; }
        string Attribute { get; set; }

        bool? IsVisible { get; set; }
    }
    public class BaseModel<TAttribute> : IBaseModel
        where TAttribute: IAttribute
    {
        /// <summary>
        /// Gets or sets the entity identifier
        /// </summary>
        public long Id { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime LastUpdatedDateTime { get; set; }
        public string CreatedBy { get; set; }
        public string LastUpdatedBy { get; set; }
        
        private string _status = StatusObject.Enable;
        public string Status
        {
            get {
                if (IsVisible.HasValue)
                {
                    _status = IsVisible.Value ? StatusObject.Enable : StatusObject.Disable;
                }
                
                return _status; 
            }
            set { _status = value; }
        }


        private string _attribute = null;
        public string Attribute
        {
            get
            {
                if (AttributeObj != null)
                {
                    _attribute = JavascriptUtil.Serialize(AttributeObj);
                }
                return _attribute;
            }
            set { _attribute = value; }
        }

        private TAttribute _attributeObj;
        public TAttribute AttributeObj 
        { 
            get 
            {
                return _attributeObj;
            }
            set 
            {
                _attributeObj = value;
            }
        }

        private bool? _isVisible;
        public bool? IsVisible
        {
            get {
                return _isVisible; }
            set { _isVisible = value; }
        }


    }

    
}
