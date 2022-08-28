using System;
using System.ComponentModel.DataAnnotations;

namespace ShnoFeeh.BusinessService.Dto
{
    public class UserEmailDto
    {
        [Required]
        [EmailAddress]
        public string email { get; set; }
    }
  
}
