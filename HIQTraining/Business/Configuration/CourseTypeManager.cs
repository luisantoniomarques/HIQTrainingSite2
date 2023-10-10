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

namespace HIQTraining.Business.CourseType
{
   public class CourseTypeManager {
       
            ICourseTypeDao dao;

            public CourseTypeManager(ICourseTypeDao dao)
            {
                this.dao = dao;
            }

            /// <summary>
            /// Gets all the course levels
            /// </summary>
            /// <returns></returns>
            public IEnumerable<CourseTypeDetail> GetCourseTypes()
            {
                return dao.GetCoursesTypes();
            }

            /// <summary>
            /// Gets a paged list of course levels
            /// </summary>
            /// <param name="page"></param>
            /// <param name="pageSize"></param>
            /// <returns></returns>
            public IPagedList<CourseTypeDetail> GetPagedCourseTypes(int page = 1, int pageSize = 10)
            {
                return dao.GetPagedCoursesTypes(page, pageSize);
            }

            /// <summary>
            /// gets a course level by id
            /// </summary>
            /// <param name="id"></param>
            /// <returns></returns>
            public Model.CourseType GetCourseTypeById(int id)
            {
                return dao.GetCourseTypeById(id);
            }

            /// <summary>
            /// Adds a new course level 
            /// </summary>
            /// <param name="courseLevel"></param>
            /// <param name="user"></param>
            /// <returns></returns>
            public Model.CourseType Add(Model.CourseType courseType, string user)
            {
                courseType.UserCreated = user;
                courseType.CreateDate = DateTime.Now;

                return dao.Add(courseType);
            }

            /// <summary>
            /// updates a new course level 
            /// </summary>
            /// <param name="courseLevel"></param>
            /// <param name="user"></param>
            /// <returns></returns>
            public int Update(Model.CourseType courseType, string user)
            {
            Model.CourseType type = dao.GetCourseTypeById(courseType.Id);

                if (type == null)
                    throw new HIQTrainingException(HIQResource.errorMessageUnknownRecord);

                type.Name = courseType.Name;
                type.UserUpdated = user;
                type.UpdateDate = DateTime.Now;

                return dao.Update(courseType);
            }

            /// <summary>
            /// deletes a new course level 
            /// </summary>
            /// <param name="id"></param>
            /// <returns></returns>
            public int Delete(int id)
            {
                Model.CourseType type = dao.GetCourseTypeById(id);
                if (type == null)
                    throw new HIQTrainingException(HIQResource.errorMessageUnknownRecord);

                return dao.Delete(type);
            }
        
    }
}
