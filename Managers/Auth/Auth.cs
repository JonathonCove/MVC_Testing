using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Utils.Time;
using System.Security.Cryptography;
using Managers.Context;
using Managers.Models;

namespace DataManager.Auth
{
    public class Auth
    {
        protected string CookieID = "LoginCookieAuth";

        protected MainDataContext db;

        public enum CookieAuthTypes : byte
        {
            Admin = 0,
            User = 1
        }

        public HttpCookie GetCookie()
        {
            HttpCookie cookie = null;

            if (HttpContext.Current.Request.Cookies[CookieID] != null)
            {
                cookie = HttpContext.Current.Request.Cookies[CookieID];
                cookie.Expires = UKTime.Now.AddHours(1);
            }

            return cookie;
        }

        public string GetHashedString(string password)
        {
            return Utils.Encryption.PasswordHash.CreateHash(password);
        }

        public void CreateSession(int recordID, CookieAuthTypes userType)
        {
            AuthSession session = new AuthSession
            {
                IPAddress = HttpContext.Current.Request.GetUserIPAddress(),
                Created = DateTime.UtcNow,
                SessionCode = Guid.NewGuid().ToString(),
                CookieID = CookieID
            };

            if (userType == CookieAuthTypes.Admin)
            {
                session.AdminID = recordID;
            }
            else if (userType == CookieAuthTypes.User)
            {
                session.UserID = recordID;
            } else if (userType == CookieAuthTypes.TrainingAdmin) {
                session.TrainingAdminID = recordID;
            } else if (userType == CookieAuthTypes.Employee) {
                session.EmployeeID = recordID;
            }


            db.AuthSessions.InsertOnSubmit(session);
            db.SubmitChanges();

            HttpCookie cookie = new HttpCookie(CookieID);

            cookie.Expires = DateTime.UtcNow.Add(TimeSpan.FromHours(48.0));
            cookie.Values["authcode"] = session.SessionCode;
            cookie.HttpOnly = true;
            HttpContext.Current.Response.Cookies.Add(cookie);
        }

        public int GetRecordID(CookieAuthTypes userType)
        {
            int recordID = -1; // start at -1. if the user table is cleared and re-seeded it may start at 0

            HttpCookie cookie = GetCookie();

            if (cookie != null)
            {
                try
                {
                    if (!String.IsNullOrEmpty(cookie.Values["authcode"]))
                    {
                        string sessionCode = cookie.Values["authcode"].ToString();
                        AuthSession authSession = db.AuthSessions.SingleOrDefault(p => p.SessionCode == sessionCode && p.CookieID == CookieID
                            && p.IPAddress == HttpContext.Current.Request.GetUserIPAddress());

                        if (authSession != null)
                        {
                            if (userType == CookieAuthTypes.Admin && authSession.AdminID.HasValue)
                                recordID = authSession.AdminID.Value;
                            if (userType == CookieAuthTypes.User && authSession.UserID.HasValue)
                                recordID = authSession.UserID.Value;
                            if (userType == CookieAuthTypes.TrainingAdmin && authSession.TrainingAdminID.HasValue)
                                recordID = authSession.TrainingAdminID.Value;
                            if (userType == CookieAuthTypes.Employee && authSession.EmployeeID.HasValue)
                                recordID = authSession.EmployeeID.Value;
                        }
                    }
                }
                catch (InvalidCastException)
                {
                    //Sometimes the auth session lookup can throw a cast exception, suck it up and remove the cookie data
                    Logout();
                }
            }

            return recordID;
        }


        public void Logout()
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[CookieID];
            if (cookie != null)
            {
                cookie.Value = "";
                cookie.Expires = DateTime.MinValue;
                cookie.HttpOnly = true;
                HttpContext.Current.Response.Cookies.Add(cookie);
            }
        }
    }
}