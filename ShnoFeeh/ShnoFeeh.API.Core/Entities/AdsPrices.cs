using System;
using System.Collections.Generic;
using System.Text;

namespace ShnoFeeh.API.Core.Entities
{
    public class AdsPrices : BaseEntity
    {
        public string DayOfWeek { get; set; }
        public decimal Amount { get; set; }
        public int? CityId { get; set; }
        public string Currency { get; set; }

    }
}
