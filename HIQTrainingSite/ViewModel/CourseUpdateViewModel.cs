using HIQTraining.ModelDetail;
using HIQTrainingSite.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HIQTrainingSite.ViewModel {
    public class CourseUpdateViewModel {

        public CourseUpdateViewModel() {
            LocationsList = new List<CourseLocationDetail>();
            LevelsList = new List<CourseLevelDetail>();
            CalendarDetails = new CalendarDetail();
        }

        public int Id { get; set; }

        [MinLength(3)]
        [MaxLength(100)]
        [Display(Name = "labelCourseName", ResourceType = typeof(HIQResources))]
        [Required(ErrorMessageResourceName = "errorMessageRequired", ErrorMessageResourceType = typeof(HIQResources))]
        public string Name { get; set; }

        [Display(Name = "labelCourseLevel", ResourceType = typeof(HIQResources))]
        public int SelectedLevelId { get; set; }

        public IList<CourseLevelDetail> LevelsList { get; set; }

        public IEnumerable<SelectListItem> LevelsDrop {
            get {
                return new SelectList(LevelsList, "Id", "Name");
            }
        }

        [Required(ErrorMessageResourceName = "errorMessageRequired", ErrorMessageResourceType = typeof(HIQResources))]
        [CourseCodeUpdateValidation("Code")]
        [MinLength(3)]
        [MaxLength(10)]
        [Display(Name = "labelCode", ResourceType = typeof(HIQResources))]
        public string Code { get; set; }

        [Display(Name = "labelLocation", ResourceType = typeof(HIQResources))]
        public int SelectedLocationId { get; set; }

        public IList<CourseLocationDetail> LocationsList { get; set; }

        public IEnumerable<SelectListItem> LocationsDrop {
            get {
                return new SelectList(LocationsList, "Id", "Name");
            }
        }

        [MinLength(3)]
        [Display(Name = "labelTeacherName", ResourceType = typeof(HIQResources))]
        [Required(ErrorMessageResourceName = "errorMessageRequired", ErrorMessageResourceType = typeof(HIQResources))]
        public string Teacher { get; set; }

        public int TeacherId { get; set; }

        [MinLength(3)]
        [MaxLength(50)]
        [Display(Name = "labelCourseType", ResourceType = typeof(HIQResources))]
        [Required(ErrorMessage = "Tem de inserir o tipo da formação.")]
        public string Type { get; set; }

        public int TypeId { get; set; }

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

        //[Required]
        public string UserUpdated { get; set; }

        //[Required]
        public DateTime UpdatedDate { get; set; }

        [Display(Name = "labelCourseCalendar", ResourceType = typeof(HIQResources))]
        public string CourseCalendar { get; set; }

        [Display(Name = "labelStartDate", ResourceType = typeof(HIQResources))]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }

        public CalendarDetail CalendarDetails { get; set; }

    }

}