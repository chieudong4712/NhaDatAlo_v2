using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResourceMetadata.Core.Common
{
    public class HierarchyItem
    {
        public string Title { get; set; }
        public long Id { get; set; }
        public long? ParentId { get; set; }
        public string ParentTitle { get; set; }
    }
}
