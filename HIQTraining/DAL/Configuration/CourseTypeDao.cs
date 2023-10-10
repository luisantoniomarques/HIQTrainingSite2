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
    public class CourseTypeDao : ICourseTypeDao
    {


      public IEnumerable<CourseTypeDetail> GetCoursesTypes()
        {
            using (DBEntities db = new DBEntities())
            {
                var types = from l in db.CourseTypes
                            select new CourseTypeDetail
                            {
                                Id = l.Id,
                                Name = l.Name
                            };

                return types.ToList();
            }
        }

       
      public IPagedList<CourseTypeDetail> GetPagedCoursesTypes(int page, int pageSize)
        {
            using (DBEntities db = new DBEntities())
            {
                var types = from l in db.CourseTypes
                            orderby (l.Name)
                            select new CourseTypeDetail
                            {
                                Id = l.Id,
                                Name = l.Name
                            };

                return types.ToPagedList(page, pageSize);
            }
        }

        /// <summary>
        /// Gets a course level by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CourseType GetCourseTypeById(int id)
        {
            using (DBEntities db = new DBEntities())
            {
                return db.CourseTypes.Find(id);
            }
        }

        /// <summary>
        /// Adds a new course level
        /// </summary>
        /// <param name="courseLevel"></param>
        /// <returns></returns>
        public CourseType Add(CourseType courseType)
        {
            using (DBEntities db = new DBEntities())
            {
                db.Entry(courseType).State = System.Data.Entity.EntityState.Added;
                db.SaveChanges();
                return courseType;
            }
        }

        /// <summary>
        /// Updates an existing course level
        /// </summary>
        /// <param name="courseLevel"></param>
        /// <returns></returns>
        public int Update(CourseType courseType)
        {
            using (DBEntities db = new DBEntities())
            {
                db.Entry(courseType).State = System.Data.Entity.EntityState.Modified;
                return db.SaveChanges();
            }
        }

        /// <summary>
        /// Deletes a course level
        /// </summary>
        /// <param name="courseLevel"></param>
        /// <returns></returns>
        public int Delete(CourseType courseType)
        {
            using (DBEntities db = new DBEntities())
            {
                db.Entry(courseType).State = System.Data.Entity.EntityState.Deleted;
                return db.SaveChanges();
            }
        }

        
    }
}
