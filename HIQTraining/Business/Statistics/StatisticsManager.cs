using HIQTraining.DAL.Statistics;
using HIQTraining.ModelDetail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIQTraining.Business.Statistics {
    public class StatisticsManager
    {
        IStatisticsDao dao;

        public StatisticsManager(IStatisticsDao dao)
        {
            this.dao = dao;
        }

        /// <summary>
        /// gets the course success rate by course
        /// </summary>
        /// <param name="courseId"></param>
        /// <returns></returns>
        public CourseSuccessRateDetail GetCourseSuccessRate(int courseId)
        {
            return dao.GetCourseSuccessRate(courseId);
        }

        public CourseCanceledRateDetail GetCourseCanceledRate(int courseId)
        {
            return dao.GetCourseCanceledRate(courseId);
        }

        /// <summary>
        /// get the attendance rate from a course
        /// </summary>
        /// <param name="courseId"></param>
        /// <returns></returns>
        public IEnumerable<CourseAttendanceRateDetail> GetAttendanceRate(int courseId) {
            return dao.GetCourseAttendanceRate(courseId);
        }

        public CourseFullAttendanceDetail GetCourseFullAttendance(int courseId)
        {
            return dao.GetCourseFullAttendance(courseId);
        }

        public EffortsCollectionDetail GetCourseEffortPerUserSelection(int month, int year, string courseType, int selection)
        {
            return dao.GetCoursesEffortPerUserSelection(month, year, courseType, selection);
        }

        public List<string> GetCourseTypes()
        {
            return dao.GetCourseTypes();
        }

        public int GetCompanyStudentstByCourseType(int companyId, string courseType)
        {
            return dao.GetCompanyStudentsPerType(companyId, courseType);
        }

        public int[] GetCompanyStudentsPerMonthAndYear(int companyId, int month, int year)
        {
            return dao.GetCompanyStudentsPerMonthAndYear(companyId, month, year);
        }

        public int[] GetCompanyStudentsPerMonth(int companyId, int month)
        {
            return dao.GetCompanyStudentsPerMonth(companyId, month);
        }
        public int[] GetCompanyStudentsPerYear(int companyId, int year)
        {
            return dao.GetCompanyStudentsPerYear(companyId, year);
        }

        public int[] GetStudentsPerMonthAndYear(int month, int year)
        {
            return dao.GetStudentsPerMonthAndYear(month, year);
        }

        public StudentsPerYearDetail GetStudentsPerYear(int year)
        {
            return dao.GetStudentsPerYear(year);
        }

        public int[] GetStudentsPerType(string type)
        {
            return dao.GetStudentsPerType(type);
        }
    }
}
