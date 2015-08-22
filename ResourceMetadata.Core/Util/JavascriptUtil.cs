using System;
using System.Web.Script.Serialization;

namespace ResourceMetadata.Core.Util 
{ 
    public class JavascriptUtil {
        public static string Serialize(object obj) {
            var jss = new JavaScriptSerializer();
            return jss.Serialize(obj);
        }

        public static T Deserialize<T>(string str)
        {
            if (String.IsNullOrEmpty(str)) return default(T);
            var jss = new JavaScriptSerializer();
            return jss.Deserialize<T>(str);
        }
    }
}