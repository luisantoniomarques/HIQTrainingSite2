using HIQTraining.Model;
using HIQTraining.ModelDetail;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIQTraining.DAL.Certification {
    public interface ICertificationDao {

        Model.Certification Add(Model.Certification certification, string user);

        IEnumerable<CertificationDetail> GetCertifications();
      
        IEnumerable<CertificationTypeDetail> GetAllCertifications();

        IPagedList<CertificationDetail> GetPagedCertifications(int page, int pageSize);

        int Delete(int id);

        CertificationFullDetail GetCertificationById(int id);

        int Save(Model.Certification certification, string user);

        bool GetCode(Model.Certification certification);

        IPagedList<CertificationDetail> GetPagedStudentCertifications(int id, int page, int pageSize);
        IPagedList<CertificationDetail> GetPagedCertificationsByName(string userName, int page, int pageSize);
        IPagedList<CertificationDetail> GetCertificationsSearchResults(string name, int? companyId, int? state, int page, int pageSize);
    }
}
