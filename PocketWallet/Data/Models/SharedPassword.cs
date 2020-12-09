using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PocketWallet.Data.Models
{
    public class SharedPassword
    {
        public Guid UserId { get; set; }
        public Guid PasswordId { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }
        [ForeignKey("PasswordId")]
        public virtual Password Password { get; set; }
    }
}
