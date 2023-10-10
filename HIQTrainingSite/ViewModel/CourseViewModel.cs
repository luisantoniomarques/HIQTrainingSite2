using HIQTraining.Model;
using HIQTraining.ModelDetail;
using HIQTraining.Utils;
using HIQTrainingSite.Validation;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;

namespace HIQTrainingSite.ViewModel {
    public class CourseViewModel {
        

        public CourseViewModel() {
            LocationsList = new List<CourseLocationDetail>();
            LevelsList = new List<CourseLevelDetail>();
            CourseTypeList = new List<CourseTypeDetail>();
            CalendarDetails = new CalendarDetail();
            FormativeEntitiesList = new List<FormativeEntityDetail>();
            this.Alert = new AlertViewModel();
            TeacherModel = new TeacherViewModel();
        }

        public AlertViewModel Alert { get; set; }

        public TeacherViewModel TeacherModel { get; set; }
        public int Id { get; set; }

        [Display(Name = "labelCourseName", ResourceType = typeof(HIQResources))]
        [Required(ErrorMessageResourceName = "errorMessageRequired", ErrorMessageResourceType = typeof(HIQResources))]
        [MaxLength(100)]
        [StringLength(100, ErrorMessageResourceName = "errorMessageStringMaxMinLength", ErrorMessageResourceType = typeof(HIQResources), MinimumLength = 3)]
        public string Name { get; set; }

        [Display(Name = "labelCourseLevel", ResourceType = typeof(HIQResources))]
        public int SelectedLevelId { get; set; }

        public IList<CourseLevelDetail> LevelsList { get; set; }

        public IEnumerable<SelectListItem> LevelsDrop {
            get {
                return new SelectList(LevelsList, "Id", "Name", SelectedLevelId);
            }
        }
        [Display(Name = "labelCourseType", ResourceType = typeof(HIQResources))]
        public int SelectedCourseTypeId{ get; set; }

        public IList<CourseTypeDetail> CourseTypeList { get; set; }

        public IEnumerable<SelectListItem> CourseTypesDrop {
            get
            {
                return new SelectList(CourseTypeList, "Id", "Name", SelectedCourseTypeId);
            }
        }

        [Display(Name = "labelCode", ResourceType = typeof(HIQResources))]
        [Required(ErrorMessageResourceName = "errorMessageRequired", ErrorMessageResourceType = typeof(HIQResources))]
        [CodeValidation("Code")]
        [MaxLength(10)]
        [StringLength(10, ErrorMessageResourceName = "errorMessageStringMaxMinLength", ErrorMessageResourceType = typeof(HIQResources), MinimumLength = 3)]
        public string Code { get; set; }

        [Display(Name = "labelLocation", ResourceType = typeof(HIQResources))]
        public int SelectedLocationId { get; set; }

        public IList<CourseLocationDetail> LocationsList { get; set; }

        public IEnumerable<SelectListItem> LocationsDrop {
            get {
                return new SelectList(LocationsList, "Id", "Name", SelectedLocationId);
            }
        }

        [Display(Name = "labelTeacherName", ResourceType = typeof(HIQResources))]
        [Required(ErrorMessageResourceName = "errorMessageRequired", ErrorMessageResourceType = typeof(HIQResources))]
        [MaxLength(50)]
        [StringLength(50, ErrorMessageResourceName = "errorMessageStringMaxMinLength", ErrorMessageResourceType = typeof(HIQResources), MinimumLength = 3)]
        public string Teacher { get; set; }

        public int? TeacherId { get; set; }

        public string TeacherEmail { get; set; }

        [Display(Name = "labelFormativeEntityName", ResourceType = typeof(HIQResources))]
        public int EntityFormativeId { get; set; }

        public IList<FormativeEntityDetail> FormativeEntitiesList { get; set; }

        public IEnumerable<SelectListItem> FormativeEntitiesDrop {
            get {
                return new SelectList(FormativeEntitiesList, "Id", "Name", EntityFormativeId);
            }
        }

        [MinLength(3)]
        [MaxLength(500)]
        [Display(Name = "labelObservations", ResourceType = typeof(HIQResources))]
        [DataType(DataType.MultilineText)]
        public string Observation { get; set; }
        
        [Required(ErrorMessageResourceName = "errorMessageRequired", ErrorMessageResourceType = typeof(HIQResources))]
        [Display(Name = "labelStartHour", ResourceType = typeof(HIQResources))]
        public TimeSpan StartHour { get; set; }
        
        [Required(ErrorMessageResourceName = "errorMessageRequired", ErrorMessageResourceType = typeof(HIQResources))]
        [Display(Name = "labelEndHour", ResourceType = typeof(HIQResources))]
        public TimeSpan EndHour { get; set; }

      //[Required(ErrorMessageResourceName = "errorMessageRequired", ErrorMessageResourceType = typeof(HIQResources))]
        [Display(Name = "labelCourseCalendar", ResourceType = typeof(HIQResources))]
        public string CourseCalendar { get; set; }

        [Required(ErrorMessageResourceName = "errorMessageRequired", ErrorMessageResourceType = typeof(HIQResources))]
        [Display(Name = "labelStartDate", ResourceType = typeof(HIQResources))]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }

        public CalendarDetail CalendarDetails { get; set; }

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
        public bool IsFinished { get; set; }

        [Display(Name = "labelCanceled", ResourceType = typeof(HIQResources))]
        /// <summary>
        /// prop que indica se a formacao foi cancelada ou nao
        /// </summary>
        public bool IsCanceled { get; set; }
        /// <summary>
        /// propriedade que indica se a formação foi terminada e se esta pronta para ser lançado o Dtp dessa formaçao
        /// </summary>
        [Display(Name = "labelDtp", ResourceType = typeof(HIQResources))]    
        public bool isDtp { get; set; }
        [MinLength(3)]
        [MaxLength(500)]
        [Display(Name = "labelCanceledObservation", ResourceType = typeof(HIQResources))]
        [DataType(DataType.MultilineText)]
        public string CanceledObservation { get; set; }

        [Display(Name = "labelEffort", ResourceType = typeof(HIQResources))]
        [Required(ErrorMessageResourceName = "errorMessageRequired", ErrorMessageResourceType = typeof(HIQResources))]
        public string Effort { get; set; }
        // TODO: excel
        [Required(ErrorMessageResourceName = "errorMessageRequired", ErrorMessageResourceType = typeof(HIQResources))]
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

        [Display(Name = "Valor Diário")]
        public string PayRoll { get; set; }

        //[Required(ErrorMessage = "As datas de formação são de preenchimento obrigatório")]
        //public string CalendarDates { get; set; }

    }
}