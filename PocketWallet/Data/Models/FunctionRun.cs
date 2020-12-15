using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PocketWallet.Data.Models
{
    public class FunctionRun
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid FunctionId { get; set; }
        public DateTime DateTime { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }
        [ForeignKey("FunctionId")]
        public virtual Function Function { get; set; }
    }
}
