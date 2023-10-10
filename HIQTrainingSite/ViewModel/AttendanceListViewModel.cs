using HIQTraining.ModelDetail;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HIQTrainingSite.ViewModel
{
    public class AttendanceListViewModel
    {
        public AttendanceListViewModel()
        {
            AttendanceList = new List<AttendanceDetail>();
            Alert = new AlertViewModel();
        }

        public AlertViewModel Alert { get; set; }

        public int CourseId { get; set; }
        [Display(Name = "labelCourseName", ResourceType = typeof(HIQResources))]
        public string CourseName { get; set; }

        [Display(Name = "labelCourseDate", ResourceType = typeof(HIQResources))]
        public List<CalendarDetail> CourseDates { get; set; }
        public int SelectedDateId { get; set; }
        public IEnumerable<SelectListItem> CourseDateItems
        {
            get {
                if (CourseDates == null)
                    CourseDates = new List<CalendarDetail>();
                return new SelectList(CourseDates, "Id", "DateOnly", SelectedDateId);
            }
        }

        

        public List<AttendanceDetail> AttendanceList { get; set; }

        public bool IsCourseFinished { get; set; }
    }
}