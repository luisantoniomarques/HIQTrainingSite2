using HIQTraining.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HIQTrainingSite.ViewModel {
    public class TeacherSearchViewModel {

        public TeacherSearchViewModel() {
            Companies = new CompanyDetailListViewModel();
            Companies.SelectedCompanyId = -1; // all
            Status = -1; // all
        }
        public TeacherSearchViewModel(string name, int? companyId, int? status) {
            Name = name;
            Companies = new CompanyDetailListViewModel();
            Companies.SelectedCompanyId = companyId ?? -1;
            Status = status ?? -1;
        }

        [Display(Name = "labelStudentName", ResourceType = typeof(HIQResources))]
        [StringLength(50, ErrorMessageResourceName = "errorMessageStringMaxLength", ErrorMessageResourceType = typeof(HIQResources))]
        public string Name { get; set; }

        [Display(Name = "labelCompanyName", ResourceType = typeof(HIQResources))]
        public CompanyDetailListViewModel Companies { get; set; }


        [Display(Name = "labelStudentStatus", ResourceType = typeof(HIQResources))]
        public int Status { get; set; }
        public string StatusDescription {
            get {
                return StudentStatus.GetStatusDescription(Status);
            }
        }

        public IEnumerable<SelectListItem> StatusItems {
            get {
                return new SelectList(StudentStatus.GetStatusListForSearch(), "Id", "Description", Status);
            }
        }

    }
}