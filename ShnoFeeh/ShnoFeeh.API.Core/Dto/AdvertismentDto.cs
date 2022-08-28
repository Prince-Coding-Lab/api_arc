using ShnoFeeh.API.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShnoFeeh.API.Core.Dto
{
    /*
   This class contains data transfer object
   properties for get all Advertisments
   */
    public class AdvertismentDto
    {
        public int Id { get; set; }
        public string Category { get; set; }
        public string CategoryAr { get; set; }
        public string City { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public List<AdvertismentImagesDto> Images { get; set; }
    }
    public class AdvertismentImagesDto
    {
        public int Id { get; set; }
        public int AdvertismentId { get; set; }
        public string ImageUrl { get; set; }
    }
    /*
   This class contains data transfer object
   properties for Add new Advertisment functionality
   */
    public class AddAdvertismentDto
    {
        public int CategoryId { get; set; }
        public int? CityId { get; set; }
        public int? CreatedBy { get; set; }
        public List<AdvertismentsImages> AdvertismentsImages { get; set; }
    }
    /*
    This class contains data transfer object
    properties for Update new Advertisment functionality
   */
    public class UpdateAdvertismentDto
    {
        public int AdvertismentId { get; set; }
        public int CategoryId { get; set; }
        public int? ModifiedBy { get; set; }
        public int? CityId { get; set; }
        public List<AdvertismentsImages> AdvertismentsImages { get; set; }
    }
}
