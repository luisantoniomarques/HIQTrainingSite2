using HIQTraining.Model;
using HIQTraining.ModelDetail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PagedList;

namespace HIQTraining.DAL.Configuration
{
    public class InscriptionTypeDao : IInscriptionTypeDao
    {

        /// <summary>
        /// adds a new inscription type
        /// </summary>
        /// <param name="inscriptionType"></param>
        /// <returns></returns>
        public InscriptionType Add(InscriptionType inscriptionType) {
            using(DBEntities db = new DBEntities()) {
                db.Entry(inscriptionType).State = System.Data.Entity.EntityState.Added;
                db.SaveChanges();

                return inscriptionType;
            }
        }

        /// <summary>
        /// deletes a inscription type
        /// </summary>
        /// <param name="inscriptionType"></param>
        /// <returns></returns>
        public int Delete(InscriptionType inscriptionType) {
            using(DBEntities db = new DBEntities()) {
                db.Entry(inscriptionType).State = System.Data.Entity.EntityState.Deleted;

                return db.SaveChanges();
            }
        }

        /// <summary>
        /// gets a inscription type by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public InscriptionType GetInscriptionTypeById(int id) {
            using(DBEntities db = new DBEntities()) {
                return db.InscriptionTypes.Find(id);
            }
        }

        /// <summary>
        /// returns all the inscriptions types
        /// </summary>
        /// <returns></returns>
        public List<InscriptionTypeDetail> GetInscriptionTypes()
        {
            using (var db = new DBEntities())
            {
                var types = from i in db.InscriptionTypes
                    select new InscriptionTypeDetail
                    {
                        Id = i.Id,
                        Description = i.Description
                    };

                return types.ToList();
            }
        }

        /// <summary>
        /// Returns all inscription types
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IPagedList<InscriptionTypeDetail> GetPagedInscriptionsTypes(int page, int pageSize) {
            using(DBEntities db = new DBEntities()) {
                var inscriptionTypeDetail = from i in db.InscriptionTypes
                                            select new InscriptionTypeDetail {
                                                Description = i.Description,
                                                Id = i.Id
                                            };

                return inscriptionTypeDetail.OrderBy(i => i.Description).ToPagedList(page, pageSize);
            }
        }

        /// <summary>
        /// updates a inscription type
        /// </summary>
        /// <param name="inscriptionType"></param>
        /// <returns></returns>
        public int Update(InscriptionType inscriptionType) {
            using(DBEntities db = new DBEntities()) {
                var updatedInscriptionType = db.InscriptionTypes.Find(inscriptionType.Id);

                if(updatedInscriptionType == null) {
                    throw new Exception("Tipo de inscrição não encontrada"); // TODO
                }

                updatedInscriptionType.Description = inscriptionType.Description;

                db.Entry(updatedInscriptionType).State = System.Data.Entity.EntityState.Modified;

                return db.SaveChanges();
            }
        }
    }
}
