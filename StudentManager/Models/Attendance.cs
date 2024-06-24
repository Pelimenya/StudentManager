using System;
using System.Collections.Generic;

namespace StudentManager.Models;

public partial class Attendance
{
    public int AttendanceId { get; set; }
    public int StudentId { get; set; }
    public DateTime Date { get; set; }
    public bool? Status { get; set; }

    public virtual Student Student { get; set; } = null!;
}
