using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HIQTraining.ModelDetail;
using HIQTraining.Model;
using PagedList;

namespace HIQTraining.DAL.Configuration
{
    public class CourseLevelDao : ICourseLevelDao {

        /// <summary>
        /// Gets all the course levels 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<CourseLevelDetail> GetCoursesLevels()
        {
            using(DBEntities db = new DBEntities()) {
                var level = from l in db.CourseLevels
                            select new CourseLevelDetail {
                                Id = l.Id,
                                Name = l.Name
                            };

                return level.ToList();
            }
        }

        /// <summary>
        /// Gets a paged list of course levels
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IPagedList<CourseLevelDetail> GetPagedCoursesLevels(int page, int pageSize)
        {
            using (DBEntities db = new DBEntities())
            {
                var level = from l in db.CourseLevels
                            orderby (l.Name)
                            select new CourseLevelDetail
                            {
                                Id = l.Id,
                                Name = l.Name
                            };

                return level.ToPagedList(page, pageSize);
            }
        }

        /// <summary>
        /// Gets a course level by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CourseLevel GetCourseLevelById(int id)
        {
            using (DBEntities db = new DBEntities())
            {
                return db.CourseLevels.Find(id);
            }
        }

        /// <summary>
        /// Adds a new course level
        /// </summary>
        /// <param name="courseLevel"></param>
        /// <returns></returns>
        public CourseLevel Add(CourseLevel courseLevel)
        {
            using (DBEntities db = new DBEntities())
            {
                db.Entry(courseLevel).State = System.Data.Entity.EntityState.Added;
                db.SaveChanges();
                return courseLevel;
            }
        }

        /// <summary>
        /// Updates an existing course level
        /// </summary>
        /// <param name="courseLevel"></param>
        /// <returns></returns>
        public int Update(CourseLevel courseLevel)
        {
            using (DBEntities db = new DBEntities())
            {
                db.Entry(courseLevel).State = System.Data.Entity.EntityState.Modified;
                return db.SaveChanges();
            }
        }

        /// <summary>
        /// Deletes a course level
        /// </summary>
        /// <param name="courseLevel"></param>
        /// <returns></returns>
        public int Delete(CourseLevel courseLevel)
        {
            using (DBEntities db = new DBEntities())
            {
                db.Entry(courseLevel).State = System.Data.Entity.EntityState.Deleted;
                return db.SaveChanges();
            }
        }

    }
}
