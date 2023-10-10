using HIQTraining.DAL.Attendance;
using HIQTraining.ModelDetail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIQTraining.Business.Attendance
{
    public class AttendanceManager
    {
        IAttendanceDao dao;

        public AttendanceManager(IAttendanceDao dao)
        {
            this.dao = dao;
        }

        /// <summary>
        /// gets the attendance from a specific date from a course
        /// </summary>
        /// <param name="courseId"></param>
        /// <param name="calendarId"></param>
        /// <returns></returns>
        public List<AttendanceDetail> GetCourseAttendanceForDate(int courseId, int calendarId)
        {
            return dao.GetCourseAttendanceForDate(courseId, calendarId);
        }

        /// <summary>
        /// Saves the attendance list from a specific date
        /// </summary>
        /// <param name="courseId"></param>
        /// <param name="calendarId"></param>
        /// <param name="attendanceList"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public int SaveAttendanceList(int courseId, int calendarId, List<AttendanceDetail> attendanceList, string user)
        {
            int result = 0;
            using (var db = new Model.DBEntities())
            {
                foreach (var attendance in attendanceList)
                {
                    // is new record...
                    if (!attendance.Id.HasValue)
                    {
                        var newAttendance = new Model.Attendance
                        {
                            InscriptionId = attendance.InscriptionId,
                            CalendarId = attendance.CalendarId,
                            Status = attendance.Status.Value,
                            Observation = attendance.Observation,
                            UserCreated = user,
                            CreatedDate = DateTime.Now,
                        };
                        dao.AddAttendance(newAttendance, db);
                    }
                    else
                    {
                        Model.Attendance att = dao.GetAttendance(attendance.Id.Value, db);
                        if (att != null)
                        {
                            att.Status = attendance.Status.Value;
                            att.Observation = attendance.Observation;
                            att.UserUpdated = user;
                            att.UpdateDate = DateTime.Now;
                            dao.UpdateAttendance(att, db);
                        } // else throw Exception ???
                    }
                }

                result = db.SaveChanges();
            }

            return result;
        }
    }
}
