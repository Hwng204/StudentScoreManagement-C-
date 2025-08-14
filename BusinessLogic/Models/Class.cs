using System;
using System.Collections.Generic;

namespace BusinessLogic.Models;

public partial class Class
{
    public int ClassId { get; set; }

    public string ClassCode { get; set; } = null!;

    public string ClassName { get; set; } = null!;

    public int? SubjectId { get; set; }

    public int? TeacherId { get; set; }

    public int Slot { get; set; }

    public virtual ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();

    public virtual Subject? Subject { get; set; }

    public virtual Teacher? Teacher { get; set; }
}
