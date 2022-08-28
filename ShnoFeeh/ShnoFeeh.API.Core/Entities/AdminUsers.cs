using System;

namespace ShnoFeeh.API.Core.Entities
{
    /*
     This class contains properties of database entity AdminUsers
   */
    public class AdminUsers : BaseEntity
    {
        #region Properties
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string CountryCode { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public string Password { get; set; }
        public int RoleId { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public string CompanyName { get; set; }
        public int CompanyTypeId { get; set; }
        public DateTime? Lastlogin { get; set; }
        public int UserStatus { get; set; }
        public int? CountryId { get; set; }
        public int? CityId { get; set; }
        public string Photo { get; set; }
        #endregion

    }
}
