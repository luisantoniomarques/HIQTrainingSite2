using HIQTraining.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace HIQTraining.ModelDetail {
    public class CourseFullDetail {

        public int Id { get; set; }

        public string Name { get; set; }

        public string Level { get; set; }

        public string Type { get; set; }

        public int TypeId { get; set; }

        public int LevelId { get; set; }

        public string Code { get; set; }

        public string Location { get; set; }

        public int LocationId { get; set; }

        public string Teacher { get; set; }

        public int TeacherId { get; set; }

        public string Entity { get; set; }

        public int EntityId { get; set; }

        public string Observation { get; set; }

        public TimeSpan StartHour { get; set; }

        public TimeSpan EndHour { get; set; }

        public string Effort { get; set; }

        public string PayRoll { get; set; }

        public DateTime StartDate { get; set; }

        public int? StatusId { get; set; }

        public string StatusDescription {
            get {
                return (StatusId == null) ? null : CourseStatus.GetStatusDescription(StatusId.Value);
            }
        }

        public IEnumerable<SelectListItem> StatusDrop {
            get {
                return new SelectList(CourseStatus.GetStates(), "Id", "Description", StatusId);
            }
        }

        /// <summary>
        /// prop que indica se a formacao ja acabou ou nao
        /// </summary>
        public bool IsFinished {
            get {
                return (StatusId.Value == 1) ? false : ((StatusId.Value == 2) ? true : false);
            }
            set { }
        }

        /// <summary>
        /// prop que indica se a formacao foi cancelada ou nao
        /// </summary>
        public bool IsCanceled
        {
            get
            {
                return (StatusId.Value == 1) ? false : ((StatusId.Value == 3) ? true : false);
            }
            set { }
        }
        public bool IsDtp
        {
            get
            {
                return (StatusId.Value == 1) ? false : ((StatusId.Value == 2) ? true : false);
            }
            set { }
        }

        public string CanceledObservation { get; set; }

        // TODO: excel
        public DateTime CloseDate { get; set; }

        public int InscriptionEmail { get; set; }

        public int ConfirmationEmail { get; set; }

        public int Reminder { get; set; }

        public int Documentation { get; set; }

        public int Intranet { get; set; }

        public int Material { get; set; }

        public int Dtp { get; set; }

        public int Certificates { get; set; }

        public int Avaliation { get; set; }

        public int Confidential { get; set; }

        public int UniqueReport { get; set; }

    }
}
