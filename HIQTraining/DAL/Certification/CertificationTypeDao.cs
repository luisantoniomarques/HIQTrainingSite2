using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HIQTraining.ModelDetail;
using HIQTraining.Model;
using PagedList;

namespace HIQTraining.DAL.Certification {
    public class CertificationTypeDao : ICertificationTypeDao {

        /// <summary>
        /// checks whethers a certificatino type has a valid code or not 
        /// </summary>
        /// <param name="certificationType"></param>
        /// <returns></returns>
        public bool IsValidCode(CertificationType certificationType) {
            using(DBEntities db = new DBEntities()) {
                return db.CertificationTypes.Any(c => !c.Name.ToLower().Equals(certificationType.Name.ToLower()) && c.Code.ToLower().Equals(certificationType.Code.ToLower()) && !c.Id.Equals(certificationType.Id));
            }
        }

        /// <summary>
        /// Adds a new certification type
        /// </summary>
        /// <param name="certificationType"></param>
        /// <returns></returns>
        public CertificationType Add(CertificationType certificationType) {
            using(DBEntities db = new DBEntities()) {
                db.Entry(certificationType).State = System.Data.Entity.EntityState.Added;
                db.SaveChanges();

                return certificationType;
            }
        }

        /// <summary>
        /// deletes a certification type
        /// </summary>
        /// <param name="certificationType"></param>
        /// <returns></returns>
        public int Delete(CertificationType certificationType) {
            using(DBEntities db = new DBEntities()) {
                db.Entry(certificationType).State = System.Data.Entity.EntityState.Deleted;

                return db.SaveChanges();
            }
        }

        /// <summary>
        /// Gets all certiciation types
        /// </summary>
        /// <returns></returns>
        public IEnumerable<CertificationTypeDetail> GetCertificationsTypes() {
            using(DBEntities db = new DBEntities()) {
                var certificationTypes = db.CertificationTypes.Select(c => new CertificationTypeDetail { Id = c.Id, Name = c.Name });

                return certificationTypes.ToList();
            }
        }

        /// <summary>
        /// Returns all certification types by name
        /// </summary>
        /// <param name="certificationName"></param>
        /// <returns></returns>
        public IEnumerable<CertificationTypeDetail> GetCertificationsTypesByName(string certificationName) {
            using(DBEntities db = new DBEntities()) {
                return db.CertificationTypes.Where(c => c.Name.StartsWith(certificationName)).Select(x => new CertificationTypeDetail { Id = x.Id, Name = x.Name, Code = x.Code }).ToList();
            }
        }

        /// <summary>
        /// returns a certification type by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CertificationType GetCertificationTypeById(int id) {
            using(DBEntities db = new DBEntities()) {
                return db.CertificationTypes.Find(id);
            }
        }

        /// <summary>
        /// returns all certification types
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IPagedList<CertificationTypeDetail> GetPagedCertificationTypes(int page, int pageSize) {
            using(DBEntities db = new DBEntities()) {
                return db.CertificationTypes.Select(c => new CertificationTypeDetail { Id = c.Id, Code = c.Code, Name = c.Name }).OrderBy(c => c.Name).ToPagedList(page, pageSize);
            }
        }

        /// <summary>
        /// updates a certification type 
        /// </summary>
        /// <param name="certificationType"></param>
        /// <returns></returns>
        public int Update(CertificationType certificationType) {
            using(DBEntities db = new DBEntities()) {
                var updatedCertificationType = db.CertificationTypes.Find(certificationType.Id);

                if(updatedCertificationType == null) {
                    throw new Exception("Tipo de certificação não encontrada.");
                }

                updatedCertificationType.Name = certificationType.Name;
                updatedCertificationType.Code = certificationType.Code;
                db.Entry(updatedCertificationType).State = System.Data.Entity.EntityState.Modified;

                return db.SaveChanges();
            };
        }
    }
}
