using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using TestApi.Models;
using TestApi.Services;
using TestApi.Extensions;
using TestApi.Contracts.V1;
using TestApi.Contracts.V1.Requests;
using TestApi.Contracts.V1.Responses;

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

        #region GET Todo
        [HttpGet(ApiRoutes.Todos.Get)]
        public async Task<IActionResult> GetTodo([FromRoute] int id)
        {
            int _userId = Int32.Parse(HttpContext.GetUserId());

            var todo = await _todosService.GetTodoByIdAsync(id, _userId);
            if (todo == null)
            {
                return NotFound(new
                {
                    Error = new[] { "Todo not found." }
                });
            }
            return Ok(todo);
        }
        #endregion

        #region POST Todo
        [HttpPost(ApiRoutes.Todos.Create)]
        public async Task<IActionResult> PostTodo([FromBody] TodosRequest request)
        {
            int _userId = Int32.Parse(HttpContext.GetUserId());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Todos todo = new Todos
            {
                title = request.title,
                completed = request.completed,
                userId = _userId,
                createdAt = DateTime.Now,
                updatedAt = DateTime.Now
            };
            var resTodo = await _todosService.CreateTodoAsync(todo);
            return Ok(resTodo);
        }
        #endregion

        #region PUT Todo
        [HttpPut(ApiRoutes.Todos.Update)]
        public async Task<IActionResult> PutTodo([FromRoute] int id, [FromBody] TodosRequest request)
        {
            int _userId = Int32.Parse(HttpContext.GetUserId());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Todos todo = await _todosService.GetTodoByIdAsync(id, _userId);
            if (todo == null)
            {
                return NotFound(new
                {
                    Error = new[] { "Todo not found." }
                });
            }
            todo.title = request.title;
            todo.completed = request.completed;
            todo.updatedAt = DateTime.Now;
            var resTodo = await _todosService.UpdateTodoAsync(todo);
            return Ok(resTodo);
        }
        #endregion

        #region DELETE Todo
        [HttpDelete(ApiRoutes.Todos.Delete)]
        public async Task<IActionResult> DeleteTodo([FromRoute] int id)
        {
            int _userId = Int32.Parse(HttpContext.GetUserId());

            Todos todo = await _todosService.GetTodoByIdAsync(id, _userId);
            if (todo == null)
            {
                return NotFound(new
                {
                    Error = new[] { "Todo not found." }
                });
            }

            var resTodo = await _todosService.DeleteTodoAsync(todo);
            return Ok(resTodo);
        }
        #endregion
    }
}
