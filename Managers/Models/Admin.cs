using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Managers.Models
{
    public partial class Admin
    {
        [Key]
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string PasswordHash { get; set; }
        public string EmailAddress { get; set; }
        public string SecurityStamp { get; set; }
        public int AccessFailedCount { get; set; }
        public DateTime DateJoined { get; set; }
        public DateTime LastLogin { get; set; }
        public virtual ICollection<AuthSession> AuthSessions { get; set; }
    }
}
