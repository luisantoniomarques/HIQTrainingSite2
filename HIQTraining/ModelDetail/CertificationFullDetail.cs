using HIQTraining.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIQTraining.ModelDetail {
    public class CertificationFullDetail {

        
        public int Id { get; set; }

        public string CertificationName { get; set; }

        public string CertificationType { get; set; }

        public int CertificationTypeId { get; set; }

        public string StudentName { get; set; }

        public int StudentId { get; set; }

        public string StudentEmail { get; set; }

        public string StudentCompany { get; set; }

        public int StudentCompanyId { get; set; }

        public decimal Result { get; set; }

        public int StatusId { get; set; }
        public string pdf { get; set; }

        public string StatusDescription {
            get {
                return CertificationStatus.GetStatusDescription(StatusId);
            }
        }

        public string Entity { get; set; }

        public int EntityId { get; set; }

        public string Code { get; set; }

        public string CourseType { get; set; }

        public int CourseTypeId { get; set; }

        public DateTime Date { get; set; }

        public string Observation { get; set; }

    }
}
