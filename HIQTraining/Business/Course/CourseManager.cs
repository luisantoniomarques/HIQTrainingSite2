using HIQTraining.DAL.Course;
using HIQTraining.ModelDetail;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIQTraining.Business.Course {
    public class CourseManager {
        ICourseDao dao;

        public CourseManager(ICourseDao dao) {
            this.dao = dao;
        }

        /// <summary>
        /// gets all courses
        /// </summary>
        /// <returns></returns>
        public IEnumerable<CourseDetail> GetCourses() {
            return dao.GetCourses().ToList();
        }

        public IPagedList<CourseDetail> GetPagedCourse(int page = 1, int pageSize = Common.HIQTrainingConstants.General.MAX_SEARCH_RECORDS) {
            return dao.GetCoursesSearchResults(page, pageSize);
        }

        /// <summary>
        /// adds a new course
        /// </summary>
        /// <param name="course"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public Model.Course Add(Model.Course course, string user) {
            return dao.Add(course, user);
        }

        /// <summary>
        /// checks if a code was already used or not
        /// </summary>
        /// <param name="course"></param>
        /// <returns></returns>
        public bool GetCode(Model.Course course) {
            return dao.GetCode(course);
        }

        public bool ValidateDate(Model.Course course, DateTime date)
        {
            return dao.ValidateDate(course, date);
        }
        /// <summary>
        /// Gets an course by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CourseFullDetail GetCourseById(int id) {
            return dao.GetCourseById(id);
        }

        public int Save(Model.Course course, string username) {
            return dao.Update(course, username);
        }

        /// <summary>
        /// deletes a course
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int Delete(int id) {
            return dao.Delete(id);
        }

        /// <summary>
        /// gets all courses by name
        /// </summary>
        /// <param name="courseName"></param>
        /// <returns></returns>
        public IEnumerable<CourseDetail> GetCoursesByName(string courseName) {
            return dao.GetCoursesByName(courseName);
        }

        /// <summary>
        /// get all courses by name (paged)
        /// </summary>
        /// <param name="courseName"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IPagedList<CourseDetail> GetPagedCourseByName(string courseName, int page = 1, int pageSize = Common.HIQTrainingConstants.General.MAX_SEARCH_RECORDS) {
            return dao.GetPagedCoursesByName(courseName, page, pageSize);
        }

        /// <summary>
        /// disables a date from a course
        /// </summary>
        /// <param name="courseId"></param>
        /// <param name="date"></param>
        /// <param name="observation"></param>
        /// <returns></returns>
        public int DisableCourseDate(int courseId, DateTime date, string observation) {
            return dao.DisabledCourseDate(courseId, date, observation);
        }

        /// <summary>
        /// returns the search results from course page
        /// </summary>
        /// <param name="courseName"></param>
        /// <param name="courseLevel"></param>
        /// <param name="teacherName"></param>
        /// <param name="date"></param>
        /// <param name="estado"></param>
        /// <param name="entidade"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IPagedList<CourseDetail> GetCoursesSearchResults(string courseName, string courseLevel, string teacherName, DateTime? date, int? estado, string entidade, int page = 1, int pageSize = Common.HIQTrainingConstants.General.MAX_SEARCH_RECORDS) {
            return dao.GetCoursesSearchResults(courseName, courseLevel, teacherName, date, estado, entidade, page, pageSize);
        }

        /// <summary>
        /// Returns the student by id
        /// </summary>
        /// <param name="studentId"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IEnumerable<CourseDetail> GetCoursesByStudentId(int studentId, int page = 1, int pageSize = Common.HIQTrainingConstants.General.MAX_SEARCH_RECORDS)
        {
            return dao.GetCoursesByStudentId(studentId, page, pageSize);
        }


        /// <summary>
        /// Returns the teacher by id
        /// </summary>
        /// <param name="teacherId"></param>
        /// <returns></returns>
        public IEnumerable<CourseDetail> GetCoursesByTeacherId(int teacherId)
        {
            return dao.GetCoursesByTeacherId(teacherId);
        }

        public int GetTeacherNumberOfCourses(int? teacherId)
        {
            return dao.GetTeacherNumberOfCourses(teacherId);
        }

        public int GetTeacherIdByEmail(string email)
        {
            return dao.GetTeacherIdByEmail(email);
        }

    }
}

