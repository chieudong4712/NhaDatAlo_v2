using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ResourceMetadata.Core.Common;

namespace ResourceMetadata.Core.Util
{
    public static class SortUtil
    {
        public static SortType Reverse(SortType sorttype)
        {

            return sorttype == SortType.ASC ? SortType.DESC : SortType.ASC;
        }
    }
}
