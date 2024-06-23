using System;
using System.Collections.Generic;

namespace StudentManager.Models;

public partial class Identification
{
    public int IdentificationId { get; set; }

    public int StudentId { get; set; }

    public string Inn { get; set; } = null!;

    public string? PassportSeries { get; set; }

    public string? PassportNumber { get; set; }

    public virtual Student Student { get; set; } = null!;
}
