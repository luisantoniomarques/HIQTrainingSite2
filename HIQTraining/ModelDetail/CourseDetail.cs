using HIQTraining.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIQTraining.ModelDetail
{
    public class CourseDetail
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string  FormativeEntity { get; set; }
        public string  Level { get; set; }
        public string Teacher { get; set; }
        public string Type { get; set; }
        public int Status { get; set; }
        public string Effort { get; set; }
        public string DisplayColor { get; set; }

        public string StatusDescription {
            get
            {
                return CourseStatus.GetStatusDescription(Status);
            }
        }

        public Nullable<DateTime> StartDate { get; set; }

        public Nullable<DateTime> EndDate { get; set; }

        public string BeginDate {
        [DataType(DataType.Date)]
            get {
                if (StartDate.HasValue)
                    return StartDate.Value.ToString(Common.HIQTrainingConstants.General.DATE_FORMAT);
                else
                    return null;
            }
        }

        public string CloseDate
        {
            [DataType(DataType.Date)]
            get
            {
                if (EndDate.HasValue)
                    return EndDate.Value.ToString(Common.HIQTrainingConstants.General.DATE_FORMAT);
                else
                    return null;
            }
        }

        public int NumberOfStudents { get; set; }

        public TimeSpan StartHour { get; set; }
        public TimeSpan EndHour { get; set; }

        public int CourseAttendancePercentage { get; set; }
    }
}
