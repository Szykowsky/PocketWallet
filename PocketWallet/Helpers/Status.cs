using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PocketWallet.Helpers
{
    public class Status
    {
        public Status()
        {

        }

        public Status(bool success, string message)
        {
            Success = success;
            Messege = message;
        }
        public bool Success { get; set; }
        public string Messege { get; set; }
    }
}
