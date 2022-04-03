using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollabClothing.Data.Entities
{
    public class TransactionOnline
    {
        public int Id { get; set; }
        public DateTime TransactionDate { get; set; }
        public string ExternalTransactionId { get; set; }
        public decimal Amount { get; set; }
        public decimal Fee { get; set; }
        public string Result { get; set; }
        public string Message { get; set; }
        public Status TansactionStatus { get; set; }
        public string Provider { get; set; }
        public Guid UserId { get; set; }
        public AppUser AppUser { get; set; }
    }
}
