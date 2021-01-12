using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace PocketWallet.Data.Models
{
    public enum ActionType
    {
        CREATE,
        EDIT,
        DELETE,
        RESTORE
    }

    public class DataChange
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid RecordId { get; set; }
        public string PreviousValue { get; set; }
        public string CurrentValue { get; set; }
        public DateTime UpdatedAt { get; set; }
        public ActionType ActionType { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }
    }
}
