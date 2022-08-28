using System;
using System.Collections.Generic;
using System.Text;

namespace ShnoFeeh.API.Core.Entities
{
    /*
        This class contains properties of database entity OrderAds
    */
    public class OrderAds 
    {
        #region Properties
        public int Id { get;  set; }
        public DateTime? CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public int OrderId { get; set; }
        public int? CampaginId { get; set; }
        public string CampaginName { get; set; }
        public int? AdId { get; set; }
        public int? StatusId { get; set; }
        public decimal? Price { get; set; }
        public int? CityId { get; set; }
        public string CityName { get; set; }
        public string Keyword { get; set; }
        public int? CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string ActiveLink { get; set; }
        public string Description { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string URL { get; set; }
        public string Phone { get; set; }
        public string Title { get; set; }
        public string Title_Ar { get; set; }
        public string Keyword_Ar { get; set; }
        public string Desc_Ar { get; set; }
        #endregion

    }
}
