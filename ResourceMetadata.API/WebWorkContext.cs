using ResourceMetadata.Data.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Controllers;
using Microsoft.Owin;
using System.Web.Hosting;
using System.IO;

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

        /// <summary>
        /// Maps a virtual path to a physical disk path.
        /// </summary>
        /// <param name="path">The path to map. E.g. "~/bin"</param>
        /// <returns>The physical path. E.g. "c:\inetpub\wwwroot\bin"</returns>
        public virtual string MapPath(string path)
        {
            if (HostingEnvironment.IsHosted)
            {
                //hosted
                return HostingEnvironment.MapPath(path);
            }
            else
            {
                //not hosted. For example, run in unit tests
                string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
                path = path.Replace("~/", "").TrimStart('/').Replace('/', '\\');
                return Path.Combine(baseDirectory, path);
            }
        }


    }
}
