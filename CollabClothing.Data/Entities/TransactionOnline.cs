using System;
using System.Collections.Generic;

#nullable disable

namespace CollabClothing.Data.Entities
{
    public partial class TransactionOnline
    {
        public int Id { get; set; }
        public DateTime TransactionDate { get; set; }
        public string ExternalTransactionId { get; set; }
        public decimal Amount { get; set; }
        public decimal Fee { get; set; }
        public string Result { get; set; }
        public string Message { get; set; }
        public int TansactionStatus { get; set; }
        public string Provider { get; set; }
        public Guid UserId { get; set; }

        public virtual AspNetUser User { get; set; }
    }
}
