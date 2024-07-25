using Microsoft.EntityFrameworkCore;
using MvcTask.Models;

namespace MvcTask.Database;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<TaskModel> Tasks { get; set; }
}