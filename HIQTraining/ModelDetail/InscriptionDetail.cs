using HIQTraining.LocalResources;
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
    public class InscriptionDetail : ModelBaseDetail
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public string StudentName { get; set; }
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public string CompanyName { get; set; }
        public int TypeId { get; set; }
        public string TypeName { get; set; }
        [Display(Name = "labelObservations", ResourceType = typeof(HIQResource))]
        [StringLength(500, ErrorMessageResourceName = "errorMessageStringMaxLength", ErrorMessageResourceType = typeof(HIQResource))]
        public string Observation { get; set; }
        public int Status { get; set; }
        public string StatusDescription {
            get
            {
                return InscriptionStatus.GetStatusDescription(Status);
            }
        }

        public IEnumerable<SelectListItem> StateItems
        {
            get { return new SelectList(InscriptionStatus.GetStates(), "Id", "Description", Status); }
        }
    }
}
