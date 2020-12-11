using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PocketWallet.Data.Models
{
    public class Password
    {
        public Guid Id { get; set; }
        public string PasswordValue { get; set; }
        public string WebAddress { get; set; }
        public string Description { get; set; }
        public string Login { get; set; }
        public Guid UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        public virtual ICollection<SharedPassword> SharedPasswords { get; set; }
    }
}
