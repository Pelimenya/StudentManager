using System;
using System.Collections.Generic;

namespace StudentManager.Models;

public partial class Group
{
    public int GroupId { get; set; }

    public string GroupName { get; set; } = null!;

    public int CourseNumber { get; set; }

    public virtual ICollection<Exam> Exams { get; set; } = new List<Exam>();

    public virtual ICollection<Student> Students { get; set; } = new List<Student>();
}
