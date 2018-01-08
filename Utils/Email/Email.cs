using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.IO;
using System.Net.Mail;
using System.Web.Configuration;
using System.Configuration;

namespace Utils.Email
{
    public partial class Email
    {
        private static string[] UnsightlyEmailCharacters = { "{", "}", "|", "~", "\"" };

        private static string[] UnsightlyAfterAmpersandEmailCharacters = { "'" };

        public static bool SendTemplateEmail(string email, string subject, MailAddressCollection mailTo,
            MailAddressCollection mailCc, MailAddressCollection mailBcc, Dictionary<string, string> attachments)
        {
            return SendTemplateEmail(email, subject, mailTo, mailCc, mailBcc, attachments, Constants.EmailsFrom);
        }

        public static bool SendTemplateEmail(string email, string subject, MailAddressCollection mailTo,
            MailAddressCollection mailCc, MailAddressCollection mailBcc, Dictionary<string, string> attachments, MailPriority priority) {
            
            return SendTemplateEmail(email, subject, mailTo, mailCc, mailBcc, attachments, Constants.EmailsFrom, priority);
        }

        /// <summary>
        /// Send an email using the credentials in the web.config
        /// </summary>
        /// <param name="email">The body (HTML content) of the email</param>
        /// <param name="subject">The subject of the email</param>
        /// <param name="mailTo">The list of to recipients, either comma or semi-colon separated</param>
        /// <param name="mailCc">The list of cc recipients, either comma or semi-colon separated</param>
        /// <param name="mailBcc">The list of bcc recipients, either comma or semi-colon separated</param>
        /// <param name="attachments"></param>
        /// <returns></returns>
        public static bool SendTemplateEmail(string email, string subject, string mailTo,
            string mailCc, string mailBcc, Dictionary<string, string> attachments)
        {
            MailAddressCollection toColl = new MailAddressCollection();
            string[] toEmails = mailTo.Split(',');
            if (toEmails.Length == 1)
                toEmails = mailTo.Split(';');
            foreach (string emailStr in toEmails)
            {
                try
                {
                    toColl.Add(emailStr);
                }
                catch { }
            }

            MailAddressCollection ccColl = new MailAddressCollection();
            string[] ccEmails = mailCc.Split(',');
            if (ccEmails.Length == 1)
                ccEmails = mailCc.Split(';');
            foreach (string emailStr in ccEmails)
            {
                try
                {
                    ccColl.Add(emailStr);
                }
                catch { }
            }

            MailAddressCollection bccColl = new MailAddressCollection();
            string[] bccEmails = mailBcc.Split(',');
            if (bccEmails.Length == 1)
                bccEmails = mailBcc.Split(';');
            foreach (string emailStr in bccEmails)
            {
                try
                {
                    bccColl.Add(emailStr);
                }
                catch { }
            }
            return SendTemplateEmail(email, subject, toColl, ccColl, bccColl, attachments);
        }

