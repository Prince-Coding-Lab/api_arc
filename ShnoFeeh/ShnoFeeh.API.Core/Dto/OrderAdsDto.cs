using System;
using System.Collections.Generic;
using System.Text;

namespace ShnoFeeh.API.Core.Dto
{
    public class OrderAdsDto
    {
        #region Properties
        public int Id { get; set; }
        public int AdId { get; set; }
        public int OrderId { get; set; }
        public string CampaginName { get; set; }
        public string AdDesc { get; set; }
        public string Status { get; set; }
        public string StatusCode { get; set; }
        public decimal? Price { get; set; }
        public string City { get; set; }
        public string CatName { get; set; }
        public string Description { get; set; }
        public string ActiveLink { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string URL { get; set; }
        public string Phone { get; set; }
        public string Title { get; set; }
        public string Title_Ar { get; set; }
        public string Keyword_Ar { get; set; }
        public string Desc_Ar { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        #endregion
    }
}
