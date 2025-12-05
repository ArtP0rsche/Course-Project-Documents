using System;
using System.Collections.Generic;

namespace CourseProject.DataModels;

public partial class Request
{
    public int RequestId { get; set; }

    public int UserId { get; set; }

    public int EventId { get; set; }

    public string Content { get; set; } = null!;

    public string Status { get; set; } = null!;

    public DateOnly UpdatedOn { get; set; }

    public string? Institution { get; set; }

    public sbyte? PeopleNumber { get; set; }

    public virtual Event Event { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
