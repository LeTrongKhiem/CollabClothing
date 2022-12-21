using System;

namespace CollabClothing.ViewModels.Catalog.Statistic
{
    public class BenefitRequest
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }

    public class OutputStatisticTMDT
    {
        public string NameCategory { get; set; }
        public decimal TONGCONG { get; set; }
    }
}
