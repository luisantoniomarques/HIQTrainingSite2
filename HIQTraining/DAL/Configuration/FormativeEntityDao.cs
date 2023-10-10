using HIQTraining.Common;
using HIQTraining.Model;
using HIQTraining.ModelDetail;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIQTraining.DAL.Configuration
{
    public class FormativeEntityDao : IFormativeEntityDao
    {
        /// <summary>
        /// Gets all course types
        /// </summary>
        /// <returns></returns>
        public IEnumerable<FormativeEntityDetail> GetFormativesEntities()
        {
            using (DBEntities db = new DBEntities())
            {
                var formativeEntities = from c in db.FormativeEntities
                                  select new FormativeEntityDetail {
                                      Id = c.Id,
                                      Name = c.Name
                                  };

                return formativeEntities.ToList();
            }
        }

        /// <summary>
        /// gets all formative entities (paged)
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IPagedList<FormativeEntityDetail> GetPagedFormativeEntities(int page, int pageSize) {
            using(DBEntities db = new DBEntities()) {
                var formativeEntities = from c in db.FormativeEntities
                                  select new FormativeEntityDetail {
                                      Id = c.Id,
                                      Name = c.Name
                                  };

                return formativeEntities.OrderBy(x => x.Name).ToPagedList(page, pageSize);
            }
        }

        /// <summary>
        /// Gets the course type detail
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public FormativeEntity GetFormativeEntityById(int id)
        {
            using (DBEntities db = new DBEntities())
            {
                return db.FormativeEntities.Find(id);
            }
        }

        /// <summary>
        /// Searches by name (starting with name value)
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public IEnumerable<FormativeEntityDetail> GetFormativeEntitiesByName(string name)
        {
            using (DBEntities db = new DBEntities())
            {
                var formativeEntities = (from c in db.FormativeEntities
                                  .Where(x => name.StartsWith(name))
                                   select new FormativeEntityDetail
                                   {
                                       Id = c.Id,
                                       Name = c.Name
                                   }).Take(HIQTrainingConstants.General.MAX_SEARCH_RECORDS);

                return formativeEntities.ToList();
            }
        }

        /// <summary>
        /// Adds a new Course Type
        /// </summary>
        /// <param name="formativeEntity"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public FormativeEntity Add(FormativeEntity formativeEntity, string user)
        {
            using (DBEntities db = new DBEntities())
            {
                formativeEntity.UserCreated = user;
                formativeEntity.CreatedDate = DateTime.Now;
                db.Entry(formativeEntity).State = System.Data.Entity.EntityState.Added;
                db.SaveChanges();
                return formativeEntity;
            }
        }

        /// <summary>
        /// Deletes a course type
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int Delete(int id)
        {
            using (DBEntities db = new DBEntities())
            {
                var formativeEntity = db.FormativeEntities.Find(id);
                if (formativeEntity == null)
                    throw new Exception("not Found"); // TODO ApplicationException
                
                db.Entry(formativeEntity).State = System.Data.Entity.EntityState.Deleted;
                return db.SaveChanges();
            }
        }

        /// <summary>
        /// Saves an existing course type
        /// </summary>
        /// <param name="formativeEntity"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public int Save(FormativeEntity formativeEntity, string user)
        {
            using (DBEntities db = new DBEntities())
            {
                var courseTypeRecord = db.FormativeEntities.Find(formativeEntity.Id);
                if (courseTypeRecord == null)
                    throw new Exception("not Found"); // TODO ApplicationException

                courseTypeRecord.Name = formativeEntity.Name;
                courseTypeRecord.UpdateDate = DateTime.Now;
                courseTypeRecord.UserUpdated = user;

                db.Entry(courseTypeRecord).State = System.Data.Entity.EntityState.Modified;
                return db.SaveChanges();
            }
        }

        

    }
}
