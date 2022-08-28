using System.ComponentModel.DataAnnotations;

namespace ShnoFeeh.BusinessService.Dto
{
    public class AuthenticateModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
