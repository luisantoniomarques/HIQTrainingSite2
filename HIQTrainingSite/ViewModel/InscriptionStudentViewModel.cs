using HIQTraining.ModelDetail;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HIQTrainingSite.ViewModel
{
    public class InscriptionStudentViewModel
    {
        public InscriptionStudentViewModel()
        {
            Companies = new CompanyDetailListViewModel();
            this.Alert = new AlertViewModel();
        }

        public AlertViewModel Alert { get; set; }

        [Required(ErrorMessageResourceName = "errorMessageRequired", ErrorMessageResourceType = typeof(HIQResources))]
        public int IdStudentInscription { get; set; }

        [Required(ErrorMessageResourceName = "errorMessageRequired", ErrorMessageResourceType = typeof(HIQResources))]
        [Display(Name = "labelStudentName", ResourceType = typeof(HIQResources))]
        public string NameStudentInscription { get; set; }

        [EmailAddress]
        [Display(Name = "labelEmail", ResourceType = typeof(HIQResources))]
        public string EmailStudentInscription { get; set; }

        [Required(ErrorMessageResourceName = "errorMessageRequired", ErrorMessageResourceType = typeof(HIQResources))]
        [Display(Name = "labelCompanyName", ResourceType = typeof(HIQResources))]
        public int CompanyIdStudentInscription { get; set; }

        [Display(Name = "labelCompanyName", ResourceType = typeof(HIQResources))]
        public string CompanyNameStudentInscription { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Display(Name = "labelPhoneNumber", ResourceType = typeof(HIQResources))]
        [RegularExpression(pattern: Common.HIQSiteConstants.RegularExpression.PHONE, ErrorMessageResourceName = "errorMessageInvalidPhoneNumber", ErrorMessageResourceType = typeof(HIQResources))]
        public string PhoneNumberStudentInscription { get; set; }

        public bool isExternal { get; set; }

        [Display(Name = "labelObservations", ResourceType = typeof(HIQResources))]
        public string Observation { get; set; }
        [Display(Name = "labelStudentStatus", ResourceType = typeof(HIQResources))]
        public int? Status { get; set; }
        [Display(Name = "labelStudentStatus", ResourceType = typeof(HIQResources))]
        public string StatusDescription { get; set; }

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

        public CompanyDetailListViewModel Companies { get; set; }

        public IList<CompanyDetail> CompaniesListExternal { get; set; }


        [Display(Name = "labelCompanyName", ResourceType = typeof(HIQResources))]
        public int SelectedCompanyIdExternal { get; set; }


        public IEnumerable<SelectListItem> CompaniesDropExternal
        {
            get
            {
                return new SelectList(CompaniesListExternal, "Id", "Name", SelectedCompanyIdExternal);
            }
        }

        public IList<CompanyDetail> CompaniesList { get; set; }

        public IEnumerable<SelectListItem> CompaniesDrop
        {
            get
            {
                return new SelectList(CompaniesList, "Id", "Name", CompanyIdStudentInscription);
            }
        }

        public List<StatusDetail> StatusList { get; set; }

        private int? selectedStatusId;
        public int? SelectedStatusId
        {
            get
            {
                return selectedStatusId;
            }
            set
            {
                selectedStatusId = value;
                Status = value;
            }
        }

        public IEnumerable<SelectListItem> StatusDescriptionItems
        {
            get { return new SelectList(StatusList, "Id", "Description", Status); }
        }
    }
}