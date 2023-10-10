using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HIQTraining.Model;
using HIQTraining.ModelDetail;
using PagedList;
using System.Collections;

namespace HIQTraining.DAL.Certification {

    public class CertificationDao : ICertificationDao {

        public IEnumerable<CertificationTypeDetail> GetAllCertifications()
        {
            using (DBEntities db = new DBEntities())
            {
                var certifications =( from c in db.CertificationTypes
                                     select new CertificationTypeDetail
                                     {
                                       Id = c.Id,
                                       Code = c.Code,
                                       Name = c.Name    
                                     }).ToList();

                return certifications;
            }
        }



        /// <summary>
        /// gets all certifications
        /// </summary>
        /// <returns></returns>
        public IEnumerable<CertificationDetail> GetCertifications() {
            using (DBEntities db = new DBEntities())
            {
                var certifications = from c in db.Certifications
                                     join s in db.Students on c.StudentId equals s.Id
                                     select new CertificationDetail
                                     {
                                         CertificationName = c.Name,
                                         StudentEmail = s.Email,
                                         StudentName = s.Name,
                                         Date = c.Date,
                                         Id = c.Id,
                                         StudentId = s.Id,
                                         StudentCompany = s.Company.Name,
                                         StudentCompanyId = s.Company.Id,
                                         StudentStatusId = s.Status,
                                         StudentNumberCertifications = s.Certifications.Count()
                                     };
                return certifications;
            }
        }

        /// <summary>
        /// Retuens all certifications (paged)
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IPagedList<CertificationDetail> GetPagedCertifications(int page, int pageSize) {
            using(DBEntities db = new DBEntities()) {
                var certifications = (from s in db.Students
                                     select new CertificationDetail {
                                         Id = s.Id,
                                         StudentName = s.Name,
                                         StudentEmail = s.Email,
                                         StudentStatusId = s.Status,
                                         StudentCompany = s.Company.Name,
                                         StudentNumberCertifications = s.Certifications.Count()
                                     });

                return certifications.OrderBy(x => x.StudentName).ToPagedList(page, pageSize) ;
            }
        }

        /// <summary>
        /// returns all certifications by name
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IPagedList<CertificationDetail> GetPagedCertificationsByName(string userName, int page, int pageSize) {
            using(DBEntities db = new DBEntities()) {
                var certifications = from c in db.Certifications
                                     join s in db.Students on c.StudentId equals s.Id
                                     where s.Name.StartsWith(userName)
                                     group s.Name by s into g
                                     select new CertificationDetail {
                                         StudentEmail = g.Key.Email,
                                         StudentName = g.Key.Name,
                                         StudentCompany = g.Key.Company.Name,
                                         Id = g.Key.Id,
                                         StudentId = g.Key.Id,
                                         StudentStatusId = g.Key.Status,
                                         StudentNumberCertifications = g.Key.Certifications.Count()
                                     };

                return certifications.OrderBy(x => x.StudentName).ToPagedList(page, pageSize);
            }
        }

        /// <summary>
        /// adds a new certification
        /// </summary>
        /// <param name="certification"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public Model.Certification Add(Model.Certification certification, string user) {
            using(DBEntities db = new DBEntities()) {
                certification.UserCreated = user;
                certification.CreatedDate = DateTime.Now;
                db.Entry(certification).State = System.Data.Entity.EntityState.Added;
                db.Certifications.Add(certification);
                db.SaveChanges();

                return certification;
            }
        }

        /// <summary>
        /// deletes a certification
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int Delete(int id) {
            using(DBEntities db = new DBEntities()) {
                var certification = db.Certifications.Find(id);
                if(certification == null) {
                    throw new Exception("Certificação não encontrada.");
                }

                db.Entry(certification).State = System.Data.Entity.EntityState.Deleted;
                db.Certifications.Remove(certification);

                return db.SaveChanges();
            }
        }

