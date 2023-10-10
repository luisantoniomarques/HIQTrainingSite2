using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HIQTrainingSite.ViewModel
{
    public class InscriptionViewModel :AlertViewModel
    {
        public InscriptionViewModel()
        {
            InscriptionTypes = new InscriptionTypeDetailListViewModel();
            Student = new StudentViewModel();
            this.Alert = new AlertViewModel();
        }

        public int Id { get; set; }

        public AlertViewModel Alert { get; set; }

        public StudentViewModel Student { get; set; }
        [Display(Name ="labelExternInscription", ResourceType = typeof(HIQResources))]
        public bool ExternInscription { get; set; }

        public int CourseId { get; set; }
        [Display(Name = "labelCourseName", ResourceType = typeof(HIQResources))]
        public string CourseName { get; set; }
        [Required(ErrorMessageResourceName = "errorMessageRequired", ErrorMessageResourceType = typeof(HIQResources))]
        [Display(Name = "labelInscriptionType", ResourceType = typeof(HIQResources))]
        public int TypeId { get; set; }
        [Display(Name = "labelInscriptionType", ResourceType = typeof(HIQResources))]
        public string TypeName { get; set; }
        [Required(ErrorMessageResourceName = "errorMessageRequired", ErrorMessageResourceType = typeof(HIQResources))]
        [Display(Name = "labelInscriptionStatus", ResourceType = typeof(HIQResources))]
        public int Status { get; set; }
        [Display(Name = "labelInscriptionStatus", ResourceType = typeof(HIQResources))]
        public string StatusDescription { get; set; }
        public string Observation { get; set; }

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

        public InscriptionTypeDetailListViewModel InscriptionTypes { get; set; }
    }
}
