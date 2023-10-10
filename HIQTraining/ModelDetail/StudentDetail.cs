using HIQTraining.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIQTraining.ModelDetail {
    public class StudentDetail : ModelBaseDetail {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public int? Status { get; set; }
        public string DisplayColor { get; set; }
        public string Observation { get; set; }
        public string StatusDescription {
            get {
                return StudentStatus.GetStatusDescription(Status);
            }
        }

        [DataType(DataType.PhoneNumber)]
        [Required(ErrorMessageResourceName = "errorMessageRequired", ErrorMessageResourceType = typeof(HIQResources))]
        [RegularExpression(pattern: @"^\d{9}$", ErrorMessageResourceName = "errorMessageInvalidPhoneNumber", ErrorMessageResourceType = typeof(HIQResources))]
        public string Phone { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Required(ErrorMessageResourceName = "errorMessageRequired", ErrorMessageResourceType = typeof(HIQResources))]
        [RegularExpression(pattern: @"^\d{9}$", ErrorMessageResourceName = "errorMessageInvalidPhoneNumber", ErrorMessageResourceType = typeof(HIQResources))]
        public string PhoneNumber { get; set; }
        public string Email { get; set; }

        public int StudentNumberCertifications { get; set; }
        public int StudentNumberInscriptions { get; set; }
    }
}
