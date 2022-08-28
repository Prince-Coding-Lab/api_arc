using System;

namespace ShnoFeeh.API.Core.Dto
{
    public class ContactUsDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Subject { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Content { get; set; }
        public int? StatusId { get; set; }
        public string Status { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
    }
    /*
       This class contains data transfer object
       properties for Add ContactUs functionality
   */
    public class AddContactUsDto
    {
        #region Properties
        public string Name { get; set; }
        public string Subject { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Content { get; set; }
        public int StatusId { get; set; }
        public int? CreatedBy { get; set; }
        #endregion
    }
    /*
       This class contains data transfer object
       properties for Update ContactUs functionality
   */
    public class UpdateContactUsDto
    {
        #region Properties
        public int Id { get; set; }
        public string Name { get; set; }
        public string Subject { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Content { get; set; }
        public int? StatusId { get; set; }
        public int? ModifiedBy { get; set; }
        public string Logo { get; set; }
        #endregion
    }
}
