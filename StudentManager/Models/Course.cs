using System;
using System.Collections.Generic;

namespace StudentManager.Models;

public partial class Course
{
    public int CourseId { get; set; }

    public int CourseNumber { get; set; }

    public string? CourseName { get; set; }

    public virtual ICollection<Exam> Exams { get; set; } = new List<Exam>();
}
