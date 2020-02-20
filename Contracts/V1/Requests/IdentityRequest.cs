using System.ComponentModel.DataAnnotations;

namespace TestApi.Contracts.V1.Requests
{
    public class IdentityRequest
    {
        [Required]
        public string username { get; set; }
        [Required]
        public string password { get; set; }
    }

    public class IdentityRegisterRequest
    {
        [Required]
        [EmailAddress]
        public string email { get; set; }
        [Required]
        public string username { get; set; }
        [Required]
        public string password { get; set; }
    }
}
