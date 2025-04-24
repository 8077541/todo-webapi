
using Microsoft.EntityFrameworkCore;
using TodoApi.Data;
using TodoApi.Interfaces;
using TodoApi.Models;

namespace TodoApi.Repositories
{
    public class TodoRepository : ITodoRepository
    {
        private readonly TodoDbContext _dbContext;

        public TodoRepository(TodoDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        public async Task<IEnumerable<Todo>> GetAllAsync()
        {
            return await _dbContext.Todos.OrderBy(t => t.ExpiryDateTime).ToListAsync();
        }


        public async Task<Todo?> GetByIdAsync(int id)
        {
            return await _dbContext.Todos.FindAsync(id);
        }


        public async Task<IEnumerable<Todo>> GetByExpiryRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _dbContext.Todos
                .Where(t => t.ExpiryDateTime >= startDate && t.ExpiryDateTime <= endDate)
                .OrderBy(t => t.ExpiryDateTime)
                .ToListAsync();
        }

        public async Task<Todo> CreateAsync(Todo todo)
        {
            todo.CreatedAt = DateTime.UtcNow;

            _dbContext.Todos.Add(todo);
            await _dbContext.SaveChangesAsync();

            return todo;
        }


        public async Task<Todo?> UpdateAsync(Todo todo)
        {
            var existingTodo = await _dbContext.Todos.FindAsync(todo.Id);

            if (existingTodo == null)
            {
                return null;
            }

            existingTodo.Title = todo.Title;
            existingTodo.Description = todo.Description;
            existingTodo.ExpiryDateTime = todo.ExpiryDateTime;
            existingTodo.PercentComplete = todo.PercentComplete;
            existingTodo.UpdatedAt = DateTime.UtcNow;

            await _dbContext.SaveChangesAsync();

            return existingTodo;
        }

        public async Task<Todo?> UpdatePercentCompleteAsync(int id, int percentComplete)
        {
            var todo = await _dbContext.Todos.FindAsync(id);

            if (todo == null)
            {
                return null;
            }

            todo.PercentComplete = percentComplete;
            todo.UpdatedAt = DateTime.UtcNow;


            if (percentComplete == 100)
            {
                todo.IsDone = true;
            }

            await _dbContext.SaveChangesAsync();

            return todo;
        }


        public async Task<Todo?> MarkAsDoneAsync(int id)
        {
            var todo = await _dbContext.Todos.FindAsync(id);

            if (todo == null)
            {
                return null;
            }

            todo.IsDone = true;
            todo.PercentComplete = 100;
            todo.UpdatedAt = DateTime.UtcNow;

            await _dbContext.SaveChangesAsync();

            return todo;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var todo = await _dbContext.Todos.FindAsync(id);

            if (todo == null)
            {
                return false;
            }

            _dbContext.Todos.Remove(todo);
            await _dbContext.SaveChangesAsync();

            return true;
        }
    }
}