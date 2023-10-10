using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HIQTraining.ModelDetail;
using HIQTraining.Model;
using PagedList;

namespace HIQTraining.DAL.Certification
{
    class CertificationNameDao : ICertificationNameDao
    {
        public IEnumerable<CertificationNameDetail> GetCertificationsName()
        {
            using (DBEntities db = new DBEntities())
            {
                var certificationNames = db.CertificationNames.Select(c => new CertificationNameDetail { Id = c.Id, Name = c.Name});

                return certificationNames.ToList();
            }
        }
    }
}
