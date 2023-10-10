using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIQTraining.ModelDetail {
    public class CourseAttendanceRateDetail {

        public DateTime Date { get; set; }

        public int Attended { get; set; }

        public int NotAttended { get; set; }

        public int Justified { get; set; }

    }
}