        public static bool SendTemplateEmail(string email, string subject, MailAddressCollection mailTo,
            MailAddressCollection mailCc, MailAddressCollection mailBcc, Dictionary<string, string> attachments, string emailFrom, MailPriority priority = MailPriority.Normal)
        {
            StreamReader f = new StreamReader(Files.Files.MapPath(Constants.EmailTemplatePath + "template.html"));
            StringBuilder template = new StringBuilder(f.ReadToEnd());
            f.Close();

            StringBuilder body = new StringBuilder(email);

            MailMessage mm = new MailMessage();
            if (Constants.IsRedirectAllMailToDev)
            {
                mm.To.Clear();
                mm.CC.Clear();
                mm.Bcc.Clear();
                mm.To.Add(Constants.DeveloperEmail);
            }
            else
            {
                foreach (var item in mailTo)
                {
                    mm.To.Add(item);
                }

                foreach (var item in mailCc)
                {
                    mm.CC.Add(item);
                }

                foreach (var item in mailBcc)
                {
                    mm.Bcc.Add(item);
                }

                if (Constants.SendEmailsToDev)
                {
                    mm.Bcc.Add(Constants.DeveloperEmail);
                }
            }

            foreach (KeyValuePair<string, string> att in attachments)
            {
                if (Files.Files.IsFileExist(att.Key))
                {
                    Attachment attachment = new Attachment(att.Key);
                    attachment.Name = att.Value;
                    mm.Attachments.Add(attachment);
                }
            }

            if (Email.IsValidEmail(emailFrom))
            {
                mm.From = new MailAddress(emailFrom);
            }
            else
            {
                mm.From = new MailAddress(Constants.EmailsFrom);
            }

            if (priority != null) mm.Priority = priority;

            mm.Subject = subject;
            mm.IsBodyHtml = true;

            template.Replace("@SUBJECT@", subject);
            template.Replace("@BODY@", body.ToString());
            template.Replace("@DOMAIN@", Constants.DomainName);
            template.Replace("@IMAGEPATH_HEADER@", Constants.DomainName.TrimEnd('/') + Constants.EmailImagePath + "header.jpg");
            template.Replace("@HEADER_LINE@", subject);

            mm.Body = template.ToString();


            SmtpClient smtp = new SmtpClient();
            for (int i = 0; i < 5; i++)
            {
                try
                {
                    smtp.Send(mm);
                    LogEmail(body.ToString(), subject, mailTo, mailCc, mailBcc);

                    foreach (Attachment a in mm.Attachments)
                        a.Dispose();

                    return true;
                }
                catch (Exception ex)
                {
                    Utils.Error.ErrorLog.AddEntry(ex);
                }
            }
            return false;
        }

        public static bool SendNoneTemplateEmail(string email, string subject, MailAddressCollection mailTo,
            MailAddressCollection mailCc, MailAddressCollection mailBcc, Dictionary<string, string> attachments)
        {
            return SendNoneTemplateEmail(email, subject, mailTo, mailCc, mailBcc, attachments, Constants.EmailsFrom);
        }
        public static bool SendNoneTemplateEmail(string email, string subject, MailAddressCollection mailTo,
            MailAddressCollection mailCc, MailAddressCollection mailBcc, Dictionary<string, string> attachments, string emailFrom)
        {
            MailMessage mm = new MailMessage();
            if (Constants.IsRedirectAllMailToDev)
            {
                mm.To.Clear();
                mm.CC.Clear();
                mm.Bcc.Clear();
                mm.To.Add(Constants.DeveloperEmail);
            }
            else
            {
                foreach (var item in mailTo)
                {
                    mm.To.Add(item);
                }

                foreach (var item in mailCc)
                {
                    mm.CC.Add(item);
                }

                foreach (var item in mailBcc)
                {
                    mm.Bcc.Add(item);
                }
                if (Constants.SendEmailsToDev)
                {
                    mm.Bcc.Add(Constants.DeveloperEmail);
                }
            }

            foreach (KeyValuePair<string, string> att in attachments)
            {
                if (Files.Files.IsFileExist(att.Key))
                {
                    Attachment attachment = new Attachment(att.Key);
                    attachment.Name = att.Value;
                    mm.Attachments.Add(attachment);
                }
            }

            if (Email.IsValidEmail(emailFrom))
            {
                mm.From = new MailAddress(emailFrom);
            }
            else
            {
                mm.From = new MailAddress(Constants.EmailsFrom);
            }

            mm.Subject = subject;
            mm.IsBodyHtml = true;

            mm.Body = email.Replace("@DOMAIN@", Constants.DomainName);

            SmtpClient smtp = new SmtpClient();
            LogEmail(email, subject, mailTo, mailCc, mailBcc);


            for (int i = 0; i < 5; i++)
            {
                try
                {
                    smtp.Send(mm);
                    LogEmail(email, subject, mailTo, mailCc, mailBcc);

                    foreach (Attachment a in mm.Attachments)
                        a.Dispose();

                    return true;
                }
                catch (Exception ex)
                {
                    Utils.Error.ErrorLog.AddEntry(ex);
                }
            }
            return false;
        }

