using Managers.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Managers.Context
{
    public class MainDataContextInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<MainDataContext>
    {
        protected override void Seed(MainDataContext context)
        {
            var users = new List<User>
            {
                new User() { Username = "Big_Poppa", DateJoined = new DateTime(2000, 1, 1), AccessFailedCount = 0, EmailAddress="test@test.com", LastLogin = new DateTime(2000,1,1), PasswordHash = "hash", SecurityStamp  ="123"  }
            };

            context.Users.AddRange(users);
            context.SaveChanges();
        }
    }
}
