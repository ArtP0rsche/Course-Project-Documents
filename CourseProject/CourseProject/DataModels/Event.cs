using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CourseProject.DataModels;

public partial class Event
{
    public int EventId { get; set; }

    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    public sbyte AvailableSpace { get; set; }

    [DisplayFormat(DataFormatString = "{0:dd MMMM в HH:mm}", ApplyFormatInEditMode = false)]
    public DateTime EventDate { get; set; }

    [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
    public DateOnly? UpdatedOn { get; set; }

    public string? Status { get; set; } = null!;

    public byte[]? Photo { get; set; }

    public virtual ICollection<Request> Requests { get; set; } = new List<Request>();
}
