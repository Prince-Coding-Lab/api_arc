using System;
using System.Collections.Generic;

namespace ShnoFeeh.API.Core.Entities
{
    /*
     This class contains properties of database entity Ads
   */
    public class Ads : BaseEntity
    {
        #region Properties
        public int? CampaginId { get; set; }
        public int? CityId { get; set; }
        public int? CategoryId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Keyword { get; set; }
        public string URL { get; set; }
        public string Phone { get; set; }
        public string ActiveLink { get; set; }
        public int? StatusId { get; set; }
        public int? Views { get; set; }
        public string Desc { get; set; }
        public string Title { get; set; }
        public decimal? ProductPrice { get; set; }
        public string Desc_Ar { get; set; }
        public string Title_Ar { get; set; }
        public string Keyword_Ar { get; set; }
        public List<AdsMedia> AdsMedia { get; set; }

        #endregion

    }
}
