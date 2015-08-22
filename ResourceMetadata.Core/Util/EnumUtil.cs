using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResourceMetadata.Core.Util
{
    public static class EnumUtil
    {
        /// <summary>
        /// Get all values of an enum type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IEnumerable<T> GetValues<T>()
        {
            return Enum.GetValues(typeof(T)).Cast<T>();
        }
    }
}
