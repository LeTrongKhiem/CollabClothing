using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace CollabClothing.WebApp.Models
{
    [Table("TransactionOnline")]
    public partial class TransactionOnline
    {
        [Key]
        public int Id { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime TransactionDate { get; set; }
        [Column(TypeName = "decimal(18, 0)")]
        public decimal Amount { get; set; }
        [Column(TypeName = "decimal(18, 0)")]
        public decimal? Fee { get; set; }
        [Column(TypeName = "decimal(18, 0)")]
        public decimal Result { get; set; }
        [Column(TypeName = "text")]
        public string MessageTran { get; set; }
        public bool StatusTran { get; set; }
        [StringLength(255)]
        public string ProviderTran { get; set; }
        public int UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        [InverseProperty("TransactionOnlines")]
        public virtual User User { get; set; }
    }
}
