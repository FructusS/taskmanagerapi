using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using taskmanagerapi.Models.IdentityModels;

namespace taskmanagerapi.Models;

public partial class TaskUser
{
  
    public int? TaskId { get; set; }

    public int TaskuserId { get; set; }

    public string? UserId { get; set; }
    [JsonIgnore]
    public virtual Task? Task { get; set; }
    [JsonIgnore]
    public virtual AspNetUser? User { get; set; }
}
