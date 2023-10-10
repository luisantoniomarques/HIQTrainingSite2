using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HIQTrainingSite.ViewModel {
    public class InscriptionTypeViewModel {

        public InscriptionTypeViewModel() {
            Alert = new AlertViewModel();
        }

        public int Id { get; set; }

        [Required(ErrorMessageResourceName = "errorMessageRequired", ErrorMessageResourceType = typeof(HIQResources))]
        [MaxLength(50)]
        [StringLength(50, ErrorMessageResourceName = "errorMessageStringMaxMinLength", ErrorMessageResourceType = typeof(HIQResources), MinimumLength = 3)]
        [Display(Name = "labelInscriptionTypeName", ResourceType = typeof(HIQResources))]
        public string Description { get; set; }

        public AlertViewModel Alert { get; set; }

    }
}