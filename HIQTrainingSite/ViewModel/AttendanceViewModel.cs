using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HIQTrainingSite.ViewModel
{
    public class AttendanceViewModel
    {
        public AttendanceViewModel()
        {
        }

        public int Id { get; set; }
        [Required(ErrorMessageResourceName = "errorMessageRequired", ErrorMessageResourceType = typeof(HIQResources))]
        public int StudentId { get; set; }
        [Required(ErrorMessageResourceName = "errorMessageRequired", ErrorMessageResourceType = typeof(HIQResources))]
        [Display(Name = "labelStudentName", ResourceType = typeof(HIQResources))]
        public string StudentName { get; set; }
        [Display(Name = "labelCourseDate", ResourceType = typeof(HIQResources))]
        public DateTime Date { get; set; }
        [Display(Name = "labelAttended", ResourceType = typeof(HIQResources))]
        public Boolean Attendanted { get; set; }


        [Display(Name = "labelGenericUserCreated", ResourceType = typeof(HIQResources))]
        public string UserCreated { get; set; }
        [Display(Name = "labelGenericCreatedDate", ResourceType = typeof(HIQResources))]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
        public DateTime CreatedDate { get; set; }
        [Display(Name = "labelGenericUserUpdated", ResourceType = typeof(HIQResources))]
        public string UserUpdated { get; set; }
        [Display(Name = "labelGenericUpdatedDate", ResourceType = typeof(HIQResources))]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
        public Nullable<DateTime> UpdateDate { get; set; }

    }
}