using System;
using System.Collections.Generic;
using System.Text;

namespace ShnoFeeh.API.Core.Entities
{
    /*
    This class contains properties of database entity Advertisment
  */
    public class Advertisments: BaseEntity
    {
        public string CategoryId { get; set; }
        public int? CityId { get; set; }
        public List<AdvertismentsImages> AdvertismentsImages { get; set; }
    }
}
