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
                .Where(x => x.userId == userId)
                .ToListAsync();
        }
    }
}
