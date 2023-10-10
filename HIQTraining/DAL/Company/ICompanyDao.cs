using HIQTraining.ModelDetail;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HIQTraining.Model;

namespace HIQTraining.DAL.Company
{
    public interface ICompanyDao
    {
        List<CompanyDetail> GetCompanies();
        List<CompanyDetail> GetExternalCompanies();
        IPagedList<CompanyDetail> GetCompanySearchResults(int page, int pageSize);
        Model.Company Add(Model.Company company, string user);
        Model.Company GetCompanyById(int id);
        int Update(Model.Company company, string user);
        int Delete(int id);
        //List<CompanyDetail> GetExternalCompanies();

        List<CompanyDetail> GetAllCompanies();
    }
}
