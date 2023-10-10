using HIQTraining.ModelDetail;
using HIQTraining.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace HIQTrainingSite.ViewModel
{
    public class StudentViewModel
    {
        public StudentViewModel()
        {
            Companies = new CompanyDetailListViewModel();
            StatusList = StudentStatus.GetStatusList();
            this.Alert = new AlertViewModel();
        }

        public AlertViewModel Alert { get; set; }

        [Required(ErrorMessageResourceName = "errorMessageRequired", ErrorMessageResourceType = typeof(HIQResources))]
        public int Id { get; set; }


        [Required(ErrorMessageResourceName = "errorMessageRequired", ErrorMessageResourceType = typeof(HIQResources))]
        [Display(Name = "labelStudentName", ResourceType = typeof(HIQResources))]
        public string Name { get; set; }

        [EmailAddress(ErrorMessageResourceName = "errorInvalidEmail", ErrorMessageResourceType = typeof(HIQResources))]
        [Required(ErrorMessageResourceName = "errorMessageRequired", ErrorMessageResourceType = typeof(HIQResources))]
        [Display(Name = "labelEmail", ResourceType = typeof(HIQResources))]
        public string Email { get; set; }



        [Required(ErrorMessageResourceName = "errorMessageRequired", ErrorMessageResourceType = typeof(HIQResources))]
        [Display(Name = "labelCompanyName", ResourceType = typeof(HIQResources))]
        public int CompanyId { get; set; }
        [Display(Name = "labelCompanyName", ResourceType = typeof(HIQResources))]
        public string CompanyName { get; set; }


        [DataType(DataType.PhoneNumber)]
        [Display(Name = "labelPhoneNumber", ResourceType = typeof(HIQResources))]
        [RegularExpression(pattern: Common.HIQSiteConstants.RegularExpression.PHONE, ErrorMessageResourceName = "errorMessageInvalidPhoneNumber", ErrorMessageResourceType = typeof(HIQResources))]
        public string PhoneNumber { get; set; }

        public bool isExternal { get; set; }

        public bool createStudentByInscription { get; set; }

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
                return new SelectList(CompaniesList, "Id", "Name", CompanyId);
            }
        }


        public List<StatusDetail> StatusList { get; set; }

        private int? selectedStatusId;
        public int? SelectedStatusId {
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
