using System;
using System.Collections.Generic;

namespace StudentManager.Models;

public partial class Student
{
    public int StudentId { get; set; }

    public string FullName { get; set; } = null!;

    public int GroupId { get; set; }

    public DateOnly? DateOfBirth { get; set; }

    public string? Gender { get; set; }

    public int? UserId { get; set; }

    public virtual ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();

    public virtual ICollection<Contact> Contacts { get; set; } = new List<Contact>();

    public virtual ICollection<Exam> Exams { get; set; } = new List<Exam>();

    public virtual Group Group { get; set; } = null!;

    public virtual ICollection<Identification> Identifications { get; set; } = new List<Identification>();

    public virtual ICollection<LibraryCard> LibraryCards { get; set; } = new List<LibraryCard>();

    public virtual User? User { get; set; }
}
