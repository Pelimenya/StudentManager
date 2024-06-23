using System;
using System.Collections.Generic;

namespace StudentManager.Models;

public partial class Contact
{
    public int ContactId { get; set; }

    public int StudentId { get; set; }

    public string? Phone { get; set; }

    public string? Email { get; set; }

    public string? Address { get; set; }

    public virtual Student Student { get; set; } = null!;
}
