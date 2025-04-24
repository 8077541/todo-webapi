using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoApi.Models;

namespace TodoApi.Interfaces
{
    public interface ITodoRepository
    {
        Task<IEnumerable<Todo>> GetAllAsync();

        Task<Todo?> GetByIdAsync(int id);


        Task<IEnumerable<Todo>> GetByExpiryRangeAsync(DateTime startDate, DateTime endDate);

        Task<Todo> CreateAsync(Todo todo);
        Task<Todo?> UpdatePercentCompleteAsync(int id, int percentComplete);
        Task<Todo?> UpdateAsync(Todo todo);

        Task<Todo?> MarkAsDoneAsync(int id);


        Task<bool> DeleteAsync(int id);
    }
}