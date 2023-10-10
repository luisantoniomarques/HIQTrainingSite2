using HIQTraining.Business.Company;
using HIQTraining.Business.Course;
using HIQTraining.Business.Statistics;
using HIQTraining.DAL.Company;
using HIQTraining.DAL.Course;
using HIQTraining.DAL.Statistics;
using HIQTraining.ModelDetail;
using HIQTrainingSite.ViewModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HIQTrainingSite.Controllers.Statistics {
    [Authorize]
    public class StatisticsController : BaseController {

        private StatisticsManager manager;

        public StatisticsController() {
            this.manager = new StatisticsManager(new StatisticsDao());
        }

        // GET: Statistics
        public ActionResult CourseSuccessRate() {
            return View();
        }

        /// <summary>
        /// returns all courses by name
        /// </summary>
        /// <param name="courseName"></param>
        /// <returns></returns>
        public JsonResult GetCourseNamesByName(string courseName) {
            CourseManager courseManager = new CourseManager(new CourseDao());
            var courses = courseManager.GetCoursesByName(courseName);

            return Json(courses, JsonRequestBehavior.AllowGet);
        }
       
        public JsonResult GetCourseSuccessRate(int courseId) {
            var rate = manager.GetCourseSuccessRate(courseId);
            
            return Json(rate, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CourseCanceledRate()
        {
            return View();
        }

        public JsonResult GetCourseCanceledRate(int courseId)
        {
            var rate = manager.GetCourseCanceledRate(courseId);

            return Json(rate, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CourseStatistics() {
            return View();
        }

        public JsonResult GetCourseAttendanceRate(int courseId) {
            var rate = manager.GetAttendanceRate(courseId);

            return Json(rate, JsonRequestBehavior.AllowGet);

        }

        public ActionResult CourseAttendancePercentage()
        {
            return View();
        }

        public JsonResult GetCourseFullAttendance(int courseId)
        {
            var rate = manager.GetCourseFullAttendance(courseId);

            return Json(rate, JsonRequestBehavior.AllowGet);

        }

        public ActionResult CourseEffort()
        {
            StartStatisticsFilters();

            return View();
        }

        public JsonResult GetCourseEffortByPeriod(int month, int year)
        {
            int typeOfSearch;

            if(month != -1 && year != -1)
            {
                typeOfSearch = 1;
            }
            else if(year != -1)
            {
                typeOfSearch = 2;
            }
            else
            {
                typeOfSearch = 4;
            }

            var rate = manager.GetCourseEffortPerUserSelection(month, year, "", typeOfSearch);

            return Json(rate, JsonRequestBehavior.AllowGet);

        }

        public JsonResult GetCourseEffortByCourseType(string courseType)
        {
            var rate = manager.GetCourseEffortPerUserSelection(0, 0, courseType, 3);

            return Json(rate, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CompanyStudents()
        {
            StartStatisticsFilters();

            CompanyManager companyManager = new CompanyManager(new CompanyDao());
            ViewBag.Companies = new SelectList(companyManager.GetAllCompanies(), "Id", "Name");

            return View();
        }

        public JsonResult GetCompanyStudentsByPeriod(int companyId, int month, int year)
        {
            int[] studentsArray;

            if(year == -1)
            {
                studentsArray = manager.GetCompanyStudentsPerMonth(companyId, month);
            }
            else if(month == -1)
            {
                studentsArray = manager.GetCompanyStudentsPerYear(companyId, year);
            }
            else
            {
                studentsArray = manager.GetCompanyStudentsPerMonthAndYear(companyId, month, year);
            }

            return Json(studentsArray, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCompanyStudentsByCourseType(int companyId, string courseType)
        {
            int studentsNumber = manager.GetCompanyStudentstByCourseType(companyId, courseType);

            return Json(studentsNumber, JsonRequestBehavior.AllowGet);
        }

        public ActionResult StudentInscriptions()
        {
            StartStatisticsFilters();

            return View();
        }

        public JsonResult GetStudentsByMonthAndYear(int month, int year)
        {
            int[] studentsArray = manager.GetStudentsPerMonthAndYear(month, year);

            return Json(studentsArray, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetStudentsByYear(int year)
        {
            var students = manager.GetStudentsPerYear(year);

            return Json(students, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetStudentsByCourseType(string courseType)
        {
            int[] studentsNumber = manager.GetStudentsPerType(courseType);

            return Json(studentsNumber, JsonRequestBehavior.AllowGet);
        }

        private void StartStatisticsFilters()
        {
            ViewBag.Months = new SelectList(Enumerable.Range(1, 12)
                .Select(x => new SelectListItem()
                {
                    Text = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(x),
                    Value = x.ToString()
                }), "Value", "Text");



            ViewBag.Years = new SelectList(Enumerable.Range(DateTime.Today.Year - 5, 15).Select(x =>

                 new SelectListItem()
                 {
                     Text = x.ToString(),
                     Value = x.ToString()
                 }), "Value", "Text");

            ViewBag.CourseTypes = new SelectList(manager.GetCourseTypes());
        }


    }
}