using HIQTraining.DAL.Certification;
using HIQTraining.Model;
using HIQTraining.ModelDetail;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIQTraining.Business.Certification {
    public class CertificationTypeManager {

        ICertificationTypeDao dao;

        public CertificationTypeManager(ICertificationTypeDao dao) {
            this.dao = dao;
        }

        /// <summary>
        /// gets all certification types
        /// </summary>
        /// <returns></returns>
        public IEnumerable<CertificationTypeDetail> GetCertificationsTypes() {
            return dao.GetCertificationsTypes();
        }

        /// <summary>
        /// gets all certifications by name
        /// </summary>
        /// <param name="certificationType"></param>
        /// <returns></returns>
        public IEnumerable<CertificationTypeDetail> GetCertificationsTypesByName(string certificationType) {
            return dao.GetCertificationsTypesByName(certificationType);
        }

        /// <summary>
        /// gets all the certification types (paged)
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IPagedList<CertificationTypeDetail> GetPagedCertificationTypes(int page = 1, int pageSize = 10) {
            return dao.GetPagedCertificationTypes(page, pageSize);
        }

        /// <summary>
        /// gets a certification type by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CertificationType GetCertificationTypeById(int id) {
            return dao.GetCertificationTypeById(id);
        }

        /// <summary>
        /// Adds a new certification type
        /// </summary>
        /// <param name="certificationType"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public CertificationType Add(CertificationType certificationType, string userName) {
            certificationType.UserCreated = userName;
            certificationType.CreatedDate = DateTime.Now;

            return dao.Add(certificationType);
        }

        /// <summary>
        /// updates a certification type
        /// </summary>
        /// <param name="certificationType"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public int Update(CertificationType certificationType, string userName) {
            certificationType.CreatedDate = DateTime.Now;
            certificationType.UserCreated = userName;

            return dao.Update(certificationType);
        }

        /// <summary>
        /// deletes a certification type
        /// </summary>
        /// <param name="certificationType"></param>
        /// <returns></returns>
        public int Delete(CertificationType certificationType) {
            return dao.Delete(certificationType);
        }

        /// <summary>
        /// checks whether a certification code is valid or not 
        /// </summary>
        /// <param name="certificationType"></param>
        /// <returns></returns>
        public bool IsValidCode(CertificationType certificationType) {
            return dao.IsValidCode(certificationType);
        }

    }
}
