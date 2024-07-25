using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MvcTask.Database;
using MvcTask.Models;

namespace MvcTask.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TasksController(ApplicationDbContext context) : Controller
{
    private readonly ApplicationDbContext _context = context;

    // GET: api/tasks
    [HttpGet]
    public IActionResult GetList([FromQuery] int limit = 10, [FromQuery] int offset = 0, [FromQuery] string q = "")
    {
        var totalCount = this._context.Tasks.Count();
        var query = this._context.Tasks.Skip(offset).Take(limit);
        if (q != null && !"".Equals(q)) {
            query.Where(r => r.Name.Contains(q));
        }
        var tasks = query.ToList();
        return Ok(new { 
            success = true, 
            payload = new {
                tasks,
                totalCount,
            }
        });
        
    }


    // GET: api/customers/{id}
    [HttpGet("{id}")]
    public IActionResult GetTask(int id)
    {
        var task = _context.Tasks.Find(id);
        if (task == null)
        {
            return NotFound(new {
                success = false,
                errors = new {
                    message = "NOT_FOUND"
                }
            });
        }
        return Ok(new {
            success = true,
            payload = new {
                task
            }
        });
    }


    [HttpPost]
    public IActionResult CreateTask([FromBody] TaskModel task)
    {
        this._context.Tasks.Add(task);
        task.CreatedAt = DateTime.Now;
        task.UpdatedAt = DateTime.Now;
        this._context.SaveChanges();
        return CreatedAtAction(nameof(GetTask), new { id = task.Id }, new {
            success = true,
            payload = new {
                task
            }
        });
    }


    // PUT: api/tasks/{id}
    [HttpPut("{id}")]
    public IActionResult UpdateTask(int id, [FromBody] TaskModel request)
    {
        if (id != request.Id)
        {
            return BadRequest();
        }
        
        //this._context.Entry(task).State = EntityState.Modified;
        var task = _context.Tasks.Find(id);
        if (null == task) {
            return NotFound(new {
                success = false,
                errors = new {
                    message = "NOT_FOUND"
                }
            });
        }

        task.Name = request.Name;
        if (null != request.Description) {
            task.Description = request.Description;
        }
        if (null != request.Status) {
            task.Status = request.Status;
        }
        task.UpdatedAt = DateTime.Now;
        this._context.SaveChanges();
        return Ok(new {
            success = true,
            payload = new {
                task
            }
        });
    }

    // DELETE: api/tasks/{id}
    [HttpDelete("{id}")]
    public IActionResult DeleteTask(int id)
    {
        var task = _context.Tasks.Find(id);
        if (task == null)
        {
            return NotFound(new {
                success = false,
                errors = new {
                    message = "NOT_FOUND"
                }
            });
        }
        this._context.Tasks.Remove(task);
        this._context.SaveChanges();
        return Ok(new {
            success = true
        });
    }
}
