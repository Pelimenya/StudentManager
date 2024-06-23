using System;
using System.Collections.Generic;

namespace StudentManager.Models;

public partial class User
{
    public int UserId { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Role { get; set; } = null!;

    public virtual ICollection<Student> Students { get; set; } = new List<Student>();
}
