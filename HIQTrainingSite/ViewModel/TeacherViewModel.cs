using HIQTraining.ModelDetail;
using HIQTraining.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HIQTrainingSite.ViewModel {
    public class TeacherViewModel {

        public TeacherViewModel() {
            this.Alert = new AlertViewModel();

        }

        public AlertViewModel Alert { get; set; }

        public int Id { get; set; }

        [Display(Name = "labelStudentName", ResourceType = typeof(HIQResources))]
        [Required(ErrorMessageResourceName = "errorMessageRequired", ErrorMessageResourceType = typeof(HIQResources))]
        public string Name { get; set; }

        [Display(Name = "labelCompanyName", ResourceType = typeof(HIQResources))]
        public int SelectedCompanyId { get; set; }

        [Display(Name = "labelCompanyName", ResourceType = typeof(HIQResources))]
        public string SelectedCompany { get; set; }

        [Display(Name = "labelskills", ResourceType = typeof(HIQResources))]
        public string TecnicalSkill { get; set; }

        [Display(Name = "labelPayRoll", ResourceType = typeof(HIQResources))]
        [RegularExpression(pattern: Common.HIQSiteConstants.RegularExpression.PAYROLL, ErrorMessageResourceName = "errorMessageInvalidPayRoll", ErrorMessageResourceType = typeof(HIQResources))]
        public string PayRoll { get; set; }

        public bool isExternal { get; set; }

        public bool createTeacherBycourses { get; set; }

        public IList<CompanyDetail> CompaniesList { get; set; }

        public IEnumerable<SelectListItem> CompaniesDrop {
            get {
                return new SelectList(CompaniesList, "Id", "Name", SelectedCompanyId);
            }
        }

        [Display(Name = "labelCertificationStatus", ResourceType = typeof(HIQResources))]
        public int? StatusId { get; set; }

        [Display(Name = "labelCertificationStatus", ResourceType = typeof(HIQResources))]
        public string StatusDescription {
            get {
                return (StatusId == null) ? null : StudentStatus.GetStatusDescription(StatusId.Value);
            }
        }

        public IEnumerable<SelectListItem> StatusDrop {
            get {
                return new SelectList(StudentStatus.GetStatusListForSearch(), "Id", "Description", StatusId);
            }
        }

        [DataType(DataType.PhoneNumber)]
        [Display(Name = "labelPhoneNumber", ResourceType = typeof(HIQResources))]
        [RegularExpression(pattern: Common.HIQSiteConstants.RegularExpression.PHONE, ErrorMessageResourceName = "errorMessageInvalidPhoneNumber", ErrorMessageResourceType = typeof(HIQResources))]
        public string PhoneNumber { get; set; }

        [EmailAddress]
        [Display(Name = "labelEmail", ResourceType = typeof(HIQResources))]
        public string Email { get; set; }


     
        [Display(Name = "labelCompanyName", ResourceType = typeof(HIQResources))]
        public int SelectedCompanyIdExternal { get; set; }

   

        public IList<CompanyDetail> CompaniesListExternal { get; set; }

        public IEnumerable<SelectListItem> CompaniesDropExternal
        {
            get
            {
                return new SelectList(CompaniesListExternal, "Id", "Name", SelectedCompanyIdExternal);
            }
        }


        [Display(Name = "labelCertificationStatus", ResourceType = typeof(HIQResources))]
        public string StatusDescriptionExternal
        {
            get
            {
                return (StatusId == null) ? null : StudentStatus.GetStatusDescription(StatusId.Value);
            }
        }

        public IEnumerable<SelectListItem> StatusDropExternal
        {
            get
            {
                return new SelectList(StudentStatus.GetStatusListForSearch(), "Id", "Description", StatusId);
            }
        }


    }
}