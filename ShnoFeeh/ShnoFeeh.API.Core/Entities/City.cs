using System;
using System.Collections.Generic;
using System.Text;

namespace ShnoFeeh.API.Core.Entities
{
    public class City : BaseEntity
    {
        public string CityName { get; set; }
        public string CityAr { get; set; }
        public int CountryId { get; set; }
        public bool IsActive { get; set; }
        public string Logo { get; set; }

    }
}
