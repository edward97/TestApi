using System.ComponentModel.DataAnnotations;

namespace TestApi.Contracts.V1.Requests
{
    public class IdentityRequest
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
