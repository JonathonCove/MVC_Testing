using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Utils.Web
{
    public static class HttpRequestExtension
    {
        public static string GetUserIPAddress(this HttpRequest request) {
            string ip = request["HTTP_X_FORWARDED_FOR"];
            if (String.IsNullOrEmpty(ip))
                ip = request["X_FORWARDED_FOR"];
            if (String.IsNullOrEmpty(ip))
                ip = request["REMOTE_ADDR"];
            else {
                string[] addresses = ip.Split(',');
                if (addresses.Length != 0) {
                    ip = addresses[0];
                }
            }
            return ip;
        }
    }
}
