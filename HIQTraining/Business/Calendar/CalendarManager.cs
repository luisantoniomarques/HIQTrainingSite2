using HIQTraining.DAL.Calendar;
using HIQTraining.ModelDetail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIQTraining.Business.Calendar {
    public class CalendarManager {

        ICalendarDao dao;

        public CalendarManager(ICalendarDao dao) {
            this.dao = dao;
        }

        /// <summary>
        /// Gets the course's calendar
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IEnumerable<CalendarDetail> GetCourseCalendar(int id) {
            return dao.GetCourseCalendar(id).ToList();
        }

        /// <summary>
        /// returns the information from a specific course's day
        /// </summary>
        /// <param name="courseId"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public CalendarDetail GetDateInformation(int courseId, DateTime date) {
            return dao.GetDateInformation(courseId, date);
        }

        /// <summary>
        /// gets a list of calendars
        /// </summary>
        /// <param name="calendar"></param>
        /// <returns></returns>
        public List<CalendarDetail> GetCalendarList(string calendar) {
            return dao.GetCalendarList(calendar).ToList();
        }

        /// <summary>
        /// gets all calendars
        /// </summary>
        /// <returns></returns>
        public IEnumerable<CalendarFullDetail> GetCoursesCalendar() {
            return dao.GetCoursesCalendar();
        }

        /// <summary>
        /// gets the attendance from a specific date
        /// </summary>
        /// <param name="courseId"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public int GetCalendarAttendance(int courseId, DateTime date) {
            return dao.GetCalendarAttendance(courseId, date);
        }

        public int DeleteCoursesCalendar(int courseId)
        {
            return dao.DeleteCourseCalendar(courseId);
        }

        public bool CourseHasCalendarEntries(int courseId)
        {
            return dao.CourseHasCalendarEntries(courseId);
        }

    }
}
