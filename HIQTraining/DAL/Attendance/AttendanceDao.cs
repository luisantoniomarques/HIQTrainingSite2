using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HIQTraining.ModelDetail;
using HIQTraining.Model;


namespace HIQTraining.DAL.Attendance
{
    public class AttendanceDao : IAttendanceDao
    {

        /// <summary>
        /// returns all attendance from a date
        /// </summary>
        /// <param name="courseId"></param>
        /// <param name="calendarId"></param>
        /// <returns></returns>
        public List<AttendanceDetail> GetCourseAttendanceForDate(int courseId, int calendarId)
        {
            using (var db = new DBEntities())
            {
                var attendance = from ca in db.Calendars 
                                 join i in db.Inscriptions on ca.CourseId equals i.CourseId
                                 join a in db.Attendances on new { s1 = i.Id, s2 = ca.Id } equals new {s1 = a.InscriptionId, s2 = a.CalendarId} into attendanceTable
                                 from z in attendanceTable.DefaultIfEmpty() 
                                 where (ca.CourseId == courseId && ca.Id == calendarId)
                                 orderby i.Student.Name
                                 select new AttendanceDetail
                                 {
                                     StudentName = i.Student.Name,
                                     CompanyName = i.Student.Company.Name,
                                     InscriptionId = i.Id,
                                     StudentId = i.StudentId,
                                     CalendarId = ca.Id,
                                     Date = ca.CalendarDate,
                                     Id = z.Id,
                                     Status = z.Status,
                                     Observation = z.Observation,
                                     UserCreated = z.UserCreated,
                                     CreatedDate = z.CreatedDate,
                                     UserUpdated = z.UserUpdated,
                                     UpdateDate = z.UpdateDate
                                 };

                return attendance.ToList();
            }
        }

        /// <summary>
        /// Finds the attendance by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dbEntities"></param>
        /// <returns></returns>
        public Model.Attendance GetAttendance(int id, DBEntities dbEntities)
        {
            return dbEntities.Attendances.Find(id);
        }

        /// <summary>
        /// Adds a new attendance
        /// </summary>
        /// <param name="attendance"></param>
        /// <param name="dbEntities"></param>
        public void AddAttendance(Model.Attendance attendance, DBEntities dbEntities)
        {
            dbEntities.Entry(attendance).State = System.Data.Entity.EntityState.Added;
        }

        /// <summary>
        /// updates the attendance 
        /// </summary>
        /// <param name="attendance"></param>
        /// <param name="dbEntities"></param>
        public void UpdateAttendance(Model.Attendance attendance, DBEntities dbEntities)
        {
            dbEntities.Entry(attendance).State = System.Data.Entity.EntityState.Modified;
        }

    }
}
