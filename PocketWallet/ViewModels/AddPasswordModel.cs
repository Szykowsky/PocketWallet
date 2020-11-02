using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PocketWallet.ViewModels
{
    public class AddPasswordModel : PasswordWalletModel
    {
        public Guid UserId { get; set; }
    }
}
