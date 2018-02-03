using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils.Files
{
    public static class Files
    {
        public static string MapPath(string relative) {
            return System.Web.Hosting.HostingEnvironment.MapPath(relative);
        }
    }
}
