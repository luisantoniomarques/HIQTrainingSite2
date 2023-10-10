using HIQTraining.Model;
using HIQTraining.ModelDetail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PagedList;

namespace HIQTraining.DAL.Company
{
    public class CompanyDao : ICompanyDao
    {

        /// <summary>
        /// returns all companies
        /// </summary>
        /// <returns></returns>
        public List<CompanyDetail> GetCompanies()
        {
            using (var db = new DBEntities())
            {
                var companies = from i in db.Companies
                                where i.External == false
                                select new CompanyDetail
                                {
                                Id = i.Id,
                                Name = i.Name
                                };

                return companies.ToList();
            }
        }
        public List<CompanyDetail> GetExternalCompanies()
        {
            using (var db = new DBEntities())
            {
                var companies = from i in db.Companies
                                where i.External == true
                                select new CompanyDetail
                                {
                                    Id = i.Id,
                                    Name = i.Name,
                                };
                return companies.ToList();
            }
        }
        private IQueryable<CompanyDetail> GetPagedCompanies(DBEntities db)
        {

            var companies = from company in db.Companies
                          orderby company.CreatedDate ascending
                          select new CompanyDetail
                          {
                              Id = company.Id,
                              Name = company.Name,
                          };

            return companies;

        }

        public IPagedList<CompanyDetail> GetCompanySearchResults(int page, int pageSize)
        {

            using (DBEntities db = new DBEntities())
            {
                var companies = GetPagedCompanies(db);

            
                return companies.OrderBy(x => x.Name).ToPagedList(page, pageSize);
            }
        }

        public Model.Company Add(Model.Company company, string user)
        {
            using (DBEntities db = new DBEntities())
            {
                company.UserCreated = user;
                company.CreatedDate = DateTime.Now;

                db.Entry(company).State = System.Data.Entity.EntityState.Added;
                db.Companies.Add(company);
                db.SaveChanges();

                return company;
            }
           
        }

        public Model.Company GetCompanyById(int id)
        {
            using (DBEntities db = new DBEntities())
            {
                return db.Companies.Find(id);
            }
        }

        public int Update(Model.Company company, string user)
        {
            using (DBEntities db = new DBEntities())
            {
                var companyRecord = db.Companies.Find(company.Id);
                if (companyRecord == null)
                    throw new Exception("not Found"); // TODO ApplicationException

                companyRecord.Name = company.Name;
                companyRecord.External = company.External;
                companyRecord.UpdateDate = DateTime.Now;
                companyRecord.UserUpdated = user;

                db.Entry(companyRecord).State = System.Data.Entity.EntityState.Modified;
                return db.SaveChanges();
            }
        }

        public int Delete(int id)
        {
            using (DBEntities db = new DBEntities())
            {
                var company = db.Companies.Find(id);
                if (company == null)
                    throw new Exception("not Found"); // TODO ApplicationException

                db.Entry(company).State = System.Data.Entity.EntityState.Deleted;
                return db.SaveChanges();
            }
        }

        public List<CompanyDetail> GetAllCompanies()
        {
            using (var db = new DBEntities())
            {
                var companies = from i in db.Companies
                                orderby i.Name
                                select new CompanyDetail
                                {
                                    Id = i.Id,
                                    Name = i.Name
                                };

                return companies.ToList();
            }
        }
    }
}
