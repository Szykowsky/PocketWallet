using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PocketWallet.ViewModels
{
    public class PasswordWalletFlagModel : PasswordWalletModel
    {
        public bool CanEdit { get; set; }
        public bool CanDelete { get; set; }
        public bool CanShare { get; set; }
        public bool CanShowActions { get; set; }
    }
}
