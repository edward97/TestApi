using System.Collections.Generic;

namespace TestApi.Contracts.V1.Responses
{
    public class IdentityResponse
    {
        public bool Success { get; set; }
        public int Id { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Token { get; set; }
        public IEnumerable<string> Errors { get; set; }
    }
}
