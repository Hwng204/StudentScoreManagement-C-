using System;
using System.Collections.Generic;

namespace BusinessLogic.Models;

public partial class Teacher
{
    public int TeacherId { get; set; }

    public string TeacherCode { get; set; } = null!;

    public string FullName { get; set; } = null!;

    public string? Email { get; set; }

    public string? Password { get; set; }

    public string? Status { get; set; }

    public virtual ICollection<Class> Classes { get; set; } = new List<Class>();
}
