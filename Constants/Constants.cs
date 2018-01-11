using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Constants
{
    public class Constants
    {
        public static bool SendErrorEmails {
            get {
                return ((ConstantsConfig)System.Configuration.ConfigurationManager.GetSection(
                    "constantsConfigGroup/constantsConfig")).SendErrorEmails;
            }
        }

        public static string WebsiteName {
            get {
                return ((ConstantsConfig)System.Configuration.ConfigurationManager.GetSection(
                    "constantsConfigGroup/constantsConfig")).WebsiteName;
            }
        }

        public static string DomainName {
            get {
                return ((ConstantsConfig)System.Configuration.ConfigurationManager.GetSection(
                    "constantsConfigGroup/constantsConfig")).DomainName;
            }
        }

        public static string EmailsTo {
            get {
                return ((ConstantsConfig)System.Configuration.ConfigurationManager.GetSection(
                    "constantsConfigGroup/constantsConfig")).EmailsTo;
            }
        }

        public static string EmailsFrom {
            get {
                return ((ConstantsConfig)System.Configuration.ConfigurationManager.GetSection(
                    "constantsConfigGroup/constantsConfig")).EmailsFrom;
            }
        }

        public static string DeveloperEmail {
            get {
                return ((ConstantsConfig)System.Configuration.ConfigurationManager.GetSection(
                    "constantsConfigGroup/constantsConfig")).DeveloperEmail;
            }
        }

        public static bool TestMode {
            get {
                return ((ConstantsConfig)System.Configuration.ConfigurationManager.GetSection(
                    "constantsConfigGroup/constantsConfig")).TestMode;
            }
        }

        public static bool RedirectAllMailToDev {
            get {
                return ((ConstantsConfig)System.Configuration.ConfigurationManager.GetSection(
                    "constantsConfigGroup/constantsConfig")).RedirectAllMailToDev;
            }
        }
    }
}
