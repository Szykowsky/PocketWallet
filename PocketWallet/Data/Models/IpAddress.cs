using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PocketWallet.Data.Models
{
    public class IpAddress
    {
        public string FromIpAddress { get; set; }
        public int IncorrectSignInCount { get; set; }
        public bool IsPermanentlyBlocked { get; set; }
    }
}
