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
    public class CertificationManager {

        ICertificationDao dao;

        public CertificationManager(ICertificationDao dao) {
            this.dao = dao;
        }

        /// <summary>
        /// adds a new certification
        /// </summary>
        /// <param name="certification"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public Model.Certification Add(Model.Certification certification, string user) {
            return dao.Add(certification, user);
        }

        /// <summary>
        /// gets the certifications list
        /// </summary>
        /// <returns></returns>
        public IEnumerable<CertificationDetail> GetCertifications() {
            return dao.GetCertifications();
        }
        /// <summary>
        /// gets all certifications list
        /// </summary>
        /// <returns></returns>
        public IEnumerable<CertificationTypeDetail> GetAllCertifications()
        {
            return dao.GetAllCertifications();
        }



        /// <summary>
        /// returns all the certifications (paged)
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IPagedList<CertificationDetail> GetPagedCertifications(int page, int pageSize = Common.HIQTrainingConstants.General.MAX_SEARCH_RECORDS) {
            return dao.GetPagedCertifications(page, pageSize);
        }

        /// <summary>
        /// returns all the certifications by name (paged)
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IPagedList<CertificationDetail> GetPagedCertificationsByName(string userName, int page, int pageSize) {
            return dao.GetPagedCertificationsByName(userName, page, pageSize);
        }

        /// <summary>
        /// deletes a certification
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int Delete(int id) {
            return dao.Delete(id);
        }

        /// <summary>
        /// checks whether a certification code is valid or not 
        /// </summary>
        /// <param name="certification"></param>
        /// <returns></returns>
        public bool GetCode(Model.Certification certification) {
            return dao.GetCode(certification);
        }

        /// <summary>
        /// Gets a certification by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CertificationFullDetail GetCertificationById(int id) {
            return dao.GetCertificationById(id);
        }

        /// <summary>
        /// updates a certification
        /// </summary>
        /// <param name="certification"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public int Save(Model.Certification certification, string user) {
            return dao.Save(certification, user);
        }

        /// <summary>
        /// returns all certifications from a specific student (paged)
        /// </summary>
        /// <param name="id"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IPagedList<CertificationDetail> GetPagedStudentCertifications(int id, int page, int pageSize) {
            return dao.GetPagedStudentCertifications(id, page, pageSize);
        }

        /// <summary>
        /// returns the results from the certifications search (paged)
        /// </summary>
        /// <param name="name"></param>
        /// <param name="companyId"></param>
        /// <param name="state"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IPagedList<CertificationDetail> GetCertificationsSearchResults(string name, int? companyId, int? state, int page = 1, int pageSize = Common.HIQTrainingConstants.General.MAX_SEARCH_RECORDS) {
            return dao.GetCertificationsSearchResults(name, companyId, state, page, pageSize);
        }

    }
}
