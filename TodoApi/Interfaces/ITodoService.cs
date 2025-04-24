using TodoApi.Models;
using TodoApi.Models.DTOs;

public interface ITodoService
{

    Task<IEnumerable<TodoResponse>> GetAllAsync();


    Task<TodoResponse?> GetByIdAsync(int id);


    Task<IEnumerable<TodoResponse>> GetTodayTodosAsync();


    Task<IEnumerable<TodoResponse>> GetNextDayTodosAsync();


    Task<IEnumerable<TodoResponse>> GetCurrentWeekTodosAsync();


    Task<TodoResponse> CreateAsync(TodoDto todoDto);


    Task<TodoResponse?> UpdateAsync(int id, TodoDto todoDto);


    Task<TodoResponse?> UpdatePercentCompleteAsync(int id, UpdateTodoPercentDto updateDto);


    Task<TodoResponse?> MarkAsDoneAsync(int id);

    Task<bool> DeleteAsync(int id);
}

