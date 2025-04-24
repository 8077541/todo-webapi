using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TodoApi.Models;
using TodoApi.Models.DTOs;

namespace TodoApi.Controllers
{
    [ApiController]
    [Route("api/todo")]
    public class TodosController : ControllerBase
    {
        private readonly ITodoService _todoService;

        public TodosController(ITodoService todoService)
        {
            _todoService = todoService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<TodoResponse>>> GetAll()
        {
            var todos = await _todoService.GetAllAsync();
            return Ok(todos);
        }


        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TodoResponse>> GetById(int id)
        {
            var todo = await _todoService.GetByIdAsync(id);

            if (todo == null)
            {
                return NotFound();
            }

            return Ok(todo);
        }

        [HttpGet("today")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<TodoResponse>>> GetToday()
        {
            var todos = await _todoService.GetTodayTodosAsync();
            return Ok(todos);
        }

        [HttpGet("tomorrow")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<TodoResponse>>> GetTomorrow()
        {
            var todos = await _todoService.GetNextDayTodosAsync();
            return Ok(todos);
        }

        [HttpGet("week")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<TodoResponse>>> GetWeek()
        {
            var todos = await _todoService.GetCurrentWeekTodosAsync();
            return Ok(todos);
        }



        //DATE FORMAT - 2025-04-24T19:33:11.095Z 
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<TodoResponse>> Create(TodoDto todoDto)
        {
            var createdTodo = await _todoService.CreateAsync(todoDto);
            return CreatedAtAction(nameof(GetById), new { id = createdTodo.Id }, createdTodo);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<TodoResponse>> Update(int id, TodoDto todoDto)
        {
            var updatedTodo = await _todoService.UpdateAsync(id, todoDto);

            if (updatedTodo == null)
            {
                return NotFound();
            }

            return Ok(updatedTodo);
        }

        [HttpPatch("{id}/percent")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<TodoResponse>> UpdatePercentComplete(int id, UpdateTodoPercentDto updateDto)
        {
            var updatedTodo = await _todoService.UpdatePercentCompleteAsync(id, updateDto);

            if (updatedTodo == null)
            {
                return NotFound();
            }

            return Ok(updatedTodo);
        }


        [HttpPatch("{id}/done")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TodoResponse>> MarkAsDone(int id)
        {
            var updatedTodo = await _todoService.MarkAsDoneAsync(id);

            if (updatedTodo == null)
            {
                return NotFound();
            }

            return Ok(updatedTodo);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _todoService.DeleteAsync(id);

            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}