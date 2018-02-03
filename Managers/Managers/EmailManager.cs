using Constants;
using Managers.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils.Email;
using Utils.Files;

namespace Managers.Managers
{
    public class EmailManager
    {

        public void SendVerificationEmail(User user) {
            //TODO: add email to verify account

            StreamReader f = new StreamReader(Files.MapPath(Constants.Constants.EmailTemplatePath + "verifyemail.html"));
            StringBuilder sbOutput = new StringBuilder(f.ReadToEnd());

            sbOutput.Replace("@LINK@", Constants.Constants.DomainName + "verify-email/" + user.EmailVerifyHash);

            Email.SendTemplateEmail(sbOutput.ToString(), "Verify your email", user.EmailAddress, "", "", new Dictionary<string, string>());

        }
    }
}
