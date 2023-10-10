using HIQTraining.Model;
using HIQTraining.ModelDetail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIQTraining.DAL.Attendance
{
    public interface IAttendanceDao
    {
        List<AttendanceDetail> GetCourseAttendanceForDate(int courseId, int calendarId);

        Model.Attendance GetAttendance(int id, DBEntities dbEntities);

        void AddAttendance(Model.Attendance attendance, DBEntities dbEntities);

        void UpdateAttendance(Model.Attendance attendance, DBEntities dbEntities);

    }
}
