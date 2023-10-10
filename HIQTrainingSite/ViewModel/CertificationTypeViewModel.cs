using HIQTrainingSite.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HIQTrainingSite.ViewModel {
    public class CertificationTypeViewModel {

        public CertificationTypeViewModel() {
            this.Alert = new AlertViewModel();
        }

        public int Id { get; set; }

        [Required(ErrorMessageResourceName = "errorMessageRequired", ErrorMessageResourceType = typeof(HIQResources))]
        [MaxLength(50)]
        [StringLength(50, ErrorMessageResourceName = "errorMessageStringMaxMinLength", ErrorMessageResourceType = typeof(HIQResources), MinimumLength = 3)]
        [Display(Name = "labelCertificationName", ResourceType = typeof(HIQResources))]
        public string Name { get; set; }

        [Required(ErrorMessageResourceName = "errorMessageRequired", ErrorMessageResourceType = typeof(HIQResources))]
        [MaxLength(10)]
        [StringLength(10, ErrorMessageResourceName = "errorMessageStringMaxMinLength", ErrorMessageResourceType = typeof(HIQResources), MinimumLength = 3)]
        [Display(Name = "labelCode", ResourceType = typeof(HIQResources))]
        [CodeValidation("Code")]
        public string Code { get; set; }

        public AlertViewModel Alert { get; set; }
    }
}