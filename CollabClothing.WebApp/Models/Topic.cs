using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace CollabClothing.WebApp.Models
{
    [Table("Topic")]
    public partial class Topic
    {
        public Topic()
        {
            Contacts = new HashSet<Contact>();
        }

        [Key]
        [StringLength(255)]
        public string Id { get; set; }
        [Required]
        [StringLength(255)]
        public string NameTopic { get; set; }

        [InverseProperty(nameof(Contact.Topic))]
        public virtual ICollection<Contact> Contacts { get; set; }
    }
}
