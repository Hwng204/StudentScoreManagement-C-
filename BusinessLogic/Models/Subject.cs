using System;
using System.Collections.Generic;

namespace BusinessLogic.Models;

public partial class Subject
{
    public int SubjectId { get; set; }

    public string SubjectCode { get; set; } = null!;

    public string SubjectName { get; set; } = null!;

    public virtual ICollection<AssessmentComponent> AssessmentComponents { get; set; } = new List<AssessmentComponent>();

    public virtual ICollection<Class> Classes { get; set; } = new List<Class>();
}
