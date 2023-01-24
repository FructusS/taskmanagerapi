using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace taskmanagerapi.Models;

public partial class Task
{
 
    public int TaskId { get; set; }

    public string TaskTitle { get; set; } = null!;

    public string? TaskDescription { get; set; }

    public bool? IsComplete { get; set; }
    [JsonIgnore]

    public virtual ICollection<TaskUser> TaskUsers { get; } = new List<TaskUser>();
}