        private static void LogEmail(string body, string subject, MailAddressCollection mailTo, MailAddressCollection mailCc, MailAddressCollection mailBcc)
        {
            string timeStamp = DateTime.UtcNow.ToString("yyyy-MM-dd-HH-mm-ss-ffff");

            // create html file //
            try
            {
                Utils.Files.Files.CheckDirectory(Constants.EmailLogPath);
                string filePath = Files.Files.MapPath(Constants.EmailLogPath + timeStamp + ".html");

                FileStream fs = File.Open(filePath, FileMode.CreateNew, FileAccess.Write);

                StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.UTF8);

                sw.WriteLine("<p><strong>Subject:</strong> " + subject + "</p>");
                sw.WriteLine("<p><strong>To:</strong> " + mailTo.ToString() + "</p>");
                sw.WriteLine("<p><strong>Cc:</strong> " + mailCc.ToString() + "</p>");
                sw.WriteLine("<p><strong>Bcc:</strong> " + mailBcc.ToString() + "</p>");
                sw.WriteLine("<p><strong>Timestamp:</strong> " + timeStamp + "</p>");

                sw.Write(body);

                sw.Close();
                fs.Close();

                // amend email log //

                string logFilePath = Files.Files.MapPath(Constants.EmailLogPath + "email-log.txt");

                fs = File.Open(logFilePath, FileMode.Append, FileAccess.Write);

                sw = new StreamWriter(fs, System.Text.Encoding.UTF8);

                sw.WriteLine(timeStamp + " - " + subject + " - " + mailTo.ToString());

                sw.Close();
                fs.Close();
            }
            catch (System.IO.IOException)
            {
                // throw away ex.
            }
        }

        /// <summary>
        /// Returns the supplied email surrounded in javascript to prevent spamming
        /// </summary>
        /// <param name="email">The email address to hide in the javascript.</param>
        /// <param name="displayName">The text that should display as the link.</param>
        /// <returns></returns>
        public static string JavascriptEmail(string email, string displayName)
        {
            if (!email.Contains("@"))
                return email;
            string jsFormat = "<script language=\"javascript\">" +
                                "user = '{0}'; site = '{1}';" +
                                "document.write('<a href=\"mailto:' + user + '@' + site + '\">');" +
                                "document.write('" + displayName + "</a>');" +
                              "</script>" +
                              "<noscript>&nbsp;</noscript>";
            return String.Format(jsFormat, email.Split('@')[0], email.Split('@')[1]);
        }

        /// <summary>
        /// Returns the supplied email surrounded in javascript to prevent spamming
        /// </summary>
        /// <param name="email">The email address to hide in the javascript.</param>
        /// <returns></returns>
        public static string JavascriptEmail(string email)
        {
            return JavascriptEmail(email, email);
        }

        /// <summary>
        /// Checks the given string to see if it is a valid email
        /// </summary>
        /// <param name="emailToCheck">The email to check</param>
        /// <returns>Whether the 'emailToCheck' is a valid email address</returns>
        public static bool IsValidEmail(string emailToCheck)
        {
            try
            {
                if (String.IsNullOrEmpty(emailToCheck))
                    return false;

                MailAddress ma = new MailAddress(emailToCheck);
                if (UnsightlyEmailCharacters.Any(a => emailToCheck.Contains(a)) 
                    || UnsightlyAfterAmpersandEmailCharacters.Any(a => emailToCheck.LastIndexOf(a) > emailToCheck.IndexOf('@')))
                {
                    throw new Exception("Email contains invalid character(s)");
                }
                return true;
            }
            catch { return false; }
        }
    }
}