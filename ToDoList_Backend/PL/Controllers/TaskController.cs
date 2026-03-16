using Domain.DTOs;
using Domain.Entities;
using Domain.Interfaces.Services.TaskServices;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace PL.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaskController : ControllerBase
    {
        private readonly ITaskCommandService _taskCommandservice;
        private readonly ITaskQueryService _taskQueryservice;
        private readonly ITaskFilterService _taskFilterService;
        private readonly ITaskStatusService _taskStatusService;

        public TaskController(ITaskCommandService taskCommandservice,
            ITaskQueryService taskQueryservice,
            ITaskFilterService taskFilterService,
            ITaskStatusService taskStatusService)
        {
            _taskCommandservice = taskCommandservice;
            _taskQueryservice = taskQueryservice;
            _taskFilterService = taskFilterService;
            _taskStatusService = taskStatusService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            try
            {
                var items = await _taskQueryservice.GetAllAsync();
                var total = items.Count();

                var allItems = items
                    .OrderBy(t => t.CreatedAt)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                var result = new PagedResultDTO<TaskItem>() 
                { 
                    items = allItems,
                    total = total,
                    page = page,
                    pageSize = pageSize
                };

                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var item = await _taskQueryservice.GetTaskAsync(id);
                return Ok(item);
            }
            catch (ValidationException ex)
            {
                return BadRequest(new { Message = ex.Errors });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Message = ex.Message});
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }
        [HttpGet("completed")]
        public async Task<IActionResult> GetCompletedTasks(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            try
            {
                var items = await _taskFilterService.GetCompletedTasksAsync();
                var total = items.Count();

                var allItems = items
                    .OrderBy(t => t.CreatedAt)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                var result = new PagedResultDTO<TaskItem>()
                {
                    items = allItems,
                    total = total,
                    page = page,
                    pageSize = pageSize
                };

                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }
        [HttpGet("due")]
        public async Task<IActionResult> GetDueTasks(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            try
            {
                var items = await _taskFilterService.GetDueTasksAsync();
                var total = items.Count();

                var allItems = items
                    .OrderBy(t => t.CreatedAt)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                var result = new PagedResultDTO<TaskItem>()
                {
                    items = allItems,
                    total = total,
                    page = page,
                    pageSize = pageSize
                };

                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }
        [HttpGet("status")]
        public async Task<IActionResult> GetByStatus(
            bool status,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            try
            {
                var items = await _taskFilterService.GetTasksByStatusAsync(status);
                var total = items.Count();

                var allItems = items
                    .OrderBy(t => t.CreatedAt)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                var result = new PagedResultDTO<TaskItem>()
                {
                    items = allItems,
                    total = total,
                    page = page,
                    pageSize = pageSize
                };

                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] TaskItemDTO taskItem)
        {
            try
            {
                await _taskCommandservice.CreateTaskAsync(taskItem);
                return Created();
            }
            catch (ValidationException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> MarkAsCompleted(int id)
        {
            try
            {
                await _taskStatusService.MarkAsCompleted(id);
                
                return NoContent();
            }
            catch (ValidationException ex)
            {
                return BadRequest(new { Message = ex.Errors });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutTask(int id, [FromBody] TaskItemDTO taskItemDTO)
        {
            try
            {
                await _taskCommandservice.UpdateTaskAsync(id, taskItemDTO);
                return NoContent();
            }
            catch (ValidationException ex)
            {
                return BadRequest(new { Message = ex.Errors });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            try
            {
                await _taskCommandservice.DeleteTaskAsync(id);
                return NoContent();
            }
            catch (ValidationException ex)
            {
                return BadRequest(new { Message = ex.Errors });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }
    }
}
