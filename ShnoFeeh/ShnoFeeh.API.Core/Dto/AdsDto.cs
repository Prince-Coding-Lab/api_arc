using ShnoFeeh.API.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace ShnoFeeh.API.Core.Dto
{
    /*
        This class contains data transfer object
        properties for ads functionality
    */
    public class AdsDto
    {
        #region Properties
        public int Id { get; set; }
        public int? CampaginId { get; set; }
        public string CampaginName { get; set; }
        public int? CityId { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public int? CategoryId { get; set; }
        public string CatName { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Keyword { get; set; }
        public string URL { get; set; }
        public string Phone { get; set; }
        public string ActiveLink { get; set; }
        public int? StatusId { get; set; }
        public string Status { get; set; }
        public int? Views { get; set; }
        public string Desc { get; set; }
        public string Title { get; set; }
        public decimal? ProductPrice { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public string Desc_Ar { get; set; }
        public string Title_Ar { get; set; }
        public string Keyword_Ar { get; set; }
        public List<AdsMediaDto> AdsMedia { get; set; }
        #endregion

    }
    /*
       This class contains data transfer object
       properties for Add new ads functionality
   */
   
    public class AddAdsDto
    {
        #region Properties
        public int? CampaginId { get; set; }
        public int? CityId { get; set; }
        public int? CategoryId { get; set; }       
        public DateTime? StartDate { get; set; }
        public string Keyword { get; set; }
        public DateTime? EndDate { get; set; }
        public string URL { get; set; }
        [Required]
        [Phone]
        public string Phone { get; set; }
        [Required]
        public string ActiveLink { get; set; }
        public int? StatusId { get; set; }
        public int? Views { get; set; }
        [Required]
        [StringLength(3999, MinimumLength = 3)]
        public string Desc { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Title { get; set; }
        [Range(0, Double.MaxValue)]
        public decimal? ProductPrice { get; set; }
        public int? CreatedBy { get; set; }
        public string Desc_Ar { get; set; }
        //[Required]
        //[StringLength(100, MinimumLength = 3)]
        public string Title_Ar { get; set; }
        public string Keyword_Ar { get; set; }
        public List<AdsMedia> AdsMedia { get; set; }
        #endregion
    }
    /*
   This class contains data transfer object
   properties for AdsMedia functionality
   */
    public class AdsMediaDto
    {
        #region Properties
        public int Id { get; set; }
        public string ImageUrl { get; set; }
        public int? AdId { get; set; }
        public bool IsMain { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        #endregion

    }
    /*
    This class contains data transfer object
    properties for Add new ads functionality
    */
    public class UpdateAdsDto
    {
        [Required]
        public int AdId { get; set; }
        public int? CampaginId { get; set; }
        public int? CityId { get; set; }
        public int? CategoryId { get; set; }
        public DateTime? StartDate { get; set; }
        public string Keyword { get; set; }
        public DateTime? EndDate { get; set; }
        public string URL { get; set; }
        [Required]
        [Phone]
        public string Phone { get; set; }
        [Required]
        public string ActiveLink { get; set; }
        public int? StatusId { get; set; }
        public int? Views { get; set; }
        [Required]
        [StringLength(3999, MinimumLength = 3)]
        public string Desc { get; set; }
        [Required]
        [StringLength(100,MinimumLength = 3)]
        public string Title { get; set; }
        [Range(0, Double.MaxValue)]
        public decimal? ProductPrice { get; set; }
        public int? ModifiedBy { get; set; }
        //[Required]
        //[StringLength(3999, MinimumLength = 3)]
        public string Desc_Ar { get; set; }
        //[Required]
        //[StringLength(100, MinimumLength = 3)]
        public string Title_Ar { get; set; }
        public string Keyword_Ar { get; set; }
        public List<AdsMedia> AdsMedia { get; set; }
    }
}
