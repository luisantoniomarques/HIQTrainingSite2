using HIQTraining.DAL.Configuration;
using HIQTraining.Exceptions;
using HIQTraining.LocalResources;
using HIQTraining.ModelDetail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PagedList;


namespace HIQTraining.Business.CourseLevel
{
    public class CourseLevelManager {

        ICourseLevelDao dao;

        public CourseLevelManager(ICourseLevelDao dao) {
            this.dao = dao;
        }

        /// <summary>
        /// Gets all the course levels
        /// </summary>
        /// <returns></returns>
        public IEnumerable<CourseLevelDetail> GetCourseLevels() {
            return dao.GetCoursesLevels();
        }

        /// <summary>
        /// Gets a paged list of course levels
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IPagedList<CourseLevelDetail> GetPagedCourseLevels(int page = 1, int pageSize = 10)
        {
            return dao.GetPagedCoursesLevels(page, pageSize);
        }

        /// <summary>
        /// gets a course level by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Model.CourseLevel GetCourseLevelById(int id)
        {
            return dao.GetCourseLevelById(id);
        }

        /// <summary>
        /// Adds a new course level 
        /// </summary>
        /// <param name="courseLevel"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public Model.CourseLevel Add(Model.CourseLevel courseLevel, string user)
        {
            courseLevel.UserCreated = user;
            courseLevel.CreatedDate = DateTime.Now;

            return dao.Add(courseLevel);
        }

        /// <summary>
        /// updates a new course level 
        /// </summary>
        /// <param name="courseLevel"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public int Update(Model.CourseLevel courseLevel, string user)
        {
            Model.CourseLevel level = dao.GetCourseLevelById(courseLevel.Id);
            if (level == null)
                throw new HIQTrainingException(HIQResource.errorMessageUnknownRecord);

            level.Name = courseLevel.Name;
            level.UserUpdated = user;
            level.UpdateDate = DateTime.Now;

            return dao.Update(courseLevel);
        }

        /// <summary>
        /// deletes a new course level 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int Delete(int id)
        {
            Model.CourseLevel level = dao.GetCourseLevelById(id);
            if (level == null)
                throw new HIQTrainingException(HIQResource.errorMessageUnknownRecord);

            return dao.Delete(level);
        }
    }
}
