using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Threading.Tasks;
using TestApi.Data;
using TestApi.Models;
using TestApi.Extensions;

namespace TestApi.Services
{
    public class TodosService : ITodosService
    {
        private readonly DataContext _context;

        public TodosService(DataContext context)
        {
            _context = context;
        }

        public async Task<List<Todos>> GetTodosAsync(int userId)
        {
            return await _context.Todos
                // .Include(x => x.Users)
                .Where(x => x.userId == userId)
                .ToListAsync();
        }

        public async Task<Todos> GetTodoByIdAsync(int todoId, int userId)
        {
            return await _context.Todos
                .FirstOrDefaultAsync(x => x.id == todoId && x.userId == userId);
        }

        public async Task<Todos> CreateTodoAsync(Todos todo)
        {
            await _context.Todos.AddAsync(todo);
            await _context.SaveChangesAsync();
            return todo;
        }

        public async Task<Todos> UpdateTodoAsync(Todos todo)
        {
            _context.Todos.Update(todo);
            await _context.SaveChangesAsync();

            return todo;
        }

        public async Task<bool> DeleteTodoAsync(Todos todo)
        {
            _context.Todos.Remove(todo);
            var deleted = await _context.SaveChangesAsync();
            return deleted > 0;
        }
    }
}
