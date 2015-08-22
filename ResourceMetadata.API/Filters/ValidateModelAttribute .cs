using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Web.Http.ModelBinding;

namespace ResourceMetadata.API.Filters
{
    public class ValidateModelAttribute : ActionFilterAttribute
    {

        /// <summary>
        /// Validate model modeState
        /// </summary>
        /// <param name="actionContext"></param>
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            //Validate null or not
            if (actionContext.ActionArguments.Any(kv => kv.Value == null))
            {
                actionContext.Response = actionContext.Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Arguments cannot be null");
            }

            //Validate model properties
            if (actionContext.ModelState.IsValid == false)
            {
                actionContext.Response = actionContext.Request.CreateErrorResponse(
                    HttpStatusCode.BadRequest, actionContext.ModelState);
            }
        }
    }
}