using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Managers.Models
{
    public partial class AuthSession
    {
        [Key]
        public int ID { get; set; }
        public string IPAddress { get; set; }
        public DateTime Created { get; set; }
        public string SessionCode { get; set; }
        public string CookieID { get; set; }

        [ForeignKey("Admin")]
        public int? AdminID { get; set; }

        [ForeignKey("User")]
        public int? UserID { get; set; }

        public virtual Admin Admin { get; set; }
        public virtual User User { get; set; }
    }
}
