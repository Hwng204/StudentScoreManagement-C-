using System;
using System.Collections.Generic;

namespace BusinessLogic.Models;

public partial class Grade
{
    public int GradeId { get; set; }

    public int? EnrollmentId { get; set; }

    public int? ComponentId { get; set; }

    public double? Score { get; set; }

    public virtual AssessmentComponent? Component { get; set; }

    public virtual Enrollment? Enrollment { get; set; }
}
