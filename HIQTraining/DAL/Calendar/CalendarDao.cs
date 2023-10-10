using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HIQTraining.ModelDetail;
using HIQTraining.Model;

namespace HIQTraining.DAL.Calendar {
    public class CalendarDao : ICalendarDao {

        /// <summary>
        /// gets a course calendar dates by ascending order
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IEnumerable<CalendarDetail> GetCourseCalendar(int id) {
            using(DBEntities db = new DBEntities()) {
                var dates = from c in db.Courses
                            join d in db.Calendars on c.Id equals d.CourseId
                            where c.Id.Equals(id)
                            orderby d.CalendarDate
                            select new CalendarDetail {
                                Id = d.Id,
                                Date = d.CalendarDate,
                                Status = d.Status,
                                CourseId = c.Id
                            };

                return dates.ToList();
            }
        }

        /// <summary>
        /// returns the information from a specific date
        /// </summary>
        /// <param name="courseId"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public CalendarDetail GetDateInformation(int courseId, DateTime date) {
            using(DBEntities db = new DBEntities()) {
                var info = (from c in db.Calendars
                            where c.CourseId.Equals(courseId)
                            where c.CalendarDate.Equals(date)
                            select new CalendarDetail
                            {
                                Id = c.Id,
                                CourseId = courseId,
                                Date = date,
                                Observation = c.Observation,
                                Attendance = c.Attendances.Count(),
                                Status = c.Status
                            }).First();

                return info;
            }
        }

        /// <summary>
        /// metodo auxiliar que vai converter uma string em uma lista de calendar
        /// </summary>
        /// <param name="calendar"></param>
        /// <returns></returns>
        public List<CalendarDetail> GetCalendarList(string calendar) {
            if(string.IsNullOrEmpty(calendar)) {
                return new List<CalendarDetail>();
            }

            string[] date = calendar.Split(',');
            List<CalendarDetail> dateList = new List<CalendarDetail>();

            foreach(var item in date) {
                dateList.Add(new CalendarDetail { Date = Convert.ToDateTime(item) });
            }

            return dateList;
        }

        /// <summary>
        /// Gets all the courses calendar
        /// </summary>
        /// <returns></returns>
        public IEnumerable<CalendarFullDetail> GetCoursesCalendar() {
            using(DBEntities db = new DBEntities()) {
                var coursesCalendar = from c in db.Calendars
                                      join course in db.Courses on c.CourseId equals course.Id
                                      join l in db.CourseLocations on course.LocationId equals l.Id
                                      select new CalendarFullDetail {
                                          Id = course.Id,
                                          CourseName = course.Name,
                                          Date = c.CalendarDate,
                                          DisplayColor = l.DisplayColor,
                                          RoomName = l.Name
                                      };

                return coursesCalendar.ToList();
            }
        }

        /// <summary>
        /// Returns the attendance from a date
        /// </summary>
        /// <param name="courseId"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public int GetCalendarAttendance(int courseId, DateTime date) {
            using(DBEntities db = new DBEntities()) {
                var result = from c in db.Calendars
                             join a in db.Attendances on c.Id equals a.CalendarId
                             where c.CourseId.Equals(courseId)
                             where c.CalendarDate.Equals(date)
                             select a;

                return result.Count();
            }

        }

        public bool CourseHasCalendarEntries(int courseId)
        {
            using (DBEntities db = new DBEntities())
            {
                int result = db.Calendars.Where(x => x.CourseId == courseId).Count();

                return result > 0;
            }

        }

        public int DeleteCourseCalendar(int id)
        {
            using (DBEntities db = new DBEntities())
            {
                var dates = GetCourseCalendar(id);

                foreach (CalendarDetail date in dates)
                {
                    HIQTraining.Model.Calendar calendar = new HIQTraining.Model.Calendar
                    {
                        Id = date.Id
                    };

                    db.Calendars.Attach(calendar);
                    db.Calendars.Remove(calendar);
                }
                return db.SaveChanges();
            }
        }
    }
}