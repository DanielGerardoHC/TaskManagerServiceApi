using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.EntityFrameworkCore;
using TaskManagerServiceApi.Context;

namespace TaskManagerServiceApi.Context;

public partial class User
{
    public int? UserId { get; set; }

    public string? UserName { get; set; }

    public string? FullName { get; set; }

    public string? PasswordHash { get; set; }

    public string? Email { get; set; }

    [JsonIgnore]
    public virtual ICollection<Task> Tasks { get; set; } = new List<Task>();
}