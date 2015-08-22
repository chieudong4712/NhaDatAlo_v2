using ResourceMetadata.Core.Common;
using System;

namespace ResourceMetadata.Model.Base
{
    public interface ISearch
    {
        int PageSize { get; set; }
        int PageIndex { get; set; }
        int FromIndex { get; }
        int RecordTotal { get; set; }
        int PageTotal { get; }

        String SortColumn { get; set; }
        SortType SortType { get; set; }
    }
}
