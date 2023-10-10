using HIQTraining.Model;
using HIQTraining.ModelDetail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PagedList;

namespace HIQTraining.DAL.Configuration {
    public class CourseLocationDao : ICourseLocationDao {

        /// <summary>
        /// Gets all the course locations
        /// </summary>
        /// <returns></returns>
        public List<CourseLocationDetail> GetCourseLocations() {

            using(DBEntities db = new DBEntities()) {

                var locations = from l in db.CourseLocations
                                select new CourseLocationDetail {
                                    Id = l.Id,
                                    Name = l.Name
                                };

                return locations.ToList();
            }
        }

        /// <summary>
        /// Returns all course locations (paged)
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IPagedList<CourseLocationDetail> GetPagedCourseLocations(int page, int pageSize) {
            using(DBEntities db = new DBEntities()) {

                var locations = from l in db.CourseLocations
                                select new CourseLocationDetail {
                                    Id = l.Id,
                                    Name = l.Name, 
                                    DisplayColor = l.DisplayColor
                                };

                return locations.OrderBy(x => x.Name).ToPagedList(page, pageSize);
            }
        }

        /// <summary>
        /// gets a course location by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CourseLocation GetCourseLocationById(int id) {
            using(DBEntities db = new DBEntities()) {
                var location = db.CourseLocations.Find(id);
                return location;
            }
        }

        /// <summary>
        /// Adds a new course location
        /// </summary>
        /// <param name="courseLocation"></param>
        /// <returns></returns>
        public CourseLocation Add(CourseLocation courseLocation) {
            using(DBEntities db = new DBEntities()) {
                db.Entry(courseLocation).State = System.Data.Entity.EntityState.Added;
                db.SaveChanges();
                return courseLocation;
            }
        }

        /// <summary>
        /// updates a course location
        /// </summary>
        /// <param name="courseLocation"></param>
        /// <returns></returns>
        public int Update(CourseLocation courseLocation) {
            using(DBEntities db = new DBEntities()) {
                db.Entry(courseLocation).State = System.Data.Entity.EntityState.Modified;
                return db.SaveChanges();
            }
        }

        /// <summary>
        /// deletes a course location
        /// </summary>
        /// <param name="courseLocation"></param>
        /// <returns></returns>
        public int Delete(CourseLocation courseLocation) {
            using(DBEntities db = new DBEntities()) {
                db.Entry(courseLocation).State = System.Data.Entity.EntityState.Deleted;
                return db.SaveChanges();
            }
        }


    }
}