        /// <summary>
        /// gets a certification by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CertificationFullDetail GetCertificationById(int id) {
            using(DBEntities db = new DBEntities()) {
                var certification = (from c in db.Certifications
                                     join e in db.FormativeEntities on c.FormativeEntityId equals e.Id
                                     join certificationType in db.CertificationTypes on c.CertificationTypeId equals certificationType.Id
                                     join student in db.Students on c.StudentId equals student.Id
                                     where c.Id.Equals(id)
                                     select new CertificationFullDetail {
                                         Id = c.Id,
                                         CertificationName = c.Name,
                                         Code = certificationType.Code,
                                         CertificationTypeId = certificationType.Id,
                                         CertificationType = certificationType.Name,
                                         Date = c.Date,
                                         Observation = c.Observation,
                                         Result = c.Classification.Value,
                                         StatusId = c.Status,
                                         Entity = e.Name,
                                         StudentName = student.Name,
                                         StudentEmail = student.Email,
                                         StudentId = student.Id,
                                         pdf = c.pdf
                                     }).SingleOrDefault();

                return certification;
            }
        }

        /// <summary>
        /// updates a certification
        /// </summary>
        /// <param name="certification"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public int Save(Model.Certification certification, string user) {
            using(DBEntities db = new DBEntities()) {
                var updateCertification = db.Certifications.Find(certification.Id);

                if(updateCertification == null) {
                    throw new Exception("Certificação não encontrada.");
                }

                updateCertification.Name = certification.Name;
                updateCertification.Classification = certification.Classification;
                updateCertification.CertificationTypeId = certification.CertificationTypeId;
                updateCertification.Status = certification.Status;
                updateCertification.StudentId = certification.StudentId;
                updateCertification.FormativeEntityId = certification.FormativeEntityId;
                updateCertification.Date = certification.Date;
                updateCertification.Observation = certification.Observation;

                db.Entry(updateCertification).State = System.Data.Entity.EntityState.Modified;

                return db.SaveChanges();
            }
        }

        /// <summary>
        /// returns whether a certification fr
        /// </summary>
        /// <param name="certification"></param>
        /// <returns></returns>
        public bool GetCode(Model.Certification certification) {
            using(DBEntities db = new DBEntities()) {
                //TODO : bd changes
                //return db.Certifications.Any(c => !c.Name.ToLower().Equals(certification.Name.ToLower()) && c.Code.ToLower().Equals(certification.Code.ToLower()) && !c.Id.Equals(certification.Id));
                return true;
            }
        }

        /// <summary>
        /// gets all the certifications from a student
        /// </summary>
        /// <param name="id"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IPagedList<CertificationDetail> GetPagedStudentCertifications(int id, int page, int pageSize) {
            using(DBEntities db = new DBEntities()) {
                var certifications = from c in db.Certifications
                                     join f in db.FormativeEntities on c.FormativeEntityId equals f.Id
                                     join s in db.Students on c.StudentId equals s.Id
                                     where s.Id.Equals(id)
                                     select new CertificationDetail {
                                         CertificationName = c.Name,
                                         Entity = f.Name,
                                         StudentEmail = s.Email,
                                         StudentName = s.Name,
                                         Date = c.Date,
                                         Id = c.Id,
                                         StudentId = s.Id,
                                         Result = c.Classification,
                                         StatusId = c.Status,
                                         pdf = c.pdf
                                     };

                return certifications.OrderBy(x => x.CertificationName).ToPagedList(page, pageSize);
            }
        }

        /// <summary>
        /// returns the certifications results from the certification page
        /// </summary>
        /// <param name="name"></param>
        /// <param name="companyId"></param>
        /// <param name="state"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IPagedList<CertificationDetail> GetCertificationsSearchResults(string name, int? companyId, int? state, int page, int pageSize) {
            using(DBEntities db = new DBEntities()) {
                var certifications = (from s in db.Students
                                      select new CertificationDetail
                                      {
                                          Id = s.Id,
                                          StudentName = s.Name,
                                          StudentEmail = s.Email,
                                          StudentStatusId = s.Status,
                                          StudentCompany = s.Company.Name,
                                          StudentCompanyId = s.Company.Id,
                                          StudentNumberCertifications = s.Certifications.Count()
                                      });

                if (!string.IsNullOrEmpty(name)) {
                    certifications = certifications.Where(n => n.StudentName.ToLower().StartsWith(name.ToLower()));
                }

                if(companyId.HasValue && companyId >= 0) {
                    certifications = certifications.Where(c => c.StudentCompanyId.Equals(companyId.Value));
                }

                if(state.HasValue && state >= 0) {
                    certifications = certifications.Where(s => s.StudentStatusId.Equals(state.Value));
                }

                return certifications.OrderBy(x => x.StudentName).ToPagedList(page, pageSize);
            }
        }
    }
}
