using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PocketWallet.ViewModels
{
    public class ChangePasswordModel
    {
        public string Login { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public bool IsPasswordKeptAsHash { get; set; }
    }
}
