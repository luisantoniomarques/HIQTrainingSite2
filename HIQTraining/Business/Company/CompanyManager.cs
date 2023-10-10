using HIQTraining.DAL.Company;
using HIQTraining.Exceptions;
using HIQTraining.LocalResources;
using HIQTraining.Model;
using HIQTraining.ModelDetail;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.UI;

namespace HIQTraining.Business.Company
{
    public class CompanyManager
    {
        ICompanyDao dao;

        public CompanyManager(ICompanyDao dao)
        {
            this.dao = dao;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IPagedList<CompanyDetail> GetPagedCompany(int page = 1, int pageSize = Common.HIQTrainingConstants.General.MAX_SEARCH_RECORDS)
        {
            return dao.GetCompanySearchResults(page, pageSize);
        }

        /// <summary>
        /// returns all the companies
        /// </summary>
        /// <returns></returns>
        [OutputCache(CacheProfile = "Long", VaryByHeader = "X-Request-With;Accept-Language", Location = OutputCacheLocation.Server)]
        public List<CompanyDetail> GetCompanies()
        {
            return dao.GetCompanies();
        }

        [OutputCache(CacheProfile = "Long", VaryByHeader = "X-Request-With;Accept-Language", Location = OutputCacheLocation.Server)]
        public List<CompanyDetail> GetExternalCompanies()
        {
            return dao.GetExternalCompanies();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="company"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public Model.Company Add(Model.Company company, string user)
        {
            return dao.Add(company, user);
        }

        public Model.Company GetCompanyById(int id)
        {
            return dao.GetCompanyById(id);
        }

        public int Update(Model.Company company, string user)
        {
            if (string.IsNullOrWhiteSpace(user))
                throw new ArgumentException("user parameter is required");

            return dao.Update(company, user);
        }

        public int Delete(int id)
        {
            var company = dao.GetCompanyById(id);
            if (company == null)
               throw new HIQTrainingException(HIQResource.errorMessageUnknownRecord);
            return dao.Delete(id);
        }

        public List<CompanyDetail> GetAllCompanies()
        {
            return dao.GetAllCompanies();
        }
    }
}
