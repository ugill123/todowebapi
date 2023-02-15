using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoWebApi.Data;
using TodoWebApi.Models;

namespace TodoWebApi.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly TodoContext _context;

        public TasksController(TodoContext context)
        {
            _context = context;
        }

        // GET: api/Tasks
        // GET: api/Tasks/5
        /// <summary>
        /// Fetch all todos from database.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     http://localhost:5142/api/Tasks/
        ///
        /// </remarks>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Tasks>>> GetTasks()
        {
            if (_context.Tasks == null)
            {
                return NotFound();
            }
            return await _context.Tasks.ToListAsync();
        }

        // GET: api/Tasks/5
        /// <summary>
        /// Fetch specific todo with ID.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     http://localhost:5142/api/Tasks/1
        ///
        /// </remarks>
        
        [HttpGet("{id}")]
        public async Task<ActionResult<Tasks>> GetTasks(int id)
        {
            if (_context.Tasks == null)
            {
                return NotFound();
            }
            var tasks = await _context.Tasks.FindAsync(id);

            if (tasks == null)
            {
                return NotFound();
            }

            return tasks;
        }

        // PUT: api/Tasks/5
        /// <summary>
        /// Update existing Todo.
        /// </summary>
         /// <remarks>
        /// Sample request:
        ///
        ///     PUT /Todo
        ///     {
        ///        "id": 1,
        ///        "name": "update task",
        ///        "isComplete": true
        ///     }
        ///
        /// </remarks>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTasks(int id, Tasks tasks)
        {
            if (id != tasks.Id)
            {
                return BadRequest();
            }

            _context.Entry(tasks).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TasksExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Tasks
        /// <summary>
        /// Creates a TodoItem.
        /// </summary>
        /// <returns>A newly created TodoItem</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /Todo
        ///     {
        ///        "id": 1,
        ///        "name": "Item 1",
        ///        "isComplete": true
        ///     }
        ///
        /// </remarks>
        /// <response code="201">Returns the newly created item</response>
        /// <response code="400">If the item is null</response>

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Tasks>> PostTasks(Tasks tasks)
        {
            if (_context.Tasks == null)
            {
                return Problem("Entity set 'TodoContext.Tasks'  is null.");
            }
            _context.Tasks.Add(tasks);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTasks", new { id = tasks.Id }, tasks);
        }

        // DELETE: api/Tasks/5
        /// <summary>
        /// Deletes a specific TodoItem by providing ID.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTasks(int id)
        {
            if (_context.Tasks == null)
            {
                return NotFound();
            }
            var tasks = await _context.Tasks.FindAsync(id);
            if (tasks == null)
            {
                return NotFound();
            }

            _context.Tasks.Remove(tasks);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TasksExists(int id)
        {
            return (_context.Tasks?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
