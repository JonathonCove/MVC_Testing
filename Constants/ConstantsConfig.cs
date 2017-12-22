using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Constants
{
    public class ConstantsConfig : ConfigurationSection
    {
        [ConfigurationProperty("ErrorLogPath", DefaultValue = "/App_Data/Logs/Errors/", IsRequired = false)]
        public string ErrorLogPath {
            get {
                return (string)this["ErrorLogPath"];
            }
            set {
                this["ErrorLogPath"] = value;
            }
        }

        [ConfigurationProperty("WebsiteName", DefaultValue = "", IsRequired = false)]
        public string WebsiteName {
            get {
                return (string)this["WebsiteName"];
            }
            set {
                this["WebsiteName"] = value;
            }
        }
    }
}