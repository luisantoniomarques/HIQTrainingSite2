using HIQTraining.ModelDetail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HIQTraining.Model;
using PagedList;

namespace HIQTraining.DAL.Course {
    public interface ICourseDao {
        IEnumerable<CourseDetail> GetCourses();
       
        
        //IQueryable<CourseDetail> GetPagedCourses();

        Model.Course Add(Model.Course course, string user);

        bool GetCode(Model.Course course);

        bool ValidateDate(Model.Course course, DateTime date);
        int Delete(int id);

        CourseFullDetail GetCourseById(int id);

        int Update(Model.Course course, string username);

        IEnumerable<CourseDetail> GetCoursesByName(string courseName);
        IPagedList<CourseDetail> GetPagedCoursesByName(string courseName, int page, int pageSize);

        int DisabledCourseDate(int courseId, DateTime date, string observation);

        IQueryable<CourseDetail> GetPagedCourses(int page, int pageSize);
        IPagedList<CourseDetail> GetCoursesSearchResults(int page, int pageSize);
        IPagedList<CourseDetail> GetCoursesSearchResults(string courseName, string courseLevel, string teacherName, DateTime? date, int? estado, string entidade, int page, int pageSize);
        IPagedList<CourseDetail> GetCoursesByStudentId(int studentId, int page, int pageSize);
        IEnumerable<CourseDetail> GetCoursesByTeacherId(int teacherId);
        int GetTeacherNumberOfCourses(int? teacherId);
        int GetTeacherIdByEmail(string teacherEmail);
    }
}
