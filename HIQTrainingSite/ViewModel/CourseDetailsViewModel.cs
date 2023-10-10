using HIQTraining.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HIQTrainingSite.ViewModel {
    public class CourseDetailsViewModel {

        public int Id { get; set; }

        [Display(Name = "labelCourseName", ResourceType = typeof(HIQResources))]
        public string Name { get; set; }

        [Display(Name = "labelCourseLevel", ResourceType = typeof(HIQResources))]
        public string Level { get; set; }
        [Display(Name = "labelCourseType", ResourceType = typeof(HIQResources))]
        public string Type { get; set; }

        [Display(Name = "labelCode", ResourceType = typeof(HIQResources))]
        public string Code { get; set; }

        [Display(Name = "labelLocation", ResourceType = typeof(HIQResources))]
        public string Location { get; set; }

        [Display(Name = "labelTeacherName", ResourceType = typeof(HIQResources))]
        public string Teacher { get; set; }

        [Display(Name = "labelFormativeEntityName", ResourceType = typeof(HIQResources))]
        public string Entity { get; set; }

        [Display(Name = "labelObservations", ResourceType = typeof(HIQResources))]
        [DataType(DataType.MultilineText)]
        public string Observation { get; set; }

        [Display(Name = "labelStartHour", ResourceType = typeof(HIQResources))]
        public TimeSpan StartHour { get; set; }

        [Display(Name = "labelEndHour", ResourceType = typeof(HIQResources))]
        public TimeSpan EndHour { get; set; }

        [Display(Name = "labelEffort", ResourceType = typeof(HIQResources))]
        public string Effort { get; set; }

        [Display(Name = "labelStartDate", ResourceType = typeof(HIQResources))]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
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
        [Display(Name = "labelFinished", ResourceType = typeof(HIQResources))]

        /// <summary>
        /// prop que indica se a formacao ja acabou ou nao
        /// </summary>
        public bool IsFinished {
            get {
                return (StatusId.Value == 1) ? false : ((StatusId.Value == 2) ? true : false);
            }
            set { }
        }
        [Display(Name = "labelCanceled", ResourceType = typeof(HIQResources))]
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

        /// <summary>
        /// propriedade que indica se a formação foi terminada e se esta pronta para ser lançado o Dtp dessa formaçao
        /// </summary>
        [Display(Name = "labelDtp", ResourceType = typeof(HIQResources))]
        public bool isDtp {

            get {
                    return (StatusId.Value == 1) ? false : ((StatusId.Value == 2 ) ? true : false);

            } set { }
        }

        [Display(Name = "labelCanceledObservation", ResourceType = typeof(HIQResources))]
        [DataType(DataType.MultilineText)]
        public string CanceledObservation { get; set; }

        // TODO: excel
        [Display(Name = "labelCloseDate", ResourceType = typeof(HIQResources))]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
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