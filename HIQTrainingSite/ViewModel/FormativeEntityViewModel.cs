﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HIQTrainingSite.ViewModel
{
    public class FormativeEntityViewModel
    {
        public FormativeEntityViewModel()
        {
            this.Alert = new AlertViewModel();
        }
        public AlertViewModel Alert { get; set; }

        public int Id { get; set; }
        [Required(ErrorMessageResourceName = "errorMessageRequired", ErrorMessageResourceType = typeof(HIQResources))]
        [MaxLength(50)]
        [StringLength(50, ErrorMessageResourceName = "errorMessageStringMaxMinLength", ErrorMessageResourceType = typeof(HIQResources), MinimumLength = 3)]
        [Display(Name= "labelFormativeEntity", ResourceType = typeof(HIQResources))]
        public string Name { get; set; }
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