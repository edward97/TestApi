using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using TestApi.Models;
using TestApi.Contracts.V1.Responses;

namespace TestApi.Services
{
    public interface IUserService
    {
        Task<IdentityResponse> LoginAsync(string username, string password);
        Task<IdentityResponse> RegisterAsync(Users user);
    }
}
