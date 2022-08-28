using System.ComponentModel.DataAnnotations;

namespace ShnoFeeh.API.Core.Dto
{
    public class UserAuthDto
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
