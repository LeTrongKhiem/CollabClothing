using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace CollabClothing.WebApp.Models
{
    public partial class User
    {
        public User()
        {
            Orders = new HashSet<Order>();
            SystemActivities = new HashSet<SystemActivity>();
            TransactionOnlines = new HashSet<TransactionOnline>();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(255)]
        public string UserName { get; set; }
        [Required]
        [StringLength(255)]
        public string Password { get; set; }
        [Required]
        [StringLength(255)]
        public string Phone { get; set; }
        [Required]
        [StringLength(255)]
        public string Email { get; set; }
        [Column("DOB", TypeName = "datetime")]
        public DateTime Dob { get; set; }
        [Required]
        [StringLength(255)]
        public string Address { get; set; }
        [Required]
        [StringLength(255)]
        public string FullName { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime LastLoginDate { get; set; }
        public bool Active { get; set; }
        [Required]
        [StringLength(255)]
        public string Code { get; set; }

        [InverseProperty(nameof(Order.User))]
        public virtual ICollection<Order> Orders { get; set; }
        [InverseProperty(nameof(SystemActivity.User))]
        public virtual ICollection<SystemActivity> SystemActivities { get; set; }
        [InverseProperty(nameof(TransactionOnline.User))]
        public virtual ICollection<TransactionOnline> TransactionOnlines { get; set; }
    }
}
