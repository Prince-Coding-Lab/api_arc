using System;

namespace ShnoFeeh.API.Core.Entities
{
    /*
    This class is base entity which contain Id, 
    CreatedDate, CreatedBy, ModifiedDate, ModifiedBy
  */
    public class BaseEntity
    {
        #region Properties
        public int Id { get; protected set; }
        public DateTime? CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? ModifiedBy { get; set; }
        #endregion
    }
}
