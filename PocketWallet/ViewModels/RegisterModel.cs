﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PocketWallet.ViewModels
{
    public class RegisterModel : BaseAuthModel
    {
        public bool IsPasswordKeptAsHash { get; set; }
    }
}
