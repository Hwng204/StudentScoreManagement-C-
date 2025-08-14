using System;
using System.Collections.Generic;

namespace BusinessLogic.Models;

public partial class AssessmentComponent
{
    public int ComponentId { get; set; }

    public int? SubjectId { get; set; }

    public string ComponentName { get; set; } = null!;

    public double Weight { get; set; }

    public virtual ICollection<Grade> Grades { get; set; } = new List<Grade>();

    public virtual Subject? Subject { get; set; }
}
