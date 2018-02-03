using DataManager.Abstracts;
using Managers.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Managers.Context;
using Utils.Time;
using DataManager.Auth;

namespace Managers.Managers
{
    public class UserManager : ADataManager<User>
    {
        public UserManager(MainDataContext ndb) : base(ndb)
        {
        }

        public User GetUserByUsername(string username) {
            username = username.ToLower().Trim();
            return GetAllData().FirstOrDefault(u => u.Username.ToLower().Trim() == username);
        }

        public User GetUserByEmail(string email) {
            email = email.ToLower().Trim();
            return GetAllData().FirstOrDefault(u => u.EmailAddress.ToLower().Trim() == email);
        }

        public User CreateNewUser(string username, string emailaddress, string password) {

            AuthUser au = new AuthUser(db);
            string passHash = au.GetHashedString(password);

            User newUser = new User()
            {
                Username = username,
                EmailAddress = emailaddress,
                PasswordHash = passHash,
                DateJoined = UKTime.Now,
                SecurityStamp = "",
                AccessFailedCount = 0,
                EmailVerified = false,
                LastLogin = null
            };

            return newUser;
        }
    }
}
