using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PocketWallet.ViewModels
{
    public class PasswordWalletModel
    {
        public Guid Id { get; set; }
        public string Login { get; set; }
        public string Description { get; set; }
        public string Password { get; set; }
        public string WebPage { get; set; }
    }
}
