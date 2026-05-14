using System;
using System.Collections.Generic;

namespace StudentGrade.Core.Models;

public partial class Fgexport
{
    public Guid Id { get; set; }

    public string FileName { get; set; } = null!;

    public string FilePath { get; set; } = null!;

    public Guid? ExportedBy { get; set; }

    public DateTime ExportedAt { get; set; }

    public int TotalStudents { get; set; }

    public virtual User? ExportedByNavigation { get; set; }
}
