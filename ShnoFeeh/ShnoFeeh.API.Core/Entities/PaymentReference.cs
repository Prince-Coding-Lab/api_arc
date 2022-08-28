namespace ShnoFeeh.API.Core.Entities
{
    /*
    This class contains properties of database entity PaymentReference
  */
    public class PaymentReference : BaseEntity
    {
        #region Properties
        public string ExternalId { get; set; }
        public int? UserId { get; set; }
        #endregion
    }
}
