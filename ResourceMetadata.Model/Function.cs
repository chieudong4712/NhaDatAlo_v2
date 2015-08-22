using ResourceMetadata.Model.Base;
using System;
using System.Collections.Generic;


namespace ResourceMetadata.Model
{
    public class Function: BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int OrderNumber { get; set; }
        public string ActionName { get; set; }
        public string ControllerName { get; set; }
        public long? ParentId { get; set; }

      

    }
   
}