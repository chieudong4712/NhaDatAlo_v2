using System.Text;
using System.Web.Mvc;

namespace ResourceMetadata.Core.Util
{
    public static class JsonUtil
    {
        public static JsonResult AjaxResponseError(ModelStateDictionary modelState)
        {
            JsonResult r = new JsonResult();
            StringBuilder sb = new StringBuilder();
            sb.Append("<ul>");
            foreach (var model in modelState.Values)
            {
                foreach (var error in model.Errors)
                {
                    sb.Append("<li>" + error.ErrorMessage + "</li>");
                }
            }
            sb.Append("</ul>");

            r.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            r.Data = new { success = false, message = sb.ToString() };
            
            return r;
        }

        public static JsonResult AjaxResponseError(string message)
        {
            JsonResult r = new JsonResult();
            r.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            r.Data = new { success = false, message = message };

            return r;
        }

        public static JsonResult AjaxResponseSuccess(string message)
        {
            JsonResult r = new JsonResult();
            r.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            r.Data = new { success = true, message = message };

            return r;
        }

        public static JsonResult AjaxResponseSuccess(object item)
        {
            JsonResult r = new JsonResult();
            r.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            r.Data = new { success = true, item = item };

            return r;
        }
    }

    
}
