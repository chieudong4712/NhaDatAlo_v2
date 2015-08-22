using System;
using ResourceMetadata.Model.Base;
using System.Collections.Generic;

namespace ResourceMetadata.Model
{
    public class Property: BaseEntity
    {
        /// <summary>
        /// 3 Type: Category, Group, Property
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// If type Category: Phone, Car v.v.., if type Group: Connect, if type Property: 2G, 3G v.v...
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Empty is Textbox, Multivalue is Dropdown
        /// </summary>
        public string Value { get; set; }

        public int OrderNumber { get; set; }
        public virtual Property Parent { get; set; }

        /// <summary>
        /// The group's ParentId is null 
        /// </summary>
        public long? ParentId { get; set; }
        public virtual ICollection<Property> Children { get; private set; }

    }
}