using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIQTraining.ModelDetail
{
    public class CourseFullAttendanceDetail
    {
        public int MoreThanLimit { get; set; }

        public int LimitAndBelow { get; set; }

        public int NeverAttended { get; set; }
    }
}
