using HIQTraining.Business.Company;
using HIQTraining.DAL.Company;
using HIQTraining.ModelDetail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIQTraining.ActiveDirectory
{
    public class CompanyUser
    {
        ActiveDirectorySearcherFactory factory;
        CompanyManager companyManager;

        public CompanyUser()
        {
            factory = new ActiveDirectorySearcherFactory();
            companyManager = new CompanyManager(new CompanyDao());
        }


        /// <summary>
        /// Searcher for company users started by userName 
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public List<ADUserDetail> SearchUsersByName(int companyId, string userName)
        {
            string companyName = companyManager.GetCompanies().Where(c => c.Id == companyId).Select(s => s.Name).FirstOrDefault();

            HIQTraining.ActiveDirectory.Company company = Mappers.CompanyMapper.GetCompany(companyId, companyName);

            ActiveDirectorySearcherBase searcher = factory.CreateActiveDirectorySearcher(company);
            List<ADUserDetail> userList = searcher.Search(userName);
            return userList;
        }
    }
}
