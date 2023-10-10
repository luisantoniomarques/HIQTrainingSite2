using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HIQTraining.ModelDetail;
using HIQTraining.Model;

namespace HIQTraining.DAL.Certification
{
    public interface ICertificationNameDao
    {
        IEnumerable<CertificationNameDetail> GetCertificationsName();
    }
}
