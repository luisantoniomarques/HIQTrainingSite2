using HIQTraining.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIQTraining.ModelDetail {
    public class TeacherDetail {

        public int Id { get; set; }

        public string Name { get; set; }

        public string Company { get; set; }

        public int CompanyId { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }
        public string TecnicalSkills { get; set; }
        public string PayRoll { get; set; }
        public string StatusDescription {
            get {
                return StudentStatus.GetStatusDescription(StatusId);
            }
        }
        public int StatusId { get; set; }
        public int TeacherNumberCourses { get; set; }
        public string DisplayColor { get; set; }

    }
}
