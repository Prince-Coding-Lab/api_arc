using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ShnoFeeh.API.Core.Dto
{
    /*
        This class contains data transfer object
        properties for Campaign functionality
    */
    public class CampaignDto
    {
        #region Properties
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Goal { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public string UserName { get; set; }
        public int? UserId { get; set; }
        #endregion
    }

    public class CampaignAdsDto
    {
        #region Properties
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Goal { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public string UserName { get; set; }
        public int? UserId { get; set; }
        public List<AdsDto> Ads { get; set; }
        #endregion
    }
    /*
       This class contains data transfer object
       properties for Add Campaign functionality
   */
    public class AddCampaignDto
    {
        #region Properties
        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Name { get; set; }
        //[Required]
        [StringLength(500, MinimumLength = 3)]
        public string Description { get; set; }
        //[Required]
        //[StringLength(100, MinimumLength = 3)]
        public string Goal { get; set; }
        public int? CreatedBy { get; set; }
        public int? UserId { get; set; }
        #endregion
    }
    /*
       This class contains data transfer object
       properties for Update Campaign functionality
   */
    public class UpdateCampaignDto
    {
        #region Properties
        public int CampaignId { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Name { get; set; }
        //[Required]
        //[StringLength(500, MinimumLength = 3)]
        public string Description { get; set; }
        //[Required]
        //[StringLength(100, MinimumLength = 3)]
        public string Goal { get; set; }
        public int? ModifiedBy { get; set; }
        public int? UserId { get; set; }
        #endregion
    }

}
