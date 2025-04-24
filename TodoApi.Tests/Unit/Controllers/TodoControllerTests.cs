using Microsoft.AspNetCore.Mvc;
using Moq;
using TodoApi.Models;
using TodoApi.Repositories;
using TodoApi.Services;
using TodoApi.Interfaces;
using TodoApi.Models.DTOs;
using TodoApi.Controllers;
using Xunit;

namespace TodoApi.Tests.Unit.Controllers;

public class TodosControllerTests
{
    private readonly Mock<ITodoService> _mockService;
    private readonly TodosController _controller;

    public TodosControllerTests()
    {
        _mockService = new Mock<ITodoService>();
        _controller = new TodosController(_mockService.Object);
    }

    [Fact]
    public async Task GetAll_ShouldReturnOkWithTodos()
    {
        // Arrange
        var todos = new List<TodoResponse>
        {
            new() { Id = 1, Title = "Test Todo 1" },
            new() { Id = 2, Title = "Test Todo 2" }
        };

        _mockService.Setup(s => s.GetAllAsync()).ReturnsAsync(todos);

        // Act
        var result = await _controller.GetAll();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnValue = Assert.IsAssignableFrom<IEnumerable<TodoResponse>>(okResult.Value);
        Assert.Equal(2, returnValue.Count());

        _mockService.Verify(s => s.GetAllAsync(), Times.Once);
    }

    [Fact]
    public async Task GetById_WithExistingId_ShouldReturnOkWithTodo()
    {
        // Arrange
        var todo = new TodoResponse { Id = 1, Title = "Test Todo" };
        _mockService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(todo);

        // Act
        var result = await _controller.GetById(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnValue = Assert.IsType<TodoResponse>(okResult.Value);
        Assert.Equal(1, returnValue.Id);
        Assert.Equal("Test Todo", returnValue.Title);

        _mockService.Verify(s => s.GetByIdAsync(1), Times.Once);
    }

    [Fact]
    public async Task GetById_WithNonExistingId_ShouldReturnNotFound()
    {
        // Arrange
        _mockService.Setup(s => s.GetByIdAsync(999)).ReturnsAsync((TodoResponse?)null);

        // Act
        var result = await _controller.GetById(999);

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);

        _mockService.Verify(s => s.GetByIdAsync(999), Times.Once);
    }

    [Fact]
    public async Task Create_WithValidTodo_ShouldReturnCreatedWithTodo()
    {
        // Arrange
        var todoDto = new TodoDto
        {
            Title = "New Todo",
            Description = "New Todo Description",
            ExpiryDateTime = DateTime.Now.AddDays(1)
        };

        var createdTodo = new TodoResponse
        {
            Id = 1,
            Title = todoDto.Title,
            Description = todoDto.Description,
            ExpiryDateTime = todoDto.ExpiryDateTime,
            CreatedAt = DateTime.UtcNow
        };

        _mockService.Setup(s => s.CreateAsync(todoDto)).ReturnsAsync(createdTodo);

        // Act
        var result = await _controller.Create(todoDto);

        // Assert
        var createdAtResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        var returnValue = Assert.IsType<TodoResponse>(createdAtResult.Value);
        Assert.Equal("GetById", createdAtResult.ActionName);
        Assert.Equal(1, createdAtResult.RouteValues?["id"]);
        Assert.Equal(1, returnValue.Id);
        Assert.Equal("New Todo", returnValue.Title);

        _mockService.Verify(s => s.CreateAsync(todoDto), Times.Once);
    }

    [Fact]
    public async Task Update_WithExistingId_ShouldReturnOkWithTodo()
    {
        // Arrange
        int todoId = 1;
        var todoDto = new TodoDto
        {
            Title = "Updated Todo",
            Description = "Updated Description",
            ExpiryDateTime = DateTime.Now.AddDays(2)
        };

        var updatedTodo = new TodoResponse
        {
            Id = todoId,
            Title = todoDto.Title,
            Description = todoDto.Description,
            ExpiryDateTime = todoDto.ExpiryDateTime,
            UpdatedAt = DateTime.UtcNow
        };

        _mockService.Setup(s => s.UpdateAsync(todoId, todoDto)).ReturnsAsync(updatedTodo);

        // Act
        var result = await _controller.Update(todoId, todoDto);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnValue = Assert.IsType<TodoResponse>(okResult.Value);
        Assert.Equal(todoId, returnValue.Id);
        Assert.Equal("Updated Todo", returnValue.Title);

        _mockService.Verify(s => s.UpdateAsync(todoId, todoDto), Times.Once);
    }

    [Fact]
    public async Task UpdatePercentComplete_WithExistingId_ShouldReturnOkWithTodo()
    {
        // Arrange
        int todoId = 1;
        var updateDto = new UpdateTodoPercentDto { PercentComplete = 75 };

        var updatedTodo = new TodoResponse
        {
            Id = todoId,
            Title = "Test Todo",
            PercentComplete = updateDto.PercentComplete,
            UpdatedAt = DateTime.UtcNow
        };

        _mockService.Setup(s => s.UpdatePercentCompleteAsync(todoId, updateDto))
            .ReturnsAsync(updatedTodo);

        // Act
        var result = await _controller.UpdatePercentComplete(todoId, updateDto);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnValue = Assert.IsType<TodoResponse>(okResult.Value);
        Assert.Equal(todoId, returnValue.Id);
        Assert.Equal(75, returnValue.PercentComplete);

        _mockService.Verify(s => s.UpdatePercentCompleteAsync(todoId, updateDto), Times.Once);
    }

    [Fact]
    public async Task MarkAsDone_WithExistingId_ShouldReturnOkWithTodo()
    {
        // Arrange
        int todoId = 1;

        var updatedTodo = new TodoResponse
        {
            Id = todoId,
            Title = "Test Todo",
            PercentComplete = 100,
            IsDone = true,
            UpdatedAt = DateTime.UtcNow
        };

        _mockService.Setup(s => s.MarkAsDoneAsync(todoId)).ReturnsAsync(updatedTodo);

        // Act
        var result = await _controller.MarkAsDone(todoId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnValue = Assert.IsType<TodoResponse>(okResult.Value);
        Assert.Equal(todoId, returnValue.Id);
        Assert.Equal(100, returnValue.PercentComplete);
        Assert.True(returnValue.IsDone);

        _mockService.Verify(s => s.MarkAsDoneAsync(todoId), Times.Once);
    }

    [Fact]
    public async Task Delete_WithExistingId_ShouldReturnNoContent()
    {
        // Arrange
        int todoId = 1;
        _mockService.Setup(s => s.DeleteAsync(todoId)).ReturnsAsync(true);

        // Act
        var result = await _controller.Delete(todoId);

        // Assert
        Assert.IsType<NoContentResult>(result);

        _mockService.Verify(s => s.DeleteAsync(todoId), Times.Once);
    }

    [Fact]
    public async Task Delete_WithNonExistingId_ShouldReturnNotFound()
    {
        // Arrange
        int todoId = 999;
        _mockService.Setup(s => s.DeleteAsync(todoId)).ReturnsAsync(false);

        // Act
        var result = await _controller.Delete(todoId);

        // Assert
        Assert.IsType<NotFoundResult>(result);

        _mockService.Verify(s => s.DeleteAsync(todoId), Times.Once);
    }
}