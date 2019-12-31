using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using TestApi.Services;
using TestApi.Extensions;
using TestApi.Contracts.V1;
using TestApi.Contracts.V1.Requests;

namespace TestApi.Controllers.V1
{
    [EnableCors("AllowSpecificOrigin")]
    [Produces("application/json")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class TodosController : Controller
    {
        private readonly ITodosService _todosService;

        public TodosController(ITodosService todosService)
        {
            _todosService = todosService;
        }

        #region GET Todos
        [HttpGet(ApiRoutes.Todos.GetAll)]
        public async Task<IActionResult> GetTodos()
        {
            int _userId = Int32.Parse(HttpContext.GetUserId());

            var todos = await _todosService.GetTodosAsync(_userId);

            if (todos ==  null)
            {
                return NotFound(new
                {
                    Error = new[] { "Todos not found." }
                });
            }

            return Ok(todos);
        }
        #endregion
    }
}
