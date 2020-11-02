using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PocketWallet.Data.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string PasswordHash { get; set; }
        public string Login { get; set; }
        public string Salt { get; set; }
        public bool IsPasswordKeptAsHash { get; set; }
    }
}
