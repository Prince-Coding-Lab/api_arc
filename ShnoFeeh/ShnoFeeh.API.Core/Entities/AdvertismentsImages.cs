using System;
using System.Collections.Generic;
using System.Text;

namespace ShnoFeeh.API.Core.Entities
{
    /*
    This class contains properties of database entity AdvertismentImages
     */
    public class AdvertismentsImages
    {
        public int Id { get; set; }
        public int AdvertismentId { get; set; }
        public string ImageUrl { get; set; }
    }
}
