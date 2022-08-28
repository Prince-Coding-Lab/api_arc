using System;
using System.Collections.Generic;
using System.Text;

namespace ShnoFeeh.BusinessService.Models
{
    public class CategoryCheck
    {

        public int Id { get; set; }
        public string CatName { get; set; }
        public string CatNameAr { get; set; }
        public string Logo { get; set; }
        public bool isChecked { get; set; }
    }
    public class ParamUpdateAdsPricesDto
    {
        #region Properties
        public int AdPriceId { get; set; }
        public string DayOfWeek { get; set; }
        public decimal Amount { get; set; }        
        public string Currency { get; set; }

        #endregion

    }
}
