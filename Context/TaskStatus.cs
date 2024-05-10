using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TaskManagerServiceApi.Context;

public partial class TaskStatus
{
    public int StatusId { get; set; }

    public string StatusName { get; set; } = null!;

    [JsonIgnore]
    public virtual ICollection<Task> Tasks { get; set; } = new List<Task>();
}
