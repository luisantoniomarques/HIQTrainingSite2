using HIQTraining.ModelDetail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIQTraining.DAL.Calendar {
    public interface ICalendarDao {

        IEnumerable<CalendarDetail> GetCourseCalendar(int id);

        List<CalendarDetail> GetCalendarList(string calendar);

        IEnumerable<CalendarFullDetail> GetCoursesCalendar();

        int GetCalendarAttendance(int courseId, DateTime date);

        CalendarDetail GetDateInformation(int courseId, DateTime date);

        int DeleteCourseCalendar(int id);

        bool CourseHasCalendarEntries(int courseId);


    }
}
