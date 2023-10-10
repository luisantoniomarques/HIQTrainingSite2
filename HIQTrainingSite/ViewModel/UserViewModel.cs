using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HIQTrainingSite.ViewModel
{
    public class UserViewModel
    {
        public UserViewModel()
        {
            Companies = new CompanyDetailListViewModel();
            this.Alert = new AlertViewModel();
        }

        public AlertViewModel Alert { get; set; }
        public CompanyDetailListViewModel Companies { get; set; }

        [Display(Name = "labelCompanyName", ResourceType = typeof(HIQResources))]
        public int CompanyId { get; set; }
        [Display(Name = "labelCompanyName", ResourceType = typeof(HIQResources))]
        public string CompanyName { get; set; }

        [Display(Name = "labelUserType", ResourceType = typeof(HIQResources))]
        public string UserType { get; set; }

        //[Required(ErrorMessageResourceName = "errorMessageRequired", ErrorMessageResourceType = typeof(HIQResources))]
        public string Id { get; set; }

        [Required(ErrorMessageResourceName = "errorMessageRequired", ErrorMessageResourceType = typeof(HIQResources))]
        [StringLength(100, ErrorMessageResourceName = "errorMessageStringMaxMinLength", ErrorMessageResourceType = typeof(HIQResources), MinimumLength = 3)]
        [Display(Name = "labelUserName", ResourceType = typeof(HIQResources))]
        public string Name { get; set; }

        [EmailAddress(ErrorMessageResourceName = "errorMessageInvalidEmail", ErrorMessageResourceType = typeof(HIQResources))]
        [StringLength(50, ErrorMessageResourceName = "errorMessageStringMaxMinLength", ErrorMessageResourceType = typeof(HIQResources), MinimumLength = 3)]
        [Required(ErrorMessageResourceName = "errorMessageRequired", ErrorMessageResourceType = typeof(HIQResources))]
        [Display(Name = "labelUserEmail", ResourceType = typeof(HIQResources))]
        public string Email { get; set; }

        [Display(Name = "LabelUserColor", ResourceType = typeof(HIQResources))]
        public string DisplayColor { get; set; }

        [Display(Name = "labelUserRoles", ResourceType = typeof(HIQResources))]
        public List<UserRoleViewModel> UserRoles { get; set; }


        List<ApplicationRoleViewModel> _applicationRoles;
        [Display(Name = "labelApplicationRoles", ResourceType = typeof(HIQResources))]
        public List<ApplicationRoleViewModel> ApplicationRoles
        {
            get { return _applicationRoles; }
            set
            {
                _applicationRoles = value;
                if (UserRoles != null)
                {
                    foreach (var role in _applicationRoles)
                    {
                        role.IsSelected = UserRoles.Select(s => s.Id).Contains(role.Id);
                    }
                }
            }
        }
    }
}