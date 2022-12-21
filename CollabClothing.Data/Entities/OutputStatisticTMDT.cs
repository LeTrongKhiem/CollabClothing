using System.ComponentModel.DataAnnotations;

namespace CollabClothing.Data.Entities
{
    public class OutputStatisticTMDT
    {
        [Key]
        public string NameCategory { get; set; }
        public decimal TONGCONG { get; set; }
    }
}
