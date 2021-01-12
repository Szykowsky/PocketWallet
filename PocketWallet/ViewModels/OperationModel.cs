using PocketWallet.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PocketWallet.ViewModels
{
    public class OperationModel
    {
        public Guid Id { get; set; }
        public string CurrentValue { get; set; }
        public string PreviousValue { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string ActionType { get; set; }
    }
}
