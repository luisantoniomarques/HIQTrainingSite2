using HIQTraining.Model;
using HIQTraining.ModelDetail;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIQTraining.DAL.Certification {
    public interface ICertificationTypeDao {

        IEnumerable<CertificationTypeDetail> GetCertificationsTypes();

        IEnumerable<CertificationTypeDetail> GetCertificationsTypesByName(string certificationName);

        IPagedList<CertificationTypeDetail> GetPagedCertificationTypes(int page, int pageSize);

        CertificationType GetCertificationTypeById(int id);

        CertificationType Add(CertificationType certificationType);

        int Update(CertificationType certificationType);

        int Delete(CertificationType certificationType);

        bool IsValidCode(CertificationType certificationType);
    }
}
