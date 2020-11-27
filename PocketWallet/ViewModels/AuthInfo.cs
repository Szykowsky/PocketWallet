using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PocketWallet.ViewModels
{
    public class AuthInfo
    {
        public string UserLogin { get; set; }
        public DateTime SuccessFulSignIn { get; set; }
        public DateTime UnSuccessFulSignIn { get; set; }
    }
}
