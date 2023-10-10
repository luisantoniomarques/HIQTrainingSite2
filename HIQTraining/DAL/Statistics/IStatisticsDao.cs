using HIQTraining.ModelDetail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIQTraining.DAL.Statistics {
    public interface IStatisticsDao {

        CourseSuccessRateDetail GetCourseSuccessRate(int courseId);

        CourseCanceledRateDetail GetCourseCanceledRate(int courseId);

        IEnumerable<CourseAttendanceRateDetail> GetCourseAttendanceRate(int courseId);

        CourseFullAttendanceDetail GetCourseFullAttendance(int courseId);

        EffortsCollectionDetail GetCoursesEffortPerUserSelection(int month, int year, string courseType, int selection);

        List<string> GetCourseTypes();

        int[] GetCompanyStudentsPerMonthAndYear(int companyId, int month, int year);

        int[] GetCompanyStudentsPerMonth(int companyId, int month);

        int[] GetCompanyStudentsPerYear(int companyId, int year);

        int GetCompanyStudentsPerType(int companyId, string type);

        int[] GetStudentsPerMonthAndYear(int month, int year);

        StudentsPerYearDetail GetStudentsPerYear(int year);

        int[] GetStudentsPerType(string type);

    }
}
