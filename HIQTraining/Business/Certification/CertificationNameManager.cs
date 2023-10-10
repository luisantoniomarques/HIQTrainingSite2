using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HIQTraining.Model;
using HIQTraining.ModelDetail;
using HIQTraining.DAL.Certification;

namespace HIQTraining.Business.Certification
{
    class CertificationNameManager
    {
        CertificationNameDao dao;

        public IEnumerable<CertificationNameDetail> GetCertificationsTypes()
        {
            return dao.GetCertificationsName();
        }
    }
}
