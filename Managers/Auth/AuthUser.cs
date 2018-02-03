using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Managers.Models;
using Managers.Context;
using Utils.Time;

namespace DataManager.Auth
{
    public class AuthUser : Auth
    {
        public AuthUser(MainDataContext dbIn)
        {
            db = dbIn;
            CookieID = "user";
        }

        public bool Login(string email, string password)
        {
            bool result = false;

            string encryptedPassword = Utils.Encryption.PasswordHash.CreateHash(password);

            User user = db.Users.SingleOrDefault(p => p.EmailVerified && p.EmailAddress == email);

            if (user != null)
            {
                if (Utils.Encryption.PasswordHash.ValidatePassword(password, user.PasswordHash))
                {
                    CreateSession(user.ID, CookieAuthTypes.User);
                    result = true;

                    user.LastLogin = UKTime.Now;
                    db.SaveChanges();
                }
            }

            return result;
        }

        /// <summary>
        /// Prepares the user to have their password reset
        /// </summary>
        /// <param name="user">The admin to prepare</param>
        /// <returns>The reset URL</returns>
        public string PrepareForPasswordReset(User user)
        {
            string resetKey = UKTime.Now.Ticks.ToString() + "~" + Guid.NewGuid();
            user.SecurityStamp = resetKey;
            db.SaveChanges();
            return "/user/login.aspx?reset=" + resetKey;
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

            User user = db.Users.FirstOrDefault(a => a.SecurityStamp == code);
            if (user != null)
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
        /// <param name="user">The admin to update</param>
        /// <param name="newPassword">The new password (unhashed)</param>
        /// <returns>True if successful, false if not</returns>
        public bool ResetPassword(string code, User user, string newPassword)
        {
            User checkUser = db.Users.FirstOrDefault(a => a.SecurityStamp == code);
            if (checkUser == null || checkUser.ID != user.ID)
            {
                return false;
            }

            string pass = GetHashedString(newPassword);

            user.SecurityStamp = null;
            user.AccessFailedCount = 0;
            user.PasswordHash = pass;

            db.SaveChanges();
            return true;
        }

        public bool ChangePassword(User user, string currentPassword, string newPassword, out string message)
        {
            message = "";

            if (!Utils.Encryption.PasswordHash.ValidatePassword(currentPassword, user.PasswordHash))
            {
                message = "Your current password is incorrect";
                return false;
            }
            else
            {
                string hashedNewPassword = GetHashedString(newPassword);
                user.PasswordHash = hashedNewPassword;
                db.SaveChanges();
            }

            return true;
        }

        public User Authorise()
        {
            try
            {
                int recid = GetRecordID(CookieAuthTypes.User);
                return db.Users.SingleOrDefault(p => p.ID == recid);
            }
            catch (InvalidCastException)
            {
                HttpRuntime.UnloadAppDomain();
                return null;
            }
        }
    }
}