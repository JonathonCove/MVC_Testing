using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Constants
{
    public class ConstantsConfig : ConfigurationSection
    {
        [ConfigurationProperty("sendErrorEmails", DefaultValue = "", IsRequired = false)]
        public bool SendErrorEmails {
            get {
                return (bool)this["sendErrorEmails"];
            }
            set {
                this["sendErrorEmails"] = value;
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

        [ConfigurationProperty("DomainName", DefaultValue = "", IsRequired = false)]
        public string DomainName {
            get {
                return (string)this["DomainName"];
            }
            set {
                this["DomainName"] = value;
            }
        }

        [ConfigurationProperty("emailsTo", DefaultValue = "", IsRequired = false)]
        public string EmailsTo {
            get {
                return (string)this["emailsTo"];
            }
            set {
                this["emailsTo"] = value;
            }
        }

        [ConfigurationProperty("emailsFrom", DefaultValue = "", IsRequired = false)]
        public string EmailsFrom {
            get {
                return (string)this["emailsFrom"];
            }
            set {
                this["emailsFrom"] = value;
            }
        }

        [ConfigurationProperty("developerEmail", DefaultValue = "", IsRequired = false)]
        public string DeveloperEmail {
            get {
                return (string)this["developerEmail"];
            }
            set {
                this["developerEmail"] = value;
            }
        }

        [ConfigurationProperty("TestMode", DefaultValue = "", IsRequired = false)]
        public bool TestMode {
            get {
                return (bool)this["TestMode"];
            }
            set {
                this["TestMode"] = value;
            }
        }

        [ConfigurationProperty("isRedirectAllMailToDev", DefaultValue = "", IsRequired = false)]
        public bool RedirectAllMailToDev {
            get {
                return (bool)this["isRedirectAllMailToDev"];
            }
            set {
                this["isRedirectAllMailToDev"] = value;
            }
        }
    }
}
