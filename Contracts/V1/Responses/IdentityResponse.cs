using System.Collections.Generic;

namespace TestApi.Contracts.V1.Responses
{
    public class IdentityResponse
    {
        public bool success { get; set; }
        public int id { get; set; }
        public string email { get; set; }
        public string username { get; set; }
        public string token { get; set; }
        public IEnumerable<string> errors { get; set; }
    }
}
