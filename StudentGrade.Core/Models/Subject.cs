using System;
using System.Collections.Generic;

namespace StudentGrade.Core.Models;

public partial class Subject
{
    public Guid Id { get; set; }

    public string SubjectCode { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<Assessment> Assessments { get; set; } = new List<Assessment>();
}
