using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ResourceMetadata.Core.Util
{
    public static class UrlUtil
    {
        public static string Action(string action, string controller)
        {
            UrlHelper url = new UrlHelper(HttpContext.Current.Request.RequestContext);
            return url.Action(action, controller);
        }
    }
}
