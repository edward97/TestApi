using System.ComponentModel.DataAnnotations;

namespace TestApi.Contracts.V1.Requests
{
    public class TodosRequest
    {
        [Required]
        public string title { get; set; }
        [Required]
        public bool completed { get; set; }
    }
}
