using ResourceMetadata.Data.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Controllers;
using Microsoft.Owin;

namespace ResourceMetadata.API
{
    public partial class WebWorkContext : IWorkContext
    {
        private readonly IOwinContext _httpContext;

        public WebWorkContext()
        {
            if (_httpContext ==null)
            {
                _httpContext = HttpContext.Current.Request.GetOwinContext();
            }
            
        }

        public virtual string CurrentUsername
        {
            get
            {
                return _httpContext.Request.User.Identity.Name;
            }
        }


    }
}
