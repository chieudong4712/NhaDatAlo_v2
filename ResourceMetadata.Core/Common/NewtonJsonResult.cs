using System;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace ResourceMetadata.Core.Common
{
    public class NewtonJsonResult : JsonResult
    {
        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            HttpResponseBase response = context.HttpContext.Response;

            if (!String.IsNullOrEmpty(ContentType))
            {
                response.ContentType = ContentType;
            }
            else
            {
                response.ContentType = "application/json";
            }
            if (ContentEncoding != null)
            {
                response.ContentEncoding = ContentEncoding;
            }
            if (Data!=null)
            {
                response.Write(
                    JsonConvert.SerializeObject(Data, Formatting.None, 
                    new JsonSerializerSettings()
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore, MaxDepth = 2
                    }));
            }
            
        }
    }
}
