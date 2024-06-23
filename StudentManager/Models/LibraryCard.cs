using System;
using System.Collections.Generic;

namespace StudentManager.Models;

public partial class LibraryCard
{
    public int CardId { get; set; }

    public int StudentId { get; set; }

    public DateOnly? IssueDate { get; set; }

    public DateOnly? ExpiryDate { get; set; }

    public virtual Student Student { get; set; } = null!;
}
