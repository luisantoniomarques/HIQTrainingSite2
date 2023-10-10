using HIQTraining.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIQTraining.ModelDetail {
    public class CertificationDetail {

        
        public int Id { get; set; }

        public string CertificationName { get; set; }

        public string CertificationType { get; set; }

        public int CertificationTypeId { get; set; }

        public string StudentName { get; set; }

        public string StudentEmail { get; set; }

        public int StudentId { get; set; }

        public int? StudentStatusId { get; set; }

        public string pdf { get; set; }
        public string StatusDescription {
            get {
                return StudentStatus.GetStatusDescription(StudentStatusId);
            }
        }

        public string StudentCompany { get; set; }

        public int StudentCompanyId { get; set; }

        public DateTime Date { get; set; }

        public string DateOnly {
            get {
                return Date.ToString(Common.HIQTrainingConstants.General.DATE_FORMAT);
            }
        }

        public int StudentNumberCertifications { get; set; }

        public string Entity { get; set; }

        public decimal? Result { get; set; }

        public int? StatusId { get; set; }

        public string CertificationStatusDescription
        {
            get
            {
                return CertificationStatus.GetStatusDescription(StatusId);
            }
        }

    }
}
