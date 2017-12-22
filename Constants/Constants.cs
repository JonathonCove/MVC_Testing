using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Constants
{
    public class Constants
    {
        public static string ErrorLogPath {
            get {
                return ((ConstantsConfig)System.Configuration.ConfigurationManager.GetSection(
                    "constantsConfigGroup/constantsConfig")).ErrorLogPath;
            }
        }

        public static string WebsiteName {
            get {
                return ((ConstantsConfig)System.Configuration.ConfigurationManager.GetSection(
                    "constantsConfigGroup/constantsConfig")).WebsiteName;
            }
        }
    }
}