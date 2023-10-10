using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIQTraining.ModelDetail {
    public class CalendarDetail : ModelBaseDetail {

        [Required]
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public string DateOnly {
            get{
                return Date.ToString(Common.HIQTrainingConstants.General.DATE_FORMAT);
            }
        }

        public int CourseId { get; set; }

        [MinLength(3)]
        [MaxLength(500)]
        [DataType(DataType.MultilineText)]
        public string Observation { get; set; }

        public int Status { get; set; }

        public int Attendance { get; set; }

    }
}
