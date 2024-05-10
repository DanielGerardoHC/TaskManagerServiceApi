using System;
using System.Collections.Generic;

namespace TaskManagerServiceApi.Context;

public partial class Task
{
    public int? TaskId { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public DateTime? DueDate { get; set; }

    public int StatusId { get; set; }

    public int UserId { get; set; }

    public int PriorityId { get; set; }

    public virtual Priority? Priority { get; set; } = null!;

    public virtual TaskStatus? TaskStatus { get; set; } = null!;

    public virtual User? User { get; set; } = null!;
}
