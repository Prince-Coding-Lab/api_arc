using System;
using System.Collections.Generic;
using System.Text;

namespace ShnoFeeh.API.Core.Dto
{
    public class CityDto
    {
        public int Id { get; set; }
        public string CityName { get; set; }
        public string CityAr { get; set; }
        public string Country { get; set; }
        public bool IsActive { get; set; }
        public string Logo { get; set; }
    }
    public class UpdateCityDto
    {
        public int Id { get; set; }
        public string CityName { get; set; }
        public string CityAr { get; set; }
        public int CountryId { get; set; }
        public bool IsActive { get; set; }
        public string Logo { get; set; }
    }
    public class AddCityDto
    {
        public string CityName { get; set; }
        public string CityAr { get; set; }
        public int CountryId { get; set; }
        public bool IsActive { get; set; }
        public string Logo { get; set; }
    }
}
