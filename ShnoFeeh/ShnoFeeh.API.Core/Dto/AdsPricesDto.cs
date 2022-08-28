using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ShnoFeeh.API.Core.Dto
{
    /*
     This class contains data transfer object
     properties for ads price functionality
   */
    public class AdsPricesDto
    {
        #region Properties
        public int Id { get; set; }
        public string DayOfWeek { get; set; }
        public decimal? Amount { get; set; }
        public int? CityId { get; set; }
        public string City { get; set; }
        public string Currency { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        #endregion
    }
    /*
     This class contains data transfer object
     properties for update ads price functionality
   */
    public class UpdateAdsPricesDto
    {
        #region Properties
        public int AdPriceId { get; set; }
        public string DayOfWeek { get; set; }
        [Required]
        [Range(0,Double.MaxValue)]
        public decimal Amount { get; set; }
        public int? CityId { get; set; }
        public string Currency { get; set; }
        public int? ModifiedBy { get; set; }
        #endregion

    }
}
