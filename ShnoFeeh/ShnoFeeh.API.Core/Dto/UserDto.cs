using System;
using System.ComponentModel.DataAnnotations;

namespace ShnoFeeh.API.Core.Dto
{
    /*
      This class contains data transfer object
      properties for User functionality
    */
    public class UserDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string CountryCode { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public int? RoleId { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public string CompanyName { get; set; }
        public int? CompanyTypeId { get; set; }
        public DateTime? Lastlogin { get; set; }
        public string UserStatus { get; set; }
        public int StatusId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? CreatedBy { get; set; }
        public int? ModifiedBy { get; set; }
        public string RoleName { get; set; }
        public string CompanyType { get; set; }
        public string Token { get; set; }
        public int? CountryId { get; set; }
        public string Country { get; set; }
        public int? CityId { get; set; }
        public string Photo { get; set; }
    }
    /*
      This class contains data transfer object
      properties for create User functionality
    */
    public class UserCreateDto
    {
        #region Properties
        [Required]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Enter a length between 3 to 20 for First Name")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use only characters for First Name")]
        public string FirstName { get; set; }
        [Required]
        [StringLength(15, MinimumLength = 3, ErrorMessage = "Enter a length between 3 to 15 for Last Name")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use only characters for Last Name")]
        public string LastName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        [Required]
        public string CountryCode { get; set; }
        [Required]
        [Phone]
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        [Required]
        public string Password { get; set; }
        public int RoleId { get; set; }
        public bool TwoFactorEnabled { get; set; }
        [Required]
        public string CompanyName { get; set; }
        public int? CompanyTypeId { get; set; }
        public int? UserStatus { get; set; }
        public int? CreatedBy { get; set; }
        public int? CountryId { get; set; }
        public int? CityId { get; set; }
        public string Photo { get; set; }
        #endregion
    }
    /*
      This class contains data transfer object
      properties for Login User functionality
    */
    public class LoginUserDto
    {
        #region Properties
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string CountryCode { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public int? RoleId { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public int? CompanyTypeId { get; set; }
        public string CompanyName { get; set; }
        public DateTime? Lastlogin { get; set; }
        public string UserStatus { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? PlanId { get; set; }
        public string RoleName { get; set; }
        public string CompanyType { get; set; }
        public string Token { get; set; }
        #endregion

    }
    /*
      This class contains data transfer object
      properties for User password functionality
    */
    public class UserPasswordDto
    {
        #region Properties
        public int UserId { get; set; }
        [Required]
        public string CurrentPassword { get; set; }
        [Required]
        public string NewPassword { get; set; }
        [Required]
        public string ConfirmPassword { get; set; }
        #endregion
    }
    /*
      This class contains data transfer object
      properties for reset password functionality
    */
    public class ResetPasswordDto
    {
        #region Properties
        public string code { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string ConfirmPassword { get; set; }
        public int? ModifiedBy { get; set; }
        #endregion
    }
    /*
      This class contains data transfer object
      properties for Update User functionality
    */
    public class UserUpdateDto
    {
        [Required]
        [StringLength(15,MinimumLength = 3,ErrorMessage = "Enter a length between 3 to 15 for First Name")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use only characters for First Name")]
        public string FirstName { get; set; }
        [Required]
        [StringLength(15, MinimumLength = 3, ErrorMessage = "Enter a length between 3 to 15 for Last Name")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use only characters for Last Name")]
        public string LastName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string CountryCode { get; set; }
        [Required]
        [Phone]
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public int RoleId { get; set; }
        public bool TwoFactorEnabled { get; set; }
        [Required]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Enter a length between 3 to 20 for Company Name")]
        public string CompanyName { get; set; }
        public int? CompanyTypeId { get; set; }
        public int UserStatus { get; set; }
        public int ModifiedBy { get; set; }
        public int Id { get; set; }
        public int? CountryId { get; set; }
        public int? CityId { get; set; }
        public string Photo { get; set; }
    }
}
