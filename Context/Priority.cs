using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TaskManagerServiceApi.Context;

public partial class Priority
{
    public int PriorityId { get; set; }

    public string PriorityStatus { get; set; } = null!;

    [JsonIgnore]
    public virtual ICollection<Task> Tasks { get; set; } = new List<Task>();
}
