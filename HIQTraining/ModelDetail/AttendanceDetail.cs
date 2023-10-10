﻿using HIQTraining.LocalResources;
using HIQTraining.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace HIQTraining.ModelDetail
{
    public class AttendanceDetail : ModelBaseDetail
    {
        [Required]
        public int? Id { get; set; }
        public int InscriptionId { get; set; }
        public int CalendarId { get; set; }
        public int StudentId { get; set; }
        public string StudentName { get; set; }
        public string CompanyName { get; set; }
        public System.DateTime Date { get; set; }
        [Display(Name = "labelObservations", ResourceType = typeof(HIQResource))]
        [StringLength(500, ErrorMessageResourceName = "errorMessageStringMaxLength", ErrorMessageResourceType = typeof(HIQResource))]
        public string Observation { get; set; }

        public int? Status { get; set; }
        public string StatusDescription
        {
            get {
                return (Status == null) ? null : AttendanceStatus.GetStatusDescription(Status.Value);
            }
        }

        public IEnumerable<SelectListItem> StateItems
        {
            get { return new SelectList(AttendanceStatus.GetStates(), "Id", "Description", Status); }
        }

    }
}
