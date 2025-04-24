using Moq;
using TodoApi.Models;
using TodoApi.Repositories;
using TodoApi.Services;
using TodoApi.Interfaces;
using TodoApi.Models.DTOs;

using Xunit;
namespace TodoApi.Tests.Unit.Services
{
    public class TodoServiceTests
    {
        private readonly Mock<ITodoRepository> _mockRepository;
        private readonly TodoService _service;

        public TodoServiceTests()
        {
            _mockRepository = new Mock<ITodoRepository>();
            _service = new TodoService(_mockRepository.Object);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllTodos()
        {
            // Arrange
            var todos = new List<Todo>
        {
            new() { Id = 1, Title = "Test Todo 1", ExpiryDateTime = DateTime.Now.AddDays(1) },
            new() { Id = 2, Title = "Test Todo 2", ExpiryDateTime = DateTime.Now.AddDays(2) }
        };

            _mockRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(todos);

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            Assert.Equal(2, result.Count());

            var todoResponses = result.ToList();
            Assert.Equal(1, todoResponses[0].Id);
            Assert.Equal("Test Todo 1", todoResponses[0].Title);
            Assert.Equal(2, todoResponses[1].Id);
            Assert.Equal("Test Todo 2", todoResponses[1].Title);

            _mockRepository.Verify(r => r.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_WithExistingId_ShouldReturnTodo()
        {
            // Arrange
            var todo = new Todo { Id = 1, Title = "Test Todo", ExpiryDateTime = DateTime.Now.AddDays(1) };
            _mockRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(todo);

            // Act
            var result = await _service.GetByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("Test Todo", result.Title);

            _mockRepository.Verify(r => r.GetByIdAsync(1), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_WithNonExistingId_ShouldReturnNull()
        {
            // Arrange
            _mockRepository.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Todo?)null);

            // Act
            var result = await _service.GetByIdAsync(999);

            // Assert
            Assert.Null(result);

            _mockRepository.Verify(r => r.GetByIdAsync(999), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_ShouldCreateAndReturnTodo()
        {
            // Arrange
            var todoDto = new TodoDto
            {
                Title = "New Todo",
                Description = "New Todo Description",
                ExpiryDateTime = DateTime.Now.AddDays(1),
                PercentComplete = 0
            };

            var createdTodo = new Todo
            {
                Id = 1,
                Title = todoDto.Title,
                Description = todoDto.Description,
                ExpiryDateTime = todoDto.ExpiryDateTime,
                PercentComplete = todoDto.PercentComplete,
                CreatedAt = DateTime.UtcNow
            };

            _mockRepository.Setup(r => r.CreateAsync(It.IsAny<Todo>())).ReturnsAsync(createdTodo);

            // Act
            var result = await _service.CreateAsync(todoDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("New Todo", result.Title);
            Assert.Equal("New Todo Description", result.Description);
            Assert.Equal(0, result.PercentComplete);

            _mockRepository.Verify(r => r.CreateAsync(It.IsAny<Todo>()), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_WithExistingId_ShouldUpdateAndReturnTodo()
        {
            // Arrange
            int todoId = 1;
            var todoDto = new TodoDto
            {
                Title = "Updated Todo",
                Description = "Updated Description",
                ExpiryDateTime = DateTime.Now.AddDays(2),
                PercentComplete = 50
            };

            var existingTodo = new Todo
            {
                Id = todoId,
                Title = "Original Todo",
                Description = "Original Description",
                ExpiryDateTime = DateTime.Now.AddDays(1),
                PercentComplete = 0
            };

            var updatedTodo = new Todo
            {
                Id = todoId,
                Title = todoDto.Title,
                Description = todoDto.Description,
                ExpiryDateTime = todoDto.ExpiryDateTime,
                PercentComplete = todoDto.PercentComplete,
                UpdatedAt = DateTime.UtcNow
            };

            _mockRepository.Setup(r => r.GetByIdAsync(todoId)).ReturnsAsync(existingTodo);
            _mockRepository.Setup(r => r.UpdateAsync(It.IsAny<Todo>())).ReturnsAsync(updatedTodo);

            // Act
            var result = await _service.UpdateAsync(todoId, todoDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(todoId, result.Id);
            Assert.Equal("Updated Todo", result.Title);
            Assert.Equal("Updated Description", result.Description);
            Assert.Equal(50, result.PercentComplete);

            _mockRepository.Verify(r => r.GetByIdAsync(todoId), Times.Once);
            _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<Todo>()), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_WithNonExistingId_ShouldReturnNull()
        {
            // Arrange
            int todoId = 999;
            var todoDto = new TodoDto
            {
                Title = "Updated Todo",
                Description = "Updated Description",
                ExpiryDateTime = DateTime.Now.AddDays(2),
                PercentComplete = 50
            };

            _mockRepository.Setup(r => r.GetByIdAsync(todoId)).ReturnsAsync((Todo?)null);

            // Act
            var result = await _service.UpdateAsync(todoId, todoDto);

            // Assert
            Assert.Null(result);

            _mockRepository.Verify(r => r.GetByIdAsync(todoId), Times.Once);
            _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<Todo>()), Times.Never);
        }

        [Fact]
        public async Task UpdatePercentCompleteAsync_WithExistingId_ShouldUpdatePercentAndReturnTodo()
        {
            // Arrange
            int todoId = 1;
            var updateDto = new UpdateTodoPercentDto { PercentComplete = 75 };

            var updatedTodo = new Todo
            {
                Id = todoId,
                Title = "Test Todo",
                Description = "Test Description",
                ExpiryDateTime = DateTime.Now.AddDays(1),
                PercentComplete = updateDto.PercentComplete,
                UpdatedAt = DateTime.UtcNow
            };

            _mockRepository.Setup(r => r.UpdatePercentCompleteAsync(todoId, updateDto.PercentComplete))
                .ReturnsAsync(updatedTodo);

            // Act
            var result = await _service.UpdatePercentCompleteAsync(todoId, updateDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(todoId, result.Id);
            Assert.Equal(75, result.PercentComplete);

            _mockRepository.Verify(r => r.UpdatePercentCompleteAsync(todoId, updateDto.PercentComplete), Times.Once);
        }

        [Fact]
        public async Task MarkAsDoneAsync_WithExistingId_ShouldMarkDoneAndReturnTodo()
        {
            // Arrange
            int todoId = 1;

            var updatedTodo = new Todo
            {
                Id = todoId,
                Title = "Test Todo",
                Description = "Test Description",
                ExpiryDateTime = DateTime.Now.AddDays(1),
                PercentComplete = 100,
                IsDone = true,
                UpdatedAt = DateTime.UtcNow
            };

            _mockRepository.Setup(r => r.MarkAsDoneAsync(todoId)).ReturnsAsync(updatedTodo);

            // Act
            var result = await _service.MarkAsDoneAsync(todoId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(todoId, result.Id);
            Assert.Equal(100, result.PercentComplete);
            Assert.True(result.IsDone);

            _mockRepository.Verify(r => r.MarkAsDoneAsync(todoId), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_WithExistingId_ShouldReturnTrue()
        {
            // Arrange
            int todoId = 1;
            _mockRepository.Setup(r => r.DeleteAsync(todoId)).ReturnsAsync(true);

            // Act
            var result = await _service.DeleteAsync(todoId);

            // Assert
            Assert.True(result);

            _mockRepository.Verify(r => r.DeleteAsync(todoId), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_WithNonExistingId_ShouldReturnFalse()
        {
            // Arrange
            int todoId = 999;
            _mockRepository.Setup(r => r.DeleteAsync(todoId)).ReturnsAsync(false);

            // Act
            var result = await _service.DeleteAsync(todoId);

            // Assert
            Assert.False(result);

            _mockRepository.Verify(r => r.DeleteAsync(todoId), Times.Once);
        }

        [Fact]
        public async Task GetTodayTodosAsync_ShouldReturnTodayTodos()
        {
            // Arrange
            var today = DateTime.Today;
            var todos = new List<Todo>
        {
            new() { Id = 1, Title = "Today Todo 1", ExpiryDateTime = today.AddHours(10) },
            new() { Id = 2, Title = "Today Todo 2", ExpiryDateTime = today.AddHours(16) }
        };

            _mockRepository.Setup(r => r.GetByExpiryRangeAsync(
                    It.IsAny<DateTime>(),
                    It.IsAny<DateTime>()))
                .ReturnsAsync(todos);

            // Act
            var result = await _service.GetTodayTodosAsync();

            // Assert
            Assert.Equal(2, result.Count());

            _mockRepository.Verify(r => r.GetByExpiryRangeAsync(
                It.IsAny<DateTime>(),
                It.IsAny<DateTime>()), Times.Once);
        }
    }
}