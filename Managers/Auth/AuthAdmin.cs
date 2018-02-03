using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Managers.Models;
using Managers.Context;
using Utils.Time;

namespace DataManager.Auth
{
    public class AuthAdmin : Auth
    {
        public AuthAdmin(MainDataContext dbIn)
        {
            db = dbIn;
            CookieID = "admin";
        }

        public bool Login(string email, string password)
        {
            bool result = false;

            string encryptedPassword = Utils.Encryption.PasswordHash.CreateHash(password);

            Admin admin = db.Admins.SingleOrDefault(p => p.EmailAddress == email);

            if (admin != null)
            {
                if (Utils.Encryption.PasswordHash.ValidatePassword(password, admin.PasswordHash))
                {
                    CreateSession(admin.ID, CookieAuthTypes.Admin);
                    result = true;

                    admin.LastLogin = UKTime.Now;
                    db.SaveChanges();
                }
            }

            return result;
        }

        /// <summary>
        /// Prepares the admin to have their password reset
        /// </summary>
        /// <param name="admin">The admin to prepare</param>
        /// <returns>The reset URL</returns>
        public string PrepareForPasswordReset(Admin admin)
        {
            string resetKey = UKTime.Now.Ticks.ToString() + "~" + Guid.NewGuid();
            admin.SecurityStamp = resetKey;
            db.SaveChanges();
            return "/admin/login.aspx?reset=" + resetKey;
        }

        /// <summary>
        /// Checks to make sure that the code given is a valid reset code
        /// </summary>
        /// <param name="code">The reset code matching the SecurityStamp</param>
        /// <param name="message">This will be populated with a human readable reason if the code isn't valid</param>
        /// <returns>True if the code is valid, false if not</returns>
        public bool IsResetValid(string code, out string message)
        {
            message = "";

            //First part ticks at time of request, second part the security hash
            string[] splits = code.Split('~');
            if (splits.Length != 2)
            {
                message = "The reset code is not valid. Please try resetting your password again.";
                return false;
            }
            DateTime dateRequestMade = UKTime.Now;
            try
            {
                long ticks = long.Parse(splits[0]);
                dateRequestMade = new DateTime(ticks);
            }
            catch
            {
                message = "The reset code is not valid. Please try resetting your password again.";
                return false;
            }

            if (dateRequestMade < UKTime.Now.AddHours(-1))
            {
                message = "The reset request has expired. Please reset your password again.";
                return false;
            }

            Admin admin = db.Admins.FirstOrDefault(a => a.SecurityStamp == code);
            if (admin != null)
            {
                return true;
            }
            else
            {
                message = "The reset code is not valid. Please try resetting your password again.";
                return false;
            }
        }

        /// <summary>
        /// Resets the admin password for the given admin. 
        /// </summary>
        /// <param name="code">The reset code</param>
        /// <param name="admin">The admin to update</param>
        /// <param name="newPassword">The new password (unhashed)</param>
        /// <returns>True if successful, false if not</returns>
        public bool ResetPassword(string code, Admin admin, string newPassword)
        {
            Admin checkAdmin = db.Admins.FirstOrDefault(a => a.SecurityStamp == code);
            if (checkAdmin == null || checkAdmin.ID != admin.ID)
            {
                return false;
            }

            string pass = GetHashedString(newPassword);

            admin.SecurityStamp = null;
            admin.AccessFailedCount = 0;
            admin.PasswordHash = pass;

            db.SaveChanges();
            return true;
        }

        public bool ChangePassword(Admin admin, string currentPassword, string newPassword, out string message)
        {
            message = "";

            if (!Utils.Encryption.PasswordHash.ValidatePassword(currentPassword, admin.PasswordHash))
            {
                message = "Your current password is incorrect";
                return false;
            }
            else
            {
                string hashedNewPassword = GetHashedString(newPassword);
                admin.PasswordHash = hashedNewPassword;
                db.SaveChanges();
            }

            return true;
        }

        public Admin Authorise()
        {
            try
            {
                int recid = GetRecordID(CookieAuthTypes.Admin);
                return db.Admins.SingleOrDefault(p => p.ID == recid);
            }
            catch (InvalidCastException)
            {
                HttpRuntime.UnloadAppDomain();
                return null;
            }
        }
    }
}