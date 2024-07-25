namespace MvcTask.Models;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


[Table("task")]
public class TaskModel
{
    [Key]
    public int Id {get; set;}

    [Column("name")]
    public required string Name {get; set;}

    [Column("description")]
    public string? Description {get; set;}

    [Column("status")]
    public string? Status {get; set;}

    [Column("created_at")]
    public DateTime? CreatedAt {get; set;}

    [Column("updated_at")]
    public DateTime? UpdatedAt {get; set;}
}
