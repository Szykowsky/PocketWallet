using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PocketWallet.ViewModels
{
    public class LogsModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime DateTime { get; set; }
    }
}
