using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace CollabClothing.WebApp.Models
{
    [Table("Size")]
    public partial class Size
    {
        public Size()
        {
            ProductMapSizes = new HashSet<ProductMapSize>();
        }

        [Key]
        [StringLength(255)]
        public string Id { get; set; }
        [Required]
        [StringLength(255)]
        public string NameSize { get; set; }

        [InverseProperty(nameof(ProductMapSize.Size))]
        public virtual ICollection<ProductMapSize> ProductMapSizes { get; set; }
    }
}
