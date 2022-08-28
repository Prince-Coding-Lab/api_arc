using System;
using System.Collections.Generic;
using System.Text;

namespace ShnoFeeh.API.Core.Entities
{
    public class AdsMedia
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; }
        public int? AdId { get; set; }
        public bool IsMain { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? CreatedBy { get; set; }
        public int? ModifiedBy { get; set; }
    }
}
