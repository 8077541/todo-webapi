using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoApi.Interfaces;
using TodoApi.Models;
using TodoApi.Models.DTOs;

namespace TodoApi.Services
{
    public class TodoService : ITodoService
    {
        private readonly ITodoRepository _todoRepository;

        public TodoService(ITodoRepository todoRepository)
        {
            _todoRepository = todoRepository;
        }


        public async Task<IEnumerable<TodoResponse>> GetAllAsync()
        {
            var todos = await _todoRepository.GetAllAsync();
            return todos.Select(MapToResponse);
        }


        public async Task<TodoResponse?> GetByIdAsync(int id)
        {
            var todo = await _todoRepository.GetByIdAsync(id);
            return todo != null ? MapToResponse(todo) : null;
        }


        public async Task<IEnumerable<TodoResponse>> GetTodayTodosAsync()
        {
            var today = DateTime.Today;
            var tomorrow = today.AddDays(1);

            var todos = await _todoRepository.GetByExpiryRangeAsync(today, tomorrow.AddMilliseconds(-1));
            return todos.Select(MapToResponse);
        }


        public async Task<IEnumerable<TodoResponse>> GetNextDayTodosAsync()
        {
            var tomorrow = DateTime.Today.AddDays(1);
            var dayAfterTomorrow = tomorrow.AddDays(1);

            var todos = await _todoRepository.GetByExpiryRangeAsync(tomorrow, dayAfterTomorrow.AddMilliseconds(-1));
            return todos.Select(MapToResponse);
        }


        public async Task<IEnumerable<TodoResponse>> GetCurrentWeekTodosAsync()
        {
            var today = DateTime.Today;
            var startOfWeek = today.AddDays(-(int)today.DayOfWeek);
            var endOfWeek = startOfWeek.AddDays(7).AddMilliseconds(-1);

            var todos = await _todoRepository.GetByExpiryRangeAsync(startOfWeek, endOfWeek);
            return todos.Select(MapToResponse);
        }

        public async Task<TodoResponse> CreateAsync(TodoDto todoDto)
        {
            var todo = new Todo
            {
                Title = todoDto.Title,
                Description = todoDto.Description,
                ExpiryDateTime = todoDto.ExpiryDateTime,
                PercentComplete = todoDto.PercentComplete,
                IsDone = todoDto.PercentComplete == 100
            };

            var createdTodo = await _todoRepository.CreateAsync(todo);
            return MapToResponse(createdTodo);
        }


        public async Task<TodoResponse?> UpdateAsync(int id, TodoDto todoDto)
        {
            var existingTodo = await _todoRepository.GetByIdAsync(id);

            if (existingTodo == null)
            {
                return null;
            }

            existingTodo.Title = todoDto.Title;
            existingTodo.Description = todoDto.Description;
            existingTodo.ExpiryDateTime = todoDto.ExpiryDateTime;
            existingTodo.PercentComplete = todoDto.PercentComplete;
            existingTodo.IsDone = todoDto.PercentComplete == 100;

            var updatedTodo = await _todoRepository.UpdateAsync(existingTodo);
            return updatedTodo != null ? MapToResponse(updatedTodo) : null;
        }


        public async Task<TodoResponse?> UpdatePercentCompleteAsync(int id, UpdateTodoPercentDto updateDto)
        {
            var updatedTodo = await _todoRepository.UpdatePercentCompleteAsync(id, updateDto.PercentComplete);
            return updatedTodo != null ? MapToResponse(updatedTodo) : null;
        }

        public async Task<TodoResponse?> MarkAsDoneAsync(int id)
        {
            var updatedTodo = await _todoRepository.MarkAsDoneAsync(id);
            return updatedTodo != null ? MapToResponse(updatedTodo) : null;
        }


        public async Task<bool> DeleteAsync(int id)
        {
            return await _todoRepository.DeleteAsync(id);
        }


        private static TodoResponse MapToResponse(Todo todo)
        {
            return new TodoResponse
            {
                Id = todo.Id,
                Title = todo.Title,
                Description = todo.Description,
                ExpiryDateTime = todo.ExpiryDateTime,
                PercentComplete = todo.PercentComplete,
                IsDone = todo.IsDone,
                CreatedAt = todo.CreatedAt,
                UpdatedAt = todo.UpdatedAt
            };
        }
    }
}