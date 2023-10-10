using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HIQTrainingSite.ViewModel
{
    public class CompanyViewModel
    {

        public CompanyViewModel()
        {
            this.Alert = new AlertViewModel();
        }


        public int Id { get; set; }

        [MaxLength(50)]
        [StringLength(50, ErrorMessageResourceName = "errorMessageStringMaxMinLength", ErrorMessageResourceType = typeof(HIQResources), MinimumLength = 3)]
        [Required(ErrorMessageResourceName = "errorMessageRequired", ErrorMessageResourceType = typeof(HIQResources))]
        [Display(Name = "labelCompanyName", ResourceType = typeof(HIQResources))]

        public string Name { get; set; }

        [Display(Name = "Empresa Externa")]
        public bool External { get; set; }

        public IList<CompanyDetails> companiesList { get; set; }

        public IEnumerable<SelectListItem> CompanyDrop
        {
            get
            {
                return new SelectList(companiesList, "Id", "Name", Name);
            }
        }

        public IList<CompanyDetails> ExternalOption { get; set; }

        public AlertViewModel Alert { get; set; }

    }
}