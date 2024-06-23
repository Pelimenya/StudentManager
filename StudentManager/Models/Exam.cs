using System;
using System.Collections.Generic;

namespace StudentManager.Models;

public partial class Exam
{
    public int ExamId { get; set; }

    public int CourseId { get; set; }

    public DateTime ExamDate { get; set; }

    public string? Grade { get; set; }

    public int? Groupid { get; set; }

    public virtual Course Course { get; set; } = null!;

    public virtual Group? Group { get; set; }
}
