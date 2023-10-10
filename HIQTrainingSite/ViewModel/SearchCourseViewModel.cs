using HIQTraining.ModelDetail;
using HIQTraining.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HIQTrainingSite.ViewModel {
    public class SearchCourseViewModel {

        [MaxLength(100)]
        [StringLength(100, ErrorMessageResourceName = "errorMessageStringMaxMinLength", ErrorMessageResourceType = typeof(HIQResources), MinimumLength = 3)]
        public string Nome { get; set; }

        [MaxLength(50)]
        [StringLength(50, ErrorMessageResourceName = "errorMessageStringMaxMinLength", ErrorMessageResourceType = typeof(HIQResources), MinimumLength = 3)]
        public string Nivel { get; set; }

        [MaxLength(50)]
        [StringLength(50, ErrorMessageResourceName = "errorMessageStringMaxMinLength", ErrorMessageResourceType = typeof(HIQResources), MinimumLength = 3)]
        public string Formador { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? DataInicio { get; set; }

        public int? Estado { get; set; }

        public string StatusDescription {
            get {
                return (Estado == null) ? null : CourseStatus.GetStatusDescription(Estado.Value);
            }
        }

        public IEnumerable<SelectListItem> StateItems {
            get { return new SelectList(CourseStatus.GetStatesListForSearch(), "Id", "Description", Estado); }
        }

        [MaxLength(50)]
        [StringLength(50, ErrorMessageResourceName = "errorMessageStringMaxMinLength", ErrorMessageResourceType = typeof(HIQResources), MinimumLength = 3)]
        public string EntidadeFormativa { get; set; }

    }
}