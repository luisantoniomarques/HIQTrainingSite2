using HIQTraining.DAL.Configuration;
using HIQTraining.Exceptions;
using HIQTraining.LocalResources;
using HIQTraining.ModelDetail;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIQTraining.Business.CourseLocation {
    public class CourseLocationManager {

        ICourseLocationDao dao;

        public CourseLocationManager(ICourseLocationDao dao) {
            this.dao = dao;
        }

        /// <summary>
        /// gets all the course locations
        /// </summary>
        /// <returns></returns>
        public List<CourseLocationDetail> GetCourseLocations() {
            return dao.GetCourseLocations();
        }

        /// <summary>
        /// Gets all the course locations (paged)
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IPagedList<CourseLocationDetail> GetPagedCourseLocations(int page, int pageSize) {
            return dao.GetPagedCourseLocations(page, pageSize);
        }


        /// <summary>
        /// Gets a location by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CourseLocationDetail GetCourseLocationById(int id)
        {
            CourseLocationDetail detail = null;
            var location = dao.GetCourseLocationById(id);
            if (location != null)
            {
                detail = new CourseLocationDetail();
                detail.Id = location.Id;
                detail.Name = location.Name;
                detail.UserCreated = location.UserCreated;
                detail.CreatedDate = location.CreatedDate;
                detail.DisplayColor = location.DisplayColor;
            }
            return detail;
        }

        /// <summary>
        /// adds a new course location 
        /// </summary>
        /// <param name="detail"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public Model.CourseLocation Add(CourseLocationDetail detail, string user)
        {
            if (detail == null)
                throw new ArgumentException("Course Location Detail parameter is required");

            if (string.IsNullOrWhiteSpace(user))
                throw new ArgumentException("user parameter is required");

            Model.CourseLocation courseLocation = new Model.CourseLocation();
            courseLocation.Name = detail.Name;
            courseLocation.UserCreated = user;
            courseLocation.DisplayColor = detail.DisplayColor;
            courseLocation.CreatedDate = DateTime.Now;

            return dao.Add(courseLocation);
        }

        /// <summary>
        /// updates a course location
        /// </summary>
        /// <param name="detail"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public int Update(CourseLocationDetail detail, string user)
        {
            if (detail == null)
                throw new ArgumentException("Course Location Detail parameter is required");

            if (string.IsNullOrWhiteSpace(user))
                throw new ArgumentException("user parameter is required");

            var courseLocation = dao.GetCourseLocationById(detail.Id);
            if (courseLocation == null)
                throw new HIQTrainingException(HIQResource.errorMessageUnknownRecord);

            courseLocation.Id = detail.Id;
            courseLocation.Name = detail.Name;
            courseLocation.UserUpdated = user;
            courseLocation.DisplayColor = detail.DisplayColor;
            courseLocation.UpdateDate = DateTime.Now;

            return dao.Update(courseLocation);
        }

        /// <summary>
        /// deletes a course location
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int Delete(int id)
        {
            var location = dao.GetCourseLocationById(id);
            if (location == null)
                throw new HIQTrainingException(HIQResource.errorMessageUnknownRecord);

            return dao.Delete(location);
        }

    }
}
