namespace ShnoFeeh.API.Core.Entities
{
    /*
       This class contains properties of database entity ContactUs
   */
    public class ContactUs : BaseEntity
    {
        #region Properties
        public string Name { get; set; }
        public string Subject { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Content { get; set; }
        public int StatusId { get; set; }
        #endregion
    }
}
