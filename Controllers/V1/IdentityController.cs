using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using TestApi.Services;
using TestApi.Contracts.V1;
using TestApi.Contracts.V1.Requests;

namespace TestApi.Controllers.V1
{
    [EnableCors("AllowSpecificOrigin")]
    [Produces("application/json")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class IdentityController : Controller
    {
        private readonly IUserService _identityService;

        public IdentityController(IUserService identityService)
        {
            _identityService = identityService;
        }

        #region POST Login
        [HttpPost(ApiRoutes.Identity.Login)]
        [AllowAnonymous]
        public async Task<IActionResult> PostLogin([FromBody] IdentityRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var authResponse = await _identityService.LoginAsync(request.Username, request.Password);

            if (!authResponse.Success)
            {
                return BadRequest(authResponse);
            }

            return Ok(authResponse);
        }
        #endregion
    }
}
