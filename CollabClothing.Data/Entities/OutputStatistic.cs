using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollabClothing.Data.Entities
{
    public class OutputStatistic
    {
        [Key]
        public DateTime Date { get; set; }
        public decimal Revenues { get; set; }
        public decimal Benefit { get; set; }
    }
}
