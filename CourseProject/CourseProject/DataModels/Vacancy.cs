using System;
using System.Collections.Generic;

namespace CourseProject.DataModels;

public partial class Vacancy
{
    public int VacancyId { get; set; }

    public string Title { get; set; } = null!;

    public string Company { get; set; } = null!;

    public string? Workplace { get; set; }

    public string Region { get; set; } = null!;

    public int? MinSalary { get; set; }

    public int? MaxSalary { get; set; }

    public string Address { get; set; } = null!;

    public DateOnly UpdatedOn { get; set; }

    public string Responsibility { get; set; } = null!;

    public string Requirements { get; set; } = null!;

    public string JobInformation { get; set; } = null!;
}
