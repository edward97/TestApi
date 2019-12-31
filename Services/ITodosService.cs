using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using TestApi.Models;
using TestApi.Contracts.V1.Responses;

namespace TestApi.Services
{
    public interface ITodosService
    {
        Task<List<Todos>> GetTodosAsync(int userId);
    }
}
