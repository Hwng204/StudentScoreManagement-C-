using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Models
{
    public class GradeEntry
    {
        public int ComponentId { get; set; }
        public string ComponentName { get; set; }
        public double Weight { get; set; }
        public double? Score { get; set; }
    }
}
